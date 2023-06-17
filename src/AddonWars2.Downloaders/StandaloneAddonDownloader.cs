// ==================================================================================================
// <copyright file="StandaloneAddonDownloader.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloaders
{
    using AddonWars2.Downloaders.Models;
    using AddonWars2.Services.HttpClientWrapper.Interfaces;

    /// <summary>
    /// Represens a downloader used to download standalone addons.
    /// </summary>
    public class StandaloneAddonDownloader : AddonDownloaderBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StandaloneAddonDownloader"/> class.
        /// </summary>
        /// <param name="httpClientService">A reference to <see cref="IHttpClientWrapper"/> instance.</param>
        public StandaloneAddonDownloader(IHttpClientWrapper httpClientService)
            : base(httpClientService)
        {
            // Blank.
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc/>
        protected override async Task<DownloadedObject> DownloadAsync(DownloadRequest request)
        {
            using (var response = await HttpClientService.GetAsync(request.Url))
            {
                return await ReadResponseAsync(response);
            }
        }

        #endregion Methods
    }
}
