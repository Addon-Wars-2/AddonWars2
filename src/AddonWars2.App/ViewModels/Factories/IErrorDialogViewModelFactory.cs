// ==================================================================================================
// <copyright file="IErrorDialogViewModelFactory.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels.Factories
{
    using AddonWars2.App.UIServices.Enums;
    using AddonWars2.App.ViewModels.Dialogs;

    /// <summary>
    /// Specifies a contract for <see cref="ErrorDialogViewModel"/> factories.
    /// </summary>
    public interface IErrorDialogViewModelFactory
    {
        /// <summary>
        /// Creates a new instance of <see cref="ErrorDialogViewModel"/>.
        /// </summary>
        /// <param name="title">Dialog window title.</param>
        /// <param name="message">Dialog message.</param>
        /// <param name="details">Dialog additional details.</param>
        /// <param name="buttons">Dialog buttons to show.</param>
        /// <returns>A new instance of <see cref="ErrorDialogViewModel"/>.</returns>
        public ErrorDialogViewModel Create(string title, string message, string? details = null, ErrorDialogButtons buttons = ErrorDialogButtons.OK);
    }
}
