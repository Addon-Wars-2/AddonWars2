// ==================================================================================================
// <copyright file="AddonInstallerBase.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers
{
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents a base class for addon installers.
    /// </summary>
    public abstract class AddonInstallerBase
    {
        #region Fields

        private static ILogger _logger;

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

        /// <summary>
        /// Gets the current logger instance.
        /// </summary>
        protected static ILogger Logger => _logger;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        //public abstract Task<ExtractionResult> Extract(ExtractionRequest request);

        #endregion Methods
    }
}
