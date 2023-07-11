// ==================================================================================================
// <copyright file="ExtractionResult.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Extractors.Models
{
    using System;

    /// <summary>
    /// Represents a collection of extracted items.
    /// </summary>
    public class ExtractionResult
    {
        #region Fields

        private readonly List<ExtractedFile> _extractedFiles;
        private string _version = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtractionResult"/> class.
        /// </summary>
        public ExtractionResult()
        {
            _extractedFiles = new List<ExtractedFile>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtractionResult"/> class.
        /// </summary>
        /// <param name="extractedFiles">The downloaded object name.</param>
        /// <exception cref="ArgumentException">If <paramref name="extractedFiles"/> is <see langword="null"/> or empty.</exception>
        public ExtractionResult(IEnumerable<ExtractedFile> extractedFiles)
        {
            ArgumentNullException.ThrowIfNull(extractedFiles, nameof(extractedFiles));

            _extractedFiles = extractedFiles.ToList();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the extracted items (files).
        /// </summary>
        public List<ExtractedFile> ExtractedFiles => _extractedFiles;

        /// <summary>
        /// Gets or sets the downloaded object version.
        /// </summary>
        public string Version
        {
            get => _version;
            set => _version = value;
        }

        #endregion Properties
    }
}
