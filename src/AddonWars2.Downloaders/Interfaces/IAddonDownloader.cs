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
        /// Starts to download the requested addon.
        /// </summary>
        /// <param name="request">A request objects which wraps the request information.</param>
        /// <returns><see cref="DownloadedObject"/> object.</returns>
        Task<DownloadedObject> Download(DownloadRequest request);
    }
}
