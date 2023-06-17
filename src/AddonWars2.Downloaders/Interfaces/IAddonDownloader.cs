// ==================================================================================================
// <copyright file="IAddonDownloader.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloaders.Interfaces
{
    using AddonWars2.Downloaders.Models;

    /// <summary>
    /// Provides a contract for addon downloaders.
    /// </summary>
    public interface IAddonDownloader
    {
        /// <summary>
        /// Is raised whenever the download progress has changed.
        /// </summary>
        public event DownloadProgressChangedEventHandler? DownloadProgressChanged;

        /// <summary>
        /// Starts to download the requested addon.
        /// </summary>
        /// <param name="url">URL of a file to download.</param>
        /// <returns><see cref="DownloadedObject"/> object.</returns>
        public Task<DownloadedObject> DownloadAsync(string url);
    }
}
