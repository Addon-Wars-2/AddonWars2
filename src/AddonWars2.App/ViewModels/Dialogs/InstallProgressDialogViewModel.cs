// ==================================================================================================
// <copyright file="InstallProgressDialogViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using AddonWars2.App.Configuration;
    using AddonWars2.App.ViewModels.SubViewModels;
    using AddonWars2.Core.DTO.Actions;
    using AddonWars2.Core.Interfaces;
    using AddonWars2.Downloaders;
    using AddonWars2.Downloaders.Interfaces;
    using AddonWars2.Downloaders.Models;
    using AddonWars2.Extractors.Interfaces;
    using AddonWars2.Extractors.Models;
    using AddonWars2.Installers.Enums;
    using AddonWars2.Installers.Interfaces;
    using AddonWars2.Installers.Models;
    using CommunityToolkit.Mvvm.Input;
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
        /// View model is installing addons.
        /// </summary>
        Installing,

        /// <summary>
        /// View model shows an error message.
        /// </summary>
        Failed,

        /// <summary>
        /// View model can be closed.
        /// </summary>
        Completed,
    }

    /// <summary>
    /// Represents a view model for the install progress dialog view.
    /// </summary>
    public class InstallProgressDialogViewModel : WindowBaseViewModel, IModalDialogViewModel
    {
        #region Fields

        private readonly IApplicationConfig _applicationConfig;
        private readonly IAddonDownloaderFactory _addonDownloaderFactory;
        private readonly IAddonExtractorFactory _addonExtractorFactory;
        private readonly IAddonInstallerFactory _addonInstallerFactory;
        private readonly IAddonUninstallerFactory _addonUninstallerFactory;
        private readonly BulkAddonDownloader _bulkDownloader;
        private readonly ILibraryManager _libraryManager;
        private readonly Dictionary<string, IAddonExtractor> _extractors;
        private readonly Dictionary<string, IAddonInstaller> _installers;
        private InstallProgressDialogViewModelState _viewModelState = InstallProgressDialogViewModelState.Ready;
        private IEnumerable<LoadedAddonDataViewModel> _installationSequence = new List<LoadedAddonDataViewModel>();
        private bool _isCancelDownloadButtonEnabled = true;
        private bool _isCancelInstallButtonEnabled = true;
        private bool? _dialogResult = null;
        private ObservableCollection<ProgressItemViewModel> _downloadProgressItems = new ObservableCollection<ProgressItemViewModel>();
        private ObservableCollection<ProgressItemViewModel> _installProgressItems = new ObservableCollection<ProgressItemViewModel>();
        private ProgressItemViewModel _revertProgressItem = new ProgressItemViewModel();
        private CancellationTokenSource _downloadCts = new CancellationTokenSource();
        private CancellationTokenSource _installationCts = new CancellationTokenSource();
        private string _unableToUninstallFiles = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallProgressDialogViewModel"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/> instance.</param>
        /// <param name="appConfig">A reference to <see cref="IApplicationConfig"/>.</param>
        /// <param name="addonDownloaderFactory">A reference to <see cref="IAddonDownloaderFactory"/>.</param>
        /// <param name="addonExtractorFactory">A reference to <see cref="IAddonExtractorFactory"/>.</param>
        /// <param name="addonInstallerFactory">A reference to <see cref="IAddonInstallerFactory"/>.</param>
        /// <param name="addonUninstallerFactory">A reference to <see cref="IAddonUninstallerFactory"/>.</param>
        /// <param name="libraryManager">A reference to <see cref="ILibraryManager"/>.</param>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="logger"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="appConfig"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="addonDownloaderFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="addonExtractorFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="addonInstallerFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="addonUninstallerFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="libraryManager"/> is <see langword="null"/>.</exception>
        public InstallProgressDialogViewModel(
            ILogger<WindowBaseViewModel> logger,
            IApplicationConfig appConfig,
            IAddonDownloaderFactory addonDownloaderFactory,
            IAddonExtractorFactory addonExtractorFactory,
            IAddonInstallerFactory addonInstallerFactory,
            IAddonUninstallerFactory addonUninstallerFactory,
            ILibraryManager libraryManager)
            : base(logger)
        {
            _applicationConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            _addonDownloaderFactory = addonDownloaderFactory ?? throw new ArgumentNullException(nameof(addonDownloaderFactory));
            _addonExtractorFactory = addonExtractorFactory ?? throw new ArgumentNullException(nameof(addonExtractorFactory));
            _addonInstallerFactory = addonInstallerFactory ?? throw new ArgumentNullException(nameof(addonInstallerFactory));
            _addonUninstallerFactory = addonUninstallerFactory ?? throw new ArgumentNullException(nameof(addonUninstallerFactory));
            _libraryManager = libraryManager ?? throw new ArgumentNullException(nameof(libraryManager));

            _bulkDownloader = _addonDownloaderFactory.GetBulkDownloader();
            _extractors = new Dictionary<string, IAddonExtractor>();
            _installers = new Dictionary<string, IAddonInstaller>();

            SetDialogResultCommand = new RelayCommand<bool?>(ExecuteSetDialogResultCommand);
            AbortDownloadCommand = new RelayCommand(ExecuteAbortDownloadCommand, () => IsCancelDownloadButtonEnabled);
            AbortInstallationCommand = new RelayCommand(ExecuteAbortInstallationCommand, () => IsCancelInstallButtonEnabled);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a reference to the application config.
        /// </summary>
        public IApplicationConfig AppConfig => _applicationConfig;

        /// <summary>
        /// Gets a reference to addon downloader factory.
        /// </summary>
        public IAddonDownloaderFactory AddonDownloaderFactory => _addonDownloaderFactory;

        /// <summary>
        /// Gets a reference to addon extractor factory.
        /// </summary>
        public IAddonExtractorFactory AddonExtractorFactory => _addonExtractorFactory;

        /// <summary>
        /// Gets a reference to addon installer factory.
        /// </summary>
        public IAddonInstallerFactory AddonInstallerFactory => _addonInstallerFactory;

        /// <summary>
        /// Gets a reference to addon uninstaller factory.
        /// </summary>
        public IAddonUninstallerFactory AddonUninstallerFactory => _addonUninstallerFactory;

        /// <summary>
        /// Gets a reference to the library manager.
        /// </summary>
        public ILibraryManager LibraryManager => _libraryManager;

        /// <inheritdoc/>
        public bool? DialogResult => _dialogResult;

        /// <summary>
        /// Gets or sets the view model state.
        /// </summary>
        /// <remarks>
        /// The state is used to determine which parts of the dialog UI to show.
        /// </remarks>
        public InstallProgressDialogViewModelState ViewModelState
        {
            get => _viewModelState;
            set
            {
                SetProperty(ref _viewModelState, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets the installation sequence value.
        /// </summary>
        public IEnumerable<LoadedAddonDataViewModel> InstallationSequence
        {
            get => _installationSequence;
            private set => SetProperty(ref _installationSequence, value);
        }

        /// <summary>
        /// Gets <see cref="BulkAddonDownloader"/> object used to download
        /// multiple addons asynchronously.
        /// </summary>
        public BulkAddonDownloader BulkDownloader => _bulkDownloader;

        /// <summary>
        /// Gets a collection of extractors.
        /// </summary>
        public Dictionary<string, IAddonExtractor> Extractors => _extractors;

        /// <summary>
        /// Gets a collection of installers.
        /// </summary>
        public Dictionary<string, IAddonInstaller> Installers => _installers;

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
        /// Gets or sets a collection of addons to be extracted and installed.
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
        /// Gets or sets a progress of items to be reverted.
        /// </summary>
        public ProgressItemViewModel RevertProgressItem
        {
            get => _revertProgressItem;
            set
            {
                SetProperty(ref _revertProgressItem, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the download cancel button
        /// should be enabled or disabled while processing the cancel request.
        /// </summary>
        public bool IsCancelDownloadButtonEnabled
        {
            get => _isCancelDownloadButtonEnabled;
            set
            {
                SetProperty(ref _isCancelDownloadButtonEnabled, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the installation cancel button
        /// should be enabled or disabled while processing the cancel request.
        /// </summary>
        public bool IsCancelInstallButtonEnabled
        {
            get => _isCancelInstallButtonEnabled;
            set
            {
                SetProperty(ref _isCancelInstallButtonEnabled, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets a string representing a list of files
        /// which couldn't be uninstalled on installation failure.
        /// </summary>
        public string UnableToUninstallFiles
        {
            get => _unableToUninstallFiles;
            set
            {
                SetProperty(ref _unableToUninstallFiles, value);

                // Do not log twice since uninstaller will report a list of files.
            }
        }

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

        /// <summary>
        /// Gets a command that aborts isntallation operation.
        /// </summary>
        public RelayCommand AbortInstallationCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        // SetDialogResultCommand commad logic.
        private void ExecuteSetDialogResultCommand(bool? result)
        {
            _dialogResult = result;
        }

        // AbortDownloadCommand commad logic.
        private void ExecuteAbortDownloadCommand()
        {
            IsCancelDownloadButtonEnabled = false;
            _downloadCts.Cancel();
        }

        // AbortInstallationCommand commad logic.
        private void ExecuteAbortInstallationCommand()
        {
            _installationCts.Cancel();
        }

        #endregion Commands Logic

        #region Methods

        /// <summary>
        /// Executes a complete process of downloading, extracting and installing (uninstalling in case of failure) addons.
        /// </summary>
        /// <returns><see cref="Task"/> object.</returns>
        public async Task ExecuteAllAsync()
        {
            ViewModelState = InstallProgressDialogViewModelState.Downloading;

            var downloaded = await DownloadAddonsAsync(_downloadCts.Token);

            ViewModelState = InstallProgressDialogViewModelState.Installing;

            var installResults = new List<InstallResult>();
            foreach (var item in InstallationSequence)
            {
                var downloadResult = downloaded.First(x => (string)x.Metadata["internal_name"] == item.InternalName); // TODO: find another way instead of "metadata" workaround
                var extractResult = await ExtractAsync(item, downloadResult, _installationCts.Token);

                var installer = Installers[item.InternalName];
                var installResult = await InstallAsync(item, extractResult, _installationCts.Token);

                installResults.Add(installResult);  // add even if failed for cleanup purposes

                // Revert the installation process if any of installations has failed.
                if (installResult.Status != InstallResultStatus.Success)
                {
                    Logger.LogError("The installation process was unsuccessful. Reverting the installation...");

                    var uninstallResults = await UninstallAsync(installResults);
                    if (uninstallResults.FailedToUninstallFiles.Count > 0)
                    {
                        UnableToUninstallFiles = BuildListOfFailedToUninstallFiles(uninstallResults);
                    }

                    ViewModelState = InstallProgressDialogViewModelState.Failed;

                    return;
                }
            }

            await DelayAsync(500);
            ViewModelState = InstallProgressDialogViewModelState.Completed;
        }

        /// <summary>
        /// Prepares <see cref="BulkDownloader"/>, <see cref="Extractors"/> and <see cref="Installers"/>
        /// for a given installation sequence by requesting their appropriate types and injects
        /// <see cref="ProgressItemViewModel"/> items to allow to track the progress.
        /// </summary>
        /// <param name="installationSequence">A collection of addon data items requested for installation.</param>
        public void PrepareForSequence(IEnumerable<LoadedAddonDataViewModel> installationSequence)
        {
            PrepareBulkDownloader(installationSequence);
            PrepareExtractors(installationSequence);
            PrepareInstallers(installationSequence);

            InstallationSequence = installationSequence;
        }

        // Extracts the requested addon.
        private async Task<ExtractionResult> ExtractAsync(LoadedAddonDataViewModel loadedAddon, DownloadResult downloadResult, CancellationToken cancellationToken)
        {
            var extractor = Extractors[loadedAddon.InternalName];
            var extractRequest = new ExtractionRequest(downloadResult.Name, downloadResult.Content);

            return await extractor.ExtractAsync(extractRequest, cancellationToken);
        }

        // Installs the requested addon.
        private async Task<InstallResult> InstallAsync(LoadedAddonDataViewModel loadedAddon, ExtractionResult extractionResult, CancellationToken cancellationToken)
        {
            var installer = Installers[loadedAddon.InternalName];
            var actions = loadedAddon.Model.Actions ?? new List<AddonActionBase>();
            var installRequest = new InstallRequest(Path.Join(AppConfig.UserData.Gw2DirPath, installer.Entrypoint), extractionResult, new InstallInstructions(actions));

            return await LibraryManager.InstallAddonAsync(installer, installRequest, cancellationToken);
        }

        // Uninstalls the requested addon.
        private async Task<UninstallResult> UninstallAsync(IEnumerable<InstallResult> installResults)
        {
            var request = new UninstallRequest();
            foreach (var result in installResults)
            {
                foreach (var file in result.InstalledFiles)
                {
                    request.FilesToUninstall.Add(new UninstallRequestFile(file.FilePath));
                }
            }

            var uninstaller = AddonUninstallerFactory.GetInstance();
            return await LibraryManager.UninstallAddonAsync(uninstaller, request);
        }

        // Creates a new instance of a bulk downloader and injects progress items to track
        // download progress in the UI layer.
        private void PrepareBulkDownloader(IEnumerable<LoadedAddonDataViewModel> installationSequence)
        {
            foreach (var addon in installationSequence)
            {
                var pivm = new ProgressItemViewModel()
                {
                    Token = addon.InternalName,
                    DisplayName = addon.DisplayName,
                };

                AttachProgressItem(BulkDownloader, pivm);
                DownloadProgressItems.Add(pivm);
            }
        }

        // Creates new instances of downloaded objects extractors and injects progress items to track
        // download progress in the UI layer.
        private void PrepareExtractors(IEnumerable<LoadedAddonDataViewModel> installationSequence)
        {
            foreach (var addon in installationSequence)
            {
                var extractor = AddonExtractorFactory.GetExtractor(addon.Model.DownloadType);
                Extractors.Add(addon.InternalName, extractor);

                var pivm = new ProgressItemViewModel()
                {
                    Token = addon.InternalName,
                    DisplayName = addon.DisplayName,
                };

                AttachProgressItem(extractor, pivm);
                InstallProgressItems.Add(pivm);
            }
        }

        // Creates new instances of extracted objects installers and injects progress items to track
        // download progress in the UI layer.
        private void PrepareInstallers(IEnumerable<LoadedAddonDataViewModel> installationSequence)
        {
            foreach (var addon in installationSequence)
            {
                var installer = AddonInstallerFactory.GetInstance(addon.Model.InstallMode);
                Installers.Add(addon.InternalName, installer);

                var pivm = new ProgressItemViewModel()
                {
                    Token = addon.InternalName,
                    DisplayName = addon.DisplayName,
                };

                AttachProgressItem(installer, pivm);
                InstallProgressItems.Add(pivm);
            }
        }

        // Injects a progress item into the target allowing it to update the progress of some of its process.
        // The progress item then can be used for binding to expose changes in a process into the UI layer
        // through binding mechanism.
        // It's important that items must be injected from the UI thread:
        // https://learn.microsoft.com/en-us/dotnet/desktop/wpf/advanced/threading-model?view=netframeworkdesktop-4.8
        private void AttachProgressItem(IAttachableProgress target, ProgressItemViewModel item)
        {
            AW2Application.Current.Dispatcher.Invoke(() =>
            {
                var progress = new Progress<double>(value => item.ProgressValue = value);
                target.AttachProgressItem(item.Token, progress);
            });
        }

        // Executes download operation.
        private async Task<IEnumerable<DownloadResult>> DownloadAddonsAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var addonsToInstall = InstallationSequence.Select(x => new BulkDownloadRequest(x.Model));

            return await BulkDownloader.DownloadBulkAsync(addonsToInstall, cancellationToken);
        }

        // Builds a string from a list on files failed to be uninstalled.
        private string BuildListOfFailedToUninstallFiles(UninstallResult uninstallResults)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < uninstallResults.FailedToUninstallFiles.Count; i++)
            {
                sb.Append(uninstallResults.FailedToUninstallFiles[i].FilePath);
                if (i < uninstallResults.FailedToUninstallFiles.Count - 1)
                {
                    sb.Append('\n');
                }
            }

            return sb.ToString();
        }

        // Sets a delay.
        private async Task DelayAsync(int milliseconds)
        {
            await Task.Run(async () => await Task.Delay(milliseconds));
        }

        #endregion Methods
    }
}
