// ==================================================================================================
// <copyright file="IRegistryProvider.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Providers.Interfaces
{
    using AddonWars2.Core.DTO;
    using AddonWars2.Providers.DTO;

    /// <summary>
    /// Provides a contract for addon libraries.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interfaces porovides a general interface for sources, such repositories,
    /// local storages or any other kind of libraries, which store the information
    /// about available addons.
    /// </para>
    /// <para>
    /// Note, that the interface doesn't deal with the actual addons storage.
    /// </para>
    /// </remarks>
    public interface IRegistryProvider
    {
        // TODO: Currently a list of approved providers is stored on GitHub repo and
        //       is accessed directly. It acts as the main entry point for addons search,
        //       and the search path is hardcoded within the application config data.
        //       Because multiple entry points are possible (all hardcoded or supplied by
        //       external config), I would prefer to separate "GetProviders" and "GetAddons"
        //       logic since these are two different tasks.

        /// <summary>
        /// Gets a collection of approved registry providers.
        /// It will be requested from the project repository as it's the only source
        /// of approved providers.
        /// </summary>
        /// <param name="repositoryId">A repository ID a list will be searched in.</param>
        /// <param name="path">A repository path pointing to a list of approved providers.</param>
        /// <returns>A collection of registry providers.</returns>
        public Task<IEnumerable<ProviderInfo>> GetApprovedProvidersAsync(long repositoryId, string path);

        /// <summary>
        /// Gets a collection of addons data stored within the library.
        /// </summary>
        /// <param name="provider">A provider to use.</param>
        /// <returns>A collection of addons.</returns>
        public Task<AddonsCollection> GetAddonsFromAsync(ProviderInfo provider);
    }
}
