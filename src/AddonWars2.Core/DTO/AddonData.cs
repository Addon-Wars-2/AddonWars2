// ==================================================================================================
// <copyright file="AddonData.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Core.DTO
{
    using System.Text.Json.Serialization;
    using AddonWars2.Core.Enums;

    /// <summary>
    /// Represents a Guild Wars 2 add-on, encapsulating the information about it.
    /// </summary>
    [Serializable]
    public class AddonData
    {
        #region Fields

        private string _internalName = string.Empty;
        private string _displayName = string.Empty;
        private string _description = string.Empty;
        private string _tooltip = string.Empty;
        private string _authors = string.Empty;
        private string _website = string.Empty;
        private IEnumerable<AddonHost>? _hosts;
        private DownloadType _downloadType;
        private InstallMode _installMode;
        private IEnumerable<string>? _requiredAddons;
        private IEnumerable<string>? _conflicts;
        private Dictionary<string, string> _additional;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonData"/> class.
        /// </summary>
        public AddonData()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the addon internal name.
        /// </summary>
        /// <remarks>
        /// This name must be unique across all addons since it
        /// identifies a particular addon.
        /// </remarks>
        [JsonPropertyName("internal_name")]
        public string InternalName
        {
            get => _internalName;
            set => _internalName = value;
        }

        /// <summary>
        /// Gets or sets the addon display name.
        /// </summary>
        [JsonPropertyName("display_name")]
        public string DisplayName
        {
            get => _displayName;
            set => _displayName = value;
        }

        /// <summary>
        /// Gets or sets the addon description.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description
        {
            get => _description;
            set => _description = value;
        }

        /// <summary>
        /// Gets or sets the addon tooltip.
        /// </summary>
        [JsonPropertyName("tooltip")]
        public string Tooltip
        {
            get => _tooltip;
            set => _tooltip = value;
        }

        /// <summary>
        /// Gets or sets the addon author(s).
        /// </summary>
        [JsonPropertyName("authors")]
        public string Authors
        {
            get => _authors;
            set => _authors = value;
        }

        /// <summary>
        /// Gets or sets the addon website URL string.
        /// </summary>
        [JsonPropertyName("website")]
        public string Website
        {
            get => _website;
            set => _website = value;
        }

        /// <summary>
        /// Gets or sets the addon host list.
        /// </summary>
        [JsonPropertyName("hosts")]
        public IEnumerable<AddonHost>? Hosts
        {
            get => _hosts;
            set => _hosts = value;
        }

        /// <summary>
        /// Gets or sets the addon download type.
        /// </summary>
        [JsonPropertyName("download_type")]
        public DownloadType DownloadType
        {
            get => _downloadType;
            set => _downloadType = value;
        }

        /// <summary>
        /// Gets or sets the addon download type.
        /// </summary>
        [JsonPropertyName("install_mode")]
        public InstallMode InstallMode
        {
            get => _installMode;
            set => _installMode = value;
        }

        /// <summary>
        /// Gets or sets a list of addons which are required to be installed
        /// to make the current addon work.
        /// </summary>
        [JsonPropertyName("required_addons")]
        public IEnumerable<string>? RequiredAddons
        {
            get => _requiredAddons;
            set => _requiredAddons = value;
        }

        /// <summary>
        /// Gets or sets a list of addons which conflict with the current addon.
        /// </summary>
        /// <remarks>
        /// Each item must be equal to <see cref="InternalName"/> of an addon
        /// the current one conflicts with.
        /// </remarks>
        [JsonPropertyName("conflicts")]
        public IEnumerable<string>? Conflicts
        {
            get => _conflicts;
            set => _conflicts = value;
        }

        /// <summary>
        /// Gets or sets a disctionary of additional flags and metadata.
        /// </summary>
        [JsonPropertyName("additional")]
        public Dictionary<string, string> Additional
        {
            get => _additional;
            set => _additional = value;
        }

        #endregion Properties
    }
}
