// ==================================================================================================
// <copyright file="StandaloneAddonDownloader.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloader
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using AddonWars2.Downloader.Interfaces;
    using AddonWars2.Downloader.Models;
    using AddonWars2.Services.HttpClientWrapper.Interfaces;

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
        private readonly IHttpClientWrapper _httpClientService;

        #endregion Fields

        #region Constructors

        // Static constructor.
        static StandaloneAddonDownloader()
        {
            _productName = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? string.Empty;
            _productVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version ?? string.Empty;

            var productValue = new ProductInfoHeaderValue(_productName, _productVersion);
            var commentValue = new ProductInfoHeaderValue(_productComment);

            _httpClient = new HttpClient() { Timeout = TimeSpan.FromMinutes(5) };  // TODO: reconsider timeout value
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.UserAgent.Add(productValue);
            _httpClient.DefaultRequestHeaders.UserAgent.Add(commentValue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StandaloneAddonDownloader"/> class.
        /// </summary>
        /// <param name="httpClientService">A reference to <see cref="IGenericHttpClientService"/> instance.</param>
        public StandaloneAddonDownloader(IHttpClientWrapper httpClientService)
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
        protected IHttpClientWrapper HttpClientService => _httpClientService;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public override async Task<DownloadedObject> Download(IDownloadRequest request)
        {
            using (var response = await HttpClientService.GetAsync(request.Url))
            {
                return await DownloadFromResponse(response);
            }

            ////HttpResponseMessage response;

            ////try
            ////{
            ////    response = await HttpClientService.GetAsync(request.Url);
            ////}
            ////catch (HttpRequestException)
            ////{
            ////    return new DownloadedObject(Array.Empty<byte>()) { Status = Status.Failed };
            ////}

            ////return new DownloadedObject(await response.Content.ReadAsByteArrayAsync());
        }

        private async Task<DownloadedObject> DownloadFromResponse(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();

            var filename = response.Headers.Location?.AbsoluteUri.ToString() ?? string.Empty;
            var contentLength = response.Content.Headers.ContentLength ?? 0L;

            if (contentLength == 0)
            {
                return new DownloadedObject(filename, Array.Empty<byte>());
            }

            byte[] content = Array.Empty<byte>();
            byte[] buffer = new byte[4096];  // 4k is considered to be optimal
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

        ////private async Task ProcessContentStream(long? contentLength, Stream stream)
        ////{
        ////    var totalBytesRead = 0L;
        ////    var readCount = 0L;
        ////    var buffer = new byte[8192];
        ////    var isMoreToRead = true;

        ////    using (var fileStream = new FileStream("", FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
        ////    {
        ////        do
        ////        {
        ////            var bytesRead = await responseStream.ReadAsync(buffer, 0, buffer.Length);
        ////            if (bytesRead == 0)
        ////            {
        ////                isMoreToRead = false;
        ////                TriggerProgressChanged(contentLength, totalBytesRead);
        ////                continue;
        ////            }

        ////            await fileStream.WriteAsync(buffer, 0, bytesRead);

        ////            totalBytesRead += bytesRead;
        ////            readCount += 1;

        ////            if (readCount % 100 == 0)
        ////                TriggerProgressChanged(contentLength, totalBytesRead);
        ////        }
        ////        while (isMoreToRead);
        ////    }
        ////}

        ////private void TriggerProgressChanged(long? totalDownloadSize, long totalBytesRead)
        ////{
        ////    if (ProgressChanged == null)
        ////        return;

        ////    double? progressPercentage = null;
        ////    if (totalDownloadSize.HasValue)
        ////        progressPercentage = Math.Round((double)totalBytesRead / totalDownloadSize.Value * 100, 2);

        ////    ProgressChanged(totalDownloadSize, totalBytesRead, progressPercentage);
        ////}

        #endregion Methods
    }
}
