﻿// ==================================================================================================
// <copyright file="AddonDownloaderFactory.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloader
{
    using AddonWars2.Addons.Models.AddonInfo;
    using AddonWars2.Downloader.Interfaces;
    using AddonWars2.Services.HttpClientWrapper.Interfaces;

    /// <summary>
    /// Represents a factory for downloaders.
    /// </summary>
    public class AddonDownloaderFactory : IAddonDownloaderFactory
    {
        #region Fields

        private readonly IHttpClientWrapper _httpClientService;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonDownloaderFactory"/> class.
        /// </summary>
        /// <param name="httpClientService">A reference to <see cref="IHttpClientWrapper"/> instance.</param>
        public AddonDownloaderFactory(IHttpClientWrapper httpClientService)
        {
            _httpClientService = httpClientService;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the instance of <see cref="IHttpClientWrapper"/> service.
        /// </summary>
        protected IHttpClientWrapper HttpClientService => _httpClientService;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public IAddonDownloader GetDownloader(HostType hostType)
        {
            switch (hostType)
            {
                case HostType.Standalone:
                    return new StandaloneAddonDownloader(HttpClientService);
                case HostType.GitHub:
                    throw new NotImplementedException();
                default:
                    throw new NotSupportedException($"Cannot create a downloader for the host type: {hostType.GetType().Name}. The host type is not supported.");
            }
        }

        #endregion Methods
    }
}
