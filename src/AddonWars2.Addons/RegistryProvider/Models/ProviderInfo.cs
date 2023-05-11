// ==================================================================================================
// <copyright file="ProviderInfo.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons.RegistryProvider.Models
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Encapsulates the information about a provider of addons.
    /// </summary>
    public class ProviderInfo
    {
        #region Fields

        private string _name = string.Empty;
        private string _type = string.Empty;
        private string _link = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderInfo"/> class.
        /// </summary>
        public ProviderInfo()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the addon provider name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        /// <summary>
        /// Gets or sets the addon provider type.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type
        {
            get => _type;
            set => _type = value;
        }

        /// <summary>
        /// Gets or sets the addon provider link.
        /// </summary>
        [JsonPropertyName("link")]
        public string Link
        {
            get => _link;
            set => _link = value;
        }

        #endregion Properties
    }
}
