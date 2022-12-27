// ==================================================================================================
// <copyright file="LogEntry.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Models.Logging
{
    /// <summary>
    /// Represents a single log entry.
    /// </summary>
    public readonly struct LogEntry
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntry"/> struct.
        /// </summary>
        public LogEntry()
            : this(string.Empty)
        {
            // Blank.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntry"/> struct.
        /// </summary>
        /// <param name="message">Log entry message.</param>
        public LogEntry(string message)
        {
            Message = message;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the log message.
        /// </summary>
        public readonly string Message { get; }

        #endregion Properties
    }
}
