// ==================================================================================================
// <copyright file="IInstallProgressDialogFactory.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels.Factories
{
    using AddonWars2.App.ViewModels.Dialogs;

    /// <summary>
    /// Specifies a contract for factories responsibe for creating install progress dialog view models.
    /// </summary>
    public interface IInstallProgressDialogFactory
    {
        /// <summary>
        /// Creates a new <see cref="InstallAddonsDialogViewModel"/> view model.
        /// </summary>
        /// <returns>A new instance of <see cref="InstallAddonsDialogViewModel"/>.</returns>
        public InstallProgressDialogViewModel GetInstance();
    }
}
