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
        #region Properties

        /// <summary>
        /// Gets or sets a collection of installed addons.
        /// </summary>
        public ObservableCollection<Gw2Addon> InstalledAddonsCollection { get; set; } = new ObservableCollection<Gw2Addon>();

        /// <summary>
        /// Gets or sets the currently selected add-on.
        /// </summary>
        public Gw2Addon SelectedAddon { get; set; }

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}
