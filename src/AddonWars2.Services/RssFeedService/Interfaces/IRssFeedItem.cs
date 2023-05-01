// ==================================================================================================
// <copyright file="IRssFeedItem.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Services.RssFeedService.Interfaces
{
    /// <summary>
    /// Represents a contract for RSS feed items.
    /// </summary>
    /// <remarks>
    /// See <see href="https://validator.w3.org/feed/docs/rss2.html"/> for additional details.
    /// </remarks>
    public interface IRssFeedItem
    {
        /// <summary>
        /// Gets or sets the name of the channel.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the channel description.
        /// </summary>
        public string Description { get; set; }
    }
}
