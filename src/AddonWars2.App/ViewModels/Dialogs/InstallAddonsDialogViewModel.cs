// ==================================================================================================
// <copyright file="InstallAddonsDialogViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels.Dialogs
{
    using CommunityToolkit.Mvvm.Input;
    using Microsoft.Extensions.Logging;
    using MvvmDialogs;

    /// <summary>
    /// View model used by Install Addon Dependencies view.
    /// </summary>
    public class InstallAddonsDialogViewModel : WindowBaseViewModel, IModalDialogViewModel
    {
        #region Fields

        private bool? _dialogResult = null;
        private string _dependenciesList = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallAddonsDialogViewModel"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/> instance.</param>
        public InstallAddonsDialogViewModel(
            ILogger<WindowBaseViewModel> logger)
            : base(logger)
        {
            SetDialogResultCommand = new RelayCommand<bool?>(ExecuteSetDialogResultCommand);
        }

        #endregion Constructors

        #region Properties

        /// <inheritdoc/>
        public bool? DialogResult => _dialogResult;

        /// <summary>
        /// Gets or sets a string representation of a list of dependencies.
        /// </summary>
        public string DependenciesList
        {
            get => _dependenciesList;
            set => SetProperty(ref _dependenciesList, value);
        }

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets a command that sets the dialog result.
        /// </summary>
        public RelayCommand<bool?> SetDialogResultCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        // SetDialogResultCommand commad logic.
        private void ExecuteSetDialogResultCommand(bool? result)
        {
            _dialogResult = result;
        }

        #endregion Commands Logic

    }
}
