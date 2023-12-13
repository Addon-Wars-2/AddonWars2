// ==================================================================================================
// <copyright file="InstallProgressDialogFactory.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels.Factories
{
    using System;
    using AddonWars2.App.Configuration;
    using AddonWars2.App.ViewModels.Dialogs;
    using AddonWars2.Core.Interfaces;
    using AddonWars2.Downloaders.Interfaces;
    using AddonWars2.Extractors.Interfaces;
    using AddonWars2.Installers.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represetns a factory responsibe for creating install progress dialog view models.
    /// </summary>
    public class InstallProgressDialogFactory : IInstallProgressDialogFactory
    {
        #region Fields

        private readonly ILogger<InstallProgressDialogViewModel> _logger;
        private readonly IApplicationConfig _applicationConfig;
        private readonly IAddonDownloaderFactory _addonDownloaderFactory;
        private readonly IAddonExtractorFactory _addonExtractorFactory;
        private readonly IAddonInstallerFactory _addonInstallerFactory;
        private readonly IAddonUninstallerFactory _addonUninstallerFactory;
        private readonly ILibraryManager _libraryManager;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallProgressDialogFactory"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/> instance.</param>
        /// <param name="appConfig">A reference to <see cref="IApplicationConfig"/>.</param>
        /// <param name="addonDownloaderFactory">A reference to <see cref="IAddonDownloaderFactory"/>.</param>
        /// <param name="addonExtractorFactory">A reference to <see cref="IAddonExtractorFactory"/>.</param>
        /// <param name="addonInstallerFactory">A reference to <see cref="IAddonInstallerFactory"/>.</param>
        /// <param name="addonUninstallerFactory">A reference to <see cref="IAddonUninstallerFactory"/>.</param>
        /// <param name="libraryManager">A reference to <see cref="ILibraryManager"/>.</param>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="logger"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="appConfig"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="addonDownloaderFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="addonExtractorFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="addonInstallerFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="addonUninstallerFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="libraryManager"/> is <see langword="null"/>.</exception>
        public InstallProgressDialogFactory(
            ILogger<InstallProgressDialogViewModel> logger,
            IApplicationConfig appConfig,
            IAddonDownloaderFactory addonDownloaderFactory,
            IAddonExtractorFactory addonExtractorFactory,
            IAddonInstallerFactory addonInstallerFactory,
            IAddonUninstallerFactory addonUninstallerFactory,
            ILibraryManager libraryManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _applicationConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            _addonDownloaderFactory = addonDownloaderFactory ?? throw new ArgumentNullException(nameof(addonDownloaderFactory));
            _addonExtractorFactory = addonExtractorFactory ?? throw new ArgumentNullException(nameof(addonExtractorFactory));
            _addonInstallerFactory = addonInstallerFactory ?? throw new ArgumentNullException(nameof(addonInstallerFactory));
            _addonUninstallerFactory = addonUninstallerFactory ?? throw new ArgumentNullException(nameof(addonUninstallerFactory));
            _libraryManager = libraryManager ?? throw new ArgumentNullException(nameof(libraryManager));
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc/>
        public InstallProgressDialogViewModel GetInstance()
        {
            return new InstallProgressDialogViewModel(
                _logger,
                _applicationConfig,
                _addonDownloaderFactory,
                _addonExtractorFactory,
                _addonInstallerFactory,
                _addonUninstallerFactory,
                _libraryManager);
        }

        #endregion Methods
    }
}
