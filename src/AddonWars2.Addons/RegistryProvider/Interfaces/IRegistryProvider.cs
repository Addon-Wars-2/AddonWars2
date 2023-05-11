// ==================================================================================================
// <copyright file="IRegistryProvider.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons.AddonLibProvider.Interfaces
{
    using AddonWars2.Addons.Models.AddonInfo;

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
        /// <summary>
        /// Gets a collection of addons data stored within the library.
        /// </summary>
        /// <returns>A collection of addons.</returns>
        public IEnumerable<AddonInfo> GetAddonsInfo();
    }
}
