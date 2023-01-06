// ==================================================================================================
// <copyright file="IMessageBoxService.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Services
{
    using System.Windows;

    /// <summary>
    /// Provides a contract for message box service implementation.
    /// </summary>
    public interface IMessageBoxService
    {
        /// <summary>
        /// Displays a native message box with specified text, caption, and style.
        /// </summary>
        /// <param name="text">Message text.</param>
        /// <param name="caption">Message box title.</param>
        /// <param name="button">Button(s) to be displayed.</param>
        /// <param name="icon">The icon image to be displayed.</param>
        /// <param name="defaultResult">A default message box result.</param>
        /// <param name="options">Special display options for a message box.</param>
        /// <returns><see cref="MessageBoxResult"/> result.</returns>
        public MessageBoxResult Show(
            string text,
            string caption,
            MessageBoxButton button,
            MessageBoxImage icon,
            MessageBoxResult defaultResult,
            MessageBoxOptions options);

        /// <summary>
        /// Displays a native message box with specified text, caption, and style.
        /// </summary>
        /// <param name="text">Message text.</param>
        /// <param name="caption">Message box title.</param>
        /// <param name="button">Button(s) to be displayed.</param>
        /// <param name="icon">The icon image to be displayed.</param>
        /// <param name="defaultResult">A default message box result.</param>
        /// <returns><see cref="MessageBoxResult"/> result.</returns>
        public MessageBoxResult Show(
            string text,
            string caption,
            MessageBoxButton button,
            MessageBoxImage icon,
            MessageBoxResult defaultResult);

        /// <summary>
        /// Displays a native message box with specified text, caption, and style.
        /// </summary>
        /// <param name="text">Message text.</param>
        /// <param name="caption">Message box title.</param>
        /// <param name="button">Button(s) to be displayed.</param>
        /// <param name="icon">The icon image to be displayed.</param>
        /// <returns><see cref="MessageBoxResult"/> result.</returns>
        public MessageBoxResult Show(string text, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            return Show(text, caption, button, icon, MessageBoxResult.None, MessageBoxOptions.None);
        }

        /// <summary>
        /// Displays a native message box with specified text, caption, and style.
        /// </summary>
        /// <param name="text">Message text.</param>
        /// <param name="caption">Message box title.</param>
        /// <param name="button">Button(s) to be displayed.</param>
        /// <returns><see cref="MessageBoxResult"/> result.</returns>
        public MessageBoxResult Show(string text, string caption, MessageBoxButton button);

        /// <summary>
        /// Displays a native message box with specified text, caption, and style.
        /// </summary>
        /// <param name="text">Message text.</param>
        /// <param name="caption">Message box title.</param>
        /// <returns><see cref="MessageBoxResult"/> result.</returns>
        public MessageBoxResult Show(string text, string caption);

        /// <summary>
        /// Displays a native message box with specified text, caption, and style.
        /// </summary>
        /// <param name="text">Message text.</param>
        /// <returns><see cref="MessageBoxResult"/> result.</returns>
        public MessageBoxResult Show(string text);
    }
}
