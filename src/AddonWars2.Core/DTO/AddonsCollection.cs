// ==================================================================================================
// <copyright file="AddonsCollection.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Core.DTO
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents a Guild Wars 2 add-on "modinfo" file.
    /// </summary>
    [Serializable]
    public class AddonsCollection
    {
        #region Fields

        private IEnumerable<AddonData>? _data;
        private AddonsCollectionSchema? _schema;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonsCollection"/> class.
        /// </summary>
        public AddonsCollection()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the addon data.
        /// </summary>
        [JsonPropertyName("data")]
        public IEnumerable<AddonData>? Data
        {
            get => _data;
            set => _data = value;
        }

        /// <summary>
        /// Gets or sets the template schema.
        /// </summary>
        [JsonPropertyName("schema")]
        public AddonsCollectionSchema? Schema
        {
            get => _schema;
            set => _schema = value;
        }

        #endregion Properties
    }
}
