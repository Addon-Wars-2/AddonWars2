// ==================================================================================================
// <copyright file="WindowAssist.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Extensions.Assists
{
    using System.Windows;
    using AddonWars2.App.UIServices.Interfaces;

    /// <summary>
    /// Provides attached properties for opening windows.
    /// </summary>
    public class WindowAssist
    {
        #region Attached Properties

        /// <summary>
        /// <see cref="DependencyProperty"/> for <see cref="GetWindowService(DependencyObject)"/> getter
        /// and <see cref="SetWindowService(DependencyObject, IWindowService)"/> setter.
        /// </summary>
        public static readonly DependencyProperty WindowServiceProperty =
            DependencyProperty.RegisterAttached(
                "WindowService",
                typeof(IWindowService),
                typeof(WindowAssist),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the value of <see cref="WindowServiceProperty"/>.
        /// </summary>
        /// <param name="obj">An object the value is get from.</param>
        /// <returns><see cref="IDialogService"/> instance.</returns>
        public static IWindowService GetWindowService(DependencyObject obj)
        {
            return (IWindowService)obj.GetValue(WindowServiceProperty);
        }

        /// <summary>
        /// Sets the value of <see cref="WindowServiceProperty"/>.
        /// </summary>
        /// <param name="obj">An object the value is set to.</param>
        /// <param name="value">A reference to <see cref="IDialogService"/> instance.</param>
        public static void SetWindowService(DependencyObject obj, IWindowService value)
        {
            obj.SetValue(WindowServiceProperty, value);
        }

        #endregion Attached Properties
    }
}
