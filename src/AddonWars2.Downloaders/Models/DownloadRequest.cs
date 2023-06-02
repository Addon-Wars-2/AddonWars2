// ==================================================================================================
// <copyright file="DownloadRequest.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloaders.Models
{
    /// <summary>
    /// Encapsulates addon download request.
    /// </summary>
    public class DownloadRequest
    {
        #region Fields

        private readonly string _url = string.Empty;
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadRequest"/> class.
        /// </summary>
        /// <param name="url">URL of a file to download.</param>
        /// <param name="headers">A collection of headers for the request.</param>
        public DownloadRequest(string url, Dictionary<string, string> headers)
        {
            _url = url;
            _headers = headers;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a URL that will be used to download the addon.
        /// </summary>
        public string Url => _url;

        /// <summary>
        /// Gets a collection of headers for the request.
        /// </summary>
        public Dictionary<string, string> Headers => _headers;

        #endregion Properties
    }
}
