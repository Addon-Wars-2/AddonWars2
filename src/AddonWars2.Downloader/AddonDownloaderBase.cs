// ==================================================================================================
// <copyright file="AddonDownloaderBase.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloader
{
    using System.Net.Http;
    using System.Reflection;
    using System.Threading.Tasks;
    using AddonWars2.Downloader.Events;
    using AddonWars2.Downloader.Interfaces;
    using AddonWars2.Downloader.Models;
    using AddonWars2.Services.HttpClientWrapper.Interfaces;
    using AddonWars2.SharedData;

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
            _httpClientService = httpClientService;
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
