// ==================================================================================================
// <copyright file="InstalledAddonFile.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================


namespace AddonWars2.LibraryManager.Models
{
    using AddonWars2.Repository.Models;

    public class InstalledAddonFile
    {
        public int Id { get; set; }

        public int InstalledAddonId { get; set; }

        public InstalledAddon? InstalledAddon {  get; set; } 

        public string? FilePath { get; set; }

        public string? Hash { get; set; }
}
}
