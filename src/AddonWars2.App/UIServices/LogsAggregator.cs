﻿// ==================================================================================================
// <copyright file="LogsAggregator.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.UIServices
{
    using System.Collections.ObjectModel;
    using AddonWars2.App.Logging;
    using AddonWars2.App.UIServices.Interfaces;

    /// <summary>
    /// Represents log messages aggregator.
    /// </summary>
    public class LogsAggregator : ILogsAggregator
    {
        #region Fields

        private readonly ObservableCollection<LogEntry> _logEntries;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogsAggregator"/> class.
        /// </summary>
        public LogsAggregator()
        {
            _logEntries = new ObservableCollection<LogEntry>();
        }

        #endregion Constructors

        #region Properties

        /// <inheritdoc/>
        public ObservableCollection<LogEntry> LogEntries => _logEntries;

        #endregion Properties
    }
}
