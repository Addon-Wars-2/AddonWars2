// ==================================================================================================
// <copyright file="Gw2Addon.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons
{
    using System.Text.Json.Serialization;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a Guild Wars 2 add-on, encapsulating the information about it.
    /// </summary>
    [Serializable]
    public class Gw2Addon
    {
        #region Fields

        private string _name = string.Empty;
        private string _description = string.Empty;
        private string _version = string.Empty;

        #endregion Fields

        #region Constructors

        /////// <summary>
        /////// Initializes a new instance of the <see cref="Gw2Addon"/> class.
        /////// </summary>
        /////// <param name="name">Addon name.</param>
        ////public Gw2Addon(string name)
        ////    : this(name, string.Empty, string.Empty)
        ////{
        ////    // Blank.
        ////}

        /////// <summary>
        /////// Initializes a new instance of the <see cref="Gw2Addon"/> class.
        /////// </summary>
        /////// <param name="name">Addon name.</param>
        /////// <param name="description">Addon description.</param>
        ////public Gw2Addon(string name, string description)
        ////    : this(name, description, string.Empty)
        ////{
        ////    // Blank.
        ////}

        /////// <summary>
        /////// Initializes a new instance of the <see cref="Gw2Addon"/> class.
        /////// </summary>
        /////// <param name="name">Addon name.</param>
        /////// <param name="description">Addon description.</param>
        /////// <param name="version">Addon version.</param>
        /////// <param name="isEnabled">Indicates whether the addon is enabled.</param>
        ////public Gw2Addon(string name, string description, string version)
        ////{
        ////    _name = name;
        ////    _description = description;
        ////    _version = version;
        ////}

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the addon name.
        /// </summary>
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        /// <summary>
        /// Gets or sets the addon description.
        /// </summary>
        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string Description
        {
            get => _description;
            set => _description = value;
        }

        /// <summary>
        /// Gets or sets the addon version.
        /// </summary>
        [JsonProperty("version")]
        [JsonPropertyName("version")]
        public string Version
        {
            get => _version;
            set => _version = value;
        }

        #endregion Properties
    }
}
