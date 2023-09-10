﻿// ==================================================================================================
// <copyright file="AddonDownloaderBase.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloaders
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using AddonWars2.Downloaders.Events;
    using AddonWars2.Downloaders.Interfaces;
    using AddonWars2.Downloaders.Models;
    using AddonWars2.Services.HttpClientWrapper.Interfaces;
    using Microsoft.Extensions.Logging;

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

        private static ILogger _logger;
        private readonly IHttpClientWrapper _httpClientService;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonDownloaderBase"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/>.</param>
        /// <param name="httpClientService">A reference to <see cref="IHttpClientWrapper"/> instance.</param>
        public AddonDownloaderBase(ILogger<AddonDownloaderBase> logger, IHttpClientWrapper httpClientService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClientService = httpClientService ?? throw new ArgumentNullException(nameof(httpClientService));
        }

        #endregion Constructors

        #region Events

        /// <inheritdoc/>
        public event DownloadProgressChangedEventHandler? DownloadProgressChanged;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets the current logger instance.
        /// </summary>
        protected static ILogger Logger => _logger;

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
        public async Task<DownloadResult> DownloadAsync(string url)
        {
            return await DownloadAsync(url, _cts.Token);
        }

        /// <inheritdoc/>
        public async Task<DownloadResult> DownloadAsync(string url, CancellationToken cancellationToken)
        {
            return await DownloadAsync(new DownloadRequest(url), cancellationToken);
        }

        /// <summary>
        /// Starts to download the requested addon.
        /// </summary>
        /// <param name="request">A request objects which wraps the request information.</param>
        /// <param name="cancellationToken">A task cancellation token.</param>
        /// <returns><see cref="DownloadResult"/> object.</returns>
        protected abstract Task<DownloadResult> DownloadAsync(DownloadRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Reads content from a given response.
        /// </summary>
        /// <param name="response">Response to read.</param>
        /// <param name="filename">A downloaded file name.</param>
        /// <param name="cancellationToken">A task cancellation token.</param>
        /// <returns><see cref="DownloadResult"/> object.</returns>
        protected async Task<DownloadResult> ReadResponseAsync(HttpResponseMessage response, string filename, CancellationToken cancellationToken)
        {
            Logger.LogDebug($"Reading a response for {filename}");

            var contentLength = response.Content.Headers.ContentLength ?? 0L;
            var content = Array.Empty<byte>();
            var buffer = new byte[DEFAULT_BUFFER_SIZE];
            var totalBytesRead = 0L;

            if (string.IsNullOrEmpty(filename))
            {
                Logger.LogWarning("Filename is not available!");
            }

            if (contentLength == 0L)
            {
                Logger.LogWarning("Content length is not available!");
            }

            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    var bytesRead = 0;
                    do
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            Logger.LogWarning($"A task cancellation for {filename} was requested.");
                            return new DownloadResult(filename, Array.Empty<byte>());  // return filename to inform what file was aborted
                        }

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

            return new DownloadResult(filename, content);
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
