// ==================================================================================================
// <copyright file="InstallerActionBase.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers.Queue
{
    using AddonWars2.Installers.Interfaces;

    /// <summary>
    /// A bse class for installer actions.
    /// </summary>
    public abstract class InstallerActionBase : IInstallerAction
    {
        #region Methods

        /// <inheritdoc/>
        public virtual void Execute()
        {
            ExecuteCore();
        }

        /// <summary>
        /// Holds internal execution logic.
        /// </summary>
        protected virtual void ExecuteCore()
        {
            // Blank.
        }

        #endregion Methods
    }
}
