// ==================================================================================================
// <copyright file="ErrorDialogViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels.Dialogs
{
    using AddonWars2.App.Extensions.Assists;
    using AddonWars2.App.UIServices.Enums;
    using AddonWars2.App.Utils.Helpers;
    using AddonWars2.App.Views.Dialogs;
    using CommunityToolkit.Mvvm.Input;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Windows;

    /// <summary>
    /// View model used by Install Addon Dependencies view.
    /// </summary>
    public class ErrorDialogViewModel : WindowBaseViewModel
    {
        #region Fields

        private string _title = string.Empty;
        private string _message = string.Empty;
        private string _details = string.Empty;
        private ErrorDialogResult _result = ErrorDialogResult.None;
        private ErrorDialogButtons _flags;
        private bool _isOkButtonVisible = false;
        private bool _isCancelButtonVisible = false;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorDialogViewModel"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/> instance.</param>
        public ErrorDialogViewModel(
            ILogger<ErrorDialogViewModel> logger)
            : base(logger)
        {
            SetResultCommand = new RelayCommand<string>(ExecuteSetResultCommand);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets dialog title.
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        /// <summary>
        /// Gets or sets dialog message.
        /// </summary>
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        /// <summary>
        /// Gets or sets dialog additional details.
        /// </summary>
        public string Details
        {
            get => _details;
            set => SetProperty(ref _details, value);
        }

        /// <summary>
        /// Gets or sets error dialog result.
        /// </summary>
        public ErrorDialogResult Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the OK button is visible.
        /// </summary>
        public bool IsOkButtonVisible
        {
            get => _isOkButtonVisible;
            set => SetProperty(ref _isOkButtonVisible, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Cancel button is visible.
        /// </summary>
        public bool IsCancelButtonVisible
        {
            get => _isCancelButtonVisible;
            set => SetProperty(ref _isCancelButtonVisible, value);
        }

        /// <summary>
        /// Gets or sets buttons flags.
        /// </summary>
        public ErrorDialogButtons Flags
        {
            get => _flags;
            set
            {
                SetProperty(ref _flags, value);
                TranslateFlags(value);
            }
        }

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets a command that sets the dialog result before closing it.
        /// </summary>
        public RelayCommand<string> SetResultCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        // ShowInstallAddonDependenciesDialogCommand command logic.
        private void ExecuteSetResultCommand(string? value)
        {
            var parsed = Enum.TryParse(value, out ErrorDialogResult result);
            Result = parsed ? result : ErrorDialogResult.None;
        }

        #endregion Commands Logic

        #region Methods

        // Sets a flag.
        private void WriteFlag(ErrorDialogButtons flag, bool value)
        {
            if (value)
            {
                _flags |= flag;
            }
            else
            {
                _flags &= ~flag;
            }
        }

        // Translates flags to state sets.
        private void TranslateFlags(ErrorDialogButtons flags)
        {
            if (flags.HasFlag(ErrorDialogButtons.OK))
            {
                IsOkButtonVisible = true;
            }

            if (flags.HasFlag(ErrorDialogButtons.Cancel))
            {
                IsCancelButtonVisible = true;
            }
        }

        #endregion Methods
    }
}
