// ==================================================================================================
// <copyright file="LoggingViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels
{
    using System.Collections.ObjectModel;
    using AddonWars2.App.Models.Logging;
    using AddonWars2.App.Services;
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
        /// <param name="loggingManager">A rerefence to a <see cref="LoggingService"/> object.</param>
        public LoggingViewModel(
            ILogger<BaseViewModel> logger,
            LoggingService loggingManager)
            : base(logger)
        {
            LoggingManagerInstance = loggingManager;

            Logger.LogDebug("Instance initialized.");
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a reference to <see cref="LoggingService"/> service.
        /// </summary>
        public LoggingService LoggingManagerInstance { get; private set; }

        /// <summary>
        /// Gets a collection of log entries.
        /// </summary>
        public ObservableCollection<LogEntry> LogEntries => LoggingManagerInstance.LogEntries;

        #endregion Properties
    }
}
