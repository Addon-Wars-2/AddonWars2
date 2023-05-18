// ==================================================================================================
// <copyright file="GitHubClientWrapper.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Services.GitHubClientWrapper
{
    using AddonWars2.Services.GitHubClientWrapper.Interfaces;
    using AddonWars2.SharedData;
    using Octokit;

    /// <summary>
    /// Represents a <see cref="Octokit.GitHubClient"/> wrapper.
    /// </summary>
    public class GitHubClientWrapper : IGitHubClientWrapper
    {
        #region Fields

        private readonly GitHubClient _gitHubClient;
        private readonly IWebSharedData _webSharedData;

        private string _apiToken = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubClientWrapper"/> class.
        /// </summary>
        /// <param name="gitHubClient">A reference to a <see cref="Octokit.GitHubClient"/> instance.</param>
        /// <param name="webSharedData">A reference to <see cref="IWebSharedData"/> instance.</param>
        public GitHubClientWrapper(
            GitHubClient gitHubClient,
            IWebSharedData webSharedData)
        {
            _gitHubClient = gitHubClient ?? throw new ArgumentNullException(nameof(gitHubClient));
            _webSharedData = webSharedData ?? throw new ArgumentNullException(nameof(webSharedData));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the <see cref="Octokit.GitHubClient"/> object.
        /// </summary>
        public GitHubClient GitHubClient => _gitHubClient;

        /// <summary>
        /// Gets a reference to the application web-related static data.
        /// </summary>
        public IWebSharedData WebSharedData => _webSharedData;

        /// <inheritdoc/>
        public string ApiToken
        {
            get => _apiToken;
            set
            {
                GitHubClient.Credentials = string.IsNullOrEmpty(value) ? Credentials.Anonymous : new Credentials(value);
                _apiToken = value;
            }
        }

        /// <inheritdoc/>
        public ApiInfo GitHubLastApiInfo => GitHubClient.GetLastApiInfo();

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public async Task<bool> CheckTokenValidityAsync(string token)
        {
            // Empty token is valid and means that there is no token and API
            // access is done anonymously.
            if (token == null)
            {
                return false;
            }

            var oldToken = new string(ApiToken);
            ApiToken = token;

            try
            {
                await GitHubClient.Repository.Get(WebSharedData.GitHubProjectRepositoryId);
            }
            catch (ApiException)
            {
                ApiToken = oldToken;
                return false;
            }

            return true;
        }

        #endregion Method
    }
}
