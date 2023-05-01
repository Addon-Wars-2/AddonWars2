// ==================================================================================================
// <copyright file="Gw2RssFeedItem.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Services.RssFeedService.Models
{
    /// <summary>
    /// Represents a Guild Wars 2 RSS feed item.
    /// </summary>
    public class Gw2RssFeedItem : RssFeedItem
    {
        #region Fields

        private string _link = string.Empty;
        private DateTime _publishDate = DateTime.UtcNow;
        private string _guid = string.Empty;
        private string _contentEncoded = string.Empty;
        private bool _isSticky = false;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Gw2RssFeedItem"/> class.
        /// </summary>
        public Gw2RssFeedItem()
        {
            // Blank.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Gw2RssFeedItem"/> class.
        /// </summary>
        /// <param name="title">The name of the channel.</param>
        /// <param name="link">The URL to the HTML website corresponding to the channel.</param>
        /// <param name="description">The channel description.</param>
        /// <param name="publishDate">The publication date.</param>
        /// <param name="guid">A unique identifier.</param>
        /// <param name="contentEncoded">The encoded content.</param>
        /// <param name="isSticky">Indicates whether this item should appear above all the others.</param>
        public Gw2RssFeedItem(
            string title,
            string link,
            string description,
            DateTime publishDate,
            string guid,
            string contentEncoded,
            bool isSticky = false)
            : base(title, description)
        {
            _link = link;
            _publishDate = publishDate;
            _guid = guid;
            _contentEncoded = contentEncoded;
            _isSticky = isSticky;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the URL to the HTML website corresponding to the channel.
        /// </summary>
        public string Link
        {
            get => _link;
            set => _link = value;
        }

        /// <summary>
        /// Gets or sets the publication date of the content.
        /// </summary>
        public DateTime PublishDate
        {
            get => _publishDate;
            set => _publishDate = value;
        }

        /// <summary>
        /// Gets or sets a unique identifier of the content.
        /// </summary>
        public string Guid
        {
            get => _guid;
            set => _guid = value;
        }

        /// <summary>
        /// Gets or sets the encoded content.
        /// </summary>
        public string ContentEncoded
        {
            get => _contentEncoded;
            set => _contentEncoded = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this content is "sticky"
        /// and should appear above all the other items.
        /// </summary>
        public bool IsSticky
        {
            get => _isSticky;
            set => _isSticky = value;
        }

        #endregion Properties
    }
}
