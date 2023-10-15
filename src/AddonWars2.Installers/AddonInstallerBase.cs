// ==================================================================================================
// <copyright file="AddonInstallerBase.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AddonWars2.Installers.Interfaces;
    using AddonWars2.Installers.Models;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents a base class for addon installers.
    /// </summary>
    public abstract class AddonInstallerBase : IAddonInstaller
    {
        #region Fields

        private static ILogger _logger;
        private readonly Dictionary<string, IProgress<double>> _progressCollection = new Dictionary<string, IProgress<double>>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonInstallerBase"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/>.</param>
        public AddonInstallerBase(ILogger<AddonInstallerBase> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion Constructors

        #region Events

        #endregion Events

        #region Properties

        /// <inheritdoc/>
        public Dictionary<string, IProgress<double>> ProgressCollection => _progressCollection;

        /// <summary>
        /// Gets the current logger instance.
        /// </summary>
        protected static ILogger Logger => _logger;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public abstract Task<InstallResult> InstallAsync(InstallRequest installRequest);

        /// <inheritdoc/>
        public abstract Task<UninstallResult> UninstallAsync(UninstallRequest uninstallRequest);

        /// <inheritdoc/>
        public void AttachProgressItem(string token, IProgress<double> progress)
        {
            ProgressCollection.Add(token, progress);
        }

        #endregion Methods
    }
}
