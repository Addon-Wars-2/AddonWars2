// ==================================================================================================
// <copyright file="AddonDownloaderBase.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloaders
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Reflection.PortableExecutable;
    using System.Threading.Tasks;
    using AddonWars2.Downloaders.Events;
    using AddonWars2.Downloaders.Interfaces;
    using AddonWars2.Downloaders.Models;
    using AddonWars2.Services.HttpClientWrapper.Interfaces;

    /// <summary>
    /// Represents the DownloadProgressChanged event handler.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void DownloadProgressChangedEventHandler(object? sender, DownloadProgressEventArgs e);

    /// <summary>
    /// Represents a base class for addon downloaders.
    /// </summary>
    public abstract class AddonDownloaderBase : IAddonDownloader
    {
        #region Fields

        private readonly IHttpClientWrapper _httpClientService;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonDownloaderBase"/> class.
        /// </summary>
        /// <param name="httpClientService">A reference to <see cref="IHttpClientWrapper"/> instance.</param>
        public AddonDownloaderBase(IHttpClientWrapper httpClientService)
        {
            _httpClientService = httpClientService ?? throw new ArgumentNullException(nameof(httpClientService));
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Is raised whenever the download progress has changed.
        /// </summary>
        public event DownloadProgressChangedEventHandler? DownloadProgressChanged;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets the HTTP client object, providing the ability to send HTTP requests and
        /// receive HTTP responses from a resource identified by a URI.
        /// </summary>
        protected HttpClient HttpClient => _httpClientService.HttpClient;

        /// <summary>
        /// Gets the HTTP client service instance.
        /// </summary>
        protected IHttpClientWrapper HttpClientService => _httpClientService;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public abstract Task<DownloadedObject> Download(DownloadRequest request);

        public async Task<DownloadedObject> Download(string url, Dictionary<string, string> headers)
        {
            return await Download(new DownloadRequest(url, headers));
        }

        public async Task<DownloadedObject> Download(string url)
        {
            return await Download(url, new Dictionary<string, string>());
        }

        /// <summary>
        /// Reads content from a given response.
        /// </summary>
        /// <param name="response">Response to read.</param>
        /// <returns><see cref="DownloadedObject"/> object.</returns>
        protected async Task<DownloadedObject> ReadResponse(HttpResponseMessage response)
        {
            var filename = response.Content.Headers.ContentDisposition?.FileName
                ?? Path.GetFileName(response.RequestMessage?.RequestUri?.AbsolutePath)
                ?? string.Empty;
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

        /// <summary>
        /// Raises <see cref="DownloadProgressChanged"/> event to inform subscribers
        /// the download progress value has changed.
        /// </summary>
        /// <param name="totalBytesToReceive">The total number of bytes in data download operation.</param>
        /// <param name="bytesReceived">The number of bytes received.</param>
        protected virtual void OnDownloadProgressChanged(long totalBytesToReceive, long bytesReceived)
        {
            var handler = DownloadProgressChanged;
            handler?.Invoke(this, new DownloadProgressEventArgs(totalBytesToReceive, bytesReceived));
        }

        #endregion Methods
    }
}
