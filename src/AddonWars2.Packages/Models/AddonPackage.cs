// ==================================================================================================
// <copyright file="AddonPackage.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Packages.Models
{
    using AddonWars2.Installers.Models;
    using AddonWars2.Packages.Interfaces;

    /// <summary>
    /// Represents a package containing a single addon ready for installation
    /// including installation instructions, additional files, metadata, etc.
    /// </summary>
    public class AddonPackage : IAddonPackage
    {
        public AddonPackage(InstallRequest installRequest)
        {

        }
    }
}
