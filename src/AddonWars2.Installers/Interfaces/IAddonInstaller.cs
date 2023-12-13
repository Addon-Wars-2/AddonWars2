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
        /// Is raised whenever the installation progress has changed.
        /// </summary>
        public event InstallProgressChangedEventHandler? InstallProgressChanged;

        /// <summary>
        /// Is raised right before the installation process has started.
        /// </summary>
        public event EventHandler? InstallationStarted;

        /// <summary>
        /// Is raised after the installation process has completed.
        /// </summary>
        public event EventHandler? InstallationCompleted;

        /// <summary>
        /// Gets the installation entrypoint depending on the installer type.
        /// </summary>
        /// <remarks>
        /// The value returned is a path relative to the game directory path.
        /// </remarks>
        public string Entrypoint { get; }

        /// <summary>
        /// Installs a requested addon.
        /// </summary>
        /// <param name="installRequest">An installation request.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns><see cref="InstallResult"/> object.</returns>
        public Task<InstallResult> InstallAsync(InstallRequest installRequest, CancellationToken cancellationToken);
    }
}
