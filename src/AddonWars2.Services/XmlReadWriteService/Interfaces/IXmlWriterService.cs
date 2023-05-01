// ==================================================================================================
// <copyright file="IXmlWriterService.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Services.XmlReadWriteService.Interfaces
{
    using System.Xml;

    /// <summary>
    /// Represents a contract for XML writer services.
    /// </summary>
    public interface IXmlWriterService
    {
        /// <summary>
        /// Writes a given XML string to a specified file.
        /// </summary>
        /// <param name="xmlString">XML string.</param>
        /// <param name="filename">File path.</param>
        public void WriteXml(string xmlString, string filename);

        /// <summary>
        /// Writes a given <see cref="XmlDocument"/> to a specified file.
        /// </summary>
        /// <param name="xml">XML document object.</param>
        /// <param name="filename">File path.</param>
        public void WriteXml(XmlDocument xml, string filename);
    }
}
