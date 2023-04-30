// ==================================================================================================
// <copyright file="ILoggingService.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Services.Interfaces
{
    using System.Collections.ObjectModel;
    using AddonWars2.App.Models.Logging;

    /// <summary>
    /// Represents a contract for logging services.
    /// </summary>
    public interface ILoggingService
    {
        /// <summary>
        /// Gets a collection of log entries.
        /// </summary>
        public ObservableCollection<ILogEntry> LogEntries { get; }
    }
}
