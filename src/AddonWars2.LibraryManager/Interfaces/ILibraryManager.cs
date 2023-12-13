// ==================================================================================================
// <copyright file="ILibraryManager.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Core.Interfaces
{
    using AddonWars2.Installers.Interfaces;
    using AddonWars2.Installers.Models;

    /// <summary>
    /// Specified a contract for the addon library managers.
    /// </summary>
    public interface ILibraryManager
    {
        /// <summary>
        /// Installs a new addon.
        /// </summary>
        /// <param name="installer">An addon installer to use.</param>
        /// <param name="request">The installation request.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns><see cref="Task{InstallResult}"/> object.</returns>
        public Task<InstallResult> InstallAddonAsync(IAddonInstaller installer, InstallRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Uninstalls the requested addon.
        /// </summary>
        /// <param name="uninstaller">An addon uninstaller to use.</param>
        /// <param name="request">The uninstallation request.</param>
        /// <returns><see cref="Task{UninstallResult}"/> object.</returns>
        public Task<UninstallResult> UninstallAddonAsync(IAddonUninstaller uninstaller, UninstallRequest request);
    }
}
