// ==================================================================================================
// <copyright file="GenericHttpClientService.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Services.HttpClientService
{
    using System.Net;
    using System.Net.Http;
    using System.Net.NetworkInformation;
    using AddonWars2.Services.HttpClientService.Interfaces;

    /// <summary>
    /// Represents a web service client.
    /// </summary>
    public class GenericHttpClientService : IHttpClientService
    {
        #region Fields

        private static readonly HttpClient _httpClient;

        #endregion Fields

        #region Constructors

        // Static constructor.
        static GenericHttpClientService()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericHttpClientService"/> class.
        /// </summary>
        public GenericHttpClientService()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the HTTP client object, providing the ability to send HTTP requests and
        /// receive HTTP responses from a resource identified by a URI.
        /// </summary>
        protected HttpClient HttpClient => _httpClient;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public bool IsNetworkAvailable()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">If <paramref name="url"/> is <see langword="null"/>.</exception>
        public async Task<HttpResponseMessage> GetResponseAsync(string url, HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseContentRead, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(url, nameof(url));

            if (!IsNetworkAvailable())
            {
                throw new HttpRequestException("No internet connection.");
            }

            try
            {
                return await HttpClient.GetAsync(url, httpCompletionOption).ConfigureAwait(false);
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
                return new HttpResponseMessage(HttpStatusCode.RequestTimeout);
            }
        }

        #endregion Methods
    }
}
