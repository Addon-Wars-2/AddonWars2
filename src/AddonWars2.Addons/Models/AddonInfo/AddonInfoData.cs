// ==================================================================================================
// <copyright file="AddonInfoData.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons.Models.AddonInfo
{
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Specifies the addon host type.
    /// </summary>
    public enum HostType
    {
        /// <summary>
        /// The addon is hosted on some website.
        /// </summary>
        [EnumMember(Value = "standalone")]
        Standalone,

        /// <summary>
        /// The addon is hosted on github.
        /// </summary>
        [EnumMember(Value = "github")]
        GitHub,
    }

    /// <summary>
    /// Specifies the addon download type.
    /// </summary>
    public enum DownloadType
    {
        /// <summary>
        /// The addon is distributed as a single .dll file.
        /// </summary>
        [EnumMember(Value = "dll")]
        Dll,

        /// <summary>
        /// The addon is distributed as an archive.
        /// </summary>
        [EnumMember(Value = "archive")]
        Archive,
    }

    /// <summary>
    /// Specifies the addon install mode.
    /// </summary>
    public enum InstallMode
    {
        /// <summary>
        /// The addon will be placed into Guild Wars 2 installation directory as-is.
        /// </summary>
        /// <remarks>
        /// In this mode, whether an addon contains a single .dll file or multiple files and directories,
        /// it will be merely copied into the Guild Wars 2 root directory.
        /// </remarks>
        [EnumMember(Value = "root")]
        Root,

        /// <summary>
        /// The addon is installed into its own sub-directory within "addons" directory.
        /// </summary>
        /// <remarks>
        /// In this mode the addon name is left as-is.
        /// </remarks>
        [EnumMember(Value = "binary")]
        Binary,

        /// <summary>
        /// The addon is installed as an ArcDPS plugin.
        /// </summary>
        /// <remarks>
        /// In this mode, the addon will be placed into the "arcdps" directory.
        /// </remarks>
        [EnumMember(Value = "arc")]
        Arc,
    }

    /// <summary>
    /// Represents a Guild Wars 2 add-on, encapsulating the information about it.
    /// </summary>
    [Serializable]
    public class AddonInfoData
    {
        #region Fields

        private string _internalName = string.Empty;
        private string _displayName = string.Empty;
        private string _description = string.Empty;
        private string _tooltip = string.Empty;
        private string _authors = string.Empty;
        private string _website = string.Empty;
        private HostType _hostType = HostType.Standalone;
        private string _hostUrl = string.Empty;
        private DownloadType _downloadType = DownloadType.Archive;
        private InstallMode _installMode = InstallMode.Binary;
        private ArcPluginInfo _arcModInfo = ArcPluginInfo.Empty;
        private List<string> _requiredAddons = new List<string>();
        private List<string> _conflicts = new List<string>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonInfoData"/> class.
        /// </summary>
        public AddonInfoData()
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
        public string DispayName
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
        /// Gets or sets the additional information for ArcDPS addons.
        /// </summary>
        /// <remarks>
        /// If an addon is not an ArcDPS plugin, its data will be empty.
        /// </remarks>
        [JsonPropertyName("arc_modinfo")]
        public ArcPluginInfo ArcModInfo
        {
            get => _arcModInfo;
            set => _arcModInfo = value;
        }

        /// <summary>
        /// Gets or sets a list of addons which are required to be installed
        /// to make the current addon work.
        /// </summary>
        [JsonPropertyName("required_addons")]
        public List<string> RequiredAddons
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
        public List<string> Conflicts
        {
            get => _conflicts;
            set => _conflicts = value;
        }

        #endregion Properties
    }
}
