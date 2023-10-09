// ==================================================================================================
// <copyright file="ILibraryManager.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Core.Interfaces
{
    using AddonWars2.Packages.Interfaces;

    /// <summary>
    /// Specified a contract for the addon library managers.
    /// </summary>
    public interface ILibraryManager
    {
        /// <summary>
        /// Installs a new addon package.
        /// </summary>
        /// <remarks>
        /// An addon package is a package that contains all necessary files
        /// and installation instructions. A package can contain one or more
        /// addons to install.
        /// </remarks>
        /// <param name="package">An addon package.</param>
        public void InstallPackage(IAddonPackage package);
    }
}
