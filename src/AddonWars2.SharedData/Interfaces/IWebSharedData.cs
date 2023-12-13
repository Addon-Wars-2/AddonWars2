// ==================================================================================================
// <copyright file="IWebSharedData.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.SharedData.Interfaces
{
    /// <summary>
    /// Represents a contract with web-related static data.
    /// </summary>
    public interface IWebSharedData
    {
        #region Fields and Constants

        #region GitHub

        private const string GITHUB_PROJ_REPO_URL = "https://github.com/Addon-Wars-2/AddonWars2";
        private const long GITHUB_PROJ_REPO_ID = 582041038;
        private const string GITHUB_PROJ_WIKI_URL = "https://github.com/Addon-Wars-2/AddonWars2/wiki";

        private const string GITHUB_ADDONS_LIB_URL = "https://github.com/Addon-Wars-2/AddonsLib";
        private const long GITHUB_ADDONS_LIB_REPO_ID = 590107405;

        private const string REG_PROVIDERS_FILENAME = "registry-providers.json";

        #endregion GitHub

        #region ArenaNet

        private const string ANET_HOME = "https://arena.net";
        private const string ANET_HOME_TEMPLATE = "https://arena.net/{0}";

        #endregion ArenaNet

        #region GuildWars2

        private const string GW2_HOME = "https://guildwars2.com";
        private const string GW2_HOME_TEMPLATE = "https://guildwars2.com/{0}";
        private const string GW2_WIKI_HOME = "https://wiki.guildwars2.com";
        private const string GW2_WIKI_HOME_TEMPLATE = "https://wiki-{0}.guildwars2.com";
        private const string GW2_RSS_HOME = "https://www.guildwars2.com/feed";
        private const string GW2_RSS_HOME_TEMPLATE = "https://www.guildwars2.com/{0}/feed";

        #endregion GuildWars2

        #endregion Fields and Constants

        #region Properties

        /// <summary>
        /// Gets the project repository URL (GitHub).
        /// </summary>
        public string GitHubProjectRepositoryUrl => GITHUB_PROJ_REPO_URL;

        /// <summary>
        /// Gets the project repository ID (GitHub).
        /// </summary>
        public long GitHubProjectRepositoryId => GITHUB_PROJ_REPO_ID;

        /// <summary>
        /// Gets the addons lib repository URL (GitHub).
        /// </summary>
        public string GitHubAddonsLibRepositoryUrl => GITHUB_ADDONS_LIB_URL;

        /// <summary>
        /// Gets the addons lib repository ID (GitHub).
        /// </summary>
        public long GitHubAddonsLibRepositoryId => GITHUB_ADDONS_LIB_REPO_ID;

        /// <summary>
        /// Gets a path to the list of registry providers.
        /// </summary>
        public string RegistryProvidersFileName => REG_PROVIDERS_FILENAME;

        /// <summary>
        /// Gets the project wiki URL (GitHub).
        /// </summary>
        public string GitHubProjectWikiUrl => GITHUB_PROJ_WIKI_URL;

        /// <summary>
        /// Gets the ArenaNet website URL.
        /// </summary>
        public string AnetHome => ANET_HOME;

        /// <summary>
        /// Gets the ArenaNet website URL template.
        /// </summary>
        public string AnetHomeTemplate => ANET_HOME_TEMPLATE;

        /// <summary>
        /// Gets the Guild Wars 2 website URL.
        /// </summary>
        public string Gw2Home => GW2_HOME;

        /// <summary>
        /// Gets the Guild Wars 2 website URL template.
        /// </summary>
        public string Gw2HomeTemplate => GW2_HOME_TEMPLATE;

        /// <summary>
        /// Gets the Guild Wars 2 wiki URL.
        /// </summary>
        public string Gw2WikiHome => GW2_WIKI_HOME;

        /// <summary>
        /// Gets the Guild Wars 2 wiki URL template.
        /// </summary>
        public string Gw2WikiHomeTemplate => GW2_WIKI_HOME_TEMPLATE;

        /// <summary>
        /// Gets the Guild Wars 2 RSS URL.
        /// </summary>
        public string Gw2RssHome => GW2_RSS_HOME;

        /// <summary>
        /// Gets the Guild Wars 2 RSS URL template.
        /// </summary>
        public string Gw2RssHomeTemplate => GW2_RSS_HOME_TEMPLATE;

        #endregion Properties
    }
}
