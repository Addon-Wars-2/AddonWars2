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
    using AddonWars2.Services.GitHubClientWrapper.Interfaces;
    using AddonWars2.Services.HttpClientWrapper.Interfaces;
    using Octokit;

    /// <summary>
    /// Represens a downloader used to download addons from GitHub repositories.
    /// </summary>
    public class GitHubAddonDownloader : AddonDownloaderBase
    {
        #region Fields

        private readonly IGitHubClientWrapper _gitHubClientService;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubAddonDownloader"/> class.
        /// </summary>
        /// <param name="httpClientService">A reference to <see cref="IHttpClientWrapper"/> instance.</param>
        /// <param name="gitHubClientService">A reference to <see cref="IGitHubClientWrapper"/> instance.</param>
        public GitHubAddonDownloader(IHttpClientWrapper httpClientService, IGitHubClientWrapper gitHubClientService)
            : base(httpClientService)
        {
            _gitHubClientService = gitHubClientService ?? throw new ArgumentNullException(nameof(gitHubClientService));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the instance of <see cref="IHttpClientWrapper"/> service.
        /// </summary>
        protected IGitHubClientWrapper GitHubClientService => _gitHubClientService;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        protected async override Task<DownloadedObject> DownloadAsync(DownloadRequest request)
        {
            var gitHubResponse = await GitHubClientService.GitHubClient.Connection.Get<Release>(new Uri(request.Url), TimeSpan.FromSeconds(30));
            var release = gitHubResponse.Body;
            if (release != null && release.Assets.Count > 0)
            {
                var url = release.Assets[0].BrowserDownloadUrl;
                var version = release.TagName;
                using (var response = await HttpClientService.GetAsync(url))
                {
                    var content = await ReadResponseAsync(response);
                    content.Version = version;

                    return content;
                }
            }
            else
            {
                throw new InvalidOperationException($"The returned release is either null or has no assets.");
            }
        }

        #endregion Methods
    }
}
