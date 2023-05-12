// ==================================================================================================
// <copyright file="RegistryProviderBase.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons.AddonLibProvider
{
    using System.Text.Json;
    using AddonWars2.Addons.Models.AddonInfo;
    using AddonWars2.Addons.RegistryProvider.Interfaces;
    using AddonWars2.Addons.RegistryProvider.Models;
    using AddonWars2.Services.HttpClientWrapper.Interfaces;
    using Octokit;

    /// <summary>
    /// Represents a base class for addons storage.
    /// </summary>
    public abstract class RegistryProviderBase : IRegistryProvider
    {
        #region Fields

        private static readonly string _approvedProvidersBranchName = "main";
        private readonly GitHubClient _gitHubClient;
        private readonly IHttpClientWrapper _httpClientWrapper;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryProviderBase"/> class.
        /// </summary>
        /// <param name="gitHubClient">A reference to <see cref="Octokit.GitHubClient"/> instance.</param>
        /// <param name="httpClientWrapper">A reference to <see cref="IHttpClientWrapper"/> instance.</param>
        public RegistryProviderBase(GitHubClient gitHubClient, IHttpClientWrapper httpClientWrapper)
        {
            _gitHubClient = gitHubClient;
            _httpClientWrapper = httpClientWrapper;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a repository branch name used to search for a list of approved providers.
        /// </summary>
        public static string ApprovedProvidersBranchName => _approvedProvidersBranchName;

        /// <summary>
        /// Gets GitHub client.
        /// </summary>
        protected GitHubClient GitHubClient => _gitHubClient;

        /// <summary>
        /// Gets a HTTP client wrapper.
        /// </summary>
        protected IHttpClientWrapper HttpClientWrapper => _httpClientWrapper;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<ProviderInfo>> GetApprovedProvidersAsync(long repositoryId, string path)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(path));

            var repository = await GitHubClient.Repository.Branch.Get(repositoryId, ApprovedProvidersBranchName);
            var contentList = await GitHubClient.Repository.Content.GetAllContents(repositoryId);

            var repositoryContent = contentList.FirstOrDefault(x => x?.Path == path, null);
            if (repositoryContent == null)
            {
                return new List<ProviderInfo>();
            }

            var response = await HttpClientWrapper.GetAsync(repositoryContent.DownloadUrl);
            using (var content = await response.Content.ReadAsStreamAsync())
            {
                var providers = await JsonSerializer.DeserializeAsync<ApprovedProviders>(content);
                return providers?.ApprovedProvidersCollection ?? new List<ProviderInfo>();
            }
        }

        /// <inheritdoc/>
        public abstract Task<AddonInfo> GetAddonsFromAsync(ProviderInfo provider);

        #endregion Methods
    }
}
