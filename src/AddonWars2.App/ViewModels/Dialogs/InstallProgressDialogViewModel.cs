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
    using AddonWars2.App.Messaging.Tokens;
    using AddonWars2.App.ViewModels.SubViewModels;
    using AddonWars2.Downloaders;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using CommunityToolkit.Mvvm.Messaging.Messages;
    using Microsoft.Extensions.Logging;
    using MvvmDialogs;

    /// <summary>
    /// Represents <see cref="ManageAddonsPageViewModel"/> states.
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
        /// View model is installing addons.
        /// </summary>
        Installing,

        /// <summary>
        /// View model is finished installing addons with no errors.
        /// </summary>
        Completed,

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
        //private readonly AddonDownloadProgressToken _downloadProgressToken = new AddonDownloadProgressToken(string.Empty, 0.0);
        private readonly BulkAddonDownloader _downloader;
        private InstallProgressDialogViewModelState _viewModelState = InstallProgressDialogViewModelState.Ready;
        private bool? _dialogResult = null;
        private ObservableCollection<InstallProgressItemViewModel> _installProgressItems = new ObservableCollection<InstallProgressItemViewModel>();

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
            _downloader.DownloadFailed += Downloader_DownloadFailed;

            PropertyChangedEventManager.AddHandler(this, ViewModelState_PropertyChanged, nameof(ViewModelState));

            //this.Messenger.Register<PropertyChangedMessage<object>, >

            SetDialogResultCommand = new RelayCommand<bool?>(ExecuteSetDialogResultCommand);
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
        /// Gets or sets a collection of addons to be downloaded and installed.
        /// </summary>
        public ObservableCollection<InstallProgressItemViewModel> InstallProgressItems
        {
            get => _installProgressItems;
            set
            {
                SetProperty(ref _installProgressItems, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        ///// <summary>
        ///// Gets a download progress token used in the messaging system.
        ///// </summary>
        //protected AddonDownloadProgressToken DownloadProgressToken => _downloadProgressToken;

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

        // Bulk downloader event handler to inject Progress items used to tack the download progress for each addon.
        private void Downloader_DownloadStarted(object? sender, EventArgs e)
        {
            ViewModelState = InstallProgressDialogViewModelState.Downloading;

            if (sender is BulkAddonDownloader downloader)
            {
                foreach (var ipi in InstallProgressItems)
                {
                    AW2Application.Current.Dispatcher.Invoke(() =>
                    {
                        var progress = new Progress<double>(value => ipi.ProgressValue = value);
                        downloader.AttachProgressItem(ipi.Token, progress);
                    });
                }
            }
        }

        // Bulk downloader event handler to track the moment when the download process is finished.
        private async void Downloader_DownloadCompleted(object? sender, EventArgs e)
        {
            await Delay(500); // to prevent switching from download to install view too fast
            ViewModelState = InstallProgressDialogViewModelState.Installing;
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
        private async Task Delay(int milliseconds)
        {
            await Task.Run(async () => await Task.Delay(milliseconds));
        }

        #endregion Methods
    }
}
