// ==================================================================================================
// <copyright file="IRssFeedService.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Services.RssFeedService.Interfaces
{
    /// <summary>
    /// Represents a contract for RSS feed services.
    /// </summary>
    /// <typeparam name="T"><see cref="IRssFeedItem"/> type.</typeparam>
    public interface IRssFeedService<T>
        where T : class, IRssFeedItem, new()
    {
        /// <summary>
        /// Asynchronously reads an XML stream representing an RSS feed
        /// and returns as a collection of <see cref="T"/>.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object of an RSS XML.</param>
        /// <returns>A collection of <see cref="T"/> objects.</returns>
        public Task<IList<T>> ReadXmlStreamAsync(Stream stream);

        /// <summary>
        /// Asynchronously writes RSS item to a file.
        /// </summary>
        /// <param name="item"><see cref="T"/> object to be written.</param>
        /// <param name="filename">The full file path.</param>
        /// <returns><see cref="Task"/> object.</returns>
        public Task WriteRssItemAsync(T item, string filename);
    }
}
