// ==================================================================================================
// <copyright file="IAddonInstaller.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers.Interfaces
{
    using AddonWars2.Core.Interfaces;
    using AddonWars2.Installers.Models;

    /// <summary>
    /// Specifies a contract for addon installers.
    /// </summary>
    public interface IAddonInstaller : IAttachableProgress
    {
        /// <summary>
        /// Installs a requested addon.
        /// </summary>
        /// <param name="installRequest">The installation request.</param>
        /// <returns><see cref="InstallResult"/> object.</returns>
        public Task<InstallResult> InstallAsync(InstallRequest installRequest);

        /// <summary>
        /// Uninstalls a requested addon.
        /// </summary>
        /// <param name="uninstallRequest">The uninstallation request.</param>
        /// <returns><see cref="UninstallResult"/> object.</returns>
        public Task<UninstallResult> UninstallAsync(UninstallRequest uninstallRequest);
    }
}
