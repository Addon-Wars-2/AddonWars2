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
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represens a downloader used to download standalone addons.
    /// </summary>
    public class StandaloneAddonDownloader : AddonDownloaderBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StandaloneAddonDownloader"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/>.</param>
        /// <param name="httpClientService">A reference to <see cref="IHttpClientWrapper"/> instance.</param>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="logger"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="httpClientService"/> is <see langword="null"/>.</exception>
        public StandaloneAddonDownloader(ILogger<AddonDownloaderBase> logger, IHttpClientWrapper httpClientService)
            : base(logger, httpClientService)
        {
            // Blank.
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc/>
        protected override async Task<DownloadResult> DownloadAsync(DownloadRequest request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(nameof(request));

            OnDownloadStarted();

            using (var response = await HttpClientService.GetAsync(request.Url))
            {
                Logger.LogInformation($"Downloading from {request.Url} using {typeof(StandaloneAddonDownloader).Name}.");

                var filename = response.Content.Headers.ContentDisposition?.FileName
                    ?? Path.GetFileName(response.RequestMessage?.RequestUri?.AbsolutePath)
                    ?? string.Empty;

                // TODO: inject version into the downloaded content if available.
                var content = await ReadResponseAsync(response, filename, cancellationToken);

                OnDownloadCompleted();

                return content;
            }
        }

        #endregion Methods
    }
}
