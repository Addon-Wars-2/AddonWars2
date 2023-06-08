﻿// ==================================================================================================
// <copyright file="ManageAddonsPageViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using AddonWars2.App.Configuration;
    using AddonWars2.App.UIServices.Enums;
    using AddonWars2.App.Utils.Helpers;
    using AddonWars2.App.ViewModels.Commands;
    using AddonWars2.App.ViewModels.Factories;
    using AddonWars2.App.ViewModels.SubViewModels;
    using AddonWars2.DependencyResolvers.Enums;
    using AddonWars2.DependencyResolvers.Interfaces;
    using AddonWars2.Providers;
    using AddonWars2.Providers.DTO;
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
    /// Represents <see cref="ManageAddonsPageViewModel"/> states.
    /// </summary>
    public enum ManageAddonsViewModelState
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
    /// View model used by manage addons view.
    /// </summary>
    public class ManageAddonsPageViewModel : BaseViewModel
    {
        #region Fields

        private static readonly string _networkConnectionErrorTitle = ResourcesHelper.GetApplicationResource<string>("S.ManageAddonsPage.AddonsList.Errors.NoInternetConnection.Title");
        private static readonly string _networkConnectionErrorMessage = ResourcesHelper.GetApplicationResource<string>("S.ManageAddonsPage.AddonsList.Errors.NoInternetConnection.Message");
        private static readonly string _gitHubRateLimitErrorTitle = ResourcesHelper.GetApplicationResource<string>("S.ManageAddonsPage.AddonsList.Errors.GitHubRateLimit.Title");
        private static readonly string _gitHubRateLimitErrorMessage = ResourcesHelper.GetApplicationResource<string>("S.ManageAddonsPage.AddonsList.Errors.GitHubRateLimit.Message");
        private static readonly string _gitHubTokenErrorTitle = ResourcesHelper.GetApplicationResource<string>("S.ManageAddonsPage.AddonsList.Errors.GitHubToken.Title");
        private static readonly string _gitHubTokenErrorMessage = ResourcesHelper.GetApplicationResource<string>("S.ManageAddonsPage.AddonsList.Errors.GitHubToken.Message");
        private static readonly string _gitHubNotFoundErrorTitle = ResourcesHelper.GetApplicationResource<string>("S.ManageAddonsPage.AddonsList.Errors.GitHubNotFound.Title");
        private static readonly string _gitHubNotFoundErrorMessage = ResourcesHelper.GetApplicationResource<string>("S.ManageAddonsPage.AddonsList.Errors.GitHubNotFound.Message");
        private static readonly string _providersBadCodeErrorTitle = ResourcesHelper.GetApplicationResource<string>("S.ManageAddonsPage.AddonsList.Errors.GetProvidersBadCode.Title");
        private static readonly string _providersBadCodeErrorMessage = ResourcesHelper.GetApplicationResource<string>("S.ManageAddonsPage.AddonsList.Errors.GetProvidersBadCode.Message");
        private static readonly string _deserializationFailureErrorTitle = ResourcesHelper.GetApplicationResource<string>("S.ManageAddonsPage.AddonsList.Errors.DeserializationFailure.Title");
        private static readonly string _deserializationFailureErrorMessage = ResourcesHelper.GetApplicationResource<string>("S.ManageAddonsPage.AddonsList.Errors.DeserializationFailure.Message");

        private readonly IDialogService _dialogService;
        private readonly IErrorDialogViewModelFactory _errorDialogViewModelFactory;
        private readonly IInstallAddonsDialogFactory _installAddonsDialogFactory;
        private readonly IApplicationConfig _applicationConfig;
        private readonly CommonCommands _commonCommands;
        private readonly IWebSharedData _webSharedData;
        private readonly IRegistryProviderFactory _registryProviderFactory;
        private readonly IDependencyResolverFactory _dependencyResolverFactory;
        private readonly IGitHubClientWrapper _gitHubClientWrapper;
        private readonly IHttpClientWrapper _httpClientWrapper;

        private ManageAddonsViewModelState _viewModelState = ManageAddonsViewModelState.Ready;
        private bool _isActuallyLoaded = false;
        private int _gitHubProviderRateLimit = 0;
        private int _gitHubProviderRateLimitRemaining = 0;
        private ObservableCollection<LoadedProviderDataViewModel> _providersCollection = new ObservableCollection<LoadedProviderDataViewModel>();
        private Dictionary<string, LoadedProviderDataViewModel> _cachedProvidersCollection = new Dictionary<string, LoadedProviderDataViewModel>();
        private LoadedProviderDataViewModel? _selectedProvider;
        private LoadedAddonDataViewModel? _selectedAddon;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageAddonsPageViewModel"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/>.</param>
        /// <param name="dialogService">A reference to <see cref="IDialogService"/>.</param>
        /// <param name="errorDialogViewModelFactory">A reference to <see cref="IErrorDialogViewModelFactory"/>.</param>
        /// <param name="installAddonsDialogFactory">A reference to <see cref="IInstallAddonsDialogFactory"/>.</param>
        /// <param name="appConfig">A reference to <see cref="IApplicationConfig"/>.</param>
        /// <param name="commonCommands">A reference to <see cref="Commands.CommonCommands"/>.</param>
        /// <param name="webSharedData">A reference to <see cref="IWebSharedData"/>.</param>
        /// <param name="registryProviderFactory">A reference to <see cref="GithubRegistryProvider"/>.</param>
        /// <param name="dependencyResolverFactory">A reference to <see cref="IDependencyResolverFactory"/>.</param>
        /// <param name="gitHubClientWrapper">A reference to <see cref="IGitHubClientWrapper"/>.</param>
        /// <param name="httpClientWrapper">A reference to <see cref="IHttpClientWrapper"/>.</param>
        public ManageAddonsPageViewModel(
            ILogger<NewsPageViewModel> logger,
            IDialogService dialogService,
            IErrorDialogViewModelFactory errorDialogViewModelFactory,
            IInstallAddonsDialogFactory installAddonsDialogFactory,
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
            _applicationConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            _commonCommands = commonCommands ?? throw new ArgumentNullException(nameof(commonCommands));
            _webSharedData = webSharedData ?? throw new ArgumentNullException(nameof(webSharedData));
            _registryProviderFactory = registryProviderFactory ?? throw new ArgumentNullException(nameof(registryProviderFactory));
            _dependencyResolverFactory = dependencyResolverFactory ?? throw new ArgumentNullException(nameof(dependencyResolverFactory));
            _gitHubClientWrapper = gitHubClientWrapper ?? throw new ArgumentNullException(nameof(gitHubClientWrapper));
            _httpClientWrapper = httpClientWrapper ?? throw new ArgumentNullException(nameof(httpClientWrapper));

            GetProvidersListCommand = new AsyncRelayCommand(ExecuteGetProvidersListAsyncCommand, () => IsActuallyLoaded == false);
            ReloadProvidersListCommand = new AsyncRelayCommand(ExecuteGetProvidersListAsyncCommand, () => ViewModelState == ManageAddonsViewModelState.Ready || ViewModelState == ManageAddonsViewModelState.Error);
            GetAddonsFromSelectedProviderCommand = new AsyncRelayCommand(ExecuteGetAddonsFromSelectedProviderAsyncCommand, () => SelectedProvider != null);
            InstallSelectedAddonCommand = new RelayCommand<LoadedAddonDataViewModel>(ExecuteInstallSelectedAddonCommand, (item) => item != null && SelectedProvider != null && (ViewModelState == ManageAddonsViewModelState.Ready || ViewModelState == ManageAddonsViewModelState.Error));

            Logger.LogDebug("Instance initialized.");
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
        public ManageAddonsViewModelState ViewModelState
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
        public RelayCommand<LoadedAddonDataViewModel> InstallSelectedAddonCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        #region GetProvidersListCommand

        // GetProvidersListCommand command logic.
        private async Task ExecuteGetProvidersListAsyncCommand()
        {
            Logger.LogDebug("Executing command.");

            ProvidersCollection.Clear();
            CachedProvidersCollection.Clear();

            ViewModelState = ManageAddonsViewModelState.Updating;

            if (!HttpClientWrapper.IsNetworkAvailable())
            {
                ViewModelState = ManageAddonsViewModelState.Error;

                Logger.LogError($"{_networkConnectionErrorTitle}");

                ShowErrorDialog(_networkConnectionErrorTitle, _networkConnectionErrorMessage);

                return;
            }

            var id = WebSharedData.GitHubAddonsLibRepositoryId;
            var path = WebSharedData.GitHubAddonsLibApprovedProviders;
            var finalState = ManageAddonsViewModelState.Ready;

            try
            {
                Logger.LogDebug("Getting providers.");

                var provider = RegistryProviderFactory.GetProvider(ProviderInfoHostType.GitHub);  // TODO: for now we use only one entry point
                var providers = await provider.GetApprovedProvidersAsync(id, path);
                foreach (var providerInfo in providers)
                {
                    ProvidersCollection.Add(new LoadedProviderDataViewModel(providerInfo));
                }

                GitHubProviderRateLimitRemaining = GitHubClientWrapper.GitHubLastApiInfo == null ? 0 : GitHubClientWrapper.GitHubLastApiInfo.RateLimit.Remaining;
                GitHubProviderRateLimit = GitHubClientWrapper.GitHubLastApiInfo == null ? 0 : GitHubClientWrapper.GitHubLastApiInfo.RateLimit.Limit;
            }
            catch (RateLimitExceededException e)
            {
                // GitHub API rate limit exceeded.
                finalState = ManageAddonsViewModelState.Error;
                Logger.LogError(e, $"GitHub API rate limit exceeded. The current limit is {e.Remaining}/{e.Limit}.\n");
                ShowErrorDialog(_gitHubRateLimitErrorTitle, _gitHubRateLimitErrorMessage, $"The current limit: {e.Remaining}/{e.Limit}\n{e.Message}");
            }
            catch (AuthorizationException e)
            {
                // Invalid API token.
                finalState = ManageAddonsViewModelState.Error;
                Logger.LogError(e, "Invalid GitHub API token.");
                ShowErrorDialog(_gitHubTokenErrorTitle, _gitHubTokenErrorMessage, e.Message);
            }
            catch (NotFoundException e)
            {
                // Repo or branch is not found.
                finalState = ManageAddonsViewModelState.Error;
                Logger.LogError(e, $"GitHub API returned 404 NotFound -- repository id={id} or branch \"{RegistryProviderBase.ApprovedProvidersBranchName}\" is not found.");
                ShowErrorDialog(_gitHubNotFoundErrorTitle, _gitHubNotFoundErrorMessage, e.Message);
            }
            catch (HttpRequestException e)
            {
                // Bad code from download URL request.
                finalState = ManageAddonsViewModelState.Error;
                Logger.LogError(e, "Unable to download the list of approved providers.");
                ShowErrorDialog(_providersBadCodeErrorTitle, _providersBadCodeErrorMessage, e.Message);
            }
            catch (JsonException e)
            {
                // Deserialization error.
                finalState = ManageAddonsViewModelState.Error;
                Logger.LogError(e, "Unable to deserialize the downloaded JSON.");
                ShowErrorDialog(_deserializationFailureErrorTitle, _deserializationFailureErrorMessage, e.Message);
            }

            // Even if we got an error, we always load cached library.
            var cachedProviderInfo = new ProviderInfo()
            {
                Name = "LIBCACHE",  // TODO: Move out to config
                Type = ProviderInfoHostType.Local,
                Link = AppConfig.UserData.CachedLibFilePath,
            };

            ProvidersCollection.Add(new LoadedProviderDataViewModel(cachedProviderInfo));
            SelectedProvider = ProvidersCollection.Count > 0 ? ProvidersCollection.First() : null;

            ViewModelState = finalState;

            Logger.LogInformation("Providers list updated.");
        }

        #endregion GetProvidersListCommand

        #region GetAddonsFromSelectedProviderAsyncCommand

        // GetAddonsFromSelectedProviderCommand command logic.
        private async Task ExecuteGetAddonsFromSelectedProviderAsyncCommand()
        {
            Logger.LogDebug("Executing command.");

            ViewModelState = ManageAddonsViewModelState.Updating;

            if (CachedProvidersCollection.ContainsKey(SelectedProvider!.Name)) // null is covered by CanExecute predicate defined in ctor
            {
                Logger.LogDebug("Addons are loaded from the cache.");
            }
            else
            {
                await LoadAddonsFromProviderAsync(SelectedProvider);
            }

            ViewModelState = ManageAddonsViewModelState.Ready;

            Logger.LogInformation("Addons list updated.");
        }

        // Loads addons from a source.
        private async Task LoadAddonsFromProviderAsync(LoadedProviderDataViewModel selectedProvider)
        {
            Logger.LogDebug($"Loading addons from the selected provider with type={selectedProvider.Type}.");

            var provider = RegistryProviderFactory.GetProvider(selectedProvider.Type);
            var addonsCollection = await provider.GetAddonsFromAsync(selectedProvider.Data);

            // Ensure data was deserialized normally.
            if (addonsCollection.Data == null || addonsCollection.Schema == null)
            {
                ViewModelState = ManageAddonsViewModelState.Error;
                Logger.LogWarning($"{nameof(addonsCollection)} returned invalid data or schema (null value).");

                return;
            }

            // Fill addons collection for the selected provider.
            foreach (var addonData in addonsCollection.Data)
            {
                if (addonData != null)
                {
                    selectedProvider.Addons.Add(new LoadedAddonDataViewModel(addonData));
                    Logger.LogDebug($"Addon \"{addonData.InternalName}\" added.");
                }
            }

            // Regenerate dependency graph.
            if (selectedProvider.ResolveRequired)
            {
                selectedProvider.GenerateDependencyGraph();
            }

            TryCacheProvider(selectedProvider);
        }

        // Caches addons collection.
        private void TryCacheProvider(LoadedProviderDataViewModel provider)
        {
            var isCached = CachedProvidersCollection.TryAdd(provider.Name, provider);
            if (!isCached)
            {
                Logger.LogError($"Unable to cache the addons collection. Provider={provider.Name}.");
            }
            else
            {
                Logger.LogDebug($"Cached. Provider={provider.Name}.");
            }
        }

        #endregion GetAddonsFromSelectedProviderAsyncCommand

        #region InstallSelectedAddonCommand

        // InstallSelectedAddonCommand command logic.
        private void ExecuteInstallSelectedAddonCommand(LoadedAddonDataViewModel? addonItemModel)
        {
            Logger.LogDebug("Executing command.");

            var resolved = ResolveSelectedAddonDependencies(addonItemModel);
            var installationSequence = CreateInstallationSequence(resolved);

            // Show a warning dialog if there are dependencies.
            if (resolved.Count > 1)
            {
                Logger.LogDebug("Dependencies detected.");

                var result = ShowInstallAddonsDialog(installationSequence);
                if (result.HasValue == false || result.Value == true)
                {
                    Logger.LogDebug("Installation cancelled.");
                    return;
                }
            }
        }

        // Resolves dependencies for the provided addon.
        private IList<IDNode> ResolveSelectedAddonDependencies(LoadedAddonDataViewModel? addonItemModel)
        {
            Logger.LogDebug($"Resolving the addon: {addonItemModel?.InternalName}");

            var resolver = DependencyResolverFactory.GetDependencyResolver(GraphResolverType.DFS, SelectedProvider!.DependencyGraph); // null is covered by CanExecute predicate defined in ctor
            var startNode = SelectedProvider.DependencyGraph.GetNode(addonItemModel!.InternalName);  // null is covered by CanExecute predicate defined in ctor
            resolver.Resolve(startNode);

            return resolver.Resolved;
        }

        // Walks through the addons list and collects only those which are requested by the resolver
        // and which are not already installed.
        private IList<LoadedAddonDataViewModel> CreateInstallationSequence(IList<IDNode> resolved)
        {
            Logger.LogDebug($"Creating installation sequence.");

            var installationSeq = new List<LoadedAddonDataViewModel>();
            foreach (var node in resolved)
            {
                var found = ProviderAddonsCollection.FirstOrDefault(x => x?.InternalName == node.Name, null);
                if (found != null && found.IsInstalled == false)
                {
                    installationSeq.Add(found);
                }
            }

            return installationSeq;
        }

        #endregion InstallSelectedAddonCommand

        #endregion Commdans Logic

        #region Methods

        // Shows error dialog.
        private bool? ShowErrorDialog(string title, string message, string? details = null, ErrorDialogButtons buttons = ErrorDialogButtons.OK)
        {
            var vm = ErrorDialogViewModelFactory.Create(title, message, details, buttons);
            var result = DialogService.ShowDialog(this, vm);

            return result;
        }

        // Shows a dialog when attempting to install an addon with dependencies.
        private bool? ShowInstallAddonsDialog(IList<LoadedAddonDataViewModel> dependencies)
        {
            var depString = string.Empty;
            for (var i = 0; i < dependencies.Count; i++)
            {
                depString += $"{i + 1,2:D2} {dependencies[i].DisplayName} ({dependencies[i].InternalName})\n";
            }

            var vm = InstallAddonsDialogFactory.Create();
            vm.DependenciesList = depString.TrimEnd(Environment.NewLine.ToCharArray());

            var result = DialogService.ShowDialog(this, vm);

            return result;
        }

        #endregion Methods
    }
}
