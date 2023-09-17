// ==================================================================================================
// <copyright file="ExtractedFile.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Extractors.Models
{
    /// <summary>
    /// Represents a single extracted file.
    /// </summary>
    public class ExtractedFile
    {
        #region Fields

        private readonly string _name;
        private readonly byte[] _content;
        private readonly double _contentLength;
        private readonly string _relativePath;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtractedFile"/> class.
        /// </summary>
        /// <param name="name">The extracted object name.</param>
        /// <param name="content">The extracted content represented as a byte array.</param>
        /// <param name="relativePath">The extracted object relative path.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="content"/> is <see langword="null"/>.</exception>
        /// v<exception cref="ArgumentNullException">If <paramref name="relativePath"/> is <see langword="null"/>.</exception>
        public ExtractedFile(string name, byte[] content, string relativePath)
        {
            ArgumentNullException.ThrowIfNull(name, nameof(name));
            ArgumentNullException.ThrowIfNull(content, nameof(content));
            ArgumentNullException.ThrowIfNull(relativePath, nameof(relativePath));

            _name = name;
            _content = content;
            _contentLength = content.Length;
            _relativePath = relativePath;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the extracted item name.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets the extracted item content.
        /// </summary>
        public byte[] Content => _content;

        /// <summary>
        /// Gets the extracted content size.
        /// </summary>
        public double Size => _contentLength;

        /// <summary>
        /// Gets the extracted item relative path.
        /// </summary>
        public string RelativePath => _relativePath;

        #endregion Properties
    }
}
