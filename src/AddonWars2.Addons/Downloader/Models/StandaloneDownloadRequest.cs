﻿// ==================================================================================================
// <copyright file="StandaloneDownloadRequest.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons.Downloader.Models
{
    using AddonWars2.Addons.Downloader.Interfaces;

    /// <summary>
    /// Encapsulates addon download request.
    /// </summary>
    public class StandaloneDownloadRequest : IDownloadRequest
    {
        #region Fields

        private readonly string _url = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StandaloneDownloadRequest"/> class.
        /// </summary>
        public StandaloneDownloadRequest()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <inheritdoc/>
        public string Url => _url;

        #endregion Properties
    }
}
