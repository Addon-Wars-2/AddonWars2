// ==================================================================================================
// <copyright file="GitHubTokenValidityRule.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Utils.Validation
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using AddonWars2.App.Utils.Helpers;
    using AddonWars2.Services.GitHubClientWrapper.Interfaces;

    /// <summary>
    /// Implements a <see cref="ValidationRule"/> to check if a given token
    /// is a valid GitHub API token.
    /// </summary>
    public class GitHubTokenValidityRule : ValidationRule
    {
        #region Fields

        // Backing field for the validation error message.
        private readonly string _errorMessage = ResourcesHelper.GetApplicationResource<string>("S.Common.ValidationText.GitHubAPIToken");

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets <see cref="GitHubTokenValidityRuleBindingWrapper"/> client wrapper to enable binding.
        /// </summary>
        public GitHubTokenValidityRuleBindingWrapper GitHubClientBindingWrapper { get; set; } = new GitHubTokenValidityRuleBindingWrapper();

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var stringValue = Convert.ToString(value);
            if (stringValue == null)
            {
                return new ValidationResult(false, _errorMessage);
            }

            if (stringValue == string.Empty)
            {
                return ValidationResult.ValidResult;
            }

            IGitHubClientWrapper? wrapper = null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                wrapper = GitHubClientBindingWrapper.GitHubClientWrapper;
            });

            if (wrapper == null)
            {
                return new ValidationResult(false, _errorMessage);
            }

            var isValid = Task.Run(() => CheckValidityAsync(wrapper, stringValue)).GetAwaiter().GetResult();
            if (!isValid)
            {
                return new ValidationResult(false, _errorMessage);
            }

            return ValidationResult.ValidResult;
        }

        private async Task<bool> CheckValidityAsync(IGitHubClientWrapper wrapper, string token)
        {
            return await wrapper.CheckTokenValidityAsync(token);
        }

        #endregion Methods
    }
}
