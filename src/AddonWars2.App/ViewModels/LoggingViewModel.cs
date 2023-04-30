// ==================================================================================================
// <copyright file="LoggingViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using AddonWars2.App.Models.Application;
    using AddonWars2.App.Models.Logging;
    using AddonWars2.App.Services;
    using CommunityToolkit.Mvvm.Input;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// View model used by logging view.
    /// </summary>
    public class LoggingViewModel : BaseViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingViewModel"/> class.
        /// </summary>
        /// <param name="logger">A referemnce to <see cref="ILogger"/>.</param>
        /// <param name="loggingService">A rerefence to a <see cref="LoggingService"/> object.</param>
        /// <param name="appConfig">A reference to <see cref="AppConfig"/>.</param>
        public LoggingViewModel(
            ILogger<BaseViewModel> logger,
            LoggingService loggingService,
            ApplicationConfig appConfig)
            : base(logger)
        {
            LoggingServiceInstance = loggingService;
            AppConfig = appConfig;

            OpenLogFileCommand = new RelayCommand(ExecuteOpenLogFileCommand);

            Logger?.LogDebug("Instance initialized.");
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a reference to the application config.
        /// </summary>
        public ApplicationConfig AppConfig { get; private set; }

        /// <summary>
        /// Gets a reference to <see cref="LoggingService"/> service.
        /// </summary>
        public LoggingService LoggingServiceInstance { get; private set; }

        /// <summary>
        /// Gets a collection of log entries.
        /// </summary>
        public ObservableCollection<ILogEntry> LogEntries => LoggingServiceInstance.LogEntries;

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets a command that opens the current log file.
        /// </summary>
        public RelayCommand OpenLogFileCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        // OpenLogFileCommand command logic.
        private void ExecuteOpenLogFileCommand()
        {
            try
            {
                Process.Start(new ProcessStartInfo(AppConfig.LogFileFullPath)
                {
                    Verb = "open",
                    UseShellExecute = true,
                });
            }
            catch (Exception)
            {
                return;
            }
        }

        #endregion Commands Logic
    }
}
