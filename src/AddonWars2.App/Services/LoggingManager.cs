// ==================================================================================================
// <copyright file="LoggingManager.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Services
{
    using System.Collections.ObjectModel;
    using AddonWars2.App.Models.Logging;

    /// <summary>
    /// Provides service methods for application logging.
    /// </summary>
    public class LoggingManager
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingManager"/> class.
        /// </summary>
        public LoggingManager()
        {
            LogEntries = new ObservableCollection<LogEntry>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a collection of log entries.
        /// </summary>
        public ObservableCollection<LogEntry> LogEntries { get; private set; }

        #endregion Properties
    }
}
