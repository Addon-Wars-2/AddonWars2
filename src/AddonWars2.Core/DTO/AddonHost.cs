// ==================================================================================================
// <copyright file="AddonHost.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Core.DTO
{
    using AddonWars2.Core.Enums;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Encapsulates the information about addons host.
    /// </summary>
    [Serializable]
    public class AddonHost
    {
        #region Fields

        private HostType _hostType;
        private string _hostUrl = string.Empty;
        private HostVersioning _versioning;
        private string? _versionUrl = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonHost"/> class.
        /// </summary>
        public AddonHost()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the addon host type.
        /// </summary>
        [JsonPropertyName("host_type")]
        public HostType HostType
        {
            get => _hostType;
            set => _hostType = value;
        }

        /// <summary>
        /// Gets or sets the addon host URL string.
        /// </summary>
        [JsonPropertyName("host_url")]
        public string HostUrl
        {
            get => _hostUrl;
            set => _hostUrl = value;
        }

        /// <summary>
        /// Gets or sets the addon versioning type.
        /// </summary>
        [JsonPropertyName("versioning")]
        public HostVersioning Versioning
        {
            get => _versioning;
            set => _versioning = value;
        }

        /// <summary>
        /// Gets or sets the addon version URL.
        /// </summary>
        [JsonPropertyName("version_url")]
        public string? VersionUrl
        {
            get => _versionUrl;
            set => _versionUrl = value;
        }

        #endregion Properties
    }
}
