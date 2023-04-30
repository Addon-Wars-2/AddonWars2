// ==================================================================================================
// <copyright file="RssFeedItem.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Models.GuildWars2
{
    using System;

    /// <summary>
    /// Represents a single GW2 RSS feed item.
    /// </summary>
    public class RssFeedItem
    {
        /// <summary>
        /// Gets or sets the RSS item title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the RSS item link (URL).
        /// </summary>
        public string? Link { get; set; }

        /// <summary>
        /// Gets or sets the RSS item publish date.
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// Gets or sets the RSS item unique GUID obtained
        /// from a unique URL in ANet RSS feed.
        /// </summary>
        public string? Guid { get; set; }

        /// <summary>
        /// Gets or sets the RSS item description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the RSS item content in HTML format.
        /// </summary>
        public string? ContentEncoded { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the RSS item is "sticky"
        /// (and thus should appear above all other items).
        /// </summary>
        public bool IsSticky { get; set; }
    }
}
