﻿// ==================================================================================================
// <copyright file="ProviderInfo.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons.RegistryProvider.Models
{
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Specified a type of the registry host.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProviderInfoHostType
    {
        /// <summary>
        /// This provider points to a GitHub repository.
        /// </summary>
        [EnumMember(Value = "github")]
        GitHub,

        /// <summary>
        /// This provider points to some standalone source.
        /// </summary>
        [EnumMember(Value = "standalone")]
        Standalone,

        /// <summary>
        /// This provider points to some local (cached) source.
        /// </summary>
        [EnumMember(Value = "local")]
        Local,
    }

    /// <summary>
    /// Encapsulates the information about a provider of addons.
    /// </summary>
    [Serializable]
    public class ProviderInfo
    {
        #region Fields

        private string _name = string.Empty;
        private ProviderInfoHostType _type = ProviderInfoHostType.GitHub;
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
        /// Gets or sets the addon registry type.
        /// </summary>
        [JsonPropertyName("type")]
        public ProviderInfoHostType Type
        {
            get => _type;
            set => _type = value;
        }

        /// <summary>
        /// Gets or sets the addon registry link.
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
