// ==================================================================================================
// <copyright file="IXmlSerializationService.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Services.XmlSerializerService.Interfaces
{
    /// <summary>
    /// Represents a contract for XML serializer services.
    /// </summary>
    public interface IXmlSerializationService
    {
        /// <summary>
        /// Performs an XML serialization for the specified object.
        /// </summary>
        /// <typeparam name="T">Object type to be serialized.</typeparam>
        /// <param name="obj">Object to be serialized.</param>
        /// <returns>Serialized string.</returns>
        public string SerializeXml<T>(T? obj);

        /// <summary>
        /// Performs an XML serialization for the specified object and
        /// saves it as an XML file using the specified path.
        /// </summary>
        /// <typeparam name="T">Object type to be serialized.</typeparam>
        /// <param name="obj">Object to be serialized.</param>
        /// <param name="filename">XML file path.</param>
        /// <returns>Serialized string.</returns>
        public string SerializeXml<T>(T? obj, string filename);

        /// <summary>
        /// Performs an XML deserialization for the specified file.
        /// </summary>
        /// <typeparam name="T">Object type to be deserialized to.</typeparam>
        /// <param name="filename">XML file path.</param>
        /// <returns>Deserialized object.</returns>
        public T? DeserializeXml<T>(string filename);
    }
}
