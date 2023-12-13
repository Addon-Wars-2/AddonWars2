// ==================================================================================================
// <copyright file="AddonUninstallerFactory.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers.Factories
{
    using AddonWars2.Installers.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents a factory for addon uninstallers.
    /// </summary>
    public class AddonUninstallerFactory : IAddonUninstallerFactory
    {
        #region Fields

        private static ILogger<AddonUninstaller> _logger;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonUninstallerFactory"/> class.
        /// </summary>
        /// <param name="logger">A reference to base <see cref="ILogger"/>.</param>
        public AddonUninstallerFactory(ILogger<AddonUninstaller> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public IAddonUninstaller GetInstance()
        {
            return new AddonUninstaller(_logger);
        }

        #endregion Methods
    }
}
