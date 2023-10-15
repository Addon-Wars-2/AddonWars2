// ==================================================================================================
// <copyright file="InstalledAddon.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================


namespace AddonWars2.Repository.Models
{
    using AddonWars2.LibraryManager.Models;

    public class InstalledAddon
    {
        public int Id { get; set; }

        public string? InternalName { get; set; }

        public string? DisplayName { get; set; }

        public string? Description { get; set; }

        public string? ToolTip { get; set; }

        public string? Authors { get; set; }

        public string? Website {  get; set; }

        public List<InstalledAddonFile> InstalledAddonFiles { get; set; } = new List<InstalledAddonFile>();
    }
}
