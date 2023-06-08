// ==================================================================================================
// <copyright file="InstallAddonsDialogFactory.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels.Factories
{
    using System;
    using AddonWars2.App.ViewModels.Dialogs;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents a factory for <see cref="InstallAddonsDialogViewModel"/> view models.
    /// </summary>
    public class InstallAddonsDialogFactory : IInstallAddonsDialogFactory
    {
        #region Fields

        private readonly ILogger<ErrorDialogViewModel> _logger;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallAddonsDialogFactory"/> class.
        /// </summary>
        /// <param name="logger">A referemnce to <see cref="ILogger"/>.</param>
        public InstallAddonsDialogFactory(ILogger<ErrorDialogViewModel> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc/>
        public InstallAddonsDialogViewModel Create()
        {
            return new InstallAddonsDialogViewModel(_logger);
        }

        #endregion Methods
    }
}
