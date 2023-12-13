// ==================================================================================================
// <copyright file="IAddonUninstallerFactory.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers.Interfaces
{
    /// <summary>
    /// Specifies a contract for addon uninstaller factories.
    /// </summary>
    public interface IAddonUninstallerFactory
    {
        /// <summary>
        /// Creates a new uninstaller.
        /// </summary>
        /// <returns>A new uninstaller.</returns>
        public IAddonUninstaller GetInstance();
    }
}
