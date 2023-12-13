// ==================================================================================================
// <copyright file="IInstallerCustomActionFactory.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers.Interfaces
{
    using System.Collections.ObjectModel;
    using AddonWars2.Core.DTO.Actions;
    using AddonWars2.Installers.Models;
    using AddonWars2.Installers.Queue;

    /// <summary>
    /// Specifies a contract for rule custom action factories.
    /// </summary>
    public interface IInstallerCustomActionFactory
    {
        /// <summary>
        /// Gets a new instance of a custom action.
        /// </summary>
        /// <typeparam name="T">A type of addon action.</typeparam>
        /// <param name="action">An action used to determine an installer action type.</param>
        /// <param name="request">The installation request to process.</param>
        /// <param name="result">A collection of installed items.</param>
        /// <returns>A new instance of an installer action.</returns>
        public InstallerCustomAction GetInstance<T>(T action, InstallRequest request, ObservableCollection<InstallResultFile> result)
            where T : AddonActionBase;
    }
}
