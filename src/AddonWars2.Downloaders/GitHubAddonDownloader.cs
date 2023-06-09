// ==================================================================================================
// <copyright file="GitHubAddonDownloader.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloaders
{
    using System.Threading.Tasks;
    using AddonWars2.Downloaders.Models;
    using AddonWars2.Services.GitHubClientWrapper;
    using AddonWars2.Services.HttpClientWrapper.Interfaces;
    using Octokit;

    /// <summary>
    /// Represens a downloader used to download addons from GitHub repositories.
    /// </summary>
    public class GitHubAddonDownloader : AddonDownloaderBase
    {
        #region Fields

        private readonly GitHubClientWrapper _gitHubClientService;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubAddonDownloader"/> class.
        /// </summary>
        /// <param name="httpClientService">A reference to <see cref="IHttpClientWrapper"/> instance.</param>
        /// <param name="gitHubClientService">A reference to <see cref="GitHubClientWrapper"/> instance.</param>
        public GitHubAddonDownloader(IHttpClientWrapper httpClientService, GitHubClientWrapper gitHubClientService)
            : base(httpClientService)
        {
            _gitHubClientService = gitHubClientService ?? throw new ArgumentNullException(nameof(gitHubClientService));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the instance of <see cref="IHttpClientWrapper"/> service.
        /// </summary>
        protected GitHubClientWrapper GitHubClientService => _gitHubClientService;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public override Task<DownloadedObject> Download(DownloadRequest request)
        {
            if (GitHubClientService.GitHubLastApiInfo?.RateLimit.Remaining > 0)
            {

            }
            else
            {
                
            }

            throw new NotImplementedException();
        }

        #endregion methods
    }
}
