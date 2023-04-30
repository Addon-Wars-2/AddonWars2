// ==================================================================================================
// <copyright file="ValidationAssist.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Extensions.Assists
{
    using System.Windows;
    using System.Windows.Controls.Primitives;

    /// <summary>
    /// Provides various attached properties for validation error template.
    /// </summary>
    public class ValidationAssist
    {
        #region Attached Properties

        /// <summary>
        /// <see cref="DependencyProperty"/> for <see cref="GetUsePopup(DependencyObject)"/> getter
        /// and <see cref="SetUsePopup(DependencyObject, bool)"/> setter.
        /// </summary>
        public static readonly DependencyProperty UsePopupProperty =
            DependencyProperty.RegisterAttached(
                "UsePopup",
                typeof(bool),
                typeof(ValidationAssist),
                new PropertyMetadata(false));

        /// <summary>
        /// Gets the value of <see cref="UsePopupProperty"/>.
        /// </summary>
        /// <param name="obj">An object the value is get from.</param>
        /// <returns><see langword="True"/> is popup to be used, otherwise <see langword="false"/>.</returns>
        public static bool GetUsePopup(DependencyObject obj)
        {
            return (bool)obj.GetValue(UsePopupProperty);
        }

        /// <summary>
        /// Sets the value of <see cref="UsePopupProperty"/>.
        /// </summary>
        /// <param name="obj">An object the value is set to.</param>
        /// <param name="value">Popup usage flag parameter.</param>
        public static void SetUsePopup(DependencyObject obj, bool value)
        {
            obj.SetValue(UsePopupProperty, value);
        }

        /// <summary>
        /// <see cref="DependencyProperty"/> for <see cref="GetPopupPlacement(DependencyObject)"/> getter
        /// and <see cref="SetPopupPlacement(DependencyObject, PlacementMode)"/> setter.
        /// </summary>
        public static readonly DependencyProperty PopupPlacementProperty =
            DependencyProperty.RegisterAttached(
                "PopupPlacement",
                typeof(PlacementMode),
                typeof(ValidationAssist),
                new PropertyMetadata(PlacementMode.Bottom));

        /// <summary>
        /// Gets the value of <see cref="PopupPlacementProperty"/>.
        /// </summary>
        /// <param name="obj">An object the value is get from.</param>
        /// <returns>Popup placement location.</returns>
        public static PlacementMode GetPopupPlacement(DependencyObject obj)
        {
            return (PlacementMode)obj.GetValue(PopupPlacementProperty);
        }

        /// <summary>
        /// Sets the value of <see cref="PopupPlacementProperty"/>.
        /// </summary>
        /// <param name="obj">An object the value is set to.</param>
        /// <param name="value">Popup placement location.</param>
        public static void SetPopupPlacement(DependencyObject obj, PlacementMode value)
        {
            obj.SetValue(PopupPlacementProperty, value);
        }

        #endregion Attached Properties
    }
}
