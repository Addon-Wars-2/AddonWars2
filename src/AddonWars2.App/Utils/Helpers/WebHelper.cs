// ==================================================================================================
// <copyright file="WebHelper.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Utils.Helpers
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.NetworkInformation;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    /// <summary>
    /// Provides various methods to manage web operations.
    /// </summary>
    public static class WebHelper
    {
        #region Fields

        private static readonly double _defaultTimeout = 15;
        private static readonly HttpClient _httpClient;

        #endregion Fields

        #region Constructors

        static WebHelper()
        {
            _httpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(WebHelper._defaultTimeout),
            };
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Checks whether any network connection is available.
        /// </summary>
        /// <returns><see langword="true"/> if any network connection is availavble, otherwise - <see langword="false"/>.</returns>
        public static bool IsNetworkAvailable()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }

        /// <summary>
        /// Sends a GET request to the specified URL as an asynchronous operation.
        /// </summary>
        /// <param name="url">URL string.</param>
        /// <param name="httpCompletionOption">
        /// Indicates if <see cref="HttpClient"/> operations should be considered completed
        /// either as soon as a response is available, or after reading the entire response
        /// message including the content.
        /// </param>
        /// <returns><see cref="HttpResponseMessage"/> object.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="url"/> is <see langword="null"/>.</exception>
        public static async Task<HttpResponseMessage> GetResponseAsync(
            string url,
            HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseContentRead)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (!IsNetworkAvailable())
            {
                var message = ResourcesHelper.GetApplicationResource<string>("S.Application.Exception.NoInternetConnection");
                throw new HttpRequestException(message);
            }

            try
            {
                var response = await _httpClient.GetAsync(url, httpCompletionOption).ConfigureAwait(false);
                return response;
            }
            catch (HttpRequestException e)
            {
                if (e.StatusCode.HasValue)
                {
                    return new HttpResponseMessage(e.StatusCode.Value);
                }

                return new HttpResponseMessage();
            }
            catch (TaskCanceledException)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.RequestTimeout);
            }
        }

        /// <summary>
        /// Asynchronously creates a new <see cref="XDocument"/> from the specified stream.
        /// </summary>
        /// <param name="stream">A <see cref="Stream"/> containing the raw XML to read into the newly created <see cref="XDocument"/>.</param>
        /// <param name="loadOptions">A set of <see cref="LoadOptions"/>.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A newly created <see cref="XDocument"/> object.</returns>
        public static async Task<XDocument> LoadXmlAsync(
            Stream stream,
            LoadOptions loadOptions = default,
            CancellationToken cancellationToken = default)
        {
            var xdoc = await XDocument.LoadAsync(stream, loadOptions, cancellationToken);
            return xdoc;
        }

        #endregion Methods
    }
}
