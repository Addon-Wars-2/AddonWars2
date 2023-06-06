// ==================================================================================================
// <copyright file="IErrorDialogService.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.UIServices.Interfaces
{
    using System;
    using System.Windows;
    using AddonWars2.App.UIServices.Enums;

    /// <summary>
    /// Provides a contract for custom error dialog services.
    /// </summary>
    public interface IErrorDialogService
    {
        /// <summary>
        /// Displays a custom error dialog.
        /// </summary>
        /// <param name="owner">A <see cref="Window"/> object that represents the owner of this <see cref="Window"/>.</param>
        /// <param name="title">Dialog window title.</param>
        /// <param name="message">Error message.</param>
        /// <param name="details">Error details.</param>
        /// <param name="buttons">Buttons to show.</param>
        /// <typeparam name="T">A view type to show.</typeparam>
        public void Show<T>(Window? owner, string? title, string? message, string? details, ErrorDialogButtons buttons)
            where T : Window;

        /// <summary>
        /// Displays a custom error dialog.
        /// </summary>
        /// <param name="owner">A <see cref="Window"/> object that represents the owner of this <see cref="Window"/>.</param>
        /// <param name="title">Dialog window title.</param>
        /// <param name="message">Error message.</param>
        /// <param name="details">Error details.</param>
        /// <param name="buttons">Buttons to show.</param>
        /// <param name="callback">A callback used to obtain dialog result depending on a button clicked.</param>
        /// <typeparam name="T">A view type to show.</typeparam>
        public void Show<T>(Window? owner, string? title, string? message, string? details, ErrorDialogButtons buttons, Action<ErrorDialogResult> callback)
            where T : Window;
    }
}
