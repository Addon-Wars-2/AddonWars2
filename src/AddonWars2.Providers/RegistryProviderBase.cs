// ==================================================================================================
// <copyright file="RegistryProviderBase.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Providers
{
    using AddonWars2.Core.DTO;
    using AddonWars2.Providers.DTO;
    using AddonWars2.Providers.Interfaces;
    using AddonWars2.Services.HttpClientWrapper.Interfaces;
    using Octokit;

    /// <summary>
    /// Represents a base class for addons storage.
    /// </summary>
    public abstract class RegistryProviderBase : IRegistryProvider
    {
        #region Fields

        private readonly IHttpClientWrapper _httpClientService;
        private readonly GitHubClient _gitHubClientService;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryProviderBase"/> class.
        /// </summary>
        /// <param name="httpClientService">A reference to <see cref="IHttpClientWrapper"/> instance.</param>
        /// <param name="gitHubClient">A reference to <see cref="GitHubClient"/> instance.</param>
        public RegistryProviderBase(IHttpClientWrapper httpClientService, GitHubClient gitHubClient)
        {
            _httpClientService = httpClientService ?? throw new ArgumentNullException(nameof(httpClientService));
            _gitHubClientService = gitHubClient ?? throw new ArgumentNullException(nameof(gitHubClient));
        }

        #endregion Constructors

        #region Properties

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
        public abstract Task<IEnumerable<ProviderInfo>> GetProvidersAsync(string path, long repositoryId);

        /// <inheritdoc/>
        public abstract Task<AddonsCollection> GetAddonsFromAsync(ProviderInfo provider);

        #endregion Methods
    }
}
