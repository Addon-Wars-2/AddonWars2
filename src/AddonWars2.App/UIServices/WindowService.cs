// ==================================================================================================
// <copyright file="WindowService.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.UIServices
{
    using System;
    using System.Windows;
    using AddonWars2.App.UIServices.Interfaces;

    /// <summary>
    /// Represents a service used to create application windows based
    /// on window/view-model naming convention or using a specific name
    /// provided as a parameter.
    /// </summary>
    public class WindowService : IWindowService
    {
        #region Fields

        private readonly IWindowLocator _windowLocator;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowService"/> class.
        /// </summary>
        /// <param name="windowLocator">A reference to <see cref="IWindowLocator"/> instance.</param>
        public WindowService(IWindowLocator windowLocator)
        {
            _windowLocator = windowLocator ?? throw new ArgumentNullException(nameof(windowLocator));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a window locator instance.
        /// </summary>
        protected IWindowLocator WindowLocator => _windowLocator;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public void ShowWindow<TView>(Window? owner)
            where TView : Window
        {
            var window = WindowLocator.FindWindow<TView>();
            window.Owner = owner;
            window.Show();
        }

        /// <inheritdoc/>
        public void ShowModalWindow<TView>(Window? owner)
            where TView : Window
        {
            var window = WindowLocator.FindWindow<TView>();
            window.Owner = owner;
            window.ShowDialog();
        }

        #endregion Methods
    }
}
