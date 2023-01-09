// ==================================================================================================
// <copyright file="WebHelper.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Utils.Helpers
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    /// <summary>
    /// Provides various methods to manage web operations.
    /// </summary>
    public static class WebHelper
    {
        #region Methods

        /// <summary>
        /// Sends a GET request to the specified URL as an asynchronous operation.
        /// </summary>
        /// <param name="url">URL string.</param>
        /// <returns><see cref="HttpResponseMessage"/> object.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="url"/> is <see langword="null"/>.</exception>
        public static async Task<HttpResponseMessage> GetResponseAsync(string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            using (var httpClient = new HttpClient())
            {
                try
                {
                    Debug.WriteLine(url);
                    var response = await httpClient.GetAsync(url).ConfigureAwait(false);
                    Debug.WriteLine($"response: {response.StatusCode}");
                    return response;
                }
                catch (HttpRequestException e)
                {
                    return new HttpResponseMessage(e.StatusCode.Value);
                }
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
