// ==================================================================================================
// <copyright file="InstallProgressDialogViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels.Dialogs
{
    using System;
    using System.Collections.ObjectModel;
    using AddonWars2.App.ViewModels.SubViewModels;
    using AddonWars2.Downloaders;
    using CommunityToolkit.Mvvm.Input;
    using Microsoft.Extensions.Logging;
    using MvvmDialogs;

    /// <summary>
    /// Represents a view model for the install progress dialog view.
    /// </summary>
    public class InstallProgressDialogViewModel : WindowBaseViewModel, IModalDialogViewModel
    {
        #region Fields

        private readonly BulkAddonDownloader _downloader;
        private bool? _dialogResult = null;
        private ObservableCollection<InstallProgressItemViewModel> _installProgressItems = new ObservableCollection<InstallProgressItemViewModel>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallProgressDialogViewModel"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/> instance.</param>
        /// <param name="downloader">A reference to <see cref="BulkAddonDownloader"/> instance.</param>
        public InstallProgressDialogViewModel(
            ILogger<WindowBaseViewModel> logger,
            BulkAddonDownloader downloader)
            : base(logger)
        {
            _downloader = downloader ?? throw new ArgumentNullException(nameof(downloader));
            _downloader.DownloadStarting += Downloader_DownloadStarting;

            SetDialogResultCommand = new RelayCommand<bool?>(ExecuteSetDialogResultCommand);
        }

        #endregion Constructors

        #region Properties

        /// <inheritdoc/>
        public bool? DialogResult => _dialogResult;

        public ObservableCollection<InstallProgressItemViewModel> InstallProgressItems
        {
            get => _installProgressItems;
            set
            {
                SetProperty(ref _installProgressItems, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets a bulk downloader isntance.
        /// </summary>
        protected BulkAddonDownloader Downloader => _downloader;

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

        #region Methods

        private void Downloader_DownloadStarting(object? sender, EventArgs e)
        {
            if (sender is BulkAddonDownloader downloader)
            {
                foreach (var ipi in InstallProgressItems)
                {
                    AW2Application.Current.Dispatcher.Invoke(() =>
                    {
                        var progress = new Progress<double>(value => ipi.ProgressValue = value);
                        downloader.ProgressCollection.Add(ipi.Token, progress);
                    });
                }
            }
        }

        #endregion Methods
    }
}
