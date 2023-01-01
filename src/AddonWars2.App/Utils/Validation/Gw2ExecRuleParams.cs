// ==================================================================================================
// <copyright file="Gw2ExecRuleParams.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Utils.Validation
{
    using System.Windows;

    /// <summary>
    /// Wraps <see cref="Gw2ExecRule"/> parameters to allow to bind them as <see cref="DependencyProperty"/>.
    /// </summary>
    public class Gw2ExecRuleParams : FrameworkElement
    {
        #region Dependency Properties

        /// <summary>
        /// <see cref="DependencyProperty"/> for <see cref="FileExtension"/>.
        /// </summary>
        public static readonly DependencyProperty FileExtensionProperty =
            DependencyProperty.Register(
                "FileExtension",
                typeof(string),
                typeof(Gw2ExecRuleParams),
                new PropertyMetadata(default));

        /// <summary>
        /// <see cref="DependencyProperty"/> for <see cref="ProductName"/>.
        /// </summary>
        public static readonly DependencyProperty ProductNameProperty =
            DependencyProperty.Register(
                "ProductName",
                typeof(string),
                typeof(Gw2ExecRuleParams),
                new PropertyMetadata(default));

        /// <summary>
        /// <see cref="DependencyProperty"/> for <see cref="FileDescription"/>.
        /// </summary>
        public static readonly DependencyProperty FileDescriptionProperty =
            DependencyProperty.Register(
                "FileDescription",
                typeof(string),
                typeof(Gw2ExecRuleParams),
                new PropertyMetadata(default));

        #endregion Dependency Properties

        #region Properties

        /// <summary>
        /// Gets or sets gw2 file extension.
        /// </summary>
        public string FileExtension
        {
            get { return (string)GetValue(FileExtensionProperty); }
            set { SetValue(FileExtensionProperty, value); }
        }

        /// <summary>
        /// Gets or sets product name.
        /// </summary>
        public string ProductName
        {
            get { return (string)GetValue(ProductNameProperty); }
            set { SetValue(ProductNameProperty, value); }
        }

        /// <summary>
        /// Gets or sets file description.
        /// </summary>
        public string FileDescription
        {
            get { return (string)GetValue(FileDescriptionProperty); }
            set { SetValue(FileDescriptionProperty, value); }
        }

        #endregion Properties
    }
}
