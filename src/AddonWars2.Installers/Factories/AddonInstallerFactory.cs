// ==================================================================================================
// <copyright file="AddonInstallerFactory.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers.Factories
{
    using AddonWars2.Core.Enums;
    using AddonWars2.Installers.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents a factory for addon installers.
    /// </summary>
    public class AddonInstallerFactory : IAddonInstallerFactory
    {
        #region Fields

        private static ILogger<AddonInstallerBase> _logger;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonInstallerFactory"/> class.
        /// </summary>
        /// <param name="logger">A reference to base <see cref="ILogger"/>.</param>
        public AddonInstallerFactory(
            ILogger<AddonInstallerBase> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public IAddonInstaller GetAddonInstaller(InstallMode installMode)
        {
            switch (installMode)
            {
                case InstallMode.Root:
                    return new RootAddonInstaller(_logger);
                case InstallMode.Arc:
                    return new ArcAddonInstaller(_logger);
                case InstallMode.Binary:
                    return new BinaryAddonInstaller(_logger);
                default:
                    throw new NotSupportedException($"Cannot create an installer for the install mode: {installMode}. The install mode is not supported.");
            }
        }

        #endregion Methods
    }
}
