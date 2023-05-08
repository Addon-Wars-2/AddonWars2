// ==================================================================================================
// <copyright file="InstallAddonsPageViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels
{
    using AddonWars2.App.Models.Application;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// View model used by install addons view.
    /// </summary>
    public class InstallAddonsPageViewModel : BaseViewModel
    {
        #region Fields

        private readonly ApplicationConfig _applicationConfig;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallAddonsPageViewModel"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/>.</param>
        /// <param name="appConfig">A reference to <see cref="ApplicationConfig"/>.</param>
        public InstallAddonsPageViewModel(
            ILogger<NewsPageViewModel> logger,
            ApplicationConfig appConfig)
            : base(logger)
        {
            _applicationConfig = appConfig;

            Logger.LogDebug("Instance initialized.");
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a reference to the application config.
        /// </summary>
        public ApplicationConfig AppConfig => _applicationConfig;

        #endregion Properties

        #region Commands

        #endregion Commands

        #region Commands Logic

        #endregion Commdans Logic

        #region Methods

        #endregion Methods
    }
}
