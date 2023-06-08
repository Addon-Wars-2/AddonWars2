// ==================================================================================================
// <copyright file="InstallAddonsDialogViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels.Dialogs
{
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// View model used by Install Addon Dependencies view.
    /// </summary>
    public class InstallAddonsDialogViewModel : WindowBaseViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallAddonsDialogViewModel"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/> instance.</param>
        public InstallAddonsDialogViewModel(
            ILogger<WindowBaseViewModel> logger)
            : base(logger)
        {
            // Blank.
        }

        #endregion Constructors
    }
}
