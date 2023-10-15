﻿// ==================================================================================================
// <copyright file="ArcAddonInstaller.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers
{
    using AddonWars2.Installers.Models;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents an addon installer that installs ArcDPS plug-ins.
    /// </summary>
    public class ArcAddonInstaller : AddonInstallerBase
    {
        #region Fields

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ArcAddonInstaller"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/>.</param>
        public ArcAddonInstaller(ILogger<AddonInstallerBase> logger)
            : base(logger)
        {
            // Blank.
        }

        #endregion Constructors

        #region Events

        #endregion Events

        #region Properties

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public override Task<InstallResult> InstallAsync(InstallRequest installRequest)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Task<UninstallResult> UninstallAsync(UninstallRequest uninstallRequest)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}