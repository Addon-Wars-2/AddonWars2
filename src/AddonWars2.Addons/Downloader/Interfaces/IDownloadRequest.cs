// ==================================================================================================
// <copyright file="IDownloadRequest.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons.Downloader.Interfaces
{
    /// <summary>
    /// Represents a contract for download requests.
    /// </summary>
    public interface IDownloadRequest
    {
        /// <summary>
        /// Gets a URL that will be used to download the addon.
        /// </summary>
        public string Url { get; }
    }
}
