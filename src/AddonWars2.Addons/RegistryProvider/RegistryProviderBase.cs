// ==================================================================================================
// <copyright file="RegistryProviderBase.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons.AddonLibProvider
{
    using System.Diagnostics;
    using AddonWars2.Addons.Models.AddonInfo;
    using AddonWars2.Addons.RegistryProvider.Interfaces;
    using AddonWars2.Addons.RegistryProvider.Models;
    using Octokit;

    /// <summary>
    /// Represents a base class for addons storage.
    /// </summary>
    public abstract class RegistryProviderBase : IRegistryProvider
    {
        #region Fields

        private readonly GitHubClient _gitHubClient;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryProviderBase"/> class.
        /// </summary>
        /// <param name="gitHubClient">A reference to <see cref="Octokit.GitHubClient"/> instance.</param>
        public RegistryProviderBase(GitHubClient gitHubClient)
        {
            _gitHubClient = gitHubClient;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets GitHub client.
        /// </summary>
        protected GitHubClient GitHubClient => _gitHubClient;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<ProviderInfo>> GetApprovedProvidersAsync(long repositoryId, string path)
        {
            var repository = await GitHubClient.Repository.Branch.Get(repositoryId, path);
            var contentList = await GitHubClient.Repository.Content.GetAllContents(repositoryId);
            foreach (var item in contentList)
            {
                Debug.WriteLine(item.Path);
            }

            var content = contentList.FirstOrDefault(x => x?.Path == path, null);

            Debug.WriteLine(content?.DownloadUrl);

            return null;
        }

        /// <inheritdoc/>
        public abstract Task<IEnumerable<AddonInfo>> GetAddonsFromAsync(ProviderInfo provider);

        #endregion Methods
    }
}
