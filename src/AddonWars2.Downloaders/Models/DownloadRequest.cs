// ==================================================================================================
// <copyright file="DownloadRequest.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloaders.Models
{
    /// <summary>
    /// Encapsulates an addon download request.
    /// </summary>
    public class DownloadRequest
    {
        #region Fields

        private readonly string _url = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadRequest"/> class.
        /// </summary>
        /// <param name="url">A URL of a file to download.</param>
        public DownloadRequest(string url)
        {
            _url = url;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a URL that will be used to download the addon.
        /// </summary>
        public string Url => _url;

        #endregion Properties
    }
}
