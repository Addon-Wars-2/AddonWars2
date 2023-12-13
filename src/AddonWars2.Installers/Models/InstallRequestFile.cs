// ==================================================================================================
// <copyright file="InstallRequestFile.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers.Models
{
    /// <summary>
    /// Represents a single file to install.
    /// </summary>
    public readonly struct InstallRequestFile
    {
        #region Fields

        private readonly string _filename;
        private readonly byte[] _content;
        private readonly double _size;
        private readonly string _relativePath;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallRequestFile"/> struct.
        /// </summary>
        /// <param name="filename">The extracted object name.</param>
        /// <param name="content">The extracted content represented as a byte array.</param>
        /// <param name="relativePath">The extracted object relative path.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="filename"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="content"/> is <see langword="null"/>.</exception>
        /// v<exception cref="ArgumentNullException">If <paramref name="relativePath"/> is <see langword="null"/>.</exception>
        public InstallRequestFile(string filename, byte[] content, string relativePath)
        {
            ArgumentException.ThrowIfNullOrEmpty(filename, nameof(filename));
            ArgumentNullException.ThrowIfNull(content, nameof(content));
            ArgumentNullException.ThrowIfNull(relativePath, nameof(relativePath));

            _filename = filename;
            _content = content;
            _size = content.Length;
            _relativePath = relativePath;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the filename to install.
        /// </summary>
        public string Filename => _filename;

        /// <summary>
        /// Gets the item content.
        /// </summary>
        public byte[] Content => _content;

        /// <summary>
        /// Gets the item content size.
        /// </summary>
        public double Size => _size;

        /// <summary>
        /// Gets the item relative instalation path path.
        /// A full instllation path will be combined with the installation
        /// entry point depending on the installer type.
        /// </summary>
        public string RelativePath => _relativePath;

        #endregion Properties
    }
}
