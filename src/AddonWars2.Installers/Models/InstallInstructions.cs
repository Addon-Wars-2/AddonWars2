// ==================================================================================================
// <copyright file="InstallInstructions.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers.Models
{
    using AddonWars2.Core.DTO.Rules;

    /// <summary>
    /// Represents installation instruction for an addon.
    /// </summary>
    public class InstallInstructions
    {
        #region Fields

        private readonly List<AddonRuleBase> _installRules = new List<AddonRuleBase>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallInstructions"/> class.
        /// </summary>
        public InstallInstructions()
        {
            // Blank.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallInstructions"/> class.
        /// </summary>
        /// <param name="installRules">A collection of installation rules.</param>
        public InstallInstructions(List<AddonRuleBase> installRules)
        {
            _installRules = installRules;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a collection of installation rules.
        /// </summary>
        public List<AddonRuleBase> InstallRules => _installRules;

        #endregion Properties
    }
}
