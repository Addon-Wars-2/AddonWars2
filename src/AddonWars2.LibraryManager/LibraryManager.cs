// ==================================================================================================
// <copyright file="LibraryManager.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Core
{
    using System.Diagnostics;
    using AddonWars2.Core.DTO;
    using AddonWars2.Core.Interfaces;
    using AddonWars2.Downloaders;
    using AddonWars2.Downloaders.Interfaces;
    using AddonWars2.Downloaders.Models;
    using AddonWars2.Extractors.Interfaces;
    using AddonWars2.LibraryManager.Exceptions;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents a manager responsible for operations with the addon library on a user's local machine.
    /// </summary>
    public class LibraryManager : ILibraryManager
    {
        #region Fields

        private static ILogger _logger;
        private readonly IAddonDownloaderFactory _addonDownloaderFactory;
        private readonly IAddonExtractorFactory _addonExtractorFactory;

        #endregion Fields

        #region Constructors

        public LibraryManager(
            ILogger<LibraryManager> logger,
            IAddonDownloaderFactory addonDownloaderFactory,
            IAddonExtractorFactory addonExtractorFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _addonDownloaderFactory = addonDownloaderFactory ?? throw new ArgumentNullException(nameof(addonDownloaderFactory));
            _addonExtractorFactory = addonExtractorFactory ?? throw new ArgumentNullException(nameof(addonExtractorFactory));
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

        #endregion Properties

        #region Methods

        

        #endregion Methods
    }
}
