// ==================================================================================================
// <copyright file="LibraryManager.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Core
{
    using System.Diagnostics;
    using AddonWars2.Core.DTO;
    using AddonWars2.Core.Interfaces;
    using AddonWars2.Downloaders.Interfaces;
    using AddonWars2.Downloaders.Models;
    using AddonWars2.LibraryManager.Exceptions;

    /// <summary>
    /// Represent a manager responsible for operations with the addon library on a user's local machine.
    /// </summary>
    public class LibraryManager : ILibraryManager
    {
        #region Fields

        private readonly IAddonDownloaderFactory _addonDownloaderFactory;

        #endregion Fields

        #region Constructors

        public LibraryManager(IAddonDownloaderFactory addonDownloaderFactory)
        {
            _addonDownloaderFactory = addonDownloaderFactory ?? throw new ArgumentNullException(nameof(addonDownloaderFactory));
        }

        #endregion Constructors

        #region Properties

        protected IAddonDownloaderFactory AddonDownloaderFactory => _addonDownloaderFactory;

        #endregion Properties

        #region Methods

        

        #endregion Methods
    }
}
