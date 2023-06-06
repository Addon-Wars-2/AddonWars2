// ==================================================================================================
// <copyright file="IWindowLocator.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.UIServices.Interfaces
{
    using System.Windows;

    /// <summary>
    /// Represents a contract for window locator services.
    /// </summary>
    public interface IWindowLocator
    {
        #region Methods

        /// <summary>
        /// Locates and returns a new instance of a window.
        /// </summary>
        /// <typeparam name="TView">A view type which is searched for.</typeparam>
        /// <returns><see cref="Window"/> instance.</returns>
        public Window FindWindow<TView>()
            where TView : Window;

        #endregion Methods
    }
}
