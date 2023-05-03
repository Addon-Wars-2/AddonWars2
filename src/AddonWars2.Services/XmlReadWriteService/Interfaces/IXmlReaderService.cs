// ==================================================================================================
// <copyright file="IXmlReaderService.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Services.XmlReadWriteService.Interfaces
{
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// Represents a contract for XML reader services.
    /// </summary>
    public interface IXmlReaderService
    {
        /// <summary>
        /// Asynchronously creates a new <see cref="XDocument"/> from the specified stream.
        /// </summary>
        /// <param name="stream">A <see cref="Stream"/> containing the raw XML to read into the newly created <see cref="XDocument"/>.</param>
        /// <param name="loadOptions">A set of <see cref="LoadOptions"/>.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A newly created <see cref="XDocument"/> object.</returns>
        public Task<XDocument> LoadXmlAsync(Stream stream, LoadOptions loadOptions = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Read a given XML document string and returns as an <see cref="XmlDocument"/> object.
        /// </summary>
        /// <param name="filename">File path.</param>
        /// <returns><see cref="XmlDocument"/> object.</returns>
        public XmlDocument ReadXmlAsDocument(string filename);

        /// <summary>
        /// Read a given XML document string and returns as an <see cref="string"/> object.
        /// </summary>
        /// <param name="filename">File path.</param>
        /// <returns><see cref="string"/> object.</returns>
        public string ReadXmlAsString(string filename);
    }
}
