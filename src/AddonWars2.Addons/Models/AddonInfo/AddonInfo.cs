// ==================================================================================================
// <copyright file="AddonInfo.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons.Models.AddonInfo
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents a Guild Wars 2 add-on "modinfo" file.
    /// </summary>
    public class AddonInfo
    {
        #region Fields

        private AddonInfoData _data = new AddonInfoData();
        private AddonInfoSchema _schema = new AddonInfoSchema();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonInfo"/> class.
        /// </summary>
        public AddonInfo()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the addon data.
        /// </summary>
        [JsonPropertyName("data")]
        public AddonInfoData Data
        {
            get => _data;
            set => _data = value;
        }

        /// <summary>
        /// Gets or sets the template schema.
        /// </summary>
        [JsonPropertyName("schema")]
        public AddonInfoSchema Schema
        {
            get => _schema;
            set => _schema = value;
        }

        #endregion Properties
    }
}
