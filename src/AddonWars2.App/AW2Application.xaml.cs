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
    using System.Windows.Threading;
    using AddonWars2.App.Extensions.Markup;
    using AddonWars2.App.Helpers;
    using AddonWars2.App.Models.Application;
    using AddonWars2.App.Models.Logging;
    using AddonWars2.App.Services;
    using AddonWars2.App.ViewModels;
    using Microsoft.Extensions.DependencyInjection;
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
        public void Restart()
        {
            Logger.Debug("Restarting the application.");

            // Save app/user data first.
            IOHelper.SerializeXml(Services.GetRequiredService<ApplicationConfig>().UserData, Services.GetRequiredService<ApplicationConfig>().ConfigFilePath);

            // Start a new process.
            var currExecPath = Process.GetCurrentProcess().MainModule.FileName;
            Process.Start(currExecPath);

            // Now close the currrent one.
            LogManager.Shutdown();

            Current.Shutdown();
        }

        /// <inheritdoc/>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Services = AW2ServiceProvider.ConfigureServices();
            Services.GetRequiredService<ApplicationConfig>().StartupDateTime = DateTime.Now;

            AW2App_SetupLogger();
            AW2App_SetupAppConfig();
            AW2App_SetupLocalization();

            // To set DataContext directly in markup.
            DISourceExtension.Resolver = (type) => { return Services.GetRequiredService(type); };

            var mainViewModel = Services.GetRequiredService<MainWindowViewModel>();
            MainWindowInstance = new () { DataContext = mainViewModel };
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

        // Setups logger.
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
            Logger = LogManager.GetCurrentClassLogger();
            LogManager.Configuration = IOHelper.GetLoggerConfigurationNLog();

            // Locate AppData\Roaming application dir.
            var appDataDir = IOHelper.GenerateApplicationDataDirectory();
            if (!Directory.Exists(appDataDir))
            {
                Directory.CreateDirectory(appDataDir);
            }

            Services.GetRequiredService<ApplicationConfig>().AppDataDir = appDataDir;

            Logger.Info($"Using AppData directory: {appDataDir}");

            // Locate the logs dir.
            var logCfg = LogManager.Configuration;
            var logsDir = Path.Join(appDataDir, "logs");
            if (!Directory.Exists(logsDir))
            {
                Directory.CreateDirectory(logsDir);
            }

            // Setup logger config.
            var date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var logPath = Path.Join(logsDir, $"{Services.GetRequiredService<ApplicationConfig>().LogPrefix}{date}.txt");
            var logTarget = new FileTarget()
            {
                Name = "logTarget",
                FileName = logPath,
                Layout = "${longdate} [${level:uppercase=true}] [${callsite}] ${message} ${exception:format=ToString}",
            };
            logCfg.AddRule(LogLevel.Debug, LogLevel.Fatal, logTarget);
            LogManager.Configuration = logCfg;

            Logger.Info($"Created a new log file: {logPath}");
        }

        // Setups application data (user config file).
        private void AW2App_SetupAppConfig()
        {
            // Set the default settings.
            var appCfg = new UserData(Services);
            Services.GetRequiredService<ApplicationConfig>().UserData = appCfg;
            var cfg = Services.GetRequiredService<ApplicationConfig>().UserData;  // default

            // Try to get application settings from the AppData\Roaming dir.
            var cfgPath = Services.GetRequiredService<ApplicationConfig>().ConfigFilePath;
            if (!File.Exists(cfgPath))
            {
                // Create a new one with default settings.
                IOHelper.SerializeXml(cfg, cfgPath);

                Logger.Info($"Created a new application config file: {cfgPath}");
            }
            else
            {
                // Try to load it and check if it's valid since serializer
                // will either return incorrect data (i.e. some properties are missing)
                // or will thron an exception and thus return default(T).
                cfg = IOHelper.DeserializeXml<UserData>(cfgPath);
                if (!UserData.IsValid(cfg))
                {
                    Logger.Info($"The given config file is not valid. Creating a new one.");

                    // Delete the corrupted file (since this name is reserved for the app config)
                    // and replace it with a default one.
                    File.Delete(cfgPath);
                    cfg = UserData.Default;
                    IOHelper.SerializeXml(cfg, cfgPath);

                    Logger.Info($"Created a new application config file: {cfgPath}");
                }
            }

            Services.GetRequiredService<ApplicationConfig>().UserData = cfg;

            Logger.Info($"Using the existing application config file: {cfgPath}");
        }

        // Setups application language.
        private void AW2App_SetupLocalization()
        {
            var cfg = Services.GetRequiredService<ApplicationConfig>().UserData;
            var culture = cfg.SelectedCulture;
            var selected = LocalizationHelper.SelectCulture(culture);
            cfg.SelectedCulture = cfg.AvailableCultures.FirstOrDefault(x => x.Culture == selected);

            Logger.Info($"Culture selected: {selected}");
        }

        // Shows a popup message if an unhandled exception occured.
        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // TODO: A custom and more discriptive dialog would be nice.

            ////var fullMessage = $"Source: {e.Exception.Source}\n\n{e.Exception.Message}";
            ////var exTitle = (string)Current.Resources["S.MessageBox.SDocument.UnhandledEx"];

            ////MessageBoxService.Show(fullMessage, exTitle, MessageBoxButton.OK, MessageBoxImage.Error);

            ////e.Handled = true;

            Logger.Fatal(e.Exception, "AN UNHANDLED EXCEPTION OCCURED.");
        }

        #endregion Methods
    }
}