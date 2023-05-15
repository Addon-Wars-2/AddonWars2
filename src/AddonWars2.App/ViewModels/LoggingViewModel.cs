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
    using System.Windows;
    using AddonWars2.App.Models.Application;
    using AddonWars2.App.Models.Logging;
    using AddonWars2.App.Services.Interfaces;
    using CommunityToolkit.Mvvm.Input;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// View model used by logging view.
    /// </summary>
    public class LoggingViewModel : BaseViewModel
    {
        #region Fields

        private readonly ApplicationConfig _applicationConfig;
        private readonly ILogsAggregator _logsAggregator;
        private readonly IMessageBoxService _messageBoxService;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingViewModel"/> class.
        /// </summary>
        /// <param name="logger">A referemnce to <see cref="ILogger"/> instance.</param>
        /// <param name="logsAggregator">A rerefence to a <see cref="ILogsAggregator"/> instance.</param>
        /// <param name="appConfig">A reference to <see cref="ApplicationConfig"/> instance.</param>
        /// <param name="messageBoxService">A reference to <see cref="IMessageBoxService"/> instance.</param>
        public LoggingViewModel(
            ILogger<LoggingViewModel> logger,
            ILogsAggregator logsAggregator,
            ApplicationConfig appConfig,
            IMessageBoxService messageBoxService)
            : base(logger)
        {
            _logsAggregator = logsAggregator ?? throw new ArgumentNullException(nameof(logsAggregator));
            _applicationConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));

            OpenLogFileCommand = new RelayCommand(ExecuteOpenLogFileCommand);

            Logger.LogDebug("Instance initialized.");
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a reference to the application config.
        /// </summary>
        public ApplicationConfig AppConfig => _applicationConfig;

        /// <summary>
        /// Gets a reference to <see cref="ILogsAggregator"/> service.
        /// </summary>
        public ILogsAggregator LogsAggregator => _logsAggregator;

        /// <summary>
        /// Gets a reference to <see cref="IMessageBoxService"/> service.
        /// </summary>
        public IMessageBoxService MessageBoxService => _messageBoxService;

        /// <summary>
        /// Gets a collection of log entries.
        /// </summary>
        public ObservableCollection<LogEntry> LogEntries => LogsAggregator.LogEntries;

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
                Process.Start(new ProcessStartInfo(AppConfig.LogFilePath)
                {
                    Verb = "open",
                    UseShellExecute = true,
                });
            }
            catch (Exception e)
            {
                MessageBoxService.Show(
                    e.Message,
                    "Failed to open log file",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Logger.LogError($"Failed to open log file:\n{e.Message}");
            }
        }

        #endregion Commands Logic
    }
}
