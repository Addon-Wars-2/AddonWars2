// ==================================================================================================
// <copyright file="AddonsManager.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Controllers
{
    using System.Collections.ObjectModel;
    using AddonWars2.Addons;
    using CommunityToolkit.Mvvm.ComponentModel;

    /// <summary>
    /// Represents a class that encapsulates all operations on a collection
    /// of Guild Wars 2 add-ons.
    /// </summary>
    public class AddonsManager : ObservableObject
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonsManager"/> class.
        /// </summary>
        public AddonsManager()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a collection of installed addons.
        /// </summary>
        public ObservableCollection<Gw2Addon> InstalledAddonsCollection { get; private set; } = new ObservableCollection<Gw2Addon>()
        {
            new Gw2Addon("Addon 04"),
            new Gw2Addon("Addon 05"),
            new Gw2Addon("Addon 06"),
            new Gw2Addon("Addon 01"),
            new Gw2Addon("Addon 02"),
            new Gw2Addon("Addon 03"),
            new Gw2Addon("Addon 07"),
            new Gw2Addon("Addon 08"),
            new Gw2Addon("Addon 09"),
            new Gw2Addon("Addon 10"),
            new Gw2Addon("Addon 11"),
            new Gw2Addon("Addon 12"),
            new Gw2Addon("Addon 13"),
            new Gw2Addon("Addon 14"),
            new Gw2Addon("Addon 15"),
            new Gw2Addon("Addon 16"),
            new Gw2Addon("Addon 17"),
            new Gw2Addon("Addon 18"),
        };

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}
