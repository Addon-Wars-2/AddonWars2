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
    /// View model used by Settings view.
    /// </summary>
    public class SettingsPageViewModel : BaseViewModel
    {
        #region Fields

        private readonly IApplicationConfig _applicationConfig;
        private readonly IGitHubClientWrapper _gitHubClientWrapper;
        private readonly SettingsGeneralPageViewModel _settingsGeneralPageViewModel;
        private readonly SettingsApiPageViewModel _settingsApiPageViewModel;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsPageViewModel"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/> instance.</param>
        /// <param name="appConfig">A reference to <see cref="IApplicationConfig"/> instance.</param>
        /// <param name="gitHubClientWrapper">A reference to <see cref="IGitHubClientWrapper"/> instance.</param>
        /// <param name="settingsGeneralPageViewModel">A reference to <see cref="ViewModels.SettingsGeneralPageViewModel"/> child view model.</param>
        /// <param name="settingsApiPageViewModel">A reference to <see cref="ViewModels.SettingsApiPageViewModel"/> child view model.</param>
        public SettingsPageViewModel(
            ILogger<NewsPageViewModel> logger,
            IApplicationConfig appConfig,
            IGitHubClientWrapper gitHubClientWrapper,
            SettingsGeneralPageViewModel settingsGeneralPageViewModel,
            SettingsApiPageViewModel settingsApiPageViewModel)
            : base(logger)
        {
            _applicationConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            _gitHubClientWrapper = gitHubClientWrapper ?? throw new ArgumentNullException(nameof(gitHubClientWrapper));
            _settingsGeneralPageViewModel = settingsGeneralPageViewModel ?? throw new ArgumentNullException(nameof(settingsGeneralPageViewModel));
            _settingsApiPageViewModel = settingsApiPageViewModel ?? throw new ArgumentNullException(nameof(settingsApiPageViewModel));

            InitializeAllSettings();

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
        /// Gets a reference to the General child view model.
        /// </summary>
        public SettingsGeneralPageViewModel SettingsGeneralPageViewModelInstance => _settingsGeneralPageViewModel;

        /// <summary>
        /// Gets a reference to the API child view model.
        /// </summary>
        public SettingsApiPageViewModel SettingsApiPageViewModelInstance => _settingsApiPageViewModel;

        #endregion Properties

        #region Commands

        #endregion Commands

        #region Commands Logic

        #endregion Commands Logic

        #region Methods

        /// <summary>
        /// Loads settings from a config file when the view model is loaded for the first time.
        /// </summary>
        internal void InitializeAllSettings()
        {
            SettingsGeneralPageViewModelInstance.Initialize();
            SettingsApiPageViewModelInstance.Initialize();
        }

        #endregion Methods
    }
}
