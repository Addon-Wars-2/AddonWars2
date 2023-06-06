// ==================================================================================================
// <copyright file="FileDialogAssist.cs" company="Addon-Wars-2">
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
    public class FileDialogAssist
    {
        #region Attached Properties

        /// <summary>
        /// <see cref="DependencyProperty"/> for <see cref="GetFileDialogService(DependencyObject)"/> getter
        /// and <see cref="SetFileDialogService(DependencyObject, IFileDialogService)"/> setter.
        /// </summary>
        public static readonly DependencyProperty FileDialogServiceProperty =
            DependencyProperty.RegisterAttached(
                "FileDialogService",
                typeof(IFileDialogService),
                typeof(FileDialogAssist),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the value of <see cref="FileDialogServiceProperty"/>.
        /// </summary>
        /// <param name="obj">An object the value is get from.</param>
        /// <returns><see cref="IFileDialogService"/> instance.</returns>
        public static IFileDialogService GetFileDialogService(DependencyObject obj)
        {
            return (IFileDialogService)obj.GetValue(FileDialogServiceProperty);
        }

        /// <summary>
        /// Sets the value of <see cref="FileDialogServiceProperty"/>.
        /// </summary>
        /// <param name="obj">An object the value is set to.</param>
        /// <param name="value">A reference to <see cref="IFileDialogService"/> instance.</param>
        public static void SetFileDialogService(DependencyObject obj, IFileDialogService value)
        {
            obj.SetValue(FileDialogServiceProperty, value);
        }

        /// <summary>
        /// <see cref="DependencyProperty"/> for <see cref="GetSelectedPaths(DependencyObject)"/> getter
        /// and <see cref="SetSelectedPaths(DependencyObject, string)"/> setter.
        /// </summary>
        public static readonly DependencyProperty SelectedPathsProperty =
            DependencyProperty.RegisterAttached(
                "SelectedPaths",
                typeof(string[]),
                typeof(FileDialogAssist),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the value of <see cref="SelectedPathsProperty"/>.
        /// </summary>
        /// <param name="obj">An object the value is get from.</param>
        /// <returns><see cref="IFileDialogService"/> selection results (array of strings).</returns>
        public static string[] GetSelectedPaths(DependencyObject obj)
        {
            return (string[])obj.GetValue(SelectedPathsProperty);
        }

        /// <summary>
        /// Sets the value of <see cref="SelectedPathsProperty"/>.
        /// </summary>
        /// <param name="obj">An object the value is set to.</param>
        /// <param name="value"><see cref="IFileDialogService"/> selection results (array of strings).</param>
        public static void SetSelectedPaths(DependencyObject obj, string[] value)
        {
            obj.SetValue(SelectedPathsProperty, value);
        }

        #endregion Attached Properties
    }
}