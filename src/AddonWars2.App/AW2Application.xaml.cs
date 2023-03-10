// ==================================================================================================
// <copyright file="AW2Application.xaml.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using AddonWars2.App.Controllers;
    using AddonWars2.App.Extensions.Markup;
    using AddonWars2.App.Helpers;
    using AddonWars2.App.Models.Application;
    using AddonWars2.App.Models.Logging;
    using AddonWars2.App.Services;
    using AddonWars2.App.ViewModels;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NLog;
    using NLog.Config;
    using NLog.Targets;
    using LogLevel = NLog.LogLevel;

    /// <summary>
    /// Interaction logic for AW2Application.xaml.
    /// </summary>
    public partial class AW2Application : Application
    {
        // TODO: I don't like the mess in ctor inner calls, especially due to
        //       mixing it with logger calls. Maybe to refactor it to make more verbose and readable?
        // TODO: Review dependencies inside ctor inner calls.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AW2Application"/> class.
        /// </summary>
        public AW2Application()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the current application instance.
        /// </summary>
        public static new AW2Application Current => (AW2Application)Application.Current;

        /// <summary>
        /// Gets a service provider instance.
        /// </summary>
        public IServiceProvider Services { get; private set; }

        /// <summary>
        /// Gets a reference to the main window instance.
        /// </summary>
        public MainWindow MainWindowInstance { get; private set; }

        // Gets the current logger instance.
        private static Logger Logger { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Immediately restarts the application.
        /// </summary>
        /// <param name="asAdmin">Indicates whether a new process should request admin rights to start.</param>
        public void Restart(bool asAdmin = false)
        {
            Logger.Debug("Restarting the application.");

            // Save app/user data first.
            var data = Services.GetRequiredService<ApplicationConfig>().LocalData;
            var path = Services.GetRequiredService<ApplicationConfig>().ConfigFilePath;
            ApplicationConfig.WriteLocalDataAsXml(path, data);

            // Start a new process.
            var currExecPath = Environment.ProcessPath;
            var verb = asAdmin ? "runas" : string.Empty;
            var process = new Process
            {
                StartInfo =
                {
                    FileName = currExecPath,
                    Verb = verb,
                },
            };

            process.Start();

            Current.MainWindowInstance.Close();
        }

        /// <inheritdoc/>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var startupDateTime = DateTime.Now;
            Services = AW2ServiceProvider.ConfigureServices();
            var appConfig = Services.GetRequiredService<ApplicationConfig>();
            appConfig.StartupDateTime = startupDateTime;

            foreach (var arg in e.Args)
            {
                switch (arg)
                {
                    case "-debug":
                        appConfig.IsDebugMode = true;
                        break;
                    default:
                        break;
                }
            }

            AW2App_SetupLogger();
            AW2App_SetupAppConfig();
            AW2App_SetupLocalization();

            // To enable setting DataContext directly in markup.
            DISourceExtension.Resolver = (type) => { return Services.GetRequiredService(type); };

            MainWindowInstance = new ();
            MainWindowInstance.Show();

            Logger.Info("Application loaded and ready.");
        }

        /// <inheritdoc/>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Logger.Info("Application shutdown.");
            LogManager.Shutdown();
        }

        // Setups logging.
        private void AW2App_SetupLogger()
        {
            Target.Register<NLogLoggingManagerTarget>("NLogLoggingManagerTarget");

            // Workaround details:
            // https://github.com/NLog/NLog/wiki/Dependency-injection-with-NLog

            var defautCtor = ConfigurationItemFactory.Default.CreateInstance;
            ConfigurationItemFactory.Default.CreateInstance = type =>
            {
                if (type == typeof(NLogLoggingManagerTarget))
                {
                    return new NLogLoggingManagerTarget(Services.GetRequiredService<LoggingManager>());
                }

                return defautCtor(type);
            };

            // Replace the current configuration to "reset" it.
            Services.GetRequiredService<ILogger<MainWindowViewModel>>();  // TODO: Apparently this call is required to init logger? Doesn't work without.
            Logger = LogManager.GetCurrentClassLogger();
            LogManager.Configuration = IOHelper.GetLoggerConfigurationNLog();
            Logger.Info($"Start logging.");

            // Locate AppData\Roaming application directory.
            var appDataDir = IOHelper.GenerateApplicationDataDirectory();
            if (!Directory.Exists(appDataDir))
            {
                Directory.CreateDirectory(appDataDir);
            }

            var appConfig = Services.GetRequiredService<ApplicationConfig>();
            appConfig.AppDataDir = appDataDir;

            Logger.Info($"Using AppData directory: {appDataDir}");

            // Locate the logs directory.
            var logsDirPath = Path.Join(appDataDir, appConfig.LogDirName);
            if (!Directory.Exists(logsDirPath))
            {
                Directory.CreateDirectory(logsDirPath);
            }

            // Setup logger config.
            var logCfg = LogManager.Configuration;
            var unixMsDateTime = ((DateTimeOffset)appConfig.StartupDateTime).ToUnixTimeMilliseconds();
            var logPath = Path.Join(logsDirPath, $"{appConfig.LogPrefix}{unixMsDateTime}.txt");
            var logTarget = new FileTarget()
            {
                Name = "LogFileTarget",
                FileName = logPath,
                Layout = "${longdate} [${level:uppercase=true}] [${callsite}] ${message} ${exception:format=ToString}",
            };

            var minLevel = appConfig.IsDebugMode ? LogLevel.Debug : LogLevel.Info;
            logCfg.AddRule(minLevel, LogLevel.Fatal, logTarget, "*");
            LogManager.Configuration = logCfg;

            foreach (var rule in logCfg.LoggingRules)
            {
                rule.SetLoggingLevels(minLevel, LogLevel.Fatal);
            }

            Logger.Info($"Created a new log file: {logPath}");
        }

        // Setups the application config and local data.
        private void AW2App_SetupAppConfig()
        {
            // Set the default settings.
            var appConfig = Services.GetRequiredService<ApplicationConfig>();
            var localdata = appConfig.LocalData;  // default

            // Try to get the application settings from the AppData\Roaming dir.
            var path = appConfig.ConfigFilePath;

            // File not found.
            if (!File.Exists(path))
            {
                // Create a new one with default settings.
                ApplicationConfig.WriteLocalDataAsXml(path, localdata);

                Logger.Info($"Created a new application config file: {path}");
            }
            else
            {
                // Try to load it and check if it's valid since serializer
                // will either return incorrect data (i.e. some properties are missing)
                // or will throw an exception..
                localdata = ApplicationConfig.LoadLocalDataFromXml(path);
                if (!LocalData.IsValid(localdata))
                {
                    Logger.Warn($"The given config file is not valid.");

                    // Delete the corrupted file and replace it with a default one.
                    File.Delete(path);
                    localdata = LocalData.Default;
                    ApplicationConfig.WriteLocalDataAsXml(path, localdata);

                    Logger.Info($"Created a new application config file: {path}");
                }
            }

            appConfig.LocalData = localdata;

            Logger.Info($"Using the application config file: {path}");
        }

        // Setups application language.
        private void AW2App_SetupLocalization()
        {
            var config = Services.GetRequiredService<ApplicationConfig>();
            var culture = config.LocalData.SelectedCultureString;
            var actuallySelected = LocalizationHelper.SelectCulture(culture);

            // Now we should replace it in both places since config file may contain invalid culture string.
            config.SelectedCulture = config.AvailableCultures.FirstOrDefault(x => x.Culture == actuallySelected, config.DefaultCulture);
            config.LocalData.SelectedCultureString = config.SelectedCulture.Culture;

            Logger.Info($"Culture selected: {actuallySelected}");
        }

        // Shows a popup message if an unhandled exception occured.
        ////private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        ////{
        ////    // TODO: A custom and more discriptive dialog would be nice.

        ////    var fullMessage = $"Source: {e.Exception.Source}\n\n{e.Exception.Message}";
        ////    var exTitle = (string)Current.Resources["S.MessageBox.SDocument.UnhandledEx"];

        ////    MessageBoxService.Show(fullMessage, exTitle, MessageBoxButton.OK, MessageBoxImage.Error);

        ////    e.Handled = true;

        ////    Logger.Fatal(e.Exception, "AN UNHANDLED EXCEPTION OCCURED.");
        ////}

        #endregion Methods
    }
}