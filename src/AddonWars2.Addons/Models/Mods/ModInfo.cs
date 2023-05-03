// ==================================================================================================
// <copyright file="ModInfo.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons.Models.Mods
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents a Guild Wars 2 add-on "modinfo" file.
    /// </summary>
    public class ModInfo
    {
        #region Fields

        private ModInfoData _data = new ModInfoData();
        private ModInfoSchema _schema = new ModInfoSchema();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModInfo"/> class.
        /// </summary>
        public ModInfo()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the addon data.
        /// </summary>
        [JsonPropertyName("data")]
        public ModInfoData Data
        {
            get => _data;
            set => _data = value;
        }

        /// <summary>
        /// Gets or sets the template schema.
        /// </summary>
        [JsonPropertyName("schema")]
        public ModInfoSchema Schema
        {
            get => _schema;
            set => _schema = value;
        }

        #endregion Properties
    }
}
