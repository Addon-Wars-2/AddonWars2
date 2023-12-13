// ==================================================================================================
// <copyright file="AddonActionBase.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Core.DTO.Actions
{
    using System.Text.Json.Serialization;
    using AddonWars2.Core.Enums;

    /// <summary>
    /// Represents a base class for an action that can be optionally supplied with the addon data.
    /// </summary>
    [Serializable]
    [JsonPolymorphic]
    [JsonDerivedType(typeof(RenameFileAddonAction), typeDiscriminator: "rename_file_rule")]
    public class AddonActionBase
    {
        #region Fields

        private string _ruleTypeName = typeof(AddonActionBase).Name;
        private WhenApplyAddonAction _whenApplyAddonAction = WhenApplyAddonAction.None;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonActionBase"/> class.
        /// </summary>
        public AddonActionBase()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets a type name of an actual action that was applied.
        /// </summary>
        public virtual string AddonActionTypeName
        {
            get => _ruleTypeName;
            set => _ruleTypeName = value;
        }

        /// <summary>
        /// Gets or sets an action application type. Default is <see cref="WhenApplyAddonAction.None"/>.
        /// </summary>
        public virtual WhenApplyAddonAction WhenApplyAddonAction
        {
            get => _whenApplyAddonAction;
            protected set => _whenApplyAddonAction = value;
        }

        #endregion Properties
    }
}
