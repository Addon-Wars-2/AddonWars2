// ==================================================================================================
// <copyright file="NativeMessageBoxService.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.UIServices
{
    using System.Windows;
    using AddonWars2.App.UIServices.Interfaces;

    /// <summary>
    /// Provides a service to show a native message box.
    /// </summary>
    public class NativeMessageBoxService : INativeMessageBoxService
    {

        /// <inheritdoc/>
        public MessageBoxResult Show(
            string? text,
            string? caption,
            MessageBoxButton button,
            MessageBoxImage icon,
            MessageBoxResult defaultResult,
            MessageBoxOptions options)
        {
            return MessageBox.Show(text, caption, button, icon, defaultResult, options);
        }

        /// <inheritdoc/>
        public MessageBoxResult Show(
            string? text,
            string? caption,
            MessageBoxButton button,
            MessageBoxImage icon,
            MessageBoxResult defaultResult)
        {
            return Show(text, caption, button, icon, defaultResult, MessageBoxOptions.None);
        }

        /// <inheritdoc/>
        public MessageBoxResult Show(string? text, string? caption, MessageBoxButton button, MessageBoxImage icon)
        {
            return Show(text, caption, button, icon, MessageBoxResult.None, MessageBoxOptions.None);
        }

        /// <inheritdoc/>
        public MessageBoxResult Show(string? text, string? caption, MessageBoxButton button)
        {
            return Show(text, caption, button, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
        }

        /// <inheritdoc/>
        public MessageBoxResult Show(string? text, string? caption)
        {
            return Show(text, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
        }

        /// <inheritdoc/>
        public MessageBoxResult Show(string? text)
        {
            return Show(text, string.Empty, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
        }
    }
}
