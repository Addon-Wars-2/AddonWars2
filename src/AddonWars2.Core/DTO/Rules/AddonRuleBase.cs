// ==================================================================================================
// <copyright file="AddonRuleBase.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Core.DTO.Rules
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents a base class for an additional rule  which can be optionally supplied with the addon data.
    /// </summary>
    [Serializable]
    [JsonPolymorphic]
    [JsonDerivedType(typeof(RenameFileRule), typeDiscriminator: "rename_file_rule")]
    public class AddonRuleBase
    {
        #region Fields

        private string _ruleTypeName = typeof(AddonRuleBase).Name;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonRuleBase"/> class.
        /// </summary>
        public AddonRuleBase()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets a type name of an actual rule that was applied.
        /// </summary>
        public virtual string RuleTypeName
        {
            get => _ruleTypeName;
            set => _ruleTypeName = value;
        }

        #endregion Properties
    }
}
