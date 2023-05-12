﻿// ==================================================================================================
// <copyright file="IWebStaticData.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.SharedData
{
    /// <summary>
    /// Represents a contract with web-related static data.
    /// </summary>
    public interface IWebStaticData
    {
        #region Fields and Constants

        #region GitHub

        private const string _gitHubProjectRepositoryUrl = "https://github.com/Addon-Wars-2/AddonWars2";
        private const long _gitHubProjectRepositoryId = 582041038;
        private const string _gitHubProjectApprovedProviders = @"approved\registry-providers.json";
        private const string _gitHubProjectWikiUrl = "https://github.com/Addon-Wars-2/AddonWars2/wiki";

        #endregion GitHub

        #region ArenaNet

        private const string _anetHome = "https://arena.net";
        private const string _anetHomeTemplate = "https://arena.net/{0}";

        #endregion ArenaNet

        #region GuildWars2

        private const string _gw2Home = "https://guildwars2.com";
        private const string _gw2HomeTemplate = "https://guildwars2.com/{0}";
        private const string _gw2WikiHome = "https://wiki.guildwars2.com";
        private const string _gw2WikiHomeTemplate = "https://wiki-{0}.guildwars2.com";
        private const string _gw2RssHome = "https://www.guildwars2.com/feed";
        private const string _gw2RssHomeTemplate = "https://www.guildwars2.com/{0}/feed";
        private const string _gw2ApiV2Endpoint = "https://api.guildwars2.com/v2";

        /// <summary>
        /// Gets the project repository URL (GitHub).
        /// </summary>
        public string GitHubProjectRepositoryUrl => _gitHubProjectRepositoryUrl;

        /// <summary>
        /// Gets the project repository ID (GitHub).
        /// </summary>
        public long GitHubProjectRepositoryId => _gitHubProjectRepositoryId;

        /// <summary>
        /// Gets a path to the list of approved registry providers.
        /// </summary>
        public string GitHubProjectApprovedProviders => _gitHubProjectApprovedProviders;

        /// <summary>
        /// Gets the project wiki URL (GitHub).
        /// </summary>
        public string GitHubProjectWikiUrl => _gitHubProjectWikiUrl;

        /// <summary>
        /// Gets the ArenaNet website URL.
        /// </summary>
        public string AnetHome => _anetHome;

        /// <summary>
        /// Gets the ArenaNet website URL template.
        /// </summary>
        public string AnetHomeTemplate => _anetHomeTemplate;

        /// <summary>
        /// Gets the Guild Wars 2 website URL.
        /// </summary>
        public string Gw2Home => _gw2Home;

        /// <summary>
        /// Gets the Guild Wars 2 website URL template.
        /// </summary>
        public string Gw2HomeTemplate => _gw2HomeTemplate;

        /// <summary>
        /// Gets the Guild Wars 2 wiki URL.
        /// </summary>
        public string Gw2WikiHome => _gw2WikiHome;

        /// <summary>
        /// Gets the Guild Wars 2 wiki URL template.
        /// </summary>
        public string Gw2WikiHomeTemplate => _gw2WikiHomeTemplate;

        /// <summary>
        /// Gets the Guild Wars 2 RSS URL.
        /// </summary>
        public string Gw2RssHome => _gw2RssHome;

        /// <summary>
        /// Gets the Guild Wars 2 RSS URL template.
        /// </summary>
        public string Gw2RssHomeTemplate => _gw2RssHomeTemplate;

        /// <summary>
        /// Gets the Guild Wars 2 API v2 endpoint URL.
        /// </summary>
        public string Gw2ApiV2Endpoint => _gw2ApiV2Endpoint;

        #endregion GuildWars2

        #endregion Fields and Constants
    }
}
