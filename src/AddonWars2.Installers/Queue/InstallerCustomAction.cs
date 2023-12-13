// ==================================================================================================
// <copyright file="InstallerCustomAction.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers.Queue
{
    using System.Collections.ObjectModel;
    using AddonWars2.Core.DTO.Actions;
    using AddonWars2.Installers.Models;

    /// <summary>
    /// Represents a custom installer action.
    /// </summary>
    public class InstallerCustomAction : InstallerActionBase
    {
        #region Fields

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallerCustomAction"/> class.
        /// </summary>
        /// <param name="request">The installation request.</param>
        /// <param name="addonAction">An action to apply.</param>
        /// <param name="result">A collection of installed items.</param>
        public InstallerCustomAction(InstallRequest request, AddonActionBase addonAction, ObservableCollection<InstallResultFile> result)
        {
            // Blank.
        }

        #endregion Constructors

        #region Events

        #endregion Events

        #region Properties

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}
