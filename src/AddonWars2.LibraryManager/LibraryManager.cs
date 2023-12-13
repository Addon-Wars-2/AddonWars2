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
    using AddonWars2.Installers.Models;
    using AddonWars2.Repository;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// <para>
    /// A class that repserents a state of a local library and provides mechanisms
    /// for installing and uninstalling addons with their dependencies, and also updating aready installed addons.
    /// </para>
    /// <para>
    /// While installers merely copy files and follow installation instructions, they neither update the library state,
    /// nor perfom any conflict checks. Therefore, all addons should be installed or uninstalled using a <see cref="LibraryManager"/>.
    /// </para>
    /// </summary>
    public class LibraryManager : ILibraryManager
    {
        #region Fields

        private static ILogger _logger;
        private readonly IAddonDownloaderFactory _addonDownloaderFactory;
        private readonly IAddonExtractorFactory _addonExtractorFactory;
        private readonly IAddonInstallerFactory _addonInstallerFactory;
        private readonly InstalledAddonsContext _dbContext;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryManager"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/>.</param>
        /// <param name="addonDownloaderFactory">A reference to <see cref="IAddonDownloaderFactory"/>.</param>
        /// <param name="addonExtractorFactory">A reference to <see cref="IAddonExtractorFactory"/>.</param>
        /// <param name="addonInstallerFactory">A reference to <see cref="IAddonInstallerFactory"/>.</param>
        /// <param name="dbContext">A reference to <see cref="InstalledAddonsContext"/>.</param>
        public LibraryManager(
            ILogger<LibraryManager> logger,
            IAddonDownloaderFactory addonDownloaderFactory,
            IAddonExtractorFactory addonExtractorFactory,
            IAddonInstallerFactory addonInstallerFactory,
            InstalledAddonsContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _addonDownloaderFactory = addonDownloaderFactory ?? throw new ArgumentNullException(nameof(addonDownloaderFactory));
            _addonExtractorFactory = addonExtractorFactory ?? throw new ArgumentNullException(nameof(addonExtractorFactory));
            _addonInstallerFactory = addonInstallerFactory ?? throw new ArgumentNullException(nameof(addonInstallerFactory));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
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

        /// <summary>
        /// Gets a reference to the installed addons database context.
        /// </summary>
        protected InstalledAddonsContext DbContext => _dbContext;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public async Task<InstallResult> InstallAddonAsync(IAddonInstaller installer, InstallRequest request, CancellationToken cancellationToken)
        {
            return await installer.InstallAsync(request, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<UninstallResult> UninstallAddonAsync(IAddonUninstaller uninstaller, UninstallRequest request)
        {
            return await uninstaller.UninstallAsync(request);
        }

        #endregion Methods
    }
}
