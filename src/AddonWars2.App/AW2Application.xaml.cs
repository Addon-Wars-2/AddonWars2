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
        public static void Restart()
        {
            Logger.Debug("Restarting the application.");
            var currExecPath = Process.GetCurrentProcess().MainModule.FileName;

            Process.Start(currExecPath);

            LogManager.Shutdown();
            Current.Shutdown();
        }

        /// <summary>
        /// Forces <see cref="MainWindow"/> to redraw itself without shutting the application down.
        /// </summary>
        /// <remarks>
        /// This method allows to "hot-reload" the main window without closing the whole application,
        /// and by doing so - keep logging up. The existing DataContext will be re-attached to a new window.
        /// </remarks>
        public void ReloadMainWindow()
        {
            // To prevent the app from closing we set SM to OnExplicitShutdown.
            Logger.Debug("Closing the main window.");
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            MainWindowInstance.Close();
            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            var mainViewModel = Services.GetRequiredService<MainWindowViewModel>();
            MainWindowInstance = new () { DataContext = mainViewModel };
            MainWindowInstance.Show();

            AW2App_SetupLocalization();

            Logger.Info("Main window reload complete.");
        }

        /// <inheritdoc/>
        protected override void OnStartup(StartupEventArgs e)
        {
            // Save the startup date and time.
            ApplicationGlobal.StartupDateTime = DateTime.Now;
            base.OnStartup(e);

            // Configure DI services and logging.
            Services = AW2ServiceProvider.ConfigureServices();
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
            Target.Register<NLogCustomTarget>("NLogCustomTarget");

            // Workaround details:
            // https://github.com/NLog/NLog/wiki/Dependency-injection-with-NLog
            var defautCtor = ConfigurationItemFactory.Default.CreateInstance;
            ConfigurationItemFactory.Default.CreateInstance = type =>
            {
                if (type == typeof(NLogCustomTarget))
                {
                    return new NLogCustomTarget(Services.GetRequiredService<LoggingManager>());
                }

                return defautCtor(type);
            };

            // Replace the current configuration to "reset" it.
            Logger = LogManager.GetCurrentClassLogger();
            LogManager.Configuration = IOHelper.GetLoggerConfigurationNLog();

            // Find AppData application dir.
            var appDataDir = IOHelper.GenerateApplicationDataDirectory();
            if (!Directory.Exists(appDataDir))
            {
                Directory.CreateDirectory(appDataDir);
            }

            ApplicationGlobal.AppDataDir = appDataDir;
            Logger.Info($"Using AppData directory: {appDataDir}");

            // Find the logs dir.
            var logCfg = LogManager.Configuration;
            var logsDir = Path.Join(appDataDir, "logs");
            if (!Directory.Exists(logsDir))
            {
                Directory.CreateDirectory(logsDir);
            }

            // Setup logger config.
            var date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var logPath = Path.Join(logsDir, $"{ApplicationGlobal.LogPrefix}{date}.txt");
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
            ApplicationConfig.Services = Services;  // TODO: Very dirty solution! We do it only once to init static property.
            ApplicationGlobal.AppConfig = ApplicationConfig.Default;
            var cfg = ApplicationGlobal.AppConfig;  // default

            // Try to get application settings from the AppData dir.
            var cfgPath = ApplicationGlobal.ConfigFilePath;
            if (!File.Exists(cfgPath))
            {
                // Create a new one with default settings.
                var serializedString = IOHelper.Serialize(cfg);
                IOHelper.WriteXml(serializedString, cfgPath);
                Logger.Info($"Created a new application config file: {cfgPath}");
            }
            else
            {
                // Try to load it.
                cfg = IOHelper.Deserialize<ApplicationConfig>(cfgPath);
                if (!ApplicationConfig.IsValid(cfg))
                {
                    // Delete the corrupted file (since this name is reserved for the app config).
                    Logger.Info($"The given config file is not valid. Creating a new one.");
                    File.Delete(cfgPath);

                    // Create a new default config.
                    cfg = ApplicationConfig.Default;
                    var serializedString = IOHelper.Serialize(cfg);
                    IOHelper.WriteXml(serializedString, cfgPath);
                    Logger.Info($"Created a new application config file: {cfgPath}");
                }
            }

            ApplicationGlobal.AppConfig = cfg;
            Logger.Info($"Using the existing application config file: {cfgPath}");
        }

        // Setups application language.
        private void AW2App_SetupLocalization()
        {
            var cfg = ApplicationGlobal.AppConfig;
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