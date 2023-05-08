// ==================================================================================================
// <copyright file="AddonsService.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Services
{
    using System.Collections.ObjectModel;
    using AddonWars2.Addons.Models.AddonInfo;
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
            
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a collection of installed addons.
        /// </summary>
        public ObservableCollection<AddonInfoData> InstalledAddonsCollection { get; private set; } = new ObservableCollection<AddonInfoData>()
        {
            ////new AddonInfoData("Addon 04"),
            ////new AddonInfoData("Addon 05"),
            ////new AddonInfoData("Addon 06"),
            ////new AddonInfoData("Addon 01"),
            ////new AddonInfoData("Addon 02"),
            ////new AddonInfoData("Addon 03"),
            ////new AddonInfoData("Addon 07"),
            ////new AddonInfoData("Addon 08"),
            ////new AddonInfoData("Addon 09"),
            ////new AddonInfoData("Addon 10"),
            ////new AddonInfoData("Addon 11"),
            ////new AddonInfoData("Addon 12"),
            ////new AddonInfoData("Addon 13"),
            ////new AddonInfoData("Addon 14"),
            ////new AddonInfoData("Addon 15"),
            ////new AddonInfoData("Addon 16"),
            ////new AddonInfoData("Addon 17"),
            ////new AddonInfoData("Addon 18"),
        };

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}
