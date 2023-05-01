// ==================================================================================================
// <copyright file="RssFeedItem.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Services.RssFeedService.Models
{
    using AddonWars2.Services.RssFeedService.Interfaces;

    /// <summary>
    /// Represents a class for RSS feed items.
    /// </summary>
    public class RssFeedItem : IRssFeedItem
    {
        #region Fields

        private string _title = string.Empty;
        private string _description = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RssFeedItem"/> class.
        /// </summary>
        public RssFeedItem()
        {
            // Blank.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RssFeedItem"/> class.
        /// </summary>
        /// <param name="title">The name of the channel.</param>
        /// <param name="description">The channel description.</param>
        public RssFeedItem(string title, string description)
        {
            _title = title;
            _description = description;
        }

        #endregion Constrcutors

        #region Properties

        /// <inheritdoc/>
        public string Title
        {
            get => _title;
            set => _title = value;
        }

        /// <inheritdoc/>
        public string Description
        {
            get => _description;
            set => _description = value;
        }

        #endregion Properties
    }
}
