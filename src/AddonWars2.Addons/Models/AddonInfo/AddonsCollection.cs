// ==================================================================================================
// <copyright file="AddonsCollection.cs" company="Addon-Wars-2">
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
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
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
    /// Specifies the addon host versioning type.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum HostVersioning
    {
        /// <summary>
        /// The addon version should be determined from the latest commit hash.
        /// </summary>
        [EnumMember(Value = "commit_sha")]
        CommitSha,

        /// <summary>
        /// The addonversion should be determined from the MD5 hash sum.
        /// </summary>
        [EnumMember(Value = "md5sum")]
        Md5,
    }

    /// <summary>
    /// Specifies the addon download type.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
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
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
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
