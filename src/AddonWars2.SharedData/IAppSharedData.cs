﻿// ==================================================================================================
// <copyright file="IAppSharedData.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.SharedData
{
    using System.Collections.Immutable;
    using AddonWars2.SharedData.Entities;

    /// <summary>
    /// Represents a contact for Application static data implementations.
    /// </summary>
    public interface IAppSharedData
    {
        #region Fields and Constants

        private const string LOG_DIR_NAME = "logs";
        private const string LOG_FILE_PREFIX = "aw2_log_";
        private const string LOG_FILE_PREFIX_DEBUG = "debug_aw2_log_";
        private const string CONFIG_FILE_NAME = "config.xml";
        private const string RSS_FEED_DIR_NAME = "rss";

        private static readonly CultureInfo _defaultCulture = new CultureInfo("en-US", "EN", "English");

        private static readonly ImmutableList<CultureInfo> _appSupportedCultures = ImmutableList.Create(
            new CultureInfo("en-US", "EN", "English"),
            new CultureInfo("ru-RU", "RU", "Русский"));

        private static readonly ImmutableList<CultureInfo> _anetSupportedCultures = ImmutableList.Create(
            new CultureInfo("en-US", "EN", "English"),
            new CultureInfo("es-ES", "ES", "Español"),
            new CultureInfo("de-DE", "DE", "Deutsch"),
            new CultureInfo("fr-FR", "FR", "Français"));

        #endregion Fields and Constants

        #region Properties

        /// <summary>
        /// Gets the application log directory name.
        /// </summary>
        public string LogDirName => LOG_DIR_NAME;

        /// <summary>
        /// Gets the application log file prefix (normal mode).
        /// </summary>
        public string LogFilePrefix => LOG_FILE_PREFIX;

        /// <summary>
        /// Gets the application log file prefix (debug mode).
        /// </summary>
        public string LogFilePrefixDebug => LOG_FILE_PREFIX_DEBUG;

        /// <summary>
        /// Gets the application config file name.
        /// </summary>
        public string ConfigFileName => CONFIG_FILE_NAME;

        /// <summary>
        /// Gets the directory name used to store GW2 RSS feed pages in HTML format.
        /// </summary>
        public string RssFeedDirName => RSS_FEED_DIR_NAME;

        /// <summary>
        /// Gets a default culture.
        /// </summary>
        public CultureInfo DefaultCulture => _defaultCulture;

        /// <summary>
        /// Gets a list of available cultures.
        /// </summary>
        public ImmutableList<CultureInfo> AppSupportedCultures => _appSupportedCultures;

        /// <summary>
        /// Gets a list of cultures supported by ArenaNet services (web and the game).
        /// </summary>
        public ImmutableList<CultureInfo> AnetSupportedCultures => _anetSupportedCultures;

        #endregion Properties
    }
}