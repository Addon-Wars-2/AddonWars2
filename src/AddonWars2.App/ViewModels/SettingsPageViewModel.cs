// ==================================================================================================
// <copyright file="SettingsPageViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels
{
    using System;
    using AddonWars2.App.Models.Application;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// View model used by Settings view.
    /// </summary>
    public class SettingsPageViewModel : BaseViewModel
    {
        #region Fields

        private readonly ApplicationConfig _applicationConfig;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsPageViewModel"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/> instance.</param>
        /// <param name="appConfig">A reference to <see cref="ApplicationConfig"/> instance.</param>
        public SettingsPageViewModel(
            ILogger<NewsPageViewModel> logger,
            ApplicationConfig appConfig)
            : base(logger)
        {
            _applicationConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a reference to the application config.
        /// </summary>
        public ApplicationConfig AppConfig => _applicationConfig;

        #endregion Properties
    }
}
