// ==================================================================================================
// <copyright file="IOHelper.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Helpers
{
    using System;
    using System.IO;
    using System.Security;
    using System.Xml;
    using System.Xml.Serialization;
    using AddonWars2.App.Models.Application;
    using Microsoft.Win32;
    using NLog.Config;

    /// <summary>
    /// Provides various methods to handle application data IO flow.
    /// </summary>
    public static class IOHelper
    {
        #region Properties

        #endregion Properties

        #region Methods

        /// <summary>
        /// Generates and returns the application data directory for this application.
        /// </summary>
        /// <returns>Application data directory path.</returns>
        public static string GenerateApplicationDataDirectory()
        {
            var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appName = Path.GetFileNameWithoutExtension(Environment.ProcessPath);
            var appDir = Path.Join(appDataDir, appName);
            return appDir;
        }

        /// <summary>
        /// Loads and return NLog logger configuration from the embedded config file.
        /// </summary>
        /// <returns>NLog configuration.</returns>
        public static LoggingConfiguration GetLoggerConfigurationNLog()
        {
            // The GetManifestResourceStream method will always returns null
            // if the resource "built action" property is not set to "embedded resource".
            var assembly = typeof(AW2Application).Assembly;
            var stream = assembly.GetManifestResourceStream("AddonWars2.App.Resources.NLog.config");

            string xml;
            using (var reader = new StreamReader(stream))
            {
                xml = reader.ReadToEnd();
            }

            var cfg = XmlLoggingConfiguration.CreateFromXmlString(xml);
            return cfg;
        }

        public static string TryFindGw2Exe()
        {
            string displayName = ApplicationGlobal.AppConfig.GW2ExecInfo.ProductName;
            RegistryKey parentKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
            string[] subKeyNames = parentKey.GetSubKeyNames();
            foreach (var item in subKeyNames)
            {
                var key = parentKey.OpenSubKey(item);
                try
                {
                    if (key.GetValue("DisplayName").ToString() == displayName)
                    {
                        // Apparently GW2 doesn't have "InstallLocation string.
                        var gw2exe = key.GetValue("DisplayIcon").ToString();
                        return gw2exe;
                    }
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Performs an XML serialization for the specified object.
        /// </summary>
        /// <typeparam name="T">Object type to be serialized.</typeparam>
        /// <param name="obj">Object to be serialized.</param>
        /// <returns>Serialized string.</returns>
        public static string SerializeXml<T>(this T obj)
        {
            // Note to myself: make sure setters are defined.

            // Setter will be used when deserializing a string into an object,
            // because an instance of the object needs to be created and then
            // the setter will be used to populate the property value.

            var xmlSerializer = new XmlSerializer(typeof(T));
            var xns = new XmlSerializerNamespaces();
            xns.Add(string.Empty, string.Empty);  // omit namespaces
            var stringWriter = new StringWriter();

            using (var writer = XmlWriter.Create(stringWriter))
            {
                xmlSerializer.Serialize(writer, obj, xns);
                stringWriter.Flush();  // to ensure data is written before the writer gets disposed
                return stringWriter.ToString();
            }
        }

        /// <summary>
        /// Performs an XML serialization for the specified object and
        /// saves it as an XML file using the specified path.
        /// </summary>
        /// <typeparam name="T">Object type to be serialized.</typeparam>
        /// <param name="obj">Object to be serialized.</param>
        /// <param name="filename">XML file path.</param>
        /// <returns>Serialized string.</returns>
        public static string SerializeXml<T>(this T obj, string filename)
        {
            var serializedString = SerializeXml<T>(obj);
            if (string.IsNullOrEmpty(serializedString))
            {
                return string.Empty;
            }

            WriteXml(serializedString, filename);

            return serializedString;
        }

        /// <summary>
        /// Performs an XML deserialization for the specified file.
        /// </summary>
        /// <typeparam name="T">Object type to be deserialized to.</typeparam>
        /// <param name="filename">XML file path.</param>
        /// <returns>Deserialized object.</returns>
        public static T DeserializeXml<T>(string filename)
        {
            T deserializedObj = default(T);

            if (string.IsNullOrEmpty(filename))
            {
                return default(T);
            }

            using (var reader = new StreamReader(filename))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                try
                {
                    deserializedObj = (T)serializer.Deserialize(reader);
                }
                catch (InvalidOperationException)
                {
                    return default(T);
                }
            }

            return deserializedObj;
        }

        /// <summary>
        /// Writes a given XML string to a specified file.
        /// </summary>
        /// <param name="xmlString">XML string.</param>
        /// <param name="filename">File path.</param>
        public static void WriteXml(string xmlString, string filename)
        {
            var xml = XmlDocumentFromString(xmlString);
            WriteXmlInternal(xml, filename);
        }

        /// <summary>
        /// Writes a given <see cref="XmlDocument"/> to a specified file.
        /// </summary>
        /// <param name="xml">XML document object.</param>
        /// <param name="filename">File path.</param>
        public static void WriteXml(XmlDocument xml, string filename)
        {
            WriteXmlInternal(xml, filename);
        }

        /// <summary>
        /// Read a given XML document and returns as an <see cref="XmlDocument"/> object.
        /// </summary>
        /// <param name="filename">File path.</param>
        /// <returns><see cref="XmlDocument"/> object.</returns>
        public static XmlDocument ReadXmlAsDocument(string filename)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);

            return xmlDoc;
        }

        /// <summary>
        /// Read a given XML document and returns as an <see cref="string"/> object.
        /// </summary>
        /// <param name="filename">File path.</param>
        /// <returns><see cref="string"/> object.</returns>
        public static string ReadXmlAsString(string filename)
        {
            var xmlDoc = ReadXmlAsDocument(filename);

            return xmlDoc.ToString();
        }

        // Returns XmlDocument object from a given string.
        private static XmlDocument XmlDocumentFromString(string xmlString)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);
            return xmlDoc;
        }

        // Writes a given XmlDocument to the specified file.
        private static void WriteXmlInternal(XmlDocument xml, string filename)
        {
            var xmlWriterSettings = new XmlWriterSettings()
            {
                Async = false,
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace,
            };

            using (XmlWriter writer = XmlWriter.Create(filename, xmlWriterSettings))
            {
                xml.Save(writer);
            }
        }

        #endregion Methods
    }
}
