// ==================================================================================================
// <copyright file="StandaloneAddonDownloader.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons.Downloader
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using AddonWars2.Addons.Downloader.Interfaces;
    using AddonWars2.Addons.Downloader.Models;
    using AddonWars2.Services.HttpClientService.Interfaces;

    /// <summary>
    /// Represens a <see cref="AddonDownloaderBase"/> used to download standalone addons.
    /// </summary>
    public class StandaloneAddonDownloader : AddonDownloaderBase
    {
        #region Fields

        private static readonly string _productName;
        private static readonly string _productVersion;
        private static readonly string _productComment = "+(https://github.com/Addon-Wars-2/AddonWars2)";  // TODO: move elsewhere (cfg?), do not hardcode
        private static readonly HttpClient _httpClient;
        private readonly IHttpClientService _httpClientService;

        #endregion Fields

        #region Constructors

        // Static constructor.
        static StandaloneAddonDownloader()
        {
            _productName = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? string.Empty;
            _productVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version ?? string.Empty;

            var productValue = new ProductInfoHeaderValue(_productName, _productVersion);
            var commentValue = new ProductInfoHeaderValue(_productComment);

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.UserAgent.Add(productValue);
            _httpClient.DefaultRequestHeaders.UserAgent.Add(commentValue);

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StandaloneAddonDownloader"/> class.
        /// </summary>
        /// <param name="httpClientService">A reference to <see cref="IHttpClientService"/> instance.</param>
        public StandaloneAddonDownloader(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the HTTP client object, providing the ability to send HTTP requests and
        /// receive HTTP responses from a resource identified by a URI.
        /// </summary>
        protected HttpClient HttpClient => _httpClient;

        /// <summary>
        /// Gets the HTTP client service instance.
        /// </summary>
        protected IHttpClientService HttpClientService => _httpClientService;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public override async Task<IDownloadedAddon> Download(IDownloadRequest request)
        {
            HttpResponseMessage response;

            try
            {
                response = await HttpClientService.GetResponseAsync(request.Url);
            }
            catch (HttpRequestException)
            {
                return new DownloadedStandaloneAddon(Array.Empty<byte>()) { Status = Status.Failed };
            }

            return new DownloadedStandaloneAddon(await response.Content.ReadAsByteArrayAsync());
        }

        #endregion Methods
    }
}
