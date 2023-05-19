// ==================================================================================================
// <copyright file="SettingsApiPageViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using AddonWars2.App.Models.Configuration;
    using AddonWars2.App.Utils.Helpers;
    using AddonWars2.Services.GitHubClientWrapper.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// View model used by Settings.API view.
    /// </summary>
    public class SettingsApiPageViewModel : BaseViewModel
    {
        #region Fields

        private static readonly string _gitHubApiTokenErrorMsg = ResourcesHelper.GetApplicationResource<string>("S.Common.ValidationText.GitHubAPIToken");
        private readonly IApplicationConfig _applicationConfig;
        private readonly IGitHubClientWrapper _gitHubClientWrapper;

        private string _gitHubApiToken = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsApiPageViewModel"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/> instance.</param>
        /// <param name="appConfig">A reference to <see cref="IApplicationConfig"/> instance.</param>
        /// <param name="gitHubClientWrapper">A reference to <see cref="IGitHubClientWrapper"/> instance.</param>
        public SettingsApiPageViewModel(
            ILogger<NewsPageViewModel> logger,
            IApplicationConfig appConfig,
            IGitHubClientWrapper gitHubClientWrapper)
            : base(logger)
        {
            _applicationConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            _gitHubClientWrapper = gitHubClientWrapper ?? throw new ArgumentNullException(nameof(gitHubClientWrapper));

            LoadSettingsFromConfig();

            Logger.LogDebug("Instance initialized.");
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a reference to the application config.
        /// </summary>
        public IApplicationConfig AppConfig => _applicationConfig;

        /// <summary>
        /// Gets a reference to GitHub client wrapper.
        /// </summary>
        public IGitHubClientWrapper GitHubClientWrapper => _gitHubClientWrapper;

        /// <summary>
        /// Gets or sets GitHubAPI token.
        /// </summary>
        [CustomValidation(typeof(SettingsApiPageViewModel), nameof(ValidateGitHubApiToken))]
        public string GitHubApiToken
        {
            get => _gitHubApiToken;
            set
            {
                SetProperty(ref _gitHubApiToken, value, validate: true);

                if (!GetErrors(nameof(GitHubApiToken)).Any())
                {
                    AppConfig.UserSettings.UserSettingsApi.GitHubApiToken = value;
                    GitHubClientWrapper.ApiToken = value;
                    Logger.LogDebug($"Property set: <REDACTED>");
                    return;
                }

                GitHubClientWrapper.ApiToken = string.Empty;
                Logger.LogDebug($"Property set, but invalid: <REDACTED>");
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Validates <see cref="GitHubApiToken"/> property.
        /// </summary>
        /// <param name="token">GitHub API token string.</param>
        /// <param name="context">Validation context.</param>
        /// <returns><see cref="ValidationResult"/> object.</returns>
        public static ValidationResult? ValidateGitHubApiToken(string token, ValidationContext context)
        {
            if (token == null)
            {
                return new ValidationResult(_gitHubApiTokenErrorMsg);
            }

            if (token == string.Empty)
            {
                return ValidationResult.Success;
            }

            var instance = (SettingsApiPageViewModel)context.ObjectInstance;
            var isValid = Task.Run(() => instance.GitHubClientWrapper.CheckTokenValidityAsync(token)).GetAwaiter().GetResult();

            return isValid ? ValidationResult.Success : new ValidationResult(_gitHubApiTokenErrorMsg);
        }

        // Loads settings from config on VM init.
        private void LoadSettingsFromConfig()
        {
            // Do not validate.
            _gitHubApiToken = AppConfig.UserSettings.UserSettingsApi.GitHubApiToken;
            OnPropertyChanged(GitHubApiToken);
            GitHubClientWrapper.ApiToken = GitHubApiToken;
        }

        #endregion Methods
    }
}
