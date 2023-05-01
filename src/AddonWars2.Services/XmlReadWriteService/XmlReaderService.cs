// ==================================================================================================
// <copyright file="XmlReaderService.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Services.XmlReadWriteService
{
    using System.Xml;
    using System.Xml.Linq;
    using AddonWars2.Services.XmlReadWriteService.Interfaces;

    /// <summary>
    /// Represents an XML reader service.
    /// </summary>
    public class XmlReaderService : IXmlReaderService
    {
        #region Methods

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">If <paramref name="stream"/> is <see langword="null"/>.</exception>
        public async Task<XDocument> LoadXmlAsync(Stream stream, LoadOptions loadOptions = LoadOptions.None, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(stream, nameof(stream));

            var xdoc = await XDocument.LoadAsync(stream, loadOptions, cancellationToken);
            return xdoc;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="filename"/> is <see langword="null"/>.</exception>
        public XmlDocument ReadXmlAsDocument(string filename)
        {
            ArgumentNullException.ThrowIfNull(filename, nameof(filename));

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);

            return xmlDoc;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="filename"/> is <see langword="null"/>.</exception>
        public string ReadXmlAsString(string filename)
        {
            ArgumentNullException.ThrowIfNull(filename, nameof(filename));

            var xmlDoc = ReadXmlAsDocument(filename);

            return xmlDoc.ToString() ?? string.Empty;
        }

        #endregion Methods
    }
}
