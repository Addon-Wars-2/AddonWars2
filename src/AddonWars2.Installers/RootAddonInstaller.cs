﻿// ==================================================================================================
// <copyright file="RootAddonInstaller.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers
{
    using AddonWars2.Installers.Interfaces;
    using AddonWars2.SharedData.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents an addon installer that installs files into the game's root directory.
    /// </summary>
    public class RootAddonInstaller : AddonInstallerBase
    {
        #region Fields

        private readonly string _entrypoint;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RootAddonInstaller"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/>.</param>
        /// <param name="gameSharedData">A reference to <see cref="IGameSharedData"/>.</param>
        /// <param name="ruleApplierFactory">A reference to <see cref="IInstallerCustomActionFactory"/>.</param>
        public RootAddonInstaller(
            ILogger<AddonInstallerBase> logger,
            IGameSharedData gameSharedData,
            IInstallerCustomActionFactory ruleApplierFactory)
            : base(logger, gameSharedData, ruleApplierFactory)
        {
            _entrypoint = string.Empty;
        }

        #endregion Constructors

        #region Events

        #endregion Events

        #region Properties

        /// <inheritdoc/>
        public override string Entrypoint => _entrypoint;

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}
