// ==================================================================================================
// <copyright file="GitHubAddonDownloader.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloader
{
    using System.Threading.Tasks;
    using AddonWars2.Downloader.Models;
    using AddonWars2.Services.HttpClientWrapper.Interfaces;
    using Octokit;

    /// <summary>
    /// Represens a downloader used to download addons from GitHub repositories.
    /// </summary>
    public class GitHubAddonDownloader : AddonDownloaderBase
    {
        #region Fields

        private readonly GitHubClient _gitHubClient;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubAddonDownloader"/> class.
        /// </summary>
        /// <param name="httpClientService">A reference to <see cref="IHttpClientWrapper"/> instance.</param>
        /// <param name="gitHubClient">A reference to <see cref="GitHubClient"/> instance.</param>
        public GitHubAddonDownloader(IHttpClientWrapper httpClientService, GitHubClient gitHubClient)
            : base(httpClientService)
        {
            _gitHubClient = gitHubClient ?? throw new ArgumentNullException(nameof(gitHubClient));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the instance of <see cref="IHttpClientWrapper"/> service.
        /// </summary>
        protected GitHubClient GitHubClientService => _gitHubClient;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public override Task<DownloadedObject> Download(DownloadRequest request)
        {
            throw new NotImplementedException();
        }

        #endregion methods
    }
}
