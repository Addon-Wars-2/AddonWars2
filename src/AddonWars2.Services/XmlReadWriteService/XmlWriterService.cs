// ==================================================================================================
// <copyright file="XmlWriterService.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Services.XmlReadWriteService
{
    using System.Xml;
    using AddonWars2.Services.XmlReadWriteService.Interfaces;

    /// <summary>
    /// Represents an XML writer service.
    /// </summary>
    public class XmlWriterService : IXmlWriterService
    {
        #region Methods

        /// <inheritdoc/>
        /// <exception cref="ArgumentException">Is thrown if <paramref name="xmlString"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentException">Is thrown if <paramref name="filename"/> is <see langword="null"/> or empty.</exception>
        public void WriteXml(string xmlString, string filename)
        {
            ArgumentException.ThrowIfNullOrEmpty(xmlString, nameof(xmlString));
            ArgumentException.ThrowIfNullOrEmpty(filename, nameof(filename));

            var xml = XmlDocumentFromString(xmlString);
            WriteXmlInternal(xml, filename);
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="xml"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Is thrown if <paramref name="filename"/> is <see langword="null"/> or empty.</exception>
        public void WriteXml(XmlDocument xml, string filename)
        {
            ArgumentNullException.ThrowIfNull(xml, nameof(xml));
            ArgumentException.ThrowIfNullOrEmpty(filename, nameof(filename));

            WriteXmlInternal(xml, filename);
        }

        // Returns XmlDocument object from a given string.
        private static XmlDocument XmlDocumentFromString(string xmlString)
        {
            ArgumentNullException.ThrowIfNull(xmlString, nameof(xmlString));

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            return xmlDoc;
        }

        // Writes a given XmlDocument to the specified file.
        private static void WriteXmlInternal(XmlDocument xml, string filename)
        {
            ArgumentNullException.ThrowIfNull(xml, nameof(xml));
            ArgumentException.ThrowIfNullOrEmpty(filename, nameof(filename));

            var xmlWriterSettings = new XmlWriterSettings()
            {
                Async = false,
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace,
            };

            using (XmlWriter writer = XmlWriter.Create(filename!, xmlWriterSettings))
            {
                xml.Save(writer);
            }
        }

        #endregion Methods
    }
}
