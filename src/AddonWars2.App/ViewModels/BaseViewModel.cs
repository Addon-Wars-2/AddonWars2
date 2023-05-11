// ==================================================================================================
// <copyright file="BaseViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels
{
    using System;
    using CommunityToolkit.Mvvm.ComponentModel;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A base view model class all application view models should be derived from.
    /// </summary>
    public class BaseViewModel : ObservableObject
    {
        #region Fields

        private static ILogger _logger;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel"/> class.
        /// </summary>
        /// <param name="logger">A referemnce to <see cref="ILogger"/>.</param>
        public BaseViewModel(ILogger<BaseViewModel> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            Logger.LogDebug("Instance initialized.");
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the current logger instance.
        /// </summary>
        protected static ILogger Logger => _logger;

        #endregion Properties
    }
}
