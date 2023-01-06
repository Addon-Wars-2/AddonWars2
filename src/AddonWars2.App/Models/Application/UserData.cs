// ==================================================================================================
// <copyright file="UserData.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Models.Application
{
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Holds the user settings used by the application, as well as locally stored data.
    /// </summary>
    public class UserData
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserData"/> class.
        /// </summary>
        public UserData()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a list of available cultures.
        /// </summary>
        public ObservableCollection<CultureInfo> AvailableCultures => new ObservableCollection<CultureInfo>()
        {
            new CultureInfo("en-US", "EN", "English"),
            new CultureInfo("ru-RU", "RU", "Russian"),
        };

        /// <summary>
        /// Gets the default application culture.
        /// </summary>
        public CultureInfo DefaultCulture => new CultureInfo("en-US", "EN", "English");

        /// <summary>
        /// Gets or sets the part of <see cref="UserData"/> which is stored locally.
        /// </summary>
        public LocalData LocalData { get; set; }

        #endregion Properties
    }
}
