// ==================================================================================================
// <copyright file="BindingProxy.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels
{
    using System.Windows;

    /// <summary>
    /// Represents a bindable proxy object.
    /// </summary>
    /// <remarks>
    /// This class is inherited from <see cref="Freezable"/>, which can inherit
    /// DataContext even if a <see cref="Freezable"/> object is not in the visual or logical tree.
    /// </remarks>
    public class BindingProxy : Freezable
    {
        #region Dependency Properties

        /// <summary>
        /// <see cref="DependencyProperty"/> for <see cref="Data"/>.
        /// </summary>
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(
                "Data",
                typeof(object),
                typeof(BindingProxy),
                new PropertyMetadata(default));

        #endregion Dependency Properties

        #region Properties

        /// <summary>
        /// Gets or sets a proxy data object.
        /// </summary>
        public object Data
        {
            get { return GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        #endregion Methods
    }
}
