﻿// ==================================================================================================
// <copyright file="InstallInstructions.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers.Models
{
    using AddonWars2.Core.DTO.Actions;

    /// <summary>
    /// Represents installation instruction for an addon.
    /// </summary>
    public class InstallInstructions
    {
        #region Fields

        private readonly IEnumerable<AddonActionBase> _installActions = new List<AddonActionBase>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallInstructions"/> class.
        /// </summary>
        public InstallInstructions()
        {
            // Blank.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallInstructions"/> class.
        /// </summary>
        /// <param name="installActions">A collection of installation actions.</param>
        public InstallInstructions(IEnumerable<AddonActionBase> installActions)
        {
            ArgumentNullException.ThrowIfNull(installActions, nameof(installActions));

            _installActions = installActions;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a collection of installation actions.
        /// </summary>
        public IEnumerable<AddonActionBase> InstallActions => _installActions;

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}
