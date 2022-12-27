// ==================================================================================================
// <copyright file="ApplicationGlobal.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Models.Application
{
    using System;
    using System.IO;

    /// <summary>
    /// Holds some global application information.
    /// </summary>
    public static class ApplicationGlobal
    {
        /// <summary>
        /// Gets or sets the application startup date and time.
        /// </summary>
        internal static DateTime StartupDateTime { get; set; }

        /// <summary>
        /// Gets or sets the application %APPDATA% dir.
        /// </summary>
        internal static string AppDataDir { get; set; }

        /// <summary>
        /// Gets the application log file prefix.
        /// </summary>
        internal static string LogPrefix => "aw2_log_";

        /// <summary>
        /// Gets the application config file name.
        /// </summary>
        internal static string ConfigFileName => "aw2_config.xml";

        /// <summary>
        /// Gets the application config file path.
        /// </summary>
        internal static string ConfigFilePath => Path.Join(AppDataDir, ConfigFileName);

        /// <summary>
        /// Gets or sets the current application-wide settings.
        /// </summary>
        internal static ApplicationConfig AppConfig { get; set; }
    }
}
