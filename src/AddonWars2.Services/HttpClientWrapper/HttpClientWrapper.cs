// ==================================================================================================
// <copyright file="HttpClientWrapper.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Services.HttpClientWrapper
{
    using System.Net.NetworkInformation;
    using System.Threading.Tasks;
    using AddonWars2.Services.HttpClientWrapper.Interfaces;

    /// <summary>
    /// Represents a <see cref="System.Net.Http.HttpClient"/> wrapper.
    /// </summary>
    public class HttpClientWrapper : IHttpClientWrapper
    {
        #region Fields

        private readonly HttpClient _httpClient;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientWrapper"/> class.
        /// </summary>
        /// <param name="httpClient">A reference to a <see cref="System.Net.Http.HttpClient"/> instance.</param>
        public HttpClientWrapper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #endregion Constructors

        #region Properties

        /// <inheritdoc/>
        public HttpClient HttpClient => _httpClient;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public bool IsNetworkAvailable()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }

        /// <inheritdoc/>
        /// <exception cref="HttpRequestException">Is thrown if the response has a bad status code.</exception>
        public async Task<HttpResponseMessage> GetAsync(string? uri, Dictionary<string, string>? headers = null)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }
                }

                HttpResponseMessage response = await HttpClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();

                return response;
            }
        }

        /// <inheritdoc/>
        public async Task<HttpResponseMessage> PostAsync(string uri, object data)
        {
            throw new NotImplementedException();  // TODO: implementation
        }

        #endregion Methods
    }
}
