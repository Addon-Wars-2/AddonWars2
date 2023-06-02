// ==================================================================================================
// <copyright file="StandaloneAddonDownloader.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloaders
{
    using System.Net.Http;
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
        public override async Task<DownloadedObject> Download(DownloadRequest request)
        {
            using (var response = await HttpClientService.GetAsync(request.Url))
            {
                return await DownloadFromResponse(response);
            }
        }

        // Downloads content using the specified response message.
        private async Task<DownloadedObject> DownloadFromResponse(HttpResponseMessage response)
        {
            var filename = response.Headers.Location?.AbsoluteUri.ToString() ?? string.Empty;
            var contentLength = response.Content.Headers.ContentLength ?? 0L;

            if (contentLength == 0)
            {
                return new DownloadedObject(filename, Array.Empty<byte>());
            }

            byte[] content = Array.Empty<byte>();
            byte[] buffer = new byte[4096];  // default 4k is considered to be optimal
            var totalBytesRead = 0L;

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    var bytesRead = 0;

                    do
                    {
                        bytesRead = await responseStream.ReadAsync(buffer.AsMemory(0, buffer.Length));

                        await memoryStream.WriteAsync(buffer.AsMemory(0, bytesRead));

                        totalBytesRead += bytesRead;
                        OnDownloadProgressChanged(contentLength, totalBytesRead);
                    }
                    while (bytesRead != 0);

                    OnDownloadProgressChanged(contentLength, totalBytesRead);

                    content = memoryStream.ToArray();
                }
            }

            return new DownloadedObject(filename, content);
        }

        #endregion Methods
    }
}
