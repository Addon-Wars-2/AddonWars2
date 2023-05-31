// ==================================================================================================
// <copyright file="LocalRegistryProvider.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Provider
{
    using System.Text.Json;
    using AddonWars2.Core.DTO;
    using AddonWars2.Provider.DTO;
    using AddonWars2.Services.HttpClientWrapper.Interfaces;
    using Octokit;

    /// <summary>
    /// Represents a local (cached) registry provider that keeps the information
    /// about addons which were cached on load.
    /// </summary>
    public class LocalRegistryProvider : RegistryProviderBase
    {
        #region Fields

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalRegistryProvider"/> class.
        /// </summary>
        /// <param name="httpClientService">A reference to <see cref="IHttpClientWrapper"/> instance.</param>
        /// <param name="gitHubClient">A reference to <see cref="GitHubClient"/> instance.</param>
        public LocalRegistryProvider(IHttpClientWrapper httpClientService, GitHubClient gitHubClient)
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

            var path = provider.Link;
            if (!File.Exists(path))
            {
                return new AddonsCollection();
            }

            using (var stream = File.OpenRead(path))
            {
                var addons = await JsonSerializer.DeserializeAsync<AddonsCollection>(stream);
                return addons ?? new AddonsCollection();
            }
        }

        #endregion Methods
    }
}
