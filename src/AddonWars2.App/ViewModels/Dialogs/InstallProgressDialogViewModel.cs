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
    using System.ComponentModel;
    using System.Threading.Tasks;
    using AddonWars2.App.ViewModels.SubViewModels;
    using AddonWars2.Core.Interfaces;
    using AddonWars2.Downloaders;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using Microsoft.Extensions.Logging;
    using MvvmDialogs;

    /// <summary>
    /// Represents <see cref="DownloadAddonsPageViewModel"/> states.
    /// </summary>
    public enum InstallProgressDialogViewModelState
    {
        /// <summary>
        /// View model is ready.
        /// </summary>
        Ready,

        /// <summary>
        /// View model is downloading files.
        /// </summary>
        Downloading,

        /// <summary>
        /// View model is extracting files.
        /// </summary>
        Extracting,

        /// <summary>
        /// View model is installing addons.
        /// </summary>
        Installing,

        /// <summary>
        /// View model is finished installing addons with no errors.
        /// </summary>
        Completed,

        /// <summary>
        /// The operation was aborted by user.
        /// </summary>
        Aborted,

        /// <summary>
        /// View model has failed to perform an operation.
        /// </summary>
        Error,
    }

    /// <summary>
    /// Represents a view model for the install progress dialog view.
    /// </summary>
    public class InstallProgressDialogViewModel : WindowBaseViewModel, IModalDialogViewModel
    {
        #region Fields

        private readonly IMessenger _messenger;
        private readonly BulkAddonDownloader _downloader;
        private InstallProgressDialogViewModelState _viewModelState = InstallProgressDialogViewModelState.Ready;
        private bool? _dialogResult = null;
        private ObservableCollection<ProgressItemViewModel> _downloadProgressItems = new ObservableCollection<ProgressItemViewModel>();
        private ObservableCollection<ProgressItemViewModel> _installProgressItems = new ObservableCollection<ProgressItemViewModel>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallProgressDialogViewModel"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/> instance.</param>
        /// <param name="messenger">A reference to <see cref="IMessenger"/>.</param>
        /// <param name="downloader">A reference to <see cref="BulkAddonDownloader"/> instance.</param>
        public InstallProgressDialogViewModel(
            ILogger<WindowBaseViewModel> logger,
            IMessenger messenger,
            BulkAddonDownloader downloader)
            : base(logger)
        {
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));
            _downloader = downloader ?? throw new ArgumentNullException(nameof(downloader));

            _downloader.DownloadStarted += Downloader_DownloadStarted;
            _downloader.DownloadCompleted += Downloader_DownloadCompleted;
            _downloader.DownloadAborted += Downloader_DownloadAborted;
            _downloader.DownloadFailed += Downloader_DownloadFailed;

            PropertyChangedEventManager.AddHandler(this, ViewModelState_PropertyChanged, nameof(ViewModelState));

            SetDialogResultCommand = new RelayCommand<bool?>(ExecuteSetDialogResultCommand);
            AbortDownloadCommand = new RelayCommand(ExecuteAbortDownloadCommand);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a messenger reference.
        /// </summary>
        public IMessenger Messenger => _messenger;

        /// <summary>
        /// Gets or sets the view model state.
        /// </summary>
        public InstallProgressDialogViewModelState ViewModelState
        {
            get => _viewModelState;
            set
            {
                SetProperty(ref _viewModelState, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <inheritdoc/>
        public bool? DialogResult => _dialogResult;

        /// <summary>
        /// Gets or sets a collection of addons to be downloaded.
        /// </summary>
        public ObservableCollection<ProgressItemViewModel> DownloadProgressItems
        {
            get => _downloadProgressItems;
            set
            {
                SetProperty(ref _downloadProgressItems, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets or sets a collection of addons to be extracted.
        /// </summary>
        public ObservableCollection<ProgressItemViewModel> InstallProgressItems
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
        public BulkAddonDownloader Downloader => _downloader;

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets a command that sets the dialog result.
        /// </summary>
        public RelayCommand<bool?> SetDialogResultCommand { get; private set; }

        /// <summary>
        /// Gets a command that aborts download operation.
        /// </summary>
        public RelayCommand AbortDownloadCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        // SetDialogResultCommand commad logic.
        private void ExecuteSetDialogResultCommand(bool? result)
        {
            _dialogResult = result;
        }

        private void ExecuteAbortDownloadCommand()
        {
            _downloader.CancelTask();
        }

        #endregion Commands Logic

        #region Methods

        /// <summary>
        /// Attaches a new <see cref="ProgressItemViewModel"/> to the view model and its downloader
        /// to report download progress from the backend to the UI. The downloader will update progress
        /// values through the event system, while the UI binding engine will update progress bars
        /// through INPC and binding engine.
        /// </summary>
        /// <param name="target">An <see cref="IAttachableProgress"/> object.</param>
        /// <param name="item">An item to attach.</param>
        public void AttachDownloadProgressItem(IAttachableProgress target, ProgressItemViewModel item)
        {
            AW2Application.Current.Dispatcher.Invoke(() =>
            {
                var progress = new Progress<double>(value => item.ProgressValue = value);
                target.AttachProgressItem(item.Token, progress);
            });

            DownloadProgressItems.Add(item);
        }

        /// <summary>
        /// Attaches a new <see cref="ProgressItemViewModel"/> to the VM and a given target.
        /// </summary>
        /// <param name="target">An <see cref="IAttachableProgress"/> object.</param>
        /// <param name="item">An item to attach.</param>
        public void AttachInstallProgressItem(IAttachableProgress target, ProgressItemViewModel item)
        {
            AW2Application.Current.Dispatcher.Invoke(() =>
            {
                var progress = new Progress<double>(value => item.ProgressValue = value);
                target.AttachProgressItem(item.Token, progress);
            });

            InstallProgressItems.Add(item);
        }

        // Bulk downloader event handler to inject Progress items used to tack the download progress for each addon.
        private void Downloader_DownloadStarted(object? sender, EventArgs e)
        {
            ViewModelState = InstallProgressDialogViewModelState.Downloading;
        }

        // Bulk downloader event handler to track the moment when the download process is finished.
        private async void Downloader_DownloadCompleted(object? sender, EventArgs e)
        {
            await DelayAsync(0); // to prevent switching from download to install view too fast
            ViewModelState = InstallProgressDialogViewModelState.Installing;
        }

        // The operation was aborted by user.
        private void Downloader_DownloadAborted(object? sender, EventArgs e)
        {
            ViewModelState = InstallProgressDialogViewModelState.Aborted;
        }

        // Bulk downloader event handler to track if the proccess has failed.
        private void Downloader_DownloadFailed(object? sender, EventArgs e)
        {
            ViewModelState = InstallProgressDialogViewModelState.Error;
        }

        // Handles ViewModelState PropertyChanged event.
        private void ViewModelState_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModelState))
            {
                if (ViewModelState == InstallProgressDialogViewModelState.Error || ViewModelState == InstallProgressDialogViewModelState.Completed)
                {
                    // Unsubscribe from all events.
                    _downloader.DownloadStarted -= Downloader_DownloadStarted;
                    _downloader.DownloadCompleted -= Downloader_DownloadCompleted;
                    _downloader.DownloadFailed -= Downloader_DownloadFailed;
                }
            }
        }

        // Sets a delay.
        private async Task DelayAsync(int milliseconds)
        {
            await Task.Run(async () => await Task.Delay(milliseconds));
        }

        #endregion Methods
    }
}
