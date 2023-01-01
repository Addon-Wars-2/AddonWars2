// ==================================================================================================
// <copyright file="ApplicationConfig.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Models.Application
{
    using System;
    using System.IO;

    /// <summary>
    /// Holds some globally available application information.
    /// </summary>
    public class ApplicationConfig
    {
        /// <summary>
        /// Gets or sets the application startup date and time.
        /// </summary>
        public DateTime StartupDateTime { get; set; }

        /// <summary>
        /// Gets or sets the application %APPDATA% dir.
        /// </summary>
        public string AppDataDir { get; set; }

        /// <summary>
        /// Gets the application log file prefix.
        /// </summary>
        public string LogPrefix => "aw2_log_";

        /// <summary>
        /// Gets the application config file name.
        /// </summary>
        public string ConfigFileName => "aw2_config.xml";

        /// <summary>
        /// Gets the application config file path.
        /// </summary>
        public string ConfigFilePath => Path.Join(AppDataDir, ConfigFileName);

        /// <summary>
        /// Gets or sets the current application-wide settings.
        /// </summary>
        public UserData UserData { get; set; }
    }
}
