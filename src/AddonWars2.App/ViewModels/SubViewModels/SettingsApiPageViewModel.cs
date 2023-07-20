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
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using AddonWars2.App.Configuration;
    using AddonWars2.App.Utils.Helpers;
    using AddonWars2.Services.GitHubClientWrapper.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// View model used by Settings.API view.
    /// </summary>
    public class SettingsApiPageViewModel : BaseViewModel
    {
        #region Fields

        private static readonly string _fileNotExistsErrorMsg = ResourcesHelper.GetApplicationResource<string>("S.Common.ValidationText.FileExists");
        private static readonly string _gitHubApiTokenErrorMsg = ResourcesHelper.GetApplicationResource<string>("S.Common.ValidationText.GitHubAPIToken");

        private static string _gitHubApiToken = string.Empty;

        private readonly IApplicationConfig _applicationConfig;
        private readonly IGitHubClientWrapper _gitHubClientWrapper;

        private string _gitHubApiTokenFilePath = string.Empty;

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
        public string GitHubApiTokenFilePath
        {
            get => _gitHubApiTokenFilePath;
            set
            {
                SetProperty(ref _gitHubApiTokenFilePath, value, validate: true);

                if (!GetErrors(nameof(GitHubApiTokenFilePath)).Any())
                {
                    AppConfig.UserSettings.UserSettingsApi.GitHubApiTokenFilePath = value;
                    GitHubClientWrapper.ApiToken = _gitHubApiToken;
                    Logger.LogDebug($"Property set: {value}");
                    return;
                }

                GitHubClientWrapper.ApiToken = string.Empty;
                Logger.LogDebug($"Property is invalid and only changed in the UI: {value}");
            }
        }

        #endregion Properties

        #region Methods

        #region Validation

        /// <summary>
        /// Validates <see cref="GitHubApiTokenFilePath"/> property.
        /// </summary>
        /// <param name="filepath">A filepath to the GitHub token file.</param>
        /// <param name="context">Validation context.</param>
        /// <returns><see cref="ValidationResult"/> object.</returns>
        public static ValidationResult? ValidateGitHubApiToken(string filepath, ValidationContext context)
        {
            if (!File.Exists(filepath))
            {
                return new ValidationResult(_fileNotExistsErrorMsg);
            }

            _gitHubApiToken = ReadGitHubTokenFromFile(filepath);
            var instance = (SettingsApiPageViewModel)context.ObjectInstance;
            var isValid = Task.Run(async () => await instance.Validate(_gitHubApiToken)).GetAwaiter().GetResult();  // TODO: that's really bad.

            return isValid ? ValidationResult.Success : new ValidationResult(_gitHubApiTokenErrorMsg);
        }

        // ValidateGitHubApiToken back-end logic.
        private async Task<bool> Validate(string token)
        {
            return await GitHubClientWrapper.CheckTokenValidityAsync(token);
        }

        #endregion Validation

        /// <summary>
        /// Loads settings from a config file when the view model is loaded for the first time.
        /// </summary>
        internal void Initialize()
        {
            //// Do not validate.
            _gitHubApiTokenFilePath = AppConfig.UserSettings.UserSettingsApi.GitHubApiTokenFilePath;
            OnPropertyChanged(nameof(GitHubApiTokenFilePath));
            GitHubClientWrapper.ApiToken = ReadGitHubTokenFromFile(_gitHubApiTokenFilePath);
        }

        // Reads GitHub API token from a file.
        private static string ReadGitHubTokenFromFile(string filepath)
        {
            var token = string.Empty;
            if (File.Exists(filepath))
            {
                try
                {
                    token = Regex.Unescape(File.ReadLines(filepath).First());
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Unable to read GitHub token file.");
                }
            }

            return token;
        }

        #endregion Methods
    }
}
