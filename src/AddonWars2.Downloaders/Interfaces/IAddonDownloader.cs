// ==================================================================================================
// <copyright file="IAddonDownloader.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloaders.Interfaces
{
    using AddonWars2.Core.Interfaces;
    using AddonWars2.Downloaders.Models;

    /// <summary>
    /// Provides a contract for addon downloaders.
    /// </summary>
    public interface IAddonDownloader : IAttachableProgress
    {
        /// <summary>
        /// Is raised whenever the download progress has changed.
        /// </summary>
        public event DownloadProgressChangedEventHandler? DownloadProgressChanged;

        /// <summary>
        /// Is raised right before the download process has started.
        /// </summary>
        public event EventHandler DownloadStarted;

        /// <summary>
        /// Is raised after the download process has completed.
        /// </summary>
        public event EventHandler DownloadCompleted;

        /// <summary>
        /// Starts to download the requested addon.
        /// </summary>
        /// <param name="url">URL of a file to download.</param>
        /// <returns><see cref="DownloadResult"/> object.</returns>
        public Task<DownloadResult> DownloadAsync(string url);

        /// <summary>
        /// Starts to download the requested addon.
        /// </summary>
        /// <param name="url">URL of a file to download.</param>
        /// <param name="cancellationToken">A task cancellation token.</param>
        /// <returns><see cref="DownloadResult"/> object.</returns>
        public Task<DownloadResult> DownloadAsync(string url, CancellationToken cancellationToken);
    }
}
