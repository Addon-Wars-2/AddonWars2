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
    using AddonWars2.App.Extensions.Markup;
    using AddonWars2.App.Models.Application;
    using AddonWars2.App.Models.Logging;
    using AddonWars2.App.Services;
    using AddonWars2.App.Services.Interfaces;
    using AddonWars2.App.Utils.Helpers;
    using Microsoft.Extensions.DependencyInjection;
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
        // TODO: I don't like the mess in ctor inner calls, especially due to
        //       mixing it with logger calls. Maybe to refactor it to make more verbose and readable?
        // TODO: Review dependencies inside ctor inner calls.

        #region Fields

        private const string UNIQUE_MUTEX_NAME = "c1284452-e2a7-4f2e-853d-9272848b0b1d";
        private const string UNIQUE_EVENT_NAME = "816a7b1c-9be6-4d4d-9fd4-a45e267448eb";

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
        public ApplicationConfig ApplicationConfig { get; private set; }

        /// <summary>
        /// Gets the logger instance.
        /// </summary>
        public ILogger Logger { get; private set; }

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
            var data = ApplicationConfig.LocalData;
            var path = ApplicationConfig.ConfigFilePath;

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

            EnsureMutex();

            var startupDateTime = DateTime.Now;
            Services = AW2ServiceProvider.ConfigureServices();
            ApplicationConfig = Services.GetRequiredService<ApplicationConfig>();
            ApplicationConfig.StartupDateTime = startupDateTime;

            foreach (var arg in e.Args)
            {
                switch (arg)
                {
                    case "-debug":
                        ApplicationConfig.IsDebugMode = true;
                        break;
                    default:
                        break;
                }
            }

            AW2App_SetupExceptionHandling();
            AW2App_SetupLogger();
            AW2App_SetupAppConfig();
            AW2App_SetupLocalization();

            // To enable setting DataContext directly in markup.
            DISourceExtension.Resolver = (type) => { return Services.GetRequiredService(type); };

            MainWindowInstance = new MainWindow();
            MainWindowInstance.Show();

            Logger.Information("Application loaded and ready.");
        }

        /// <inheritdoc/>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            Log.Logger.Information("Application shutdown.");

            Log.CloseAndFlush();
        }

        // Allows only one instance to exist.
        // Source: https://stackoverflow.com/a/23730146
        private void EnsureMutex()
        {
            bool isOwned;
            Mutex = new Mutex(true, UNIQUE_MUTEX_NAME, out isOwned);
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

                // It is important mark it as background otherwise it will prevent app from exiting.
                thread.IsBackground = true;
                thread.Start();

                return;
            }

            // Notify other instance so it could bring itself to foreground.
            EventWaitHandle.Set();

            // Terminate this instance.
            Shutdown();
        }

        // Setups exception handling.
        private void AW2App_SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                LogUnhandledException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

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
            // Locate AppData\Roaming application directory.
            var appDataDir = IOHelper.GenerateApplicationDataDirectory();
            if (!Directory.Exists(appDataDir))
            {
                Directory.CreateDirectory(appDataDir);
            }

            ApplicationConfig.AppDataDir = appDataDir;

            // Locate the logs directory within the application directory.
            var logsDirPath = Path.Join(appDataDir, ApplicationConfig.LogDirName);
            if (!Directory.Exists(logsDirPath))
            {
                Directory.CreateDirectory(logsDirPath);
            }

            // Session log file will contain startup datetime in milliseconds format.
            var unixMsDateTime = ((DateTimeOffset)ApplicationConfig.StartupDateTime).ToUnixTimeMilliseconds();
            ApplicationConfig.LogFileFullPath = Path.Join(logsDirPath, $"{ApplicationConfig.LogPrefix}{unixMsDateTime}.txt");

            if (ApplicationConfig.IsDebugMode)
            {
                Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithCallerInfo(
                    includeFileInfo: true,
                    assemblyPrefix: "AW2",
                    prefix: string.Empty)
                .Enrich.WithExceptionDetails()
                .Filter.ByExcluding(Matching.FromSource("System")) // to disable logging access from another thread (System.Net.Http)
                .WriteTo.Sink(
                    Services.GetRequiredService<SerilogLogsAggregatorSink>(),
                    LogEventLevel.Debug)
                .WriteTo.File(
                    ApplicationConfig.LogFileFullPath,
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
                    assemblyPrefix: "AW2",
                    prefix: string.Empty)
                .Enrich.WithExceptionDetails()
                .Filter.ByExcluding(Matching.FromSource("System")) // to disable logging access from another thread (System.Net.Http)
                .WriteTo.Sink(
                    Services.GetRequiredService<SerilogLogsAggregatorSink>(),
                    LogEventLevel.Information)
                .WriteTo.File(
                    ApplicationConfig.LogFileFullPath,
                    LogEventLevel.Information,
                    "[{Timestamp:yyyy-MM-dd HH:mm:ss.ffff zzz}] [{Level:u3}] [{Namespace}.{Method}] {Message}{NewLine}{Exception}")
                .CreateLogger();
            }

            Logger = Log.Logger;

            Logger.Information($"Logging started: {ApplicationConfig.LogFileFullPath}");
            Logger.Information($"Using AppData directory: {appDataDir}");
        }

        // Setups the application config and local data.
        private void AW2App_SetupAppConfig()
        {
            // Set the default settings.
            var localdata = ApplicationConfig.LocalData;  // default

            // Try to get the application settings from the AppData\Roaming dir.
            var path = ApplicationConfig.ConfigFilePath;

            // File not found.
            if (!File.Exists(path))
            {
                // Create a new one with default settings.
                ApplicationConfig.WriteLocalDataAsXml(path, localdata);

                Logger.Information($"Created a new application config file: {path}");
            }
            else
            {
                // Try to load it and check if it's valid since serializer
                // will either return incorrect data (i.e. some properties are missing)
                // or will throw an exception..
                localdata = ApplicationConfig.LoadLocalDataFromXml(path);
                if (!LocalData.IsValid(localdata))
                {
                    Logger.Warning($"The given config file is not valid.");

                    // Delete the corrupted file and replace it with a default one.
                    File.Delete(path);
                    localdata = LocalData.Default;
                    ApplicationConfig.WriteLocalDataAsXml(path, localdata);

                    Logger.Information($"Created a new application config file: {path}");
                }
            }

            ApplicationConfig.LocalData = localdata ?? LocalData.Default;

            Logger.Information($"Using the application config file: {path}");
        }

        // Setups application language.
        private void AW2App_SetupLocalization()
        {
            var culture = ApplicationConfig.LocalData.SelectedCultureString;
            var actuallySelected = LocalizationHelper.SelectCulture(culture);

            if (ApplicationConfig.LocalData == null)
            {
                throw new NullReferenceException(nameof(ApplicationConfig.LocalData));
            }

            // Now we should replace it in both places since config file may contain invalid culture string.
            ApplicationConfig.SelectedCulture = ApplicationConfig.AvailableCultures.FirstOrDefault(x => x.Culture == actuallySelected, ApplicationConfig.DefaultCulture);
            ApplicationConfig.LocalData.SelectedCultureString = ApplicationConfig.SelectedCulture.Culture;

            Logger.Information($"Culture selected: {actuallySelected}");
        }

        // Logs any unhandled exception.
        private void LogUnhandledException(Exception exception, string source)
        {
            // TODO: We should be able to revert any action if an unhandled exception has occured.
            //       Like if we were updating addons, and an exception was thrown in the middle of the process.

            string message = $">>> AN UNHANDLED EXCEPTION OCCURED <<<\n{source}";
            Logger.Fatal(exception, message);

#if !DEBUG
            // Try to open log file automatically, so a user can just copy-paste it.
            try
            {
                Process.Start(new ProcessStartInfo(ApplicationConfig.LogFileFullPath)
                {
                    Verb = "open",
                    UseShellExecute = true,
                });
            }
            catch (Exception)
            {
                // If we can't do this for whatever reason, do nothing.
                Logger.Error("Unable to open log file automatically.");
            }

            // Show the error message box.
            var mbs = Services.GetRequiredService<IMessageBoxService>();
            if (mbs != null)
            {
                var mbMessage =
                    $"An unhandled exception occured.\n\n" +
                    $"The application will be terminated. If the log file wasn't opened automatically, " +
                    $"it can be found under the following path:\n\n{ApplicationConfig.LogFileFullPath}";

                mbs.Show(
                    mbMessage,
                    "An unhandled exception occured",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error,
                    MessageBoxResult.OK,
                    MessageBoxOptions.DefaultDesktopOnly);
            }

            Shutdown();
#endif
        }

        #endregion Methods
    }
}