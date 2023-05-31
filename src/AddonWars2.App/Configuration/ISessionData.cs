// ==================================================================================================
// <copyright file="ISessionData.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Configuration
{
    using System.ComponentModel;
    using Config.Net;

    /// <summary>
    /// Represents a contract for the last session data.
    /// </summary>
    public interface ISessionData : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets a value indicating whether the application was executed
        /// in debug mode during the the recent session.
        /// </summary>
        [Option(Alias = "DebugMode", DefaultValue = false)]
        public bool IsDebugMode { get; set; }

        /// <summary>
        /// Gets or sets the application data directory used diring the recent session.
        /// </summary>
        [Option(Alias = "AppDataDir", DefaultValue = "")]
        public string AppDataDir { get; set; }

        /// <summary>
        /// Gets or sets the application log filepath used diring the recent session.
        /// </summary>
        [Option(Alias = "LogFilePath", DefaultValue = "")]
        public string LogFilePath { get; set; }
    }
}
