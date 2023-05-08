// ==================================================================================================
// <copyright file="AppStaticData.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.SharedData
{
    using System.Collections.Immutable;
    using AddonWars2.SharedData.Entities;

    /// <summary>
    /// Encapsulates the application static data.
    /// </summary>
    public static class AppStaticData
    {
        /// <summary>
        /// Gets the application log directory name.
        /// </summary>
        public const string LOG_DIR_NAME = "logs";

        /// <summary>
        /// Gets the application log file prefix (normal mode).
        /// </summary>
        public const string LOG_FILE_PREFIX = "aw2_log_";

        /// <summary>
        /// Gets the application log file prefix (debug mode).
        /// </summary>
        public const string LOG_FILE_PREFIX_DEBUG = "debug_aw2_log_";

        /// <summary>
        /// Gets the application config file name.
        /// </summary>
        public const string CONFIG_FILE_NAME = "config.xml";

        /// <summary>
        /// Gets the directory name used to store GW2 RSS feed pages in HTML format.
        /// </summary>
        public const string RSS_FEED_DIR_NAME = "rss";

        /// <summary>
        /// Gets a default culture.
        /// </summary>
        public static readonly CultureInfo DEFAULT_CULTURE = new CultureInfo("en-US", "EN", "English");

        private static readonly ImmutableList<CultureInfo> _appSupportedCultures = ImmutableList.Create(
            new CultureInfo("en-US", "EN", "English"),
            new CultureInfo("ru-RU", "RU", "Русский"));

        private static readonly ImmutableList<CultureInfo> _anetSupportedCultures = ImmutableList.Create(
            new CultureInfo("en-US", "EN", "English"),
            new CultureInfo("es-ES", "ES", "Español"),
            new CultureInfo("de-DE", "DE", "Deutsch"),
            new CultureInfo("fr-FR", "FR", "Français"));

        /// <summary>
        /// Gets a list of available cultures.
        /// </summary>
        public static ImmutableList<CultureInfo> APP_SUPPORTED_CULTURES => _appSupportedCultures;

        /// <summary>
        /// Gets a list of cultures supported by ArenaNet services (web and the game).
        /// </summary>
        public static ImmutableList<CultureInfo> ANET_SUPPORTED_CULTURES => _anetSupportedCultures;
    }
}
