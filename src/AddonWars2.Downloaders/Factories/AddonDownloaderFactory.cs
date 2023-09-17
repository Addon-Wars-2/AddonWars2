// ==================================================================================================
// <copyright file="AddonDownloaderFactory.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloaders.Factories
{
    using AddonWars2.Core.Enums;
    using AddonWars2.Downloaders;
    using AddonWars2.Downloaders.Interfaces;
    using AddonWars2.Services.GitHubClientWrapper;
    using AddonWars2.Services.GitHubClientWrapper.Interfaces;
    using AddonWars2.Services.HttpClientWrapper.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents a factory for downloaders.
    /// </summary>
    public class AddonDownloaderFactory : IAddonDownloaderFactory
    {
        #region Fields

        private readonly IHttpClientWrapper _httpClientService;
        private readonly IGitHubClientWrapper _gitHubClientService;
        private static ILogger<AddonDownloaderBase> _loggerBaseDownloader;
        private static ILogger<BulkAddonDownloader> _loggerBulkDownloader;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonDownloaderFactory"/> class.
        /// </summary>
        /// <param name="loggerBaseDownloader">A reference to base <see cref="ILogger"/>.</param>
        /// <param name="loggerBulkDownloader">A reference to bulk <see cref="ILogger"/>.</param>
        /// <param name="httpClientService">A reference to <see cref="IHttpClientWrapper"/> instance.</param>
        /// <param name="gitHubClientWrapper">A reference to <see cref="GitHubClientWrapper"/> instance.</param>
        public AddonDownloaderFactory(
            ILogger<AddonDownloaderBase> loggerBaseDownloader,
            ILogger<BulkAddonDownloader> loggerBulkDownloader,
            IHttpClientWrapper httpClientService,
            IGitHubClientWrapper gitHubClientWrapper)
        {
            _loggerBaseDownloader = loggerBaseDownloader ?? throw new ArgumentNullException(nameof(loggerBaseDownloader));
            _loggerBulkDownloader = loggerBulkDownloader ?? throw new ArgumentNullException(nameof(loggerBulkDownloader));
            _httpClientService = httpClientService ?? throw new ArgumentNullException(nameof(httpClientService));
            _gitHubClientService = gitHubClientWrapper ?? throw new ArgumentNullException(nameof(gitHubClientWrapper));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the instance of <see cref="IHttpClientWrapper"/> service.
        /// </summary>
        protected IHttpClientWrapper HttpClientService => _httpClientService;

        /// <summary>
        /// Gets the instance of <see cref="IHttpClientWrapper"/> service.
        /// </summary>
        protected IGitHubClientWrapper GitHubClientService => _gitHubClientService;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public IAddonDownloader GetDownloader(HostType hostType)
        {
            switch (hostType)
            {
                case HostType.Standalone:
                    return new StandaloneAddonDownloader(_loggerBaseDownloader, HttpClientService);
                case HostType.GitHub:
                    return new GitHubAddonDownloader(_loggerBaseDownloader, HttpClientService, GitHubClientService);
                case HostType.Local:
                    throw new NotSupportedException(); // TODO: implementation (especially for testing purposes)
                default:
                    throw new NotSupportedException($"Cannot create a downloader for the host type: {hostType}. The host type is not supported.");
            }
        }

        /// <inheritdoc/>
        public BulkAddonDownloader GetBulkDownloader()
        {
            return new BulkAddonDownloader(_loggerBulkDownloader, this);
        }

        #endregion Methods
    }
}
