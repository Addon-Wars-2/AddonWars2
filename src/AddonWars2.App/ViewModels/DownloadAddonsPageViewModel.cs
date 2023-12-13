// ==================================================================================================
// <copyright file="DownloadAddonsPageViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using AddonWars2.App.Configuration;
    using AddonWars2.App.UIServices.Enums;
    using AddonWars2.App.Utils.Helpers;
    using AddonWars2.App.ViewModels.Commands;
    using AddonWars2.App.ViewModels.Dialogs;
    using AddonWars2.App.ViewModels.Factories;
    using AddonWars2.App.ViewModels.SubViewModels;
    using AddonWars2.DependencyResolvers.Enums;
    using AddonWars2.DependencyResolvers.Interfaces;
    using AddonWars2.Downloaders.Exceptions;
    using AddonWars2.Providers;
    using AddonWars2.Providers.Enums;
    using AddonWars2.Providers.Interfaces;
    using AddonWars2.Services.GitHubClientWrapper.Interfaces;
    using AddonWars2.Services.HttpClientWrapper.Interfaces;
    using AddonWars2.SharedData.Interfaces;
    using CommunityToolkit.Mvvm.Input;
    using Microsoft.Extensions.Logging;
    using MvvmDialogs;
    using Octokit;

    /// <summary>
    /// Represents <see cref="DownloadAddonsPageViewModel"/> states.
    /// </summary>
    public enum DownloadAddonsViewModelState
    {
        /// <summary>
        /// View model is ready. Default state.
        /// </summary>
        Ready,

        /// <summary>
        /// View model is updating data.
        /// </summary>
        Updating,

        /// <summary>
        /// View model has failed to perform an operation.
        /// </summary>
        Error,
    }

    /// <summary>
    /// View model used by download addons view.
    /// </summary>
    public class DownloadAddonsPageViewModel : BaseViewModel
    {
        #region Fields

        private static readonly string _networkConnectionErrorTitle = ResourcesHelper.GetApplicationResource<string>("S.DownloadAddonsPage.AddonsList.Errors.NoInternetConnection.Title");
        private static readonly string _networkConnectionErrorMessage = ResourcesHelper.GetApplicationResource<string>("S.DownloadAddonsPage.AddonsList.Errors.NoInternetConnection.Message");
        private static readonly string _gitHubRateLimitErrorTitle = ResourcesHelper.GetApplicationResource<string>("S.DownloadAddonsPage.AddonsList.Errors.GitHubRateLimit.Title");
        private static readonly string _gitHubRateLimitErrorMessage = ResourcesHelper.GetApplicationResource<string>("S.DownloadAddonsPage.AddonsList.Errors.GitHubRateLimit.Message");
        private static readonly string _gitHubTokenErrorTitle = ResourcesHelper.GetApplicationResource<string>("S.DownloadAddonsPage.AddonsList.Errors.GitHubToken.Title");
        private static readonly string _gitHubTokenErrorMessage = ResourcesHelper.GetApplicationResource<string>("S.DownloadAddonsPage.AddonsList.Errors.GitHubToken.Message");
        private static readonly string _gitHubNotFoundErrorTitle = ResourcesHelper.GetApplicationResource<string>("S.DownloadAddonsPage.AddonsList.Errors.GitHubNotFound.Title");
        private static readonly string _gitHubNotFoundErrorMessage = ResourcesHelper.GetApplicationResource<string>("S.DownloadAddonsPage.AddonsList.Errors.GitHubNotFound.Message");
        private static readonly string _providersBadCodeErrorTitle = ResourcesHelper.GetApplicationResource<string>("S.DownloadAddonsPage.AddonsList.Errors.GetProvidersBadCode.Title");
        private static readonly string _providersBadCodeErrorMessage = ResourcesHelper.GetApplicationResource<string>("S.DownloadAddonsPage.AddonsList.Errors.GetProvidersBadCode.Message");
        private static readonly string _deserializationFailureErrorTitle = ResourcesHelper.GetApplicationResource<string>("S.DownloadAddonsPage.AddonsList.Errors.DeserializationFailure.Title");
        private static readonly string _deserializationFailureErrorMessage = ResourcesHelper.GetApplicationResource<string>("S.DownloadAddonsPage.AddonsList.Errors.DeserializationFailure.Message");
        private static readonly string _unavailableDependenciesErrorTitle = ResourcesHelper.GetApplicationResource<string>("S.DownloadAddonsPage.AddonsList.Errors.UnavailableDependencies.Title");
        private static readonly string _unavailableDependenciesErrorMessage = ResourcesHelper.GetApplicationResource<string>("S.DownloadAddonsPage.AddonsList.Errors.UnavailableDependencies.Message");
        private static readonly string _failedToDownloadAddonsErrorTitle = ResourcesHelper.GetApplicationResource<string>("S.DownloadAddonsPage.AddonsList.Errors.FailedToDownloadAddons.Title");
        private static readonly string _failedToDownloadAddonsErrorMessage = ResourcesHelper.GetApplicationResource<string>("S.DownloadAddonsPage.AddonsList.Errors.FailedToDownloadAddons.Message");

        private readonly IDialogService _dialogService;
        private readonly IErrorDialogViewModelFactory _errorDialogViewModelFactory;
        private readonly IInstallAddonsDialogFactory _installAddonsDialogFactory;
        private readonly IInstallProgressDialogFactory _downloadProgressDialogFactory;
        private readonly IApplicationConfig _applicationConfig;
        private readonly CommonCommands _commonCommands;
        private readonly IWebSharedData _webSharedData;
        private readonly IRegistryProviderFactory _registryProviderFactory;
        private readonly IDependencyResolverFactory _dependencyResolverFactory;
        private readonly IGitHubClientWrapper _gitHubClientWrapper;
        private readonly IHttpClientWrapper _httpClientWrapper;

        private DownloadAddonsViewModelState _viewModelState = DownloadAddonsViewModelState.Ready;
        private bool _isActuallyLoaded = false;
        private int _gitHubProviderRateLimit = 0;
        private int _gitHubProviderRateLimitRemaining = 0;
        private DateTime _gitHubProviderRateLimitReset = DateTime.MinValue;
        private ObservableCollection<LoadedProviderDataViewModel> _providersCollection = new ObservableCollection<LoadedProviderDataViewModel>();
        private Dictionary<string, LoadedProviderDataViewModel> _cachedProvidersCollection = new Dictionary<string, LoadedProviderDataViewModel>();
        private LoadedProviderDataViewModel? _selectedProvider;
        private LoadedAddonDataViewModel? _selectedAddon;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadAddonsPageViewModel"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/>.</param>
        /// <param name="dialogService">A reference to <see cref="IDialogService"/>.</param>
        /// <param name="errorDialogViewModelFactory">A reference to <see cref="IErrorDialogViewModelFactory"/>.</param>
        /// <param name="installAddonsDialogFactory">A reference to <see cref="IInstallAddonsDialogFactory"/>.</param>
        /// <param name="downloadProgressDialogFactory">A reference to <see cref="IInstallProgressDialogFactory"/>.</param>
        /// <param name="appConfig">A reference to <see cref="IApplicationConfig"/>.</param>
        /// <param name="commonCommands">A reference to <see cref="Commands.CommonCommands"/>.</param>
        /// <param name="webSharedData">A reference to <see cref="IWebSharedData"/>.</param>
        /// <param name="registryProviderFactory">A reference to <see cref="GithubRegistryProvider"/>.</param>
        /// <param name="dependencyResolverFactory">A reference to <see cref="IDependencyResolverFactory"/>.</param>
        /// <param name="gitHubClientWrapper">A reference to <see cref="IGitHubClientWrapper"/>.</param>
        /// <param name="httpClientWrapper">A reference to <see cref="IHttpClientWrapper"/>.</param>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="logger"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="dialogService"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="errorDialogViewModelFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="installAddonsDialogFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="downloadProgressDialogFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="appConfig"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="commonCommands"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="webSharedData"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="registryProviderFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="dependencyResolverFactory"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="gitHubClientWrapper"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="httpClientWrapper"/> is <see langword="null"/>.</exception>
        public DownloadAddonsPageViewModel(
            ILogger<DownloadAddonsPageViewModel> logger,
            IDialogService dialogService,
            IErrorDialogViewModelFactory errorDialogViewModelFactory,
            IInstallAddonsDialogFactory installAddonsDialogFactory,
            IInstallProgressDialogFactory downloadProgressDialogFactory,
            IApplicationConfig appConfig,
            CommonCommands commonCommands,
            IWebSharedData webSharedData,
            IRegistryProviderFactory registryProviderFactory,
            IDependencyResolverFactory dependencyResolverFactory,
            IGitHubClientWrapper gitHubClientWrapper,
            IHttpClientWrapper httpClientWrapper)
            : base(logger)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _errorDialogViewModelFactory = errorDialogViewModelFactory ?? throw new ArgumentNullException(nameof(errorDialogViewModelFactory));
            _installAddonsDialogFactory = installAddonsDialogFactory ?? throw new ArgumentNullException(nameof(installAddonsDialogFactory));
            _downloadProgressDialogFactory = downloadProgressDialogFactory ?? throw new Exception(nameof(downloadProgressDialogFactory));
            _applicationConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            _commonCommands = commonCommands ?? throw new ArgumentNullException(nameof(commonCommands));
            _webSharedData = webSharedData ?? throw new ArgumentNullException(nameof(webSharedData));
            _registryProviderFactory = registryProviderFactory ?? throw new ArgumentNullException(nameof(registryProviderFactory));
            _dependencyResolverFactory = dependencyResolverFactory ?? throw new ArgumentNullException(nameof(dependencyResolverFactory));
            _gitHubClientWrapper = gitHubClientWrapper ?? throw new ArgumentNullException(nameof(gitHubClientWrapper));
            _httpClientWrapper = httpClientWrapper ?? throw new ArgumentNullException(nameof(httpClientWrapper));

            GetProvidersListCommand = new AsyncRelayCommand(ExecuteGetProvidersListAsyncCommand, () => IsActuallyLoaded == false);
            ReloadProvidersListCommand = new AsyncRelayCommand(ExecuteGetProvidersListAsyncCommand, () => ViewModelState == DownloadAddonsViewModelState.Ready || ViewModelState == DownloadAddonsViewModelState.Error);
            GetAddonsFromSelectedProviderCommand = new AsyncRelayCommand(ExecuteGetAddonsFromSelectedProviderAsyncCommand, () => SelectedProvider != null);
            InstallSelectedAddonCommand = new AsyncRelayCommand<LoadedAddonDataViewModel>(ExecuteInstallSelectedAddonAsyncCommand, (item) => item != null && SelectedProvider != null && (ViewModelState == DownloadAddonsViewModelState.Ready || ViewModelState == DownloadAddonsViewModelState.Error));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a reference to <see cref="IDialogService"/> service.
        /// </summary>
        public IDialogService DialogService => _dialogService;

        /// <summary>
        /// Gets a reference to the error dialog factory.
        /// </summary>
        public IErrorDialogViewModelFactory ErrorDialogViewModelFactory => _errorDialogViewModelFactory;

        /// <summary>
        /// Gets a reference to the install addons dialog factory.
        /// </summary>
        public IInstallAddonsDialogFactory InstallAddonsDialogFactory => _installAddonsDialogFactory;

        /// <summary>
        /// Gets a reference to the download progress dialog factory.
        /// </summary>
        public IInstallProgressDialogFactory InstallProgressDialogFactory => _downloadProgressDialogFactory;

        /// <summary>
        /// Gets a reference to the application config.
        /// </summary>
        public IApplicationConfig AppConfig => _applicationConfig;

        /// <summary>
        /// Gets a reference to a common commands class.
        /// </summary>
        public CommonCommands CommonCommands => _commonCommands;

        /// <summary>
        /// Gets a reference to the application web-related static data.
        /// </summary>
        public IWebSharedData WebSharedData => _webSharedData;

        /// <summary>
        /// Gets a reference to a registry provider factory.
        /// </summary>
        public IRegistryProviderFactory RegistryProviderFactory => _registryProviderFactory;

        /// <summary>
        /// Gets a reference to a dependency resolver factory.
        /// </summary>
        public IDependencyResolverFactory DependencyResolverFactory => _dependencyResolverFactory;

        /// <summary>
        /// Gets a reference to GitHub client wrapper.
        /// </summary>
        public IGitHubClientWrapper GitHubClientWrapper => _gitHubClientWrapper;

        /// <summary>
        /// Gets a reference to Http client wrapper.
        /// </summary>
        public IHttpClientWrapper HttpClientWrapper => _httpClientWrapper;

        /// <summary>
        /// Gets or sets the view model state.
        /// </summary>
        public DownloadAddonsViewModelState ViewModelState
        {
            get => _viewModelState;
            set
            {
                SetProperty(ref _viewModelState, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the current view model was loaded or not.
        /// </summary>
        /// <remarks>
        /// This property is used to check if the view model is loaded already, since
        /// the <see cref="FrameworkElement.Loaded"/> event fires every time a tab
        /// becomes selected. Thus we can't bind to this event for single-time actions.
        /// </remarks>
        public bool IsActuallyLoaded
        {
            get => _isActuallyLoaded;
            set
            {
                if (_isActuallyLoaded == false)
                {
                    SetProperty(ref _isActuallyLoaded, value);
                    Logger.LogDebug($"Property set: {value}");
                }
            }
        }

        /// <summary>
        /// Gets or sets the amount of GitHub API requests which
        /// can be made until the limit reset.
        /// </summary>
        public int GitHubProviderRateLimit
        {
            get => _gitHubProviderRateLimit;
            set
            {
                SetProperty(ref _gitHubProviderRateLimit, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets or sets the remained amount of GitHub API requests which
        /// can be made until the limit reset.
        /// </summary>
        public int GitHubProviderRateLimitRemaining
        {
            get => _gitHubProviderRateLimitRemaining;
            set
            {
                SetProperty(ref _gitHubProviderRateLimitRemaining, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> timestamp indicating
        /// when the rate limit will reset.
        /// </summary>
        public DateTime GitHubProviderRateLimitReset
        {
            get => _gitHubProviderRateLimitReset;
            set
            {
                SetProperty(ref _gitHubProviderRateLimitReset, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets or sets a list of loaded addon providers.
        /// </summary>
        public ObservableCollection<LoadedProviderDataViewModel> ProvidersCollection
        {
            get => _providersCollection;
            set
            {
                SetProperty(ref _providersCollection, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets or sets the selected addon provider.
        /// </summary>
        public LoadedProviderDataViewModel? SelectedProvider
        {
            get => _selectedProvider;
            set
            {
                SetProperty(ref _selectedProvider, value);
                OnPropertyChanged(nameof(ProviderAddonsCollection));
                Logger.LogDebug($"Property set: {value}, name={value?.Name}");
            }
        }

        /// <summary>
        /// Gets a list of addons.
        /// </summary>
        public ObservableCollection<LoadedAddonDataViewModel> ProviderAddonsCollection => SelectedProvider?.Addons ?? new ObservableCollection<LoadedAddonDataViewModel>();

        /// <summary>
        /// Gets or sets a list of cached addons.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Each key represents a provider's name since is it unique.
        /// The key's value represents a cached provider which has been already loaded.
        /// </para>
        /// <para>
        /// Addons are added in this dictionary once they were loaded from a selected provider.
        /// This is done to avoid sending web requests for web-hosted providers every time
        /// it is selected from the list of available providers.
        /// </para>
        /// <para>
        /// Reloading a list of providers will also clear cached data.
        /// </para>
        /// </remarks>
        public Dictionary<string, LoadedProviderDataViewModel> CachedProvidersCollection
        {
            get => _cachedProvidersCollection;
            set
            {
                SetProperty(ref _cachedProvidersCollection, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets or sets the selected addon.
        /// </summary>
        public LoadedAddonDataViewModel? SelectedAddon
        {
            get => _selectedAddon;
            set
            {
                SetProperty(ref _selectedAddon, value);
                Logger.LogDebug($"Property set: {value}, internal_name={value?.InternalName}");
            }
        }

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets a command which loads a list of providers.
        /// </summary>
        public AsyncRelayCommand GetProvidersListCommand { get; private set; }

        /// <summary>
        /// Gets a command which reloads a list of providers.
        /// </summary>
        public AsyncRelayCommand ReloadProvidersListCommand { get; private set; }

        /// <summary>
        /// Gets a command which loads a list of addons from a selected provider.
        /// </summary>
        public AsyncRelayCommand GetAddonsFromSelectedProviderCommand { get; private set; }

        /// <summary>
        /// Gets a command which marks or unmarks addons based of their dependencies (required addons).
        /// </summary>
        public AsyncRelayCommand<LoadedAddonDataViewModel> InstallSelectedAddonCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        #region GetProvidersListCommand

        // GetProvidersListCommand command logic.
        private async Task ExecuteGetProvidersListAsyncCommand()
        {
            Logger.LogDebug("Executing command.");

            ProvidersCollection.Clear();
            CachedProvidersCollection.Clear();

            ViewModelState = DownloadAddonsViewModelState.Updating;

            if (!HttpClientWrapper.IsNetworkAvailable())
            {
                ViewModelState = DownloadAddonsViewModelState.Error;

                Logger.LogError($"{_networkConnectionErrorTitle}");

                ShowErrorDialog(_networkConnectionErrorTitle, _networkConnectionErrorMessage);

                return;
            }

            var finalState = DownloadAddonsViewModelState.Ready;

            try
            {
                Logger.LogDebug("Requesting providers.");

                // Copy embedded json to the app data dir.
                var appDataDir = AppConfig.SessionData.AppDataDir;
                var rpFileName = WebSharedData.RegistryProvidersFileName;
                var rpPath = Path.Join(appDataDir, rpFileName);
                if (!Path.Exists(rpPath))
                {
                    await IOHelper.ResourceCopyToAsync($"AddonWars2.App.Resources.{rpFileName}", rpPath);
                }

                var provider = RegistryProviderFactory.GetProvider(ProviderInfoHostType.Local);
                var providers = await provider.GetProvidersAsync(rpPath, -1);  // arbitrary id since we load from a local source
                foreach (var providerInfo in providers)
                {
                    ProvidersCollection.Add(new LoadedProviderDataViewModel(providerInfo));
                }

                await GitHubClientWrapper.GitHubClient.RateLimit.GetRateLimits();  // doesn't consume rate limit points
                UpdateGitHubRateLimitsInfo();
            }
            catch (RateLimitExceededException ex)
            {
                // GitHub API rate limit exceeded.
                finalState = DownloadAddonsViewModelState.Error;
                Logger.LogError(ex, $"GitHub API rate limit exceeded. The current limit is {ex.Remaining}/{ex.Limit}.\n");
                ShowErrorDialog(_gitHubRateLimitErrorTitle, _gitHubRateLimitErrorMessage, $"The current limit: {ex.Remaining}/{ex.Limit}\n{ex.Message}");
            }
            catch (AuthorizationException ex)
            {
                // Invalid API token.
                finalState = DownloadAddonsViewModelState.Error;
                Logger.LogError(ex, "Invalid GitHub API token.");
                ShowErrorDialog(_gitHubTokenErrorTitle, _gitHubTokenErrorMessage, ex.Message);

                // Reset token to avoid excessive error messaging.
                GitHubClientWrapper.ApiToken = string.Empty;

                // Update with the empty token.
                await GitHubClientWrapper.GitHubClient.RateLimit.GetRateLimits();  // doesn't consume rate limit points
                UpdateGitHubRateLimitsInfo();
            }
            catch (NotFoundException ex)
            {
                // Repo or branch is not found.
                finalState = DownloadAddonsViewModelState.Error;
                Logger.LogError(ex, $"GitHub API returned 404 NotFound -- repository id or branch is not found.");
                ShowErrorDialog(_gitHubNotFoundErrorTitle, _gitHubNotFoundErrorMessage, ex.Message);
            }
            catch (HttpRequestException ex)
            {
                // Bad code from download URL request.
                finalState = DownloadAddonsViewModelState.Error;
                Logger.LogError(ex, "Unable to download the list of approved providers.");
                ShowErrorDialog(_providersBadCodeErrorTitle, _providersBadCodeErrorMessage, ex.Message);
            }
            catch (JsonException ex)
            {
                // Deserialization error.
                finalState = DownloadAddonsViewModelState.Error;
                Logger.LogError(ex, "Unable to deserialize the downloaded JSON.");
                ShowErrorDialog(_deserializationFailureErrorTitle, _deserializationFailureErrorMessage, ex.Message);
            }
            finally
            {
                // Blank.
            }

            ////// We always load cached library at the end.
            ////AddCachedLocalProvider();  // TODO: A better implementation is required (especially download-extract-install process).

            SelectedProvider = ProvidersCollection.Count > 0 ? ProvidersCollection.First() : null;

            ViewModelState = finalState;

            Logger.LogInformation("Providers list updated.");
        }

        ////// Create a cached library provider.
        ////private void AddCachedLocalProvider()
        ////{
        ////    var cachedProviderInfo = new ProviderInfo()
        ////    {
        ////        Name = AppConfig.UserData.CachedLibProviderName,
        ////        Type = ProviderInfoHostType.Local,
        ////        Link = AppConfig.UserData.CachedLibFilePath,
        ////    };

        ////    ProvidersCollection.Add(new LoadedProviderDataViewModel(cachedProviderInfo));
        ////}

        #endregion GetProvidersListCommand

        #region GetAddonsFromSelectedProviderAsyncCommand

        // GetAddonsFromSelectedProviderCommand command logic.
        private async Task ExecuteGetAddonsFromSelectedProviderAsyncCommand()
        {
            Logger.LogDebug("Executing command.");

            ViewModelState = DownloadAddonsViewModelState.Updating;

            if (CachedProvidersCollection.ContainsKey(SelectedProvider!.Name)) // null is covered by CanExecute predicate defined in ctor
            {
                Logger.LogDebug("Addons are loaded from cache.");
            }
            else
            {
                Logger.LogDebug($"Loading addons from the selected provider with type={SelectedProvider.Type}.");

                await LoadAddonsFromProviderAsync(SelectedProvider);
                SelectedProvider.RegenerateDependencyGraphIfDirty();
                TryCacheProvider(SelectedProvider);
            }

            ViewModelState = DownloadAddonsViewModelState.Ready;

            Logger.LogInformation("Addons list updated.");
        }

        // Loads addons from a source.
        private async Task LoadAddonsFromProviderAsync(LoadedProviderDataViewModel selectedProvider)
        {
            var provider = RegistryProviderFactory.GetProvider(selectedProvider.Type);

            var addonsCollection = await provider.GetAddonsFromAsync(selectedProvider.Model);
            if (addonsCollection.Data == null || addonsCollection.Schema == null)
            {
                Logger.LogError($"{nameof(addonsCollection)} returned invalid data or schema (null value).");
                return;
            }

            foreach (var addonData in addonsCollection.Data)
            {
                if (addonData != null)
                {
                    selectedProvider.Addons.Add(new LoadedAddonDataViewModel(selectedProvider.Name, addonData));
                }
            }
        }

        // Attempts to cache an addon collection.
        private void TryCacheProvider(LoadedProviderDataViewModel provider)
        {
            CachedProvidersCollection.TryAdd(provider.Name, provider);
        }

        #endregion GetAddonsFromSelectedProviderAsyncCommand

        #region InstallSelectedAddonCommand

        // InstallSelectedAddonCommand command logic.
        private async Task ExecuteInstallSelectedAddonAsyncCommand(LoadedAddonDataViewModel? addonItemModel)
        {
            Logger.LogDebug("Executing command.");

            Logger.LogInformation($"Installation requested for {addonItemModel!.InternalName}"); // null is covered by CanExecute predicate defined in ctor

            // Resolve and make sure all dependencies are available for the installation.
            var resolved = ResolveAddonDependencies(addonItemModel);
            var unavailable = EnsureDependenciesAvailable(resolved);
            if (unavailable.Count > 0)
            {
                Logger.LogError($"Total {unavailable.Count} unavailable dependencies detected. The installation process is canceled.");
                ShowErrorDialog(_unavailableDependenciesErrorTitle, _unavailableDependenciesErrorMessage, string.Join("\n", unavailable));
                return;
            }

            // Create a sequence of addons to be downloaded and installed.
            var installationSequence = CreateInstallationSequence(resolved);
            if (resolved.Count > 1) // returns only itself if there are no dependencies
            {
                Logger.LogDebug("Dependencies detected.");

                var result = ShowInstallAddonsDialog(installationSequence);
                if (result == false)
                {
                    Logger.LogInformation("Installation was canceled by user.");
                    return;
                }
            }

            // TODO: Check for conflicts prior downloading anything.

            var ivm = InstallProgressDialogFactory.GetInstance();
            ivm.PrepareForSequence(installationSequence);

            var progressDialogTask = ShowInstallProgressWindowAsync(ivm);

            try
            {
                await ivm.ExecuteAllAsync();
            }
            catch (AddonDownloaderException ex)
            {
                ShowErrorDialog(_failedToDownloadAddonsErrorTitle, _failedToDownloadAddonsErrorMessage, ex.Message);
                return;
            }
            catch (OperationCanceledException)
            {
                Logger.LogWarning("An operation was interrupted.");
                return;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An unexpected error has occured");
                return;
            }
            finally
            {
                await progressDialogTask;
                await ExecuteGetProvidersListAsyncCommand();
                UpdateGitHubRateLimitsInfo();
            }
        }

        // Resolves dependencies for the provided addon.
        private IList<IDNode> ResolveAddonDependencies(LoadedAddonDataViewModel? addonItemModel)
        {
            var resolver = DependencyResolverFactory.GetDependencyResolver(GraphResolverType.DFS, SelectedProvider!.DependencyGraph); // null is covered by CanExecute predicate defined in ctor
            var startNode = SelectedProvider.DependencyGraph.GetNode(addonItemModel!.InternalName); // null is covered by CanExecute predicate defined in ctor
            resolver.Resolve(startNode);

            return resolver.Resolved;
        }

        // Iterates throw the available addons and checks whether all resolved dependencies
        // are available for the installation. This may happen when some addons specify dependencies,
        // which are not presented in a provider's list.
        private IList<string> EnsureDependenciesAvailable(IList<IDNode> resolved)
        {
            // TODO: search within all avaiable providers.

            var unavailable = new List<string>();
            foreach (var node in resolved)
            {
                var addon = ProviderAddonsCollection.FirstOrDefault(x => x?.InternalName == node.Name, null);
                if (addon == null)
                {
                    Logger.LogWarning($"The dependency \"{node.Name}\" is not found in the provider.");
                    unavailable.Add(node.Name);
                }
            }

            return unavailable;
        }

        // Walks through the addons list and collects only those which are requested by the resolver
        // and which are not already installed.
        private IList<LoadedAddonDataViewModel> CreateInstallationSequence(IList<IDNode> resolved)
        {
            var installationSequence = new List<LoadedAddonDataViewModel>();
            foreach (var node in resolved)
            {
                var addon = ProviderAddonsCollection.FirstOrDefault(x => x?.InternalName == node.Name, null);
                if (addon != null && !addon.IsInstalled && !installationSequence.Contains(addon))
                {
                    installationSequence.Add(addon);
                }
            }

            return installationSequence;
        }

        #endregion InstallSelectedAddonCommand

        #endregion Commands Logic

        #region Methods

        // Shows error dialog.
        private bool? ShowErrorDialog(string title, string message, string? details = null, ErrorDialogButtons buttons = ErrorDialogButtons.OK)
        {
            var vm = ErrorDialogViewModelFactory.Create(title, message, details, buttons);

            return DialogService.ShowDialog(this, vm);
        }

        // Shows a dialog when attempting to install an addon with dependencies.
        private bool? ShowInstallAddonsDialog(IList<LoadedAddonDataViewModel> dependencies)
        {
            var depString = string.Empty;
            for (var i = 0; i < dependencies.Count; i++)
            {
                depString += $"{i + 1,2:D2} {dependencies[i].DisplayName} ({dependencies[i].InternalName})\n";
            }

            var vm = InstallAddonsDialogFactory.GetInstance();
            vm.DependenciesList = depString.TrimEnd(Environment.NewLine.ToCharArray());

            return DialogService.ShowDialog(this, vm);
        }

        // Shows a dialog with installation progress.
        // See for non-blocking modal dialog: https://stackoverflow.com/a/33411037
        private async Task<bool?> ShowInstallProgressWindowAsync(InstallProgressDialogViewModel vm)
        {
            await Task.Yield();

            return DialogService.ShowDialog(this, vm);
        }

        // Updates current GitHub rate limits in the UI.
        private void UpdateGitHubRateLimitsInfo()
        {
            GitHubProviderRateLimitRemaining = GitHubClientWrapper.GitHubLastApiInfo == null ? 0 : GitHubClientWrapper.GitHubLastApiInfo.RateLimit.Remaining;
            GitHubProviderRateLimit = GitHubClientWrapper.GitHubLastApiInfo == null ? 0 : GitHubClientWrapper.GitHubLastApiInfo.RateLimit.Limit;
            GitHubProviderRateLimitReset = GitHubClientWrapper.GitHubLastApiInfo == null ? DateTime.MinValue : GitHubClientWrapper.GitHubLastApiInfo.RateLimit.Reset.DateTime.ToLocalTime();
        }

        #endregion Methods
    }
}
