// ==================================================================================================
// <copyright file="IAddonUninstaller.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers.Interfaces
{
    using AddonWars2.Core.Interfaces;
    using AddonWars2.Installers.Models;

    /// <summary>
    /// Specifies a contract for addon uninstallers.
    /// </summary>
    public interface IAddonUninstaller : IAttachableProgress
    {
        /// <summary>
        /// Is raised whenever the uninstallation progress has changed.
        /// </summary>
        public event UninstallProgressChangedEventHandler? UninstallProgressChanged;

        /// <summary>
        /// Is raised right before the uninstallation process has started.
        /// </summary>
        public event EventHandler? UninstallationStarted;

        /// <summary>
        /// Is raised after the uninstallation process has completed.
        /// </summary>
        public event EventHandler? UninstallationCompleted;

        /// <summary>
        /// Uninstalls a requested addon.
        /// </summary>
        /// <param name="uninstallRequest">An uninstallation request.</param>
        /// <returns><see cref="UninstallResult"/> object.</returns>
        public Task<UninstallResult> UninstallAsync(UninstallRequest uninstallRequest);
    }
}
