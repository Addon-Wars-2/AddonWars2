// ==================================================================================================
// <copyright file="RegistryProviderFactory.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Providers.Factories
{
    using AddonWars2.Providers;
    using AddonWars2.Providers.Enums;
    using AddonWars2.Providers.Interfaces;
    using AddonWars2.Services.HttpClientWrapper.Interfaces;
    using Octokit;

    /// <summary>
    /// Represents a factory for registry providers.
    /// </summary>
    public class RegistryProviderFactory : IRegistryProviderFactory
    {
        #region Fields

        private readonly IHttpClientWrapper _httpClientService;
        private readonly GitHubClient _gitHubClientService;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryProviderFactory"/> class.
        /// </summary>
        /// <param name="httpClientService">A reference to <see cref="IHttpClientWrapper"/> instance.</param>
        /// <param name="gitHubClient">A reference to <see cref="GitHubClient"/> instance.</param>
        public RegistryProviderFactory(IHttpClientWrapper httpClientService, GitHubClient gitHubClient)
        {
            _httpClientService = httpClientService ?? throw new ArgumentNullException(nameof(httpClientService));
            _gitHubClientService = gitHubClient ?? throw new ArgumentNullException(nameof(gitHubClient));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the instance of <see cref="IHttpClientWrapper"/> service.
        /// </summary>
        protected IHttpClientWrapper HttpClientService => _httpClientService;

        /// <summary>
        /// Gets the instance of <see cref="IHttpClientWrapper"/> service.
        /// </summary>
        protected GitHubClient GitHubClientService => _gitHubClientService;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public IRegistryProvider GetProvider(ProviderInfoHostType hostType)
        {
            switch (hostType)
            {
                case ProviderInfoHostType.GitHub:
                    return new GithubRegistryProvider(HttpClientService, GitHubClientService);
                case ProviderInfoHostType.Standalone:
                    throw new NotImplementedException();  // TODO: implementation
                case ProviderInfoHostType.Local:
                    return new LocalRegistryProvider(HttpClientService, GitHubClientService);
                default:
                    throw new NotSupportedException($"Cannot create a provider for the host type: {hostType.GetType().Name}. The host type is not supported.");
            }
        }

        #endregion Methods
    }
}
