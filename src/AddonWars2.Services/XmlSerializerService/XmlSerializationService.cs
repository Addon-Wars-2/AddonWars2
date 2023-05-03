// ==================================================================================================
// <copyright file="XmlSerializationService.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Services.XmlSerializerService
{
    using System.Xml;
    using System.Xml.Serialization;
    using AddonWars2.Services.XmlReadWriteService.Interfaces;
    using AddonWars2.Services.XmlSerializerService.Interfaces;

    /// <summary>
    /// Represents an XML serializer service.
    /// </summary>
    public class XmlSerializationService : IXmlSerializationService
    {
        #region Fields

        private readonly IXmlWriterService _xmlWriterService;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSerializationService"/> class.
        /// </summary>
        /// <param name="xmlWriterService">A reference to <see cref="IXmlWriterService"/> instance.</param>
        public XmlSerializationService(IXmlWriterService xmlWriterService)
        {
            _xmlWriterService = xmlWriterService;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the XML writer service.
        /// </summary>
        protected IXmlWriterService XmlWriterService => _xmlWriterService;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public string SerializeXml<T>(T? obj)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            var xns = new XmlSerializerNamespaces();
            xns.Add(string.Empty, string.Empty);  // omit namespaces

            var stringWriter = new StringWriter();
            using (var writer = XmlWriter.Create(stringWriter))
            {
                xmlSerializer.Serialize(writer, obj, xns);
                stringWriter.Flush();  // to ensure data is written before the writer gets disposed
                return stringWriter.ToString();  // TODO: serializer always returns 0.
            }
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException">Is thrown if <paramref name="filename"/> is <see langword="null"/> or empty.</exception>
        public string SerializeXml<T>(T? obj, string filename)
        {
            ArgumentException.ThrowIfNullOrEmpty(filename, nameof(filename));

            var serializedString = SerializeXml(obj);
            if (string.IsNullOrEmpty(serializedString))
            {
                return string.Empty;
            }

            XmlWriterService.WriteXml(serializedString, filename);

            return serializedString;
        }

        /// <inheritdoc/>
        public T? DeserializeXml<T>(string filename)
        {
            T? deserializedObj = default;

            if (string.IsNullOrEmpty(filename))
            {
                return default;
            }

            using (var reader = new StreamReader(filename))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                try
                {
                    deserializedObj = (T?)serializer.Deserialize(reader);
                }
                catch (InvalidOperationException)
                {
                    return default;
                }
            }

            return deserializedObj;
        }

        #endregion Methods
    }
}
