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

        //private static string _defaultProductName;
        //private static string _defaultProductVersion;
        //private static string _defaultProductComment;
        //private static string _userAgent = string.Empty;

        private readonly IHttpClientWrapper _httpClientService;

        #endregion Fields

        #region Constructors

        ///// <summary>
        ///// Initializes static members of the <see cref="AddonDownloaderBase"/> class.
        ///// </summary>
        //static AddonDownloaderBase()
        //{
        //    _defaultProductName = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? string.Empty;
        //    _defaultProductVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version ?? string.Empty;
        //    _defaultProductComment = $"+{WebStaticData.GITHUB_PROJECT_WIKI_URL}";
        //}

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

        ///// <summary>
        ///// Gets or sets the default product name used in <see cref="HttpClient"/> user agent.
        ///// </summary>
        ///// <remarks>
        ///// The initial value is equal to the executing assembly title attribute
        ///// and is set in static constructor.
        ///// </remarks>
        //public static string DefaultProductName
        //{
        //    get => _defaultProductName;
        //    set
        //    {
        //        _defaultProductName = value;
        //        ReconstructUserAgent();
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the default product version used in <see cref="HttpClient"/> user agent.
        ///// </summary>
        ///// <remarks>
        ///// The initial value is equal to the executing assembly varsion attribute
        ///// and is set in static constructor.
        ///// </remarks>
        //public static string DefaultProductVersion
        //{
        //    get => _defaultProductVersion;
        //    set
        //    {
        //        _defaultProductVersion = value;
        //        ReconstructUserAgent();
        //    }
        //}

        ///// <summary>
        ///// Gets or sets the default product comment used in <see cref="HttpClient"/> user agent.
        ///// </summary>
        ///// <remarks>
        ///// The initial value is equal to an empty string and is set in static constructor.
        ///// </remarks>
        //public static string DefaultProductComment
        //{
        //    get => _defaultProductComment;
        //    set
        //    {
        //        _defaultProductComment = value;
        //        ReconstructUserAgent();
        //    }
        //}

        ///// <summary>
        ///// Gets a User-Agent string.
        ///// </summary>
        ///// <remarks>
        ///// This value is constrcuted from <see cref="_defaultProductName"/>, <see cref="DefaultProductVersion"/>
        ///// and <see cref="DefaultProductComment"/> values and gets updated internally every time any of these
        ///// properties are changed.
        ///// </remarks>
        //public static string UserAgent => _userAgent;

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

        //// Updates User-Agent value.
        //private static void ReconstructUserAgent()
        //{
        //    _userAgent = $"{DefaultProductName}/{DefaultProductVersion} (+{DefaultProductComment})";
        //}

        #endregion Methods
    }
}
