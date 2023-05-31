// ==================================================================================================
// <copyright file="ILogsAggregator.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.UIServices.Interfaces
{
    using System.Collections.ObjectModel;
    using AddonWars2.App.Logging;

    /// <summary>
    /// Represents a contract for log messages aggregator.
    /// </summary>
    public interface ILogsAggregator
    {
        /// <summary>
        /// Gets a collection of log entries.
        /// </summary>
        public ObservableCollection<LogEntry> LogEntries { get; }
    }
}
