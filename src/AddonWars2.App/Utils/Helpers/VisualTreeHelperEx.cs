// ==================================================================================================
// <copyright file="VisualTreeHelperEx.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Utils.Helpers
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Provides access to various helpers for the visual tree operations.
    /// </summary>
    public static class VisualTreeHelperEx
    {
        /// <summary>
        /// Recursively searches for the specified named parent.
        /// </summary>
        /// <typeparam name="T">The type of a parent.</typeparam>
        /// <param name="child">The child control the search starts from.</param>
        /// <param name="name">The name of a parent element to find.</param>
        /// <returns>
        /// A parent element that matches the requested type and name.
        /// If no matching parent found or <paramref name="child"/> is <see langword="null"/>, returns <see langword="null"/>.
        /// </returns>
        public static T? FindParent<T>(DependencyObject child, string name)
            where T : DependencyObject
        {
            if (child == null)
            {
                return null;
            }

            T? foundParent = null;

            var currentParent = VisualTreeHelper.GetParent(child);

            do
            {
                var frameworkElement = currentParent as FrameworkElement;
                if (frameworkElement is T && frameworkElement.Name == name)
                {
                    foundParent = (T)currentParent;
                    break;
                }

                currentParent = VisualTreeHelper.GetParent(currentParent);
            } while (currentParent != null);

            return foundParent;
        }

        /// <summary>
        /// Recursively searches for the first parent that matches a given type.
        /// </summary>
        /// <typeparam name="T">The type of a parent to search.</typeparam>
        /// <param name="child">The child control the search starts from.</param>
        /// <returns>
        /// A parent element that matches the requested type.
        /// If no matching parent found or <paramref name="child"/> is <see langword="null"/>, returns <see langword="null"/>.
        /// </returns>
        public static T? FindParent<T>(DependencyObject child)
            where T : DependencyObject
        {
            if (child == null)
            {
                return null;
            }

            T? foundParent = null;

            var currentParent = VisualTreeHelper.GetParent(child);

            do
            {
                var frameworkElement = currentParent as FrameworkElement;
                if (frameworkElement is T)
                {
                    foundParent = (T)currentParent;
                    break;
                }

                currentParent = VisualTreeHelper.GetParent(currentParent);
            } while (currentParent != null);

            return foundParent;
        }
    }
}
