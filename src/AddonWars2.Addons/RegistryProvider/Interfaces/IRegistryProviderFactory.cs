// ==================================================================================================
// <copyright file="IRegistryProviderFactory.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons.RegistryProvider.Interfaces
{
    using AddonWars2.Addons.RegistryProvider.Models;

    /// <summary>
    /// Represents a contact for addon downloader factories.
    /// </summary>
    public interface IRegistryProviderFactory
    {
        /// <summary>
        /// Returns a new downloader based on host type.
        /// </summary>
        /// <param name="hostType">The provider host type used to determine the provider type.</param>
        /// <returns>A new provider.</returns>
        IRegistryProvider GetProvider(ProviderInfoHostType hostType);
    }
}
