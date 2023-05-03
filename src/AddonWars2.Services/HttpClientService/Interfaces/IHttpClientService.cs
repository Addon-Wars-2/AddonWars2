// ==================================================================================================
// <copyright file="IHttpClientService.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Services.HttpClientService.Interfaces
{
    /// <summary>
    /// Represents a contract for web client services.
    /// </summary>
    public interface IHttpClientService
    {
        /// <summary>
        /// Checks whether any network connection is available.
        /// </summary>
        /// <returns><see langword="true"/> if any network connection is availavble, otherwise - <see langword="false"/>.</returns>
        public bool IsNetworkAvailable();

        /// <summary>
        /// Sends a GET request to the specified URL as an asynchronous operation.
        /// </summary>
        /// <param name="url">URL string.</param>
        /// <param name="httpCompletionOption">
        /// Indicates if <see cref="HttpClient"/> operations should be considered completed
        /// either as soon as a response is available, or after reading the entire response
        /// message including the content.
        /// </param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns><see cref="HttpResponseMessage"/> object.</returns>
        public Task<HttpResponseMessage> GetResponseAsync(string url, HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseContentRead, CancellationToken cancellationToken = default);
    }
}
