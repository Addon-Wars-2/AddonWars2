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
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using AddonWars2.App.Configuration;
    using AddonWars2.App.Extensions.Markup;
    using AddonWars2.App.Logging;
    using AddonWars2.App.UIServices;
    using AddonWars2.App.Utils.Helpers;
    using AddonWars2.App.ViewModels;
    using AddonWars2.App.ViewModels.Factories;
    using AddonWars2.SharedData.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using MvvmDialogs;
    using Serilog;
    using Serilog.Enrichers.CallerInfo;
    using Serilog.Events;
    using Serilog.Exceptions;
    using Serilog.Filters;

    /// <summary>
    /// Interaction logic for AW2Application.xaml.
    /// </summary>
    public partial class AW2Application : Application
    {
        #region Fields

        private const string UNIQUE_MUTEX_NAME = "C1284452-E2A7-4F2E-853D-9272848B0B1D";
        private const string UNIQUE_EVENT_NAME = "816A7B1C-9BE6-4D4D-9FD4-A45E267448EB";

        private EventWaitHandle _eventWaitHandle;
        private Mutex _mutex;

        #endregion Fields

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

        /// <summary>
        /// Gets the application config reference.
        /// </summary>
        public IApplicationConfig ApplicationConfig { get; private set; }

        /// <summary>
        /// Gets the logger instance.
        /// </summary>
        public ILogger Logger { get; private set; }

        /// <summary>
        /// Gets the application static data.
        /// </summary>
        public IAppSharedData AppSharedData { get; private set; }

        /// <summary>
        /// Gets the application web-related static data.
        /// </summary>
        public IWebSharedData WebSharedData { get; private set; }

        /// <summary>
        /// Gets the error dialog service.
        /// </summary>
        public IDialogService DialogService { get; private set; }

        // Event wait handle.
        private EventWaitHandle EventWaitHandle
        {
            get => _eventWaitHandle;
            set => _eventWaitHandle = value;
        }

        // The mutex.
        private Mutex Mutex
        {
            get => _mutex;
            set => _mutex = value;
        }

        private DateTime StartupDateTime { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Immediately restarts the application.
        /// </summary>
        /// <param name="asAdmin">Indicates whether a new process should request admin rights to start.</param>
        public void Restart(bool asAdmin = false)
        {
            Logger.Information("Restarting the application.");

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
            StartupDateTime = DateTime.Now;

            base.OnStartup(e);

            if (!EnsureMutex())
            {
                return;
            }

            Services = AW2ServiceProvider.ConfigureServices();

            ApplicationConfig = Services.GetRequiredService<IApplicationConfig>();
            AppSharedData = Services.GetRequiredService<IAppSharedData>();
            WebSharedData = Services.GetRequiredService<IWebSharedData>();
            DialogService = Services.GetRequiredService<IDialogService>();

            bool isDebug = false;
            foreach (var arg in e.Args)
            {
                switch (arg)
                {
                    case "-debug":
                        isDebug = true;
                        break;
                    default:
                        break;
                }
            }

            ApplicationConfig.SessionData.IsDebugMode = isDebug;

            // To enable setting DataContext directly in markup.
            DISourceExtension.Resolver = (type) => { return Services.GetRequiredService(type); };

            AW2App_SetupExceptionHandling();
            AW2App_SetupLogger();
            AW2App_SetupUserData();

            MainWindowInstance = new MainWindow();
            MainWindowInstance.Show();

            Logger.Information("Application loaded and ready.");
        }

        /// <inheritdoc/>
        protected override void OnExit(ExitEventArgs e)
        {
            Logger.Information("Application shutdown.");

            base.OnExit(e);

            Log.CloseAndFlush();
        }

        // Allows only one instance to exist.
        // Source: https://stackoverflow.com/a/23730146
        private bool EnsureMutex()
        {
            Mutex = new Mutex(true, UNIQUE_MUTEX_NAME, out bool isOwned);
            EventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, UNIQUE_EVENT_NAME);

            GC.KeepAlive(Mutex);

            if (isOwned)
            {
                // Spawn a thread which will be waiting for our event.
                var thread = new Thread(
                    () =>
                    {
                        while (EventWaitHandle.WaitOne())
                        {
                            Current.Dispatcher.BeginInvoke(() => Current.MainWindowInstance.BringToForeground());
                        }
                    });

                // It is important to mark it as background, otherwise it will prevent app from exiting.
                thread.IsBackground = true;
                thread.Start();

                return isOwned;
            }

            // Notify other instance so it could bring itself to foreground.
            EventWaitHandle.Set();

            // Terminate this instance.
            Shutdown();

            return isOwned;
        }

        // Setups exception handling.
        private void AW2App_SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                LogUnhandledException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");
            };

            DispatcherUnhandledException += (s, e) =>
            {
                LogUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");
                e.Handled = true;
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                LogUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
                e.SetObserved();
            };
        }

        // Setups logging.
        private void AW2App_SetupLogger()
        {
            // Locate AppSharedData\Roaming application directory.
            var appDataDir = IOHelper.BuildApplicationDataDirectory();
            if (!Directory.Exists(appDataDir))
            {
                Directory.CreateDirectory(appDataDir);
            }

            // Locate the logs directory within the application directory.
            var logsDirPath = Path.Join(appDataDir, AppSharedData.LogDirName);
            if (!Directory.Exists(logsDirPath))
            {
                Directory.CreateDirectory(logsDirPath);
            }

            // Session log file will contain startup datetime in milliseconds format.
            var unixMsDateTime = ((DateTimeOffset)StartupDateTime).ToUnixTimeMilliseconds();
            var prefix = ApplicationConfig.SessionData.IsDebugMode ? AppSharedData.LogFilePrefixDebug : AppSharedData.LogFilePrefix;
            var logFilePath = Path.Join(logsDirPath, $"{prefix}{unixMsDateTime}.txt");

            if (ApplicationConfig.SessionData.IsDebugMode)
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .Enrich.WithCallerInfo(
                        includeFileInfo: true,
                        assemblyPrefix: "AddonWars2",
                        prefix: string.Empty)
                    .Enrich.WithExceptionDetails()
                    .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                    .Filter.ByExcluding(Matching.FromSource("System"))
                    .WriteTo.Sink(
                        Services.GetRequiredService<SerilogLogsAggregatorSink>(),
                        LogEventLevel.Debug)
                    .WriteTo.File(
                        logFilePath,
                        LogEventLevel.Debug,
                        "[{Timestamp:yyyy-MM-dd HH:mm:ss.ffff zzz}] [{Level:u3}] [{Namespace}.{Method}] {Message}{NewLine}{Exception}")
                    .CreateLogger();
            }
            else
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .Enrich.WithCallerInfo(
                        includeFileInfo: true,
                        assemblyPrefix: "AddonWars2",
                        prefix: string.Empty)
                    .Enrich.WithExceptionDetails()
                    .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                    .Filter.ByExcluding(Matching.FromSource("System"))
                    .WriteTo.Sink(
                        Services.GetRequiredService<SerilogLogsAggregatorSink>(),
                        LogEventLevel.Information)
                    .WriteTo.File(
                        logFilePath,
                        LogEventLevel.Information,
                        "[{Timestamp:yyyy-MM-dd HH:mm:ss.ffff zzz}] [{Level:u3}] [{Namespace}.{Method}] {Message}{NewLine}{Exception}")
                    .CreateLogger();
            }

            Logger = Log.Logger;

            ApplicationConfig.SessionData.AppDataDir = appDataDir;
            ApplicationConfig.SessionData.LogFilePath = logFilePath;

            Logger.Information($"Logging started: {logFilePath}");
            Logger.Information($"Using AppSharedData directory: {appDataDir}");
        }

        // Setups the application config and local data.
        private void AW2App_SetupUserData()
        {
            // Get a culture value from the config file and see if there are localization resource files for it.
            // If not - it fallbacks to a default culture which is en-US.
            var actuallySelected = LocalizationHelper.SelectCulture(ApplicationConfig.UserData.SelectedCultureString);
            Logger.Information($"Application culture selected: {actuallySelected}");

            // Double check to make sure shared data contains the information about this culture.
            if (AppSharedData.AppSupportedCultures.FindIndex(x => x.Culture == actuallySelected) < 0)
            {
                throw new NotSupportedException($"Could not find the provider culture ({ApplicationConfig.UserData.SelectedCultureString}) in shared data.");
            }

            // Set the finally selected culture.
            ApplicationConfig.UserData.SelectedCultureString = actuallySelected;

            // For the rest ANet and GW2 services we also check if the selected culture is supported by them.
            // That means, that the app selected culture may differ from the one used for ANet and GW2 services.
            var culture = AppSharedData.AnetSupportedCultures.FirstOrDefault(x => x.Culture == ApplicationConfig.UserData.SelectedCultureString, AppSharedData.DefaultCulture);

            Logger.Information($"A culture for ANet and GW2 selected: {culture}");

            // Now use templates to setup ANet and GW2 service links with the selected culture.
            ApplicationConfig.UserData.AnetHome = string.Format(WebSharedData.AnetHomeTemplate, culture.ShortName.ToLower());
            ApplicationConfig.UserData.Gw2Home = string.Format(WebSharedData.Gw2HomeTemplate, culture.ShortName.ToLower());
            ApplicationConfig.UserData.Gw2WikiHome = string.Format(WebSharedData.Gw2WikiHomeTemplate, culture.ShortName.ToLower());
            ApplicationConfig.UserData.Gw2Rss = string.Format(WebSharedData.Gw2RssHomeTemplate, culture.ShortName.ToLower());
            ApplicationConfig.UserData.AnetHome = string.Format(WebSharedData.AnetHomeTemplate, culture.ShortName.ToLower());

            // Setup the rest.
            ApplicationConfig.UserData.CachedDirName = Path.Join(ApplicationConfig.SessionData.AppDataDir, AppSharedData.CachedDirName);
            ApplicationConfig.UserData.CachedLibFilePath = Path.Join(ApplicationConfig.UserData.CachedDirName, AppSharedData.CachedLibFileName);
            ApplicationConfig.UserData.CachedLibProviderName = AppSharedData.CachedLibProviderName;
            ApplicationConfig.UserData.InstalledAddonsFilePath = Path.Join(ApplicationConfig.SessionData.AppDataDir, AppSharedData.InstalledAddonsFileName);
        }

        // Logs any unhandled exception.
        private void LogUnhandledException(Exception exception, string source)
        {
            string logMessage = $">>> AN UNHANDLED EXCEPTION OCCURED <<<\n{source}";
            Logger.Fatal(exception, logMessage);

            ////// Try to open log file automatically, so a user can just copy - paste it.
            ////try
            ////{
            ////    Process.Start(new ProcessStartInfo(ApplicationConfig.SessionData.LogFilePath)
            ////    {
            ////        Verb = "open",
            ////        UseShellExecute = true,
            ////    });
            ////}
            ////catch (Exception)
            ////{
            ////    // If we can't do this for whatever reason, do nothing.
            ////    Logger.Error("Unable to open log file automatically.");
            ////}

            // TODO: Localization.
            // Show an error message box.
            var title = "An Unhandled Exception has occured";
            var message =
                    $"The application will be terminated. If the log file wasn't opened automatically, " +
                    $"it can be found under the following path:\n\n{ApplicationConfig.SessionData.LogFilePath}";
            var details = $"{exception.Message}\n{exception.StackTrace}";

            var ownerViewModel = Services.GetRequiredService<MainWindowViewModel>();
            var dialogViewModelFactory = Services.GetRequiredService<IErrorDialogViewModelFactory>();
            var dialogViewModel = dialogViewModelFactory.Create(title, message, details);

            DialogService.ShowDialog(ownerViewModel, dialogViewModel);

            Shutdown();
        }

        #endregion Methods
    }
}