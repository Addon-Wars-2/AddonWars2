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
    using AddonWars2.SharedData.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents a factory for addon installers.
    /// </summary>
    public class AddonInstallerFactory : IAddonInstallerFactory
    {
        #region Fields

        private static ILogger<AddonInstallerBase> _logger;
        private readonly IGameSharedData _gameSharedData;
        private readonly IInstallerCustomActionFactory _installerCustomActionFactory;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonInstallerFactory"/> class.
        /// </summary>
        /// <param name="logger">A reference to base <see cref="ILogger"/>.</param>
        /// <param name="gameSharedData">A reference to <see cref="IGameSharedData"/>.</param>
        /// <param name="installerCustomActionFactory">A reference to <see cref="IInstallerCustomActionFactory"/>.</param>
        public AddonInstallerFactory(
            ILogger<AddonInstallerBase> logger,
            IGameSharedData gameSharedData,
            IInstallerCustomActionFactory installerCustomActionFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _gameSharedData = gameSharedData ?? throw new ArgumentNullException(nameof(gameSharedData));
            _installerCustomActionFactory = installerCustomActionFactory ?? throw new ArgumentNullException(nameof(installerCustomActionFactory));
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public IAddonInstaller GetInstance(InstallMode installMode)
        {
            switch (installMode)
            {
                case InstallMode.Root:
                    return new RootAddonInstaller(_logger, _gameSharedData, _installerCustomActionFactory);
                case InstallMode.Arc:
                    return new ArcAddonInstaller(_logger, _gameSharedData, _installerCustomActionFactory);
                case InstallMode.Binary:
                    return new BinaryAddonInstaller(_logger, _gameSharedData, _installerCustomActionFactory);
                default:
                    throw new NotSupportedException($"Cannot create an installer for the install mode: {installMode}. The install mode is not supported.");
            }
        }

        #endregion Methods
    }
}
