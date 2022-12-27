// ==================================================================================================
// <copyright file="AccentColorExtension.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Extensions.AttachedProperties
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Provides an extension to define accent color is desired.
    /// </summary>
    public static class AccentColorExtension
    {
        #region Attached Properties

        /// <summary>
        /// <see cref="DependencyProperty"/> for <see cref="GetAccentColor(DependencyObject)"/> getter
        /// and <see cref="SetAccentColor(DependencyObject, Brush)"/> setter.
        /// </summary>
        public static readonly DependencyProperty AccentColorProperty =
            DependencyProperty.RegisterAttached(
                "AccentColor",
                typeof(Brush),
                typeof(AccentColorExtension),
                new PropertyMetadata(default(Brush)));

        #endregion Attached Properties

        #region Attached Properties Methods

        /// <summary>
        /// Gets the value of <see cref="AccentColorProperty"/>.
        /// </summary>
        /// <param name="obj">An object the value is get from.</param>
        /// <returns>Brush value.</returns>
        public static Brush GetAccentColor(DependencyObject obj)
        {
            return (Brush)obj.GetValue(AccentColorProperty);
        }

        /// <summary>
        /// Sets the value of <see cref="AccentColorProperty"/>.
        /// </summary>
        /// <param name="obj">An object the value is set to.</param>
        /// <param name="value">Brush value.</param>
        public static void SetAccentColor(DependencyObject obj, Brush value)
        {
            obj.SetValue(AccentColorProperty, value);
        }

        #endregion Attached Properties Methods
    }
}
