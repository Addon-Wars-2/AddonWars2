// ==================================================================================================
// <copyright file="AddonDownloaderBase.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloaders
{
    using System.Diagnostics;
    using System.Net.Http;
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

        private const int DEFAULT_BUFFER_SIZE = 4096;

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

        /// <inheritdoc/>
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

        public void ClearDownloadProgressChanged()
        {
            DownloadProgressChanged = null;
        }

        /// <inheritdoc/>
        public async Task<DownloadedObject> DownloadAsync(string url)
        {
            return await DownloadAsync(new DownloadRequest(url));
        }

        /// <summary>
        /// Starts to download the requested addon.
        /// </summary>
        /// <param name="request">A request objects which wraps the request information.</param>
        /// <returns><see cref="DownloadedObject"/> object.</returns>
        protected abstract Task<DownloadedObject> DownloadAsync(DownloadRequest request);

        /// <summary>
        /// Reads content from a given response.
        /// </summary>
        /// <param name="response">Response to read.</param>
        /// <returns><see cref="DownloadedObject"/> object.</returns>
        protected async Task<DownloadedObject> ReadResponseAsync(HttpResponseMessage response)
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
            byte[] buffer = new byte[DEFAULT_BUFFER_SIZE];
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

                        if (bytesRead > 0)
                        {
                            // Calling it as sync would lead to blocking UI thread while preforming download task
                            // and not updating progress bars until the task in completed (instantly from 0% to 100%).
                            await Task.Run(() => OnDownloadProgressChanged(contentLength, totalBytesRead));
                        }
                    }
                    while (bytesRead != 0);

                    content = memoryStream.ToArray();
                }
            }

            ClearDownloadProgressChanged();

            return new DownloadedObject(filename, content);
        }

        /// <summary>
        /// Raises <see cref="DownloadProgressChanged"/> event to inform subscribers the download progress value has changed.
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
