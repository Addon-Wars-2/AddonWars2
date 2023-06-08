// ==================================================================================================
// <copyright file="IInstallAddonsDialogFactory.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels.Factories
{
    using AddonWars2.App.ViewModels.Dialogs;

    /// <summary>
    /// Specified a contact for <see cref="InstallAddonsDialogViewModel"/> factories.
    /// </summary>
    public interface IInstallAddonsDialogFactory
    {
        /// <summary>
        /// Creates a new instance of <see cref="InstallAddonsDialogViewModel"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="ErrorDialogViewModel"/>.</returns>
        public InstallAddonsDialogViewModel Create();
    }
}
