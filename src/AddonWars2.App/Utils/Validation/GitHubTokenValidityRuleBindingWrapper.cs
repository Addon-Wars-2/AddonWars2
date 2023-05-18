// ==================================================================================================
// <copyright file="GitHubTokenValidityRuleBindingWrapper.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Utils.Validation
{
    using System.Windows;
    using AddonWars2.Services.GitHubClientWrapper.Interfaces;

    /// <summary>
    /// Wraps <see cref="GitHubTokenValidityRule"/> GitHub client to perform token validation.
    /// </summary>
    public class GitHubTokenValidityRuleBindingWrapper : FrameworkElement
    {

        #region Dependency Properties

        /// <summary>
        /// <see cref="DependencyProperty"/> for <see cref="GitHubClientWrapper"/>.
        /// </summary>
        public static readonly DependencyProperty GitHubClientWrapperProperty =
            DependencyProperty.Register(
                nameof(GitHubClientWrapper),
                typeof(IGitHubClientWrapper),
                typeof(GitHubTokenValidityRuleBindingWrapper),
                new PropertyMetadata(default));

        #endregion Dependency Properties

        #region Properties

        /// <summary>
        /// Gets or sets GitHub client wrapper.
        /// </summary>
        public IGitHubClientWrapper GitHubClientWrapper
        {
            get { return (IGitHubClientWrapper)GetValue(GitHubClientWrapperProperty); }
            set { SetValue(GitHubClientWrapperProperty, value); }
        }

        #endregion Properties
    }
}
