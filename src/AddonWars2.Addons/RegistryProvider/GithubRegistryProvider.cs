// ==================================================================================================
// <copyright file="GithubRegistryProvider.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons.RegistryProvider
{
    using AddonWars2.Addons.AddonLibProvider;
    using AddonWars2.Addons.Models.AddonInfo;
    using AddonWars2.Addons.RegistryProvider.Models;
    using Octokit;

    /// <summary>
    /// Represents a Github storage that keeps the information about addons.
    /// </summary>
    public class GithubRegistryProvider : RegistryProviderBase
    {
        #region Fields

        ////private readonly GitHubClient _gitHubClient;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GithubRegistryProvider"/> class.
        /// </summary>
        /// <param name="gitHubClient">A reference to <see cref="Octokit.GitHubClient"/> instance.</param>
        public GithubRegistryProvider(GitHubClient gitHubClient)
            : base(gitHubClient)
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /////// <summary>
        /////// Gets GitHub client.
        /////// </summary>
        ////protected GitHubClient GitHubClient => _gitHubClient;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public override async Task<IEnumerable<AddonInfo>> GetAddonsFromAsync(ProviderInfo provider)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}
