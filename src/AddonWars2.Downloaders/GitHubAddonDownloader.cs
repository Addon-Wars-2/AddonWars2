﻿// ==================================================================================================
// <copyright file="GitHubAddonDownloader.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloaders
{
    using System.Threading.Tasks;
    using AddonWars2.Downloaders.Exceptions;
    using AddonWars2.Downloaders.Models;
    using AddonWars2.Services.GitHubClientWrapper.Interfaces;
    using AddonWars2.Services.HttpClientWrapper.Interfaces;
    using Microsoft.Extensions.Logging;
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
        /// <param name="logger">A reference to <see cref="ILogger"/>.</param>
        /// <param name="httpClientService">A reference to <see cref="IHttpClientWrapper"/> instance.</param>
        /// <param name="gitHubClientService">A reference to <see cref="IGitHubClientWrapper"/> instance.</param>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="logger"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="httpClientService"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="gitHubClientService"/> is <see langword="null"/>.</exception>
        public GitHubAddonDownloader(ILogger<AddonDownloaderBase> logger, IHttpClientWrapper httpClientService, IGitHubClientWrapper gitHubClientService)
            : base(logger, httpClientService)
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
        protected async override Task<DownloadResult> DownloadAsync(DownloadRequest request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(nameof(request));

            OnDownloadStarted();

            var gitHubResponse = await GitHubClientService.GitHubClient.Connection.Get<Release>(new Uri(request.Url), TimeSpan.FromSeconds(30));

            Logger.LogInformation($"Downloading from {request.Url} using {typeof(GitHubAddonDownloader).Name}.");

            var release = gitHubResponse.Body;
            if (release != null && release.Assets.Count > 0)
            {
                var url = release.Assets[0].BrowserDownloadUrl;
                var version = release.TagName;
                var filename = release.Assets[0].Name;
                using (var response = await HttpClientService.GetAsync(url))
                {
                    var content = await ReadResponseAsync(response, filename, cancellationToken);
                    content.Version = version;

                    OnDownloadCompleted();

                    return content;
                }
            }
            else
            {
                throw new AddonDownloaderException($"The returned release is either null or has no assets.");
            }
        }

        #endregion Methods
    }
}
