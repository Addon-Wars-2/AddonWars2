// ==================================================================================================
// <copyright file="AddonsService.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Services
{
    using System.Collections.ObjectModel;
    using AddonWars2.Addons.Models.ModInfo;
    using AddonWars2.Downloader.Models;
    using CommunityToolkit.Mvvm.ComponentModel;

    /// <summary>
    /// Represents a class that encapsulates all operations on a collection of Guild Wars 2 add-ons.
    /// </summary>
    public class AddonsService : ObservableObject
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonsService"/> class.
        /// </summary>
        public AddonsService()
        {
            // Blank.
            var request = new StandaloneDownloadRequest("https://www.deltaconnected.com/arcdps/gw2addon_arcdps.dll");
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a collection of installed addons.
        /// </summary>
        public ObservableCollection<ModInfoData> InstalledAddonsCollection { get; private set; } = new ObservableCollection<ModInfoData>()
        {
            ////new ModInfoData("Addon 04"),
            ////new ModInfoData("Addon 05"),
            ////new ModInfoData("Addon 06"),
            ////new ModInfoData("Addon 01"),
            ////new ModInfoData("Addon 02"),
            ////new ModInfoData("Addon 03"),
            ////new ModInfoData("Addon 07"),
            ////new ModInfoData("Addon 08"),
            ////new ModInfoData("Addon 09"),
            ////new ModInfoData("Addon 10"),
            ////new ModInfoData("Addon 11"),
            ////new ModInfoData("Addon 12"),
            ////new ModInfoData("Addon 13"),
            ////new ModInfoData("Addon 14"),
            ////new ModInfoData("Addon 15"),
            ////new ModInfoData("Addon 16"),
            ////new ModInfoData("Addon 17"),
            ////new ModInfoData("Addon 18"),
        };

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}
