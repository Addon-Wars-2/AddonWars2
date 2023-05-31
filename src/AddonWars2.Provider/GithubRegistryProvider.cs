// ==================================================================================================
// <copyright file="GithubRegistryProvider.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Provider
{
    using System.Text.Json;
    using AddonWars2.Core.DTO;
    using AddonWars2.Provider.Models;
    using AddonWars2.Services.HttpClientWrapper.Interfaces;
    using Octokit;

    /// <summary>
    /// Represents a Github storage that keeps the information about addons.
    /// </summary>
    public class GithubRegistryProvider : RegistryProviderBase
    {
        #region Fields

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GithubRegistryProvider"/> class.
        /// </summary>
        /// <param name="httpClientService">A reference to <see cref="IHttpClientWrapper"/> instance.</param>
        /// <param name="gitHubClient">A reference to <see cref="GitHubClient"/> instance.</param>
        public GithubRegistryProvider(IHttpClientWrapper httpClientService, GitHubClient gitHubClient)
            : base(httpClientService, gitHubClient)
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public override async Task<AddonsCollection> GetAddonsFromAsync(ProviderInfo provider)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(provider));

            var response = await HttpClientWrapper.GetAsync(provider.Link);
            using (var content = await response.Content.ReadAsStreamAsync())
            {
                var addons = await JsonSerializer.DeserializeAsync<AddonsCollection>(content);
                return addons ?? new AddonsCollection();
            }
        }

        #endregion Methods
    }
}
