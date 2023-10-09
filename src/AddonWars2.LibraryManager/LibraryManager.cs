// ==================================================================================================
// <copyright file="LibraryManager.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Core
{
    using AddonWars2.Core.Interfaces;
    using AddonWars2.Downloaders.Interfaces;
    using AddonWars2.Extractors.Interfaces;
    using AddonWars2.Installers.Interfaces;
    using AddonWars2.Packages.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A class that repserents a state of a local library.
    /// It is responsible for installing addon packages, uninstalling addons and their
    /// dependencies and updating installed addons.
    /// </summary>
    public class LibraryManager : ILibraryManager
    {
        #region Fields

        private static ILogger _logger;
        private readonly IAddonDownloaderFactory _addonDownloaderFactory;
        private readonly IAddonExtractorFactory _addonExtractorFactory;
        private readonly IAddonInstallerFactory _addonInstallerFactory;

        #endregion Fields

        #region Constructors

        public LibraryManager(
            ILogger<LibraryManager> logger,
            IAddonDownloaderFactory addonDownloaderFactory,
            IAddonExtractorFactory addonExtractorFactory,
            IAddonInstallerFactory addonInstallerFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _addonDownloaderFactory = addonDownloaderFactory ?? throw new ArgumentNullException(nameof(addonDownloaderFactory));
            _addonExtractorFactory = addonExtractorFactory ?? throw new ArgumentNullException(nameof(addonExtractorFactory));
            _addonInstallerFactory = addonInstallerFactory ?? throw new ArgumentNullException(nameof(addonInstallerFactory));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the current logger instance.
        /// </summary>
        protected static ILogger Logger => _logger;

        /// <summary>
        /// Gets an addon downloader factory.
        /// </summary>
        protected IAddonDownloaderFactory AddonDownloaderFactory => _addonDownloaderFactory;

        /// <summary>
        /// Gets an addon extractor factory.
        /// </summary>
        protected IAddonExtractorFactory AddonExtractorFactory => _addonExtractorFactory;

        /// <summary>
        /// Gets an addon installer factory.
        /// </summary>
        protected IAddonInstallerFactory AddonInstallerFactory => _addonInstallerFactory;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public void InstallPackage(IAddonPackage package)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}
