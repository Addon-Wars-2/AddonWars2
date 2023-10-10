// ==================================================================================================
// <copyright file="LocalRegistryProvider.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Providers
{
    using System.Collections.Generic;
    using System.Text.Json;
    using AddonWars2.Core.DTO;
    using AddonWars2.Providers.DTO;
    using AddonWars2.Services.HttpClientWrapper.Interfaces;
    using Octokit;

    /// <summary>
    /// Represents a local (cached) registry provider that keeps the information
    /// about addons which were cached on load.
    /// </summary>
    public class LocalRegistryProvider : RegistryProviderBase
    {
        #region Fields

        /// <summary>
        /// <see cref="FileStream"/> buffer size.
        /// </summary>
        private const int FILE_STREAM_BUFFER_SIZE = 4096;

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
        public override async Task<IEnumerable<ProviderInfo>> GetProvidersAsync(string path, long repositoryId)
        {
            using (var stream = new FileStream(path, System.IO.FileMode.Open, FileAccess.Read, FileShare.ReadWrite, FILE_STREAM_BUFFER_SIZE, useAsync: true))
            {
                var providers = await JsonSerializer.DeserializeAsync<ProvidersCollection>(stream);
                return providers?.Providers ?? new List<ProviderInfo>();
            }
        }

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
