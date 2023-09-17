// ==================================================================================================
// <copyright file="AddonExtractorFactory.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Extractors.Factories
{
    using AddonWars2.Core.Enums;
    using AddonWars2.Extractors.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents a factory for addon extractors.
    /// </summary>
    public class AddonExtractorFactory : IAddonExtractorFactory
    {
        #region Fields

        private static ILogger<AddonExtractorBase> _logger;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonExtractorFactory"/> class.
        /// </summary>
        /// <param name="logger">A reference to base <see cref="ILogger"/>.</param>
        public AddonExtractorFactory(
            ILogger<AddonExtractorBase> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public IAddonExtractor GetExtractor(DownloadType downloadType)
        {
            switch (downloadType)
            {
                case DownloadType.Dll:
                    return new DllAddonExtractor(_logger);
                case DownloadType.Archive:
                    return new ArchiveAddonExtractor(_logger);
                default:
                    throw new NotSupportedException($"Cannot create an extractor for the download type: {downloadType}. The download type is not supported.");
            }
        }

        #endregion Methods
    }
}
