// ==================================================================================================
// <copyright file="SettingsGeneralPageViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels
{
    using System;
    using AddonWars2.App.Models.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// View model used by Settings.General view.
    /// </summary>
    public class SettingsGeneralPageViewModel : BaseViewModel
    {
        #region Fields

        private readonly IApplicationConfig _applicationConfig;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsGeneralPageViewModel"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/> instance.</param>
        /// <param name="appConfig">A reference to <see cref="IApplicationConfig"/> instance.</param>
        public SettingsGeneralPageViewModel(
            ILogger<NewsPageViewModel> logger,
            IApplicationConfig appConfig)
            : base(logger)
        {
            _applicationConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));

            Logger.LogDebug("Instance initialized.");
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a reference to the application config.
        /// </summary>
        public IApplicationConfig AppConfig => _applicationConfig;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Loads settings from a config file when the view model is loaded for the first time.
        /// </summary>
        internal void Initialize()
        {
            // Blank.
        }

        #endregion Methods
    }
}
