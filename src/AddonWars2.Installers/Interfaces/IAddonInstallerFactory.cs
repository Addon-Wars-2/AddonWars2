// ==================================================================================================
// <copyright file="IAddonInstallerFactory.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers.Interfaces
{
    using AddonWars2.Core.Enums;

    /// <summary>
    /// Specifies a contract for addon installer factories.
    /// </summary>
    public interface IAddonInstallerFactory
    {
        /// <summary>
        /// Creates a new installer based on an install mode.
        /// </summary>
        /// <param name="installMode">The addon install mode.</param>
        /// <returns>A new installer.</returns>
        public IAddonInstaller GetAddonInstaller(InstallMode installMode);
    }
}
