// ==================================================================================================
// <copyright file="AddonDownloaderBase.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloaders
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using AddonWars2.Core.Events;
    using AddonWars2.Downloaders.Interfaces;
    using AddonWars2.Downloaders.Models;
    using AddonWars2.Services.HttpClientWrapper.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents the DownloadProgressChanged event handler.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void DownloadProgressChangedEventHandler(object? sender, ProgressEventArgs e);

    /// <summary>
    /// Represents a base class for addon downloaders.
    /// </summary>
    public abstract class AddonDownloaderBase : IAddonDownloader
    {
        #region Fields

        private const int DEFAULT_BUFFER_SIZE = 4096;

        private static ILogger _logger;
        private readonly Dictionary<string, IProgress<double>> _progressCollection = new Dictionary<string, IProgress<double>>();
        private readonly IHttpClientWrapper _httpClientService;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonDownloaderBase"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/>.</param>
        /// <param name="httpClientService">A reference to <see cref="IHttpClientWrapper"/> instance.</param>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="logger"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="httpClientService"/> is <see langword="null"/>.</exception>
        public AddonDownloaderBase(ILogger<AddonDownloaderBase> logger, IHttpClientWrapper httpClientService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClientService = httpClientService ?? throw new ArgumentNullException(nameof(httpClientService));

            DownloadProgressChanged += AddonDownloaderBase_DownloadProgressChanged;
            DownloadStarted += AddonDownloaderBase_DownloadStarted;
            DownloadCompleted += AddonDownloaderBase_DownloadCompleted;
        }

        #endregion Constructors

        #region Events

        /// <inheritdoc/>
        public event DownloadProgressChangedEventHandler? DownloadProgressChanged;

        /// <inheritdoc/>
        public event EventHandler DownloadStarted;

        /// <inheritdoc/>
        public event EventHandler DownloadCompleted;

        #endregion Events

        #region Properties

        /// <inheritdoc/>
        public Dictionary<string, IProgress<double>> ProgressCollection => _progressCollection;

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
        public void AttachProgressItem(string token, IProgress<double> progress)
        {
            ProgressCollection.Add(token, progress);
        }

        /// <inheritdoc/>
        public async Task<DownloadResult> DownloadAsync(string url)
        {
            return await DownloadAsync(url);
        }

        /// <inheritdoc/>
        public async Task<DownloadResult> DownloadAsync(string url, CancellationToken cancellationToken = default)
        {
            return await DownloadAsync(new DownloadRequest(url), cancellationToken);
        }

        /// <summary>
        /// Starts to download the requested addon.
        /// </summary>
        /// <param name="request">A request object that wraps the request information.</param>
        /// <param name="cancellationToken">A task cancellation token.</param>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <returns><see cref="Task{TResult}"/> object.</returns>
        protected abstract Task<DownloadResult> DownloadAsync(DownloadRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Reads content from a given response.
        /// </summary>
        /// <param name="response">Response to read.</param>
        /// <param name="filename">A downloaded file name.</param>
        /// <param name="cancellationToken">A task cancellation token.</param>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="response"/> is <see langword="null"/>.</exception>
        /// <returns><see cref="Task{TResult}"/> object.</returns>
        protected async Task<DownloadResult> ReadResponseAsync(HttpResponseMessage response, string filename, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(nameof(response));

            Logger.LogDebug($"Reading a response for {filename}");

            var contentLength = response.Content.Headers.ContentLength ?? 0L;  // TODO: Heavy/lightweight files (select either memory stream or filestream maybe?)
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

            using (var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    var bytesRead = 0;
                    do
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            Logger.LogWarning($"A task cancellation for {filename} was requested.");
                            cancellationToken.ThrowIfCancellationRequested();
                        }

                        bytesRead = await responseStream.ReadAsync(buffer.AsMemory(0, buffer.Length), cancellationToken);
                        await memoryStream.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
                        totalBytesRead += bytesRead;

                        if (bytesRead > 0)
                        {
                            // Calling it as sync would lead to blocking UI thread while preforming download task
                            // and not updating progress bars until the task in completed (instantly from 0% to 100%).
                            await Task.Run(() => OnDownloadProgressChanged(contentLength, totalBytesRead), cancellationToken);
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
            handler?.Invoke(this, new ProgressEventArgs(totalBytesToReceive, bytesReceived));
        }

        /// <summary>
        /// Raises <see cref="DownloadStarted"/> event to inform subscribers the download process has started.
        /// </summary>
        protected virtual void OnDownloadStarted()
        {
            var handler = DownloadStarted;
            handler?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises <see cref="DownloadCompleted"/> event to inform subscribers the download process has completed.
        /// </summary>
        protected virtual void OnDownloadCompleted()
        {
            var handler = DownloadCompleted;
            handler?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Updates all items in the progress collection.
        /// </summary>
        /// <param name="sender">Event source.</param>
        /// <param name="e">Event arguments.</param>
        private void AddonDownloaderBase_DownloadProgressChanged(object? sender, ProgressEventArgs e)
        {
            if (ProgressCollection.Count > 0)
            {
                foreach (var key in ProgressCollection.Keys)
                {
                    ProgressCollection.TryGetValue(key, out var progress);
                    progress?.Report(e.Progress);
                }
            }
        }

        private void AddonDownloaderBase_DownloadStarted(object? sender, EventArgs e)
        {
            // Blank.
        }

        private void AddonDownloaderBase_DownloadCompleted(object? sender, EventArgs e)
        {
            DownloadProgressChanged -= AddonDownloaderBase_DownloadProgressChanged;
            DownloadStarted -= AddonDownloaderBase_DownloadStarted;
            DownloadCompleted -= AddonDownloaderBase_DownloadCompleted;
        }

        #endregion Methods
    }
}
