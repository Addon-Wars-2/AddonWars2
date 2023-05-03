// ==================================================================================================
// <copyright file="AddonDownloaderBase.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons.Downloader
{
    using System.Threading.Tasks;
    using AddonWars2.Addons.Downloader.Interfaces;

    /// <summary>
    /// Represents a base class for addon downloaders.
    /// </summary>
    public abstract class AddonDownloaderBase : IAddonDownloader
    {
        /// <inheritdoc/>
        public abstract Task<IDownloadedAddon> Download(IDownloadRequest request);
    }
}
