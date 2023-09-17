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
        private readonly double _totalSize = 0d;
        private readonly string _version = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtractionResult"/> class.
        /// </summary>
        /// <param name="extractedFiles">A downloaded object name.</param>
        /// <param name="version">Copies a version of a downloaded object.</param>
        /// <exception cref="ArgumentException">If <paramref name="extractedFiles"/> is <see langword="null"/> or empty.</exception>
        public ExtractionResult(IEnumerable<ExtractedFile> extractedFiles, string version)
        {
            ArgumentNullException.ThrowIfNull(extractedFiles, nameof(extractedFiles));

            _extractedFiles = extractedFiles.ToList();
            _totalSize = _extractedFiles.Sum(x => x.Size);
            _version = version;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the extracted items (files).
        /// </summary>
        public List<ExtractedFile> ExtractedFiles => _extractedFiles;

        /// <summary>
        /// Gets a total size of extracted files.
        /// </summary>
        public double TotalSize => _totalSize;

        /// <summary>
        /// Gets the downloaded object version.
        /// </summary>
        public string Version => _version;

        #endregion Properties
    }
}
