// ==================================================================================================
// <copyright file="IHttpClientWrapper.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Services.HttpClientWrapper.Interfaces
{
    /// <summary>
    /// Provides a contract for HTTP client wrapper.
    /// </summary>
    public interface IHttpClientWrapper
    {
        /// <summary>
        /// Gets the <see cref="System.Net.Http.HttpClient"/> object.
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// Checks whether any network connection is available.
        /// </summary>
        /// <returns><see langword="true"/> if any network connection is availavble, otherwise - <see langword="false"/>.</returns>
        public bool IsNetworkAvailable();

        /// <summary>
        /// Sends a GET request to the specified URL as an asynchronous operation.
        /// </summary>
        /// <param name="uri">Request uri.</param>
        /// <param name="headers">Headers collection to set for the request.</param>
        /// <returns>Response object.</returns>
        Task<HttpResponseMessage> GetAsync(string uri, Dictionary<string, string>? headers = null);

        /// <summary>
        /// Sends a POST request to the specified URL as an asynchronous operation.
        /// </summary>
        /// <param name="uri">Request uri.</param>
        /// <param name="data">Data to send.</param>
        /// <returns>Response object.</returns>
        Task<HttpResponseMessage> PostAsync(string uri, object data);
    }
}
