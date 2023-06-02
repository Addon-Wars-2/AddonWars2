// ==================================================================================================
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
    using AddonWars2.App.Configuration;
    using AddonWars2.App.Utils.Helpers;
    using AddonWars2.App.ViewModels.Commands;
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
        /// View model is fetching a list of approved providers.
        /// </summary>
        RequestingApprovedProviders,

        /// <summary>
        /// View model is a list of addons from a selected provider.
        /// </summary>
        LoadingAddonsList,

        /// <summary>
        /// View model is in a process of resolving selected addons dependencies.
        /// </summary>
        ResolvingDependencies,

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

        private static readonly string _networkConnectionErrorMsg = ResourcesHelper.GetApplicationResource<string>("S.InstallAddonsPage.AddonsList.NoInternetConnection");
        private static readonly string _requestingApprovedProvidersMsg = ResourcesHelper.GetApplicationResource<string>("S.InstallAddonsPage.AddonsList.RequestingApprovedProvidersPlaceholder");
        private static readonly string _requestingApprovedProvidersErrorMsg = ResourcesHelper.GetApplicationResource<string>("S.InstallAddonsPage.AddonsList.FailedToUpdateProvidersPlaceholder");
        private static readonly string _loadingAddonsListMsg = ResourcesHelper.GetApplicationResource<string>("S.InstallAddonsPage.AddonsList.LoadingAddonsListPlaceholder");
        private static readonly string _loadingAddonsListErrorMsg = ResourcesHelper.GetApplicationResource<string>("S.InstallAddonsPage.AddonsList.FailedToUpdateAddonsPlaceholder");
        private static readonly string _resolvingDependenciesErrorMsg = ResourcesHelper.GetApplicationResource<string>("S.InstallAddonsPage.AddonsList.FailedResolveDependenciesPlaceholder");

        private readonly IApplicationConfig _applicationConfig;
        private readonly CommonCommands _commonCommands;
        private readonly IWebSharedData _webSharedData;
        private readonly IRegistryProviderFactory _registryProviderFactory;
        private readonly IDependencyResolverFactory _dependencyResolverFactory;
        private readonly IGitHubClientWrapper _gitHubClientWrapper;
        private readonly IHttpClientWrapper _httpClientWrapper;

        private ManageAddonsViewModelState _viewModelState = ManageAddonsViewModelState.Ready;
        private string _viewModelStatusString = string.Empty;
        private bool _isActuallyLoaded = false;
        private int _gitHubProviderRateLimit = 0;
        private int _gitHubProviderRateLimitRemaining = 0;
        private ObservableCollection<LoadedProviderDataViewModel> _providersCollection = new ObservableCollection<LoadedProviderDataViewModel>();
        private ObservableCollection<LoadedAddonDataViewModel> _selectedAddonsCollection = new ObservableCollection<LoadedAddonDataViewModel>();
        private Dictionary<string, LoadedProviderDataViewModel> _cachedProvidersCollection = new Dictionary<string, LoadedProviderDataViewModel>();
        private LoadedProviderDataViewModel? _selectedProvider;
        private LoadedAddonDataViewModel? _selectedAddon;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageAddonsPageViewModel"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/> instance.</param>
        /// <param name="appConfig">A reference to <see cref="IApplicationConfig"/> instance.</param>
        /// <param name="commonCommands">A reference to <see cref="Commands.CommonCommands"/> instance.</param>
        /// <param name="webSharedData">A reference to <see cref="IWebSharedData"/> instance.</param>
        /// <param name="registryProviderFactory">A reference to <see cref="GithubRegistryProvider"/> instance.</param>
        /// <param name="dependencyResolverFactory">A reference to <see cref="IDependencyResolverFactory"/> instance.</param>
        /// <param name="gitHubClientWrapper">A reference to <see cref="IGitHubClientWrapper"/> instance.</param>
        /// <param name="httpClientWrapper">A reference to <see cref="IHttpClientWrapper"/> instance.</param>
        public ManageAddonsPageViewModel(
            ILogger<NewsPageViewModel> logger,
            IApplicationConfig appConfig,
            CommonCommands commonCommands,
            IWebSharedData webSharedData,
            IRegistryProviderFactory registryProviderFactory,
            IDependencyResolverFactory dependencyResolverFactory,
            IGitHubClientWrapper gitHubClientWrapper,
            IHttpClientWrapper httpClientWrapper)
            : base(logger)
        {
            _applicationConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            _commonCommands = commonCommands ?? throw new ArgumentNullException(nameof(commonCommands));
            _webSharedData = webSharedData ?? throw new ArgumentNullException(nameof(webSharedData));
            _registryProviderFactory = registryProviderFactory ?? throw new ArgumentNullException(nameof(registryProviderFactory));
            _dependencyResolverFactory = dependencyResolverFactory ?? throw new ArgumentNullException(nameof(dependencyResolverFactory));
            _gitHubClientWrapper = gitHubClientWrapper ?? throw new ArgumentNullException(nameof(gitHubClientWrapper));
            _httpClientWrapper = httpClientWrapper ?? throw new ArgumentNullException(nameof(httpClientWrapper));

            GetProvidersListCommand = new AsyncRelayCommand(ExecuteGetProvidersListAsyncCommand, () => IsActuallyLoaded == false);
            ReloadProvidersListCommand = new AsyncRelayCommand( ExecuteGetProvidersListAsyncCommand, () => ViewModelState == ManageAddonsViewModelState.Ready || ViewModelState == ManageAddonsViewModelState.Error);
            GetAddonsFromSelectedProviderCommand = new AsyncRelayCommand(ExecuteGetAddonsFromSelectedProviderAsyncCommand, () => SelectedProvider != null);
            ResolveAddonDependenciesCommand = new RelayCommand<LoadedAddonDataViewModel>(ExecuteResolveAddonDependenciesCommand, (item) => item != null && SelectedProvider != null && (ViewModelState == ManageAddonsViewModelState.Ready || ViewModelState == ManageAddonsViewModelState.Error));

            Logger.LogDebug("Instance initialized.");
        }

        #endregion Constructors

        #region Properties

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
        /// Gets or sets a general status message.
        /// </summary>
        public string ViewModelStatusString
        {
            get => _viewModelStatusString;
            set
            {
                SetProperty(ref _viewModelStatusString, value);
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
                OnPropertyChanged(nameof(SelectedAddonsCollection));
                Logger.LogDebug($"Property set: {value}, name={value?.Name}");
            }
        }

        /// <summary>
        /// Gets or sets a list of addons.
        /// </summary>
        public ObservableCollection<LoadedAddonDataViewModel> SelectedAddonsCollection => SelectedProvider?.Addons ?? new ObservableCollection<LoadedAddonDataViewModel>();

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
        public RelayCommand<LoadedAddonDataViewModel> ResolveAddonDependenciesCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        #region GetProvidersListCommand

        // GetProvidersListCommand command logic.
        private async Task ExecuteGetProvidersListAsyncCommand()
        {
            Logger.LogDebug("Executing command.");

            ProvidersCollection.Clear();
            CachedProvidersCollection.Clear();

            SetViewModelState(ManageAddonsViewModelState.RequestingApprovedProviders, _requestingApprovedProvidersMsg);

            if (!HttpClientWrapper.IsNetworkAvailable())
            {
                SetViewModelState(ManageAddonsViewModelState.Error, _networkConnectionErrorMsg);
                Logger.LogError($"{ViewModelStatusString}");

                return;
            }

            var id = WebSharedData.GitHubAddonsLibRepositoryId;
            var path = WebSharedData.GitHubAddonsLibApprovedProviders;
            var finalState = ManageAddonsViewModelState.Ready;
            var finalStatusString = string.Empty;

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
                finalStatusString = _requestingApprovedProvidersErrorMsg;
                Logger.LogError(e, $"GitHub API rate limit exceeded. The current limit is {e.Remaining}/{e.Limit}.\n");
            }
            catch (AuthorizationException e)
            {
                // Invalid API token.
                finalState = ManageAddonsViewModelState.Error;
                finalStatusString = _requestingApprovedProvidersErrorMsg;
                Logger.LogError(e, "Invalid GitHub API token.");
            }
            catch (NotFoundException e)
            {
                // Repo or branch is not found.
                finalState = ManageAddonsViewModelState.Error;
                finalStatusString = _requestingApprovedProvidersErrorMsg;
                Logger.LogError(e, $"GitHub API returned 404 NotFound -- repository id={id} or branch \"{RegistryProviderBase.ApprovedProvidersBranchName}\" is not found.");
            }
            catch (HttpRequestException e)
            {
                // Bad code from download URL request.
                finalState = ManageAddonsViewModelState.Error;
                finalStatusString = _requestingApprovedProvidersErrorMsg;
                Logger.LogError(e, "Unable to download the list of approved providers.");
            }
            catch (JsonException e)
            {
                // Deserialization error.
                finalState = ManageAddonsViewModelState.Error;
                finalStatusString = _requestingApprovedProvidersErrorMsg;
                Logger.LogError(e, "Unable to deserialize the downloaded JSON.");
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

            SetViewModelState(finalState, finalStatusString);

            Logger.LogInformation("Providers list updated.");
        }

        #endregion GetProvidersListCommand

        #region GetAddonsFromSelectedProviderAsyncCommand

        // GetAddonsFromSelectedProviderCommand command logic.
        private async Task ExecuteGetAddonsFromSelectedProviderAsyncCommand()
        {
            Logger.LogDebug("Executing command.");

            SetViewModelState(ManageAddonsViewModelState.LoadingAddonsList, _loadingAddonsListMsg);

            if (CachedProvidersCollection.ContainsKey(SelectedProvider!.Name)) // null is covered by CanExecute predicate defined in ctor
            {
                Logger.LogDebug("Addons are loaded from the cache.");
            }
            else
            {
                await LoadAddonsFromProviderAsync(SelectedProvider);
            }

            SetViewModelState(ManageAddonsViewModelState.Ready);

            Logger.LogInformation("Addons list updated.");
        }

        // Loads addons from a source.
        private async Task LoadAddonsFromProviderAsync(LoadedProviderDataViewModel selectedProvider)
        {
            Logger.LogDebug($"Loading addons from the selected provider with type={selectedProvider.Type}.");

            SetViewModelState(ManageAddonsViewModelState.LoadingAddonsList, _loadingAddonsListMsg);

            var provider = RegistryProviderFactory.GetProvider(selectedProvider.Type);
            var addonsCollection = await provider.GetAddonsFromAsync(selectedProvider.Data);

            // Ensure data was deserialized normally.
            if (addonsCollection.Data == null || addonsCollection.Schema == null)
            {
                SetViewModelState(ManageAddonsViewModelState.Error, _loadingAddonsListErrorMsg);
                Logger.LogError($"{nameof(addonsCollection)} returned invalid data or schema (null value).");
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

        // UpdateMarkAddonsStatusCommand command logic.
        private void ExecuteResolveAddonDependenciesCommand(LoadedAddonDataViewModel? addonItemModel)
        {
            Logger.LogDebug("Executing command.");

            SetViewModelState(ManageAddonsViewModelState.ResolvingDependencies);

            var resolver = DependencyResolverFactory.GetDependencyResolver(GraphResolverType.DFS, SelectedProvider!.DependencyGraph); // null is covered by CanExecute predicate defined in ctor
            var startNode = SelectedProvider.DependencyGraph.GetNode(addonItemModel!.InternalName);  // null is covered by CanExecute predicate defined in ctor
            resolver.Resolve(startNode);

            //// Walk through all marked addons and collect the required addon names for all marked addons.
            //var requiredInternalNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            //foreach (var addon in SelectedProviderAddonsCollection)
            //{
            //    if (addon?.Data?.RequiredAddons != null && addon.IsMarked == true)
            //    {
            //        foreach (var requiredAddonName in addon.Data.RequiredAddons)
            //        {
            //            requiredInternalNames.Add(requiredAddonName);
            //        }
            //    }
            //}

            //foreach (var item in resolver.Resolved)
            //{
            //    Debug.WriteLine(item.Name);
            //}

            //foreach (var item in resolver.Resolved)
            //{
            //    Debug.WriteLine(item.Name);
            //}

            //// Build a dependency graph for the marked addons.
            //var graph = new DGrpah<IDNode>();
            //foreach (var addon in SelectedProviderAddonsCollection)
            //{
            //    if (requiredInternalNames.Contains(addon.Data.InternalName))
            //    {

            //    }

            //    var node = new DNode(addonName);
            //    var wasAdded = graph.TryAddNode(node);
            //    if (!wasAdded)
            //    {
            //        SetViewModelState(ManageAddonsViewModelState.Error, _resolvingDependenciesErrorMsg);
            //        Logger.LogError($"Unable to resolve addon dependencies, because a node with the name \"{node.Name}\" is already in the graph.");
            //    }
            //}

            //if (graph.IsEmpty)
            //{
            //    SetViewModelState(ManageAddonsViewModelState.Ready);
            //    Logger.LogDebug("Graph is empty and won't be resolved.");
            //    return;
            //}

            //var resolver = DependencyResolverFactory.GetDependencyResolver(GraphResolverType.DFS, graph);
            //resolver.Resolve();



            //// Walk through the list again and mark all addons collected in hash set.
            //foreach (var addon in SelectedProviderAddonsCollection)
            //{
            //    if (requiredInternalNames.Contains(addon.Data.InternalName))
            //    {
            //        addon.IsMarked = true;
            //    }
            //}

            SetViewModelState(ManageAddonsViewModelState.Ready);
        }

        #endregion Commdans Logic

        #region Methods

        // Sets the model state and status string.
        private void SetViewModelState(ManageAddonsViewModelState state, string message = "")
        {
            ViewModelState = state;
            ViewModelStatusString = message ?? string.Empty;
        }

        #endregion Methods
    }
}
