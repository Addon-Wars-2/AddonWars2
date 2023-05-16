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
        private readonly IHttpClientWrapper _httpClientService;
        private readonly GitHubClient _gitHubClientService;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryProviderBase"/> class.
        /// </summary>
        /// <param name="httpClientService">A reference to <see cref="IHttpClientWrapper"/> instance.</param>
        /// <param name="gitHubClient">A reference to <see cref="Octokit.GitHubClient"/> instance.</param>
        public RegistryProviderBase(IHttpClientWrapper httpClientService, GitHubClient gitHubClient)
        {
            _httpClientService = httpClientService ?? throw new ArgumentNullException(nameof(httpClientService));
            _gitHubClientService = gitHubClient ?? throw new ArgumentNullException(nameof(gitHubClient));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a repository branch name used to search for a list of approved providers.
        /// </summary>
        public static string ApprovedProvidersBranchName => _approvedProvidersBranchName;

        /// <summary>
        /// Gets a HTTP client wrapper.
        /// </summary>
        protected IHttpClientWrapper HttpClientWrapper => _httpClientService;

        /// <summary>
        /// Gets GitHub client.
        /// </summary>
        protected GitHubClient GitHubClientService => _gitHubClientService;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<ProviderInfo>> GetApprovedProvidersAsync(long repositoryId, string path)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(path));

            var contentList = await GitHubClientService.Repository.Content.GetAllContents(repositoryId);
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
        public abstract Task<AddonsCollection> GetAddonsFromAsync(ProviderInfo provider);

        #endregion Methods
    }
}
