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
    using AddonWars2.App.Configuration;
    using AddonWars2.App.Logging;
    using AddonWars2.App.UIServices.Enums;
    using AddonWars2.App.UIServices.Interfaces;
    using AddonWars2.App.Utils.Helpers;
    using AddonWars2.App.ViewModels.Factories;
    using CommunityToolkit.Mvvm.Input;
    using Microsoft.Extensions.Logging;
    using MvvmDialogs;

    /// <summary>
    /// View model used by logging view.
    /// </summary>
    public class LoggingViewModel : BaseViewModel
    {
        #region Fields

        private static readonly string _failedOpenLogFileErrorTitle = ResourcesHelper.GetApplicationResource<string>("S.LoggingPage.Errors.FailedOpenLogFile.Title");
        private static readonly string _failedOpenLogFileErrorMessage = ResourcesHelper.GetApplicationResource<string>("S.LoggingPage.Errors.FailedOpenLogFile.Message");

        private readonly IDialogService _dialogService;
        private readonly IErrorDialogViewModelFactory _errorDialogViewModelFactory;
        private readonly IApplicationConfig _applicationConfig;
        private readonly ILogsAggregator _logsAggregator;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingViewModel"/> class.
        /// </summary>
        /// <param name="logger">A referemnce to <see cref="ILogger"/>.</param>
        /// <param name="dialogService">A reference to <see cref="IDialogService"/>.</param>
        /// <param name="errorDialogViewModelFactory">A reference to <see cref="IErrorDialogViewModelFactory"/>.</param>
        /// <param name="logsAggregator">A rerefence to a <see cref="ILogsAggregator"/>.</param>
        /// <param name="appConfig">A reference to <see cref="IApplicationConfig"/>.</param>
        public LoggingViewModel(
            ILogger<LoggingViewModel> logger,
            IDialogService dialogService,
            IErrorDialogViewModelFactory errorDialogViewModelFactory,
            ILogsAggregator logsAggregator,
            IApplicationConfig appConfig)
            : base(logger)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _errorDialogViewModelFactory = errorDialogViewModelFactory ?? throw new ArgumentNullException(nameof(errorDialogViewModelFactory));
            _logsAggregator = logsAggregator ?? throw new ArgumentNullException(nameof(logsAggregator));
            _applicationConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));

            OpenLogFileCommand = new RelayCommand(ExecuteOpenLogFileCommand);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a reference to <see cref="IDialogService"/> service.
        /// </summary>
        public IDialogService DialogService => _dialogService;

        /// <summary>
        /// Gets a reference to the error dialog factory.
        /// </summary>
        public IErrorDialogViewModelFactory ErrorDialogViewModelFactory => _errorDialogViewModelFactory;

        /// <summary>
        /// Gets a reference to the application config.
        /// </summary>
        public IApplicationConfig AppConfig => _applicationConfig;

        /// <summary>
        /// Gets a reference to <see cref="ILogsAggregator"/> service.
        /// </summary>
        public ILogsAggregator LogsAggregator => _logsAggregator;

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
                Process.Start(new ProcessStartInfo(AppConfig.SessionData.LogFilePath)
                {
                    Verb = "open",
                    UseShellExecute = true,
                });
            }
            catch (Exception e)
            {
                Logger.LogError($"Failed to open log file:\n{e.Message}");

                ShowErrorDialog(_failedOpenLogFileErrorTitle, _failedOpenLogFileErrorMessage, e.Message);
            }
        }

        #endregion Commands Logic

        #region Methods

        // Shows error dialog.
        private bool? ShowErrorDialog(string title, string message, string? details = null, ErrorDialogButtons buttons = ErrorDialogButtons.OK)
        {
            var vm = ErrorDialogViewModelFactory.Create(title, message, details, buttons);
            var result = DialogService.ShowDialog(this, vm);

            return result;
        }

        #endregion Methods
    }
}
