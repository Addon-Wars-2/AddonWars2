// ==================================================================================================
// <copyright file="InstallProgressDialogFactory.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels.Factories
{
    using System;
    using AddonWars2.App.ViewModels.Dialogs;
    using AddonWars2.Downloaders;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represetns a factory responsibe for creating install progress dialog view models.
    /// </summary>
    public class InstallProgressDialogFactory : IInstallProgressDialogFactory
    {
        #region Fields

        private readonly ILogger<InstallProgressDialogViewModel> _logger;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallProgressDialogFactory"/> class.
        /// </summary>
        /// <param name="logger">A referemnce to <see cref="ILogger"/>.</param>
        public InstallProgressDialogFactory(ILogger<InstallProgressDialogViewModel> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc/>
        public InstallProgressDialogViewModel Create(BulkAddonDownloader downloader)
        {
            return new InstallProgressDialogViewModel(_logger, downloader);
        }

        #endregion Methods
    }
}
