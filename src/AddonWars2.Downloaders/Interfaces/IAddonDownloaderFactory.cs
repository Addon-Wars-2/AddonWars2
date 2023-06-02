// ==================================================================================================
// <copyright file="IAddonDownloaderFactory.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloaders.Interfaces
{
    using AddonWars2.Core.Enums;

    /// <summary>
    /// Represents a contact for addon downloader factories.
    /// </summary>
    public interface IAddonDownloaderFactory
    {
        /// <summary>
        /// Returns a new downloader based on host type.
        /// </summary>
        /// <param name="hostType">The addon host type used to determine the downloader type.</param>
        /// <returns>A new downloader.</returns>
        IAddonDownloader GetDownloader(HostType hostType);
    }
}