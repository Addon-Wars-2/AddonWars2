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
    public readonly struct LogEntry : ILogEntry
    {
        #region Fields

        private readonly string _message;

        #endregion Fields

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
            _message = message;
        }

        #endregion Constructors

        #region Properties

        /// <inheritdoc/>
        public readonly string? Message => _message;

        #endregion Properties
    }
}
