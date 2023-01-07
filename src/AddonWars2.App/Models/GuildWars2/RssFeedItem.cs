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
        public string Title { get; set; }

        public string Link { get; set; }

        public DateTime PublishDate { get; set; }

        public string Guid { get; set; }

        public string Description { get; set; }

        public string ContentEncoded { get; set; }

        public bool IsSticky { get; set; }
    }
}
