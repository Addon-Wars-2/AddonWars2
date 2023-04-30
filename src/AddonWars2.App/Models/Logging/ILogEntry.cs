// ==================================================================================================
// <copyright file="ILogEntry.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Models.Logging
{
    /// <summary>
    /// Represents a contract for the application log entry object.
    /// </summary>
    public interface ILogEntry
    {
        /// <summary>
        /// Gets the log message.
        /// </summary>
        public string? Message { get; }
    }
}
