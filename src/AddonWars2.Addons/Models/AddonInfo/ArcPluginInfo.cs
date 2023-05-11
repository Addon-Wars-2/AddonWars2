// ==================================================================================================
// <copyright file="ArcPluginInfo.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

////namespace AddonWars2.Addons.Models.AddonInfo
////{
////    using System.Text.Json.Serialization;
////    using System.Xml.Serialization;

////    /// <summary>
////    /// Encapsulates ArcDPS plugin information.
////    /// </summary>
////    public class ArcPluginInfo
////    {
////        #region Fields

////        private string _arcPluginName = string.Empty;

////        #endregion Fields

////        #region Constructors

////        /// <summary>
////        /// Initializes a new instance of the <see cref="ArcPluginInfo"/> class.
////        /// </summary>
////        public ArcPluginInfo()
////        {
////            // Blank.
////        }

////        #endregion Constructors

////        #region Properties

////        /// <summary>
////        /// Gets a default instance version of <see cref="ArcPluginInfo"/>.
////        /// </summary>
////        [XmlIgnore]
////        public static ArcPluginInfo Empty
////        {
////            get
////            {
////                var obj = new ArcPluginInfo();
////                obj.SetDefaultValues();
////                return obj;
////            }
////        }

////        /// <summary>
////        /// Gets or sets the ArcDPS plugin name.
////        /// </summary>
////        /// <remarks>
////        /// The plugin name must be provided as a file name including its extension.
////        /// </remarks>
////        [JsonPropertyName("arc_plugin_name")]
////        public string ArcPluginName
////        {
////            get => _arcPluginName;
////            set => _arcPluginName = value;
////        }

////        #endregion Properties

////        #region Methods

////        // Sets default values.
////        private void SetDefaultValues()
////        {
////            ArcPluginName = ArcModInfoEmptyState.ArcPluginName;
////        }

////        #endregion Methods

////        #region Classes

////        // Encapsulates the empty state of the class.
////        private static class ArcModInfoEmptyState
////        {
////            internal static string ArcPluginName => string.Empty;
////        }

////        #endregion Inner Classes
////    }
////}
