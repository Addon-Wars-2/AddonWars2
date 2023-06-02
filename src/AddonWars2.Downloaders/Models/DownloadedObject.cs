// ==================================================================================================
// <copyright file="DownloadedObject.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloaders.Models
{
    /// <summary>
    /// Represents a downloaded file.
    /// </summary>
    public class DownloadedObject
    {
        #region Fields

        private readonly string _name;
        private readonly byte[] _content;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadedObject"/> class.
        /// </summary>
        /// <param name="name">The downloaded object name.</param>
        /// <param name="content">The downloaded content represented as a byte array.</param>
        /// <exception cref="ArgumentException">If <paramref name="name"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="content"/> is <see langword="null"/>.</exception>
        public DownloadedObject(string name, byte[] content)
        {
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
            ArgumentNullException.ThrowIfNull(content, nameof(content));

            _name = name;
            _content = content;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the downloaded object name.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets the downloaded content as a byte array.
        /// </summary>
        public byte[] Content => _content;

        #endregion Properties
    }
}
