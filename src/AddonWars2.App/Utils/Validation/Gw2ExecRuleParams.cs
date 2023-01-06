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
        /// <see cref="DependencyProperty"/> for <see cref="Gw2FileExtension"/>.
        /// </summary>
        public static readonly DependencyProperty Gw2FileExtensionProperty =
            DependencyProperty.Register(
                "Gw2FileExtension",
                typeof(string),
                typeof(Gw2ExecRuleParams),
                new PropertyMetadata(default));

        /// <summary>
        /// <see cref="DependencyProperty"/> for <see cref="Gw2ProductName"/>.
        /// </summary>
        public static readonly DependencyProperty Gw2ProductNameProperty =
            DependencyProperty.Register(
                "Gw2ProductName",
                typeof(string),
                typeof(Gw2ExecRuleParams),
                new PropertyMetadata(default));

        /// <summary>
        /// <see cref="DependencyProperty"/> for <see cref="Gw2FileDescription"/>.
        /// </summary>
        public static readonly DependencyProperty Gw2FileDescriptionProperty =
            DependencyProperty.Register(
                "Gw2FileDescription",
                typeof(string),
                typeof(Gw2ExecRuleParams),
                new PropertyMetadata(default));

        #endregion Dependency Properties

        #region Properties

        /// <summary>
        /// Gets or sets gw2 file extension.
        /// </summary>
        public string Gw2FileExtension
        {
            get { return (string)GetValue(Gw2FileExtensionProperty); }
            set { SetValue(Gw2FileExtensionProperty, value); }
        }

        /// <summary>
        /// Gets or sets product name.
        /// </summary>
        public string Gw2ProductName
        {
            get { return (string)GetValue(Gw2ProductNameProperty); }
            set { SetValue(Gw2ProductNameProperty, value); }
        }

        /// <summary>
        /// Gets or sets file description.
        /// </summary>
        public string Gw2FileDescription
        {
            get { return (string)GetValue(Gw2FileDescriptionProperty); }
            set { SetValue(Gw2FileDescriptionProperty, value); }
        }

        #endregion Properties
    }
}
