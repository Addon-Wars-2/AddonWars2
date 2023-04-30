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
        #region Fields

        private string _title = string.Empty;
        private string _link = string.Empty;
        private DateTime _publishDate;
        private string _guid = string.Empty;
        private string _description = string.Empty;
        private string _contentEncoded = string.Empty;
        private bool _isSticky = false;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the RSS item title.
        /// </summary>
        public string Title
        {
            get => _title;
            set => _title = value;
        }

        /// <summary>
        /// Gets or sets the RSS item link (URL).
        /// </summary>
        public string Link
        {
            get => _link;
            set => _link = value;
        }

        /// <summary>
        /// Gets or sets the RSS item publish date.
        /// </summary>
        public DateTime PublishDate
        {
            get => _publishDate;
            set => _publishDate = value;
        }

        /// <summary>
        /// Gets or sets the RSS item unique GUID obtained
        /// from a unique URL in ANet RSS feed.
        /// </summary>
        public string Guid
        {
            get => _guid;
            set => _guid = value;
        }

        /// <summary>
        /// Gets or sets the RSS item description.
        /// </summary>
        public string Description
        {
            get => _description;
            set => _description = value;
        }

        /// <summary>
        /// Gets or sets the RSS item content in HTML format.
        /// </summary>
        public string ContentEncoded
        {
            get => _contentEncoded;
            set => _contentEncoded = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the RSS item is "sticky"
        /// (and thus should appear above all other items).
        /// </summary>
        public bool IsSticky
        {
            get => _isSticky;
            set => _isSticky = value;
        }

        #endregion Properties
    }
}
