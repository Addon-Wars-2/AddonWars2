// ==================================================================================================
// <copyright file="Gw2WebInfo.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Models.GuildWars2
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents a pre-defined information about GW2 web links and services.
    /// </summary>
    [Serializable]
    public class Gw2WebInfo
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Gw2WebInfo"/> class.
        /// </summary>
        public Gw2WebInfo()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the ArenaNet homepage URL.
        /// </summary>
        [XmlElement("anethome")]
        public string AnetHome { get; set; } = "https://arena.net";

        /// <summary>
        /// Gets or sets the GW2 homepage URL.
        /// </summary>
        [XmlElement("gw2home")]
        public string Gw2Home { get; set; } = "https://guildwars2.com";

        /// <summary>
        /// Gets or sets the GW2 wiki homepage URL.
        /// </summary>
        [XmlElement("gw2wiki")]
        public string Gw2WikiHome { get; set; } = "https://wiki.guildwars2.com/wiki/Main_Page";

        /// <summary>
        /// Gets or sets the GW2 RSS feed url.
        /// </summary>
        [XmlElement("gw2rss")]
        public string Gw2Rss { get; set; } = "https://www.guildwars2.com/en/feed";

        /// <summary>
        /// Gets or sets the GW2 APIv2 endpoint url.
        /// </summary>
        [XmlElement("gw2api2")]
        public string Gw2Api2 { get; set; } = "https://api.guildwars2.com/v2";

        #endregion Properties
    }
}
