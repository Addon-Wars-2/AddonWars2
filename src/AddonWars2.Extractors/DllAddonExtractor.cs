// ==================================================================================================
// <copyright file="DllAddonExtractor.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Extractors
{
    using AddonWars2.Extractors.Models;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents an extractor for DLL-type addons.
    /// </summary>
    public class DllAddonExtractor : AddonExtractorBase
    {
        #region Fields

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DllAddonExtractor"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/>.</param>
        public DllAddonExtractor(ILogger<AddonExtractorBase> logger)
            : base(logger)
        {
            // Blank.
        }

        #endregion Constructors

        #region Events

        #endregion Events

        #region Properties

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public override async Task<ExtractionResult> Extract(ExtractionRequest request)
        {
            var content = new byte[request.Content.Length];
            request.Content.CopyTo(content, 0);

            var extractedFile = new ExtractedFile(request.Name, content, string.Empty);
            var extractionResult = new ExtractionResult()
            {
                Version = request.Version,
            };

            extractionResult.ExtractedFiles.Add(extractedFile);

            return extractionResult;
        }

        #endregion Methods
    }
}
