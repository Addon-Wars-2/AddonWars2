// ==================================================================================================
// <copyright file="SettingsPageViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels
{
    using System;
    using AddonWars2.App.Models.Configuration;
    using AddonWars2.Services.GitHubClientWrapper.Interfaces;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents <see cref="ManageAddonsPageViewModel"/> states.
    /// </summary>
    public enum SettingsPageViewModelState
    {
        /// <summary>
        /// View model is ready. Default state.
        /// </summary>
        Ready,

        /// <summary>
        /// View model is validating a value provided to a field.
        /// </summary>
        Validating,
    }

    /// <summary>
    /// View model used by Settings view.
    /// </summary>
    public class SettingsPageViewModel : BaseViewModel
    {
        #region Fields

        private readonly IApplicationConfig _applicationConfig;
        private readonly IGitHubClientWrapper _gitHubClientWrapper;

        private SettingsPageViewModelState _viewModelState = SettingsPageViewModelState.Ready;
        private string _gitHubApiToken = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsPageViewModel"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/> instance.</param>
        /// <param name="appConfig">A reference to <see cref="IApplicationConfig"/> instance.</param>
        /// <param name="gitHubClientWrapper">A reference to <see cref="IGitHubClientWrapper"/> instance.</param>
        public SettingsPageViewModel(
            ILogger<NewsPageViewModel> logger,
            IApplicationConfig appConfig,
            IGitHubClientWrapper gitHubClientWrapper)
            : base(logger)
        {
            _applicationConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            _gitHubClientWrapper = gitHubClientWrapper ?? throw new ArgumentNullException(nameof(gitHubClientWrapper));

            LoadSettings();
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
        /// Gets or sets the view model state.
        /// </summary>
        public SettingsPageViewModelState ViewModelState
        {
            get => _viewModelState;
            set
            {
                SetProperty(ref _viewModelState, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        #region Settings.General

        #endregion Settings.General

        #region Settings.API

        /// <summary>
        /// Gets or sets GitHubAPI token.
        /// </summary>
        public string GitHubApiToken
        {
            get => _gitHubApiToken;
            set
            {
                SetProperty(ref _gitHubApiToken, value);
                AppConfig.UserSettings.GitHubApiToken = value;
                Logger.LogDebug($"Property set: {value}");
            }
        }

        #endregion Settings.API

        #endregion Properties

        #region Methods

        private void LoadSettings()
        {
            GitHubApiToken = AppConfig.UserSettings.GitHubApiToken;
        }

        #region Settings.General

        #endregion Settings.General

        #region Settings.API

        #endregion Settings.API

        #endregion Methods
    }
}
