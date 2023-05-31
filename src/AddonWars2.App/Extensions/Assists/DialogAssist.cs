// ==================================================================================================
// <copyright file="DialogAssist.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Extensions.Assists
{
    using System.Windows;
    using AddonWars2.App.UIServices.Interfaces;

    /// <summary>
    /// Provides attached properties for opening with dialogs.
    /// </summary>
    public class DialogAssist
    {
        #region Attached Properties

        /// <summary>
        /// <see cref="DependencyProperty"/> for <see cref="GetDialogService(DependencyObject)"/> getter
        /// and <see cref="SetDialogService(DependencyObject, IDialogService)"/> setter.
        /// </summary>
        public static readonly DependencyProperty DialogServiceProperty =
            DependencyProperty.RegisterAttached(
                "DialogService",
                typeof(IDialogService),
                typeof(DialogAssist),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the value of <see cref="DialogServiceProperty"/>.
        /// </summary>
        /// <param name="obj">An object the value is get from.</param>
        /// <returns><see cref="IDialogService"/> instance.</returns>
        public static IDialogService GetDialogService(DependencyObject obj)
        {
            return (IDialogService)obj.GetValue(DialogServiceProperty);
        }

        /// <summary>
        /// Sets the value of <see cref="DialogServiceProperty"/>.
        /// </summary>
        /// <param name="obj">An object the value is set to.</param>
        /// <param name="value">A reference to <see cref="IDialogService"/> instance.</param>
        public static void SetDialogService(DependencyObject obj, IDialogService value)
        {
            obj.SetValue(DialogServiceProperty, value);
        }

        /// <summary>
        /// <see cref="DependencyProperty"/> for <see cref="GetSelectedPaths(DependencyObject)"/> getter
        /// and <see cref="SetSelectedPaths(DependencyObject, string)"/> setter.
        /// </summary>
        public static readonly DependencyProperty SelectedPathsProperty =
            DependencyProperty.RegisterAttached(
                "SelectedPaths",
                typeof(string[]),
                typeof(DialogAssist),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the value of <see cref="SelectedPathsProperty"/>.
        /// </summary>
        /// <param name="obj">An object the value is get from.</param>
        /// <returns><see cref="IDialogService"/> selection results (array of strings).</returns>
        public static string[] GetSelectedPaths(DependencyObject obj)
        {
            return (string[])obj.GetValue(SelectedPathsProperty);
        }

        /// <summary>
        /// Sets the value of <see cref="SelectedPathsProperty"/>.
        /// </summary>
        /// <param name="obj">An object the value is set to.</param>
        /// <param name="value"><see cref="IDialogService"/> selection results (array of strings).</param>
        public static void SetSelectedPaths(DependencyObject obj, string[] value)
        {
            obj.SetValue(SelectedPathsProperty, value);
        }

        #endregion Attached Properties
    }
}