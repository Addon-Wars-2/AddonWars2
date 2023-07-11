// ==================================================================================================
// <copyright file="RenameFileRule.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Core.DTO.Rules
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents an addon rule that instructs to rename the file.
    /// File extension must be included in <see cref="OldName"/> and <see cref="NewName"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If an addon is a single file, i.e. .dll file, then this file will be renamed
    /// if its name matches with the one specified in <see cref="OldName"/> property.
    /// </para>
    /// <para>
    /// If an addon is an archive, then a relative to the archive root path should be specified.
    /// </para>
    /// <para>
    /// Optionally <see cref="OldName"/> property can be set to "self" (case-insensitive) to indicate
    /// that the file needs to be renamed and the original name doesn't matter. Note, that it works only
    /// for a single-file addons such as .dll addons.
    /// </para>
    /// <para>
    /// Therefore, instead of writing this:
    /// <code>
    /// "rules": [
    ///    {
    ///      "$type": "rename_file_rule",
    ///      "old_name": "my_addon.dll",
    ///      "new_name": "arcdps_my_addon.dll"
    ///    }
    /// </code>
    /// One can write:
    /// <code>
    /// "rules": [
    ///    {
    ///      "$type": "rename_file_rule",
    ///      "old_name": "self",
    ///      "new_name": "arcdps_my_addon.dll"
    ///    }
    /// </code>
    /// The rule will be applied even if the actual file name has changed (i.e. starting with some version),
    /// but its name wasn't updated in a registry.
    /// </para>
    /// </remarks>
    [Serializable]
    public class RenameFileRule : AddonRuleBase
    {
        #region Fields

        private string _oldName = string.Empty;
        private string _newName = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RenameFileRule"/> class.
        /// </summary>
        public RenameFileRule()
        {
            RuleTypeName = typeof(RenameFileRule).Name;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets a file name to rename.
        /// </summary>
        [JsonPropertyName("old_name")]
        public string OldName
        {
            get => _oldName;
            set => _oldName = value;
        }

        /// <summary>
        /// Gets or sets a new file name.
        /// </summary>
        [JsonPropertyName("new_name")]
        public string NewName
        {
            get => _newName;
            set => _newName = value;
        }

        #endregion Properties
    }
}
