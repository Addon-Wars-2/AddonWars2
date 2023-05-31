// ==================================================================================================
// <copyright file="IWindowService.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.UIServices.Interfaces
{
    using System.Windows;

    /// <summary>
    /// Represents a contract for application windows services.
    /// </summary>
    public interface IWindowService
    {
        #region Methods

        /// <summary>
        /// Shows a simple window.
        /// </summary>
        /// <typeparam name="T">A view type which needs to be shown.</typeparam>
        /// <param name="owner">A <see cref="Window"/> object that represents the owner of this <see cref="Window"/>.</param>
        public void ShowWindow<T>(Window? owner)
            where T : Window;

        /// <summary>
        /// Shows a modal window.
        /// </summary>
        /// <typeparam name="T">A view type which needs to be shown.</typeparam>
        /// <param name="owner">A <see cref="Window"/> object that represents the owner of this <see cref="Window"/>.</param>
        public void ShowModalWindow<T>(Window? owner)
            where T : Window;

        #endregion Methods
    }
}
