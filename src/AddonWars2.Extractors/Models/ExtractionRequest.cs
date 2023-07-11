// ==================================================================================================
// <copyright file="ExtractionRequest.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Extractors.Models
{
    /// <summary>
    /// Represents a wrapped request for addon extraction.
    /// </summary>
    public class ExtractionRequest
    {
        #region Fields

        private readonly string _name;
        private readonly byte[] _content;
        private string _version;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtractionRequest"/> class.
        /// </summary>
        /// <param name="name">The content name.</param>
        /// <param name="content">The content to be extracted represented as a byte array.</param>
        /// <param name="version">The content version.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="content"/> is <see langword="null"/>.</exception>
        public ExtractionRequest(string name, byte[] content, string version = "")
        {
            ArgumentNullException.ThrowIfNull(name, nameof(name));
            ArgumentNullException.ThrowIfNull(content, nameof(content));
            ArgumentNullException.ThrowIfNull(version, nameof(version));

            _name = name;
            _content = content;
            _version = version;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the downloaded object name.
        /// Typically this is the name obtained from the Content-Disposition response header or API response.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets the content as a byte array.
        /// </summary>
        public byte[] Content => _content;

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
