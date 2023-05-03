// ==================================================================================================
// <copyright file="ArcModInfo.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons.Models.Mods
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Encapsulates ArcDPS plugin information.
    /// </summary>
    public class ArcModInfo
    {
        #region Fields

        private string _arcPluginName = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ArcModInfo"/> class.
        /// </summary>
        public ArcModInfo()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the ArcDPS plugin name.
        /// </summary>
        /// <remarks>
        /// The plugin name must be provided as a file name including its extension.
        /// </remarks>
        [JsonPropertyName("arc_plugin_name")]
        public string ArcPluginName
        {
            get => _arcPluginName;
            set => _arcPluginName = value;
        }

        #endregion Properties
    }
}
