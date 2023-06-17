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

    /// <summary>
    /// Represents a factory for downloaders.
    /// </summary>
    public class AddonDownloaderFactory : IAddonDownloaderFactory
    {
        #region Fields

        private readonly IHttpClientWrapper _httpClientService;
        private readonly IGitHubClientWrapper _gitHubClientService;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonDownloaderFactory"/> class.
        /// </summary>
        /// <param name="httpClientService">A reference to <see cref="IHttpClientWrapper"/> instance.</param>
        /// <param name="gitHubClientWrapper">A reference to <see cref="GitHubClientWrapper"/> instance.</param>
        public AddonDownloaderFactory(IHttpClientWrapper httpClientService, IGitHubClientWrapper gitHubClientWrapper)
        {
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
                    return new StandaloneAddonDownloader(HttpClientService);
                case HostType.GitHub:
                    return new GitHubAddonDownloader(HttpClientService, GitHubClientService);
                default:
                    throw new NotSupportedException($"Cannot create a downloader for the host type: {hostType.GetType().Name}. The host type is not supported.");
            }
        }

        /// <inheritdoc/>
        public BulkAddonDownloader GetBulkDownloader(IAddonDownloaderFactory factory)
        {
            return new BulkAddonDownloader(factory);
        }

        #endregion Methods
    }
}
