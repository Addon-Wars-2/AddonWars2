// ==================================================================================================
// <copyright file="UIElementRemoveFocusBehavior.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Extensions.Behaviors
{
    using System.Windows;
    using System.Windows.Input;
    using Microsoft.Xaml.Behaviors;

    /// <summary>
    /// Defines a behavior that clears focus from the <see cref="UIElement"/>.
    /// </summary>
    public class UIElementRemoveFocusBehavior : Behavior<UIElement>
    {
        /// <inheritdoc/>
        protected override void OnAttached()
        {
            if (AssociatedObject != null)
            {
                base.OnAttached();
                AssociatedObject.KeyDown += AssociatedObject_KeyDown;
            }
        }

        /// <inheritdoc/>
        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                base.OnDetaching();
                AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
            }
        }

        // Removes focus.
        private void AssociatedObject_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender is UIElement element)
            {
                if (e.Key == Key.Return || e.Key == Key.Enter || e.Key == Key.Escape)
                {
                    FocusManager.SetFocusedElement(FocusManager.GetFocusScope(element), null);
                    Keyboard.ClearFocus();
                }
            }
        }
    }
}
