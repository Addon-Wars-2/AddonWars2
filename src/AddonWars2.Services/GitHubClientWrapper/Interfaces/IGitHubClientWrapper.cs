// ==================================================================================================
// <copyright file="IGitHubClientWrapper.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Services.GitHubClientWrapper.Interfaces
{
    using Octokit;

    /// <summary>
    /// Represents a contract for GitHub client wrappers.
    /// </summary>
    public interface IGitHubClientWrapper
    {
        #region Properties

        /// <summary>
        /// Gets the <see cref="Octokit.GitHubClient"/> object.
        /// </summary>
        public GitHubClient GitHubClient { get; }

        /// <summary>
        /// Gets or sets GitHub API token.
        /// </summary>
        public string ApiToken { get; set; }

        /// <summary>
        /// Gets the latest GitHub API info.
        /// </summary>
        /// <remarks>
        /// Returns <see langword="null"/> if no API calls have been made.
        /// </remarks>
        public ApiInfo GitHubLastApiInfo { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Checks of a given API token is valid.
        /// </summary>
        /// <param name="token">GitHub API token.</param>
        /// <returns><see langword="true"/> if valid, otherwise <see langword="false"/>.</returns>
        public Task<bool> CheckTokenValidityAsync(string token);

        #endregion Methods
    }
}
