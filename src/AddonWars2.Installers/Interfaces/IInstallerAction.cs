// ==================================================================================================
// <copyright file="IInstallerAction.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers.Interfaces
{
    /// <summary>
    /// Represents a contract for installer queue actions.
    /// </summary>
    public interface IInstallerAction
    {
        /// <summary>
        /// Executes an item in queue.
        /// </summary>
        public void Execute();
    }
}
