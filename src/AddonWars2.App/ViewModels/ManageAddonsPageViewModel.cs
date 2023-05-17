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
    using System.ComponentModel;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using AddonWars2.Addons.Models.AddonInfo;
    using AddonWars2.Addons.RegistryProvider.Interfaces;
    using AddonWars2.Addons.RegistryProvider.Models;
    using AddonWars2.App.Models.Addons;
    using AddonWars2.App.Models.Configuration;
    using AddonWars2.App.ViewModels.Commands;
    using AddonWars2.SharedData;
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
        /// View model failed to update the list of approved providers.
        /// Similar to Ready, but is used to indicate there is an error occured.
        /// </summary>
        FailedToLoadProviders,

        /// <summary>
        /// View model failed to update the list of approved providers.
        /// Similar to Ready, but is used to indicate there is an error occured.
        /// </summary>
        FailedToLoadAddons,
    }

    /// <summary>
    /// View model used by manage addons view.
    /// </summary>
    public class ManageAddonsPageViewModel : BaseViewModel
    {
        #region Fields

        private readonly IApplicationConfig _applicationConfig;
        private readonly CommonCommands _commonCommands;
        private readonly IWebSharedData _webStaticData;
        private readonly IRegistryProviderFactory _registryProviderFactory;

        private ManageAddonsViewModelState _viewModelState = ManageAddonsViewModelState.Ready;
        private bool _isActuallyLoaded = false;
        private ObservableCollection<ProviderInfo> _providers;
        private Dictionary<string, ObservableCollection<AddonItemModel>> _cachedProviders;
        private ProviderInfo? _selectedProvider;
        private ObservableCollection<AddonItemModel> _addonsCollection;
        private AddonItemModel? _selectedAddon;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageAddonsPageViewModel"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/> instance.</param>
        /// <param name="appConfig">A reference to <see cref="ApplicationConfig"/> instance.</param>
        /// <param name="commonCommands">A reference to <see cref="Commands.CommonCommands"/> instance.</param>
        /// <param name="webStaticData">A reference to <see cref="IWebSharedData"/> instance.</param>
        /// <param name="registryProviderFactory">A reference to <see cref="Addons.RegistryProvider.GithubRegistryProvider"/> instance.</param>
        public ManageAddonsPageViewModel(
            ILogger<NewsPageViewModel> logger,
            IApplicationConfig appConfig,
            CommonCommands commonCommands,
            IWebSharedData webStaticData,
            IRegistryProviderFactory registryProviderFactory)
            : base(logger)
        {
            _applicationConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            _commonCommands = commonCommands ?? throw new ArgumentNullException(nameof(commonCommands));
            _webStaticData = webStaticData ?? throw new ArgumentNullException(nameof(webStaticData));
            _registryProviderFactory = registryProviderFactory ?? throw new ArgumentNullException(nameof(registryProviderFactory));

            _providers = new ObservableCollection<ProviderInfo>();
            _cachedProviders = new Dictionary<string, ObservableCollection<AddonItemModel>>();
            _addonsCollection = new ObservableCollection<AddonItemModel>();

            GetProvidersListCommand = new AsyncRelayCommand(ExecuteGetProvidersListCommand, () => IsActuallyLoaded == false);
            ReloadProvidersListCommand = new AsyncRelayCommand(
                ExecuteGetProvidersListCommand,
                () => ViewModelState == ManageAddonsViewModelState.Ready
                    || ViewModelState == ManageAddonsViewModelState.FailedToLoadProviders
                    || ViewModelState == ManageAddonsViewModelState.FailedToLoadAddons);
            GetAddonsFromProviderCommand = new AsyncRelayCommand(ExecuteGetAddonsFromProviderCommand);

            PropertyChangedEventManager.AddHandler(this, InstallAddonsPageViewModel_SelectedAddonChanged, nameof(SelectedAddon));

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
        public IWebSharedData WebStaticData => _webStaticData;

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
        /// Gets or sets a cached list of addon providers.
        /// </summary>
        /// <remarks>
        /// Providers and addons data are cached in the dictionary when loaded once.
        /// </remarks>
        public Dictionary<string, ObservableCollection<AddonItemModel>> CachedProvidersCollection
        {
            get => _cachedProviders;
            set
            {
                SetProperty(ref _cachedProviders, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets or sets a list of loaded addon providers.
        /// </summary>
        public ObservableCollection<ProviderInfo> ProvidersCollection
        {
            get => _providers;
            set
            {
                SetProperty(ref _providers, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets or sets the selected addon provider.
        /// </summary>
        public ProviderInfo? SelectedProvider
        {
            get => _selectedProvider;
            set
            {
                SetProperty(ref _selectedProvider, value);
                Logger.LogDebug($"Property set: {value}, name={value?.Name}");
            }
        }

        /// <summary>
        /// Gets or sets a list of addons.
        /// </summary>
        public ObservableCollection<AddonItemModel> AddonsCollection
        {
            get => _addonsCollection;
            set
            {
                SetProperty(ref _addonsCollection, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets or sets the selected addon.
        /// </summary>
        public AddonItemModel? SelectedAddon
        {
            get => _selectedAddon;
            set
            {
                SetProperty(ref _selectedAddon, value);
                Logger.LogDebug($"Property set: {value}, internal_name={value?.Data.InternalName}");
            }
        }

        /// <summary>
        /// Gets the selected addon description.
        /// </summary>
        public string SelectedAddonDescription
        {
            get
            {
                if (SelectedAddon == null || string.IsNullOrEmpty(SelectedAddon.Data.Description))
                {
                    return "None";
                }

                return SelectedAddon.Data.Description;
            }
        }

        /// <summary>
        /// Gets the selected addon website.
        /// </summary>
        public string SelectedAddonWebsite
        {
            get
            {
                if (SelectedAddon == null || string.IsNullOrEmpty(SelectedAddon.Data.Website))
                {
                    return "None";
                }

                return SelectedAddon.Data.Website;
            }
        }

        /// <summary>
        /// Gets the selected addon authors.
        /// </summary>
        public string SelectedAddonAuthors
        {
            get
            {
                if (SelectedAddon == null || string.IsNullOrEmpty(SelectedAddon.Data.Authors))
                {
                    return "None";
                }

                return SelectedAddon.Data.Authors;
            }
        }

        /// <summary>
        /// Getsa a list of required addons for the selected addon.
        /// </summary>
        public string SelectedAddonRequired
        {
            get
            {
                if (SelectedAddon == null
                    || SelectedAddon.Data.RequiredAddons == null
                    || SelectedAddon.Data.RequiredAddons.Count() == 0)
                {
                    return "None";
                }

                return string.Join(", ", SelectedAddon.Data.RequiredAddons) ?? "None";
            }
        }

        /// <summary>
        /// Gets a list of conflicts with the selected addon authors.
        /// </summary>
        public string SelectedAddonConflicts
        {
            get
            {
                if (SelectedAddon == null
                    || SelectedAddon.Data.Conflicts == null
                    || SelectedAddon.Data.Conflicts.Count() == 0)
                {
                    return "None";
                }

                return string.Join(", ", SelectedAddon.Data.Conflicts) ?? "None";
            }
        }

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
        public AsyncRelayCommand GetAddonsFromProviderCommand { get; private set; }

        /// <summary>
        /// Gets a reference to a registry provider factory.
        /// </summary>
        public IRegistryProviderFactory RegistryProviderFactory => _registryProviderFactory;

        #endregion Commands

        #region Commands Logic

        // GetProvidersListCommand command logic.
        private async Task ExecuteGetProvidersListCommand()
        {
            Logger.LogDebug("Executing command.");

            CachedProvidersCollection.Clear();
            ProvidersCollection.Clear();
            AddonsCollection.Clear();

            ViewModelState = ManageAddonsViewModelState.RequestingApprovedProviders;

            var id = WebStaticData.GitHubAddonsLibRepositoryId;
            var path = WebStaticData.GitHubAddonsLibApprovedProviders;

            try
            {
                Logger.LogDebug("Getting providers...");
                var provider = RegistryProviderFactory.GetProvider(ProviderInfoHostType.GitHub);  // TODO: for now we use only one entry point
                var providers = await provider.GetApprovedProvidersAsync(id, path);
                ProvidersCollection = new ObservableCollection<ProviderInfo>(providers);
            }
            catch (RateLimitExceededException e)
            {
                // TODO: Add GitHub limit counter when GitHub provider is selected.
                // GitHub API rate limit exceeded.
                ViewModelState = ManageAddonsViewModelState.FailedToLoadProviders;
                Logger.LogError(e, $"GitHub API rate limit exceeded. The current limit is {e.Remaining}/{e.Limit}.\n");
                return;
            }
            catch (NotFoundException e)
            {
                // Repo or branch is not found.
                ViewModelState = ManageAddonsViewModelState.FailedToLoadProviders;
                Logger.LogError(e, $"GitHub API returned 404 NotFound -- repository id={id} or branch \"{Addons.AddonLibProvider.RegistryProviderBase.ApprovedProvidersBranchName}\" is not found.");
                return;
            }
            catch (HttpRequestException e)
            {
                // Bad code from download URL request.
                ViewModelState = ManageAddonsViewModelState.FailedToLoadProviders;
                Logger.LogError(e, "Unable to download the list of approved providers.");
                return;
            }
            catch (JsonException e)
            {
                // Deserialization error.
                ViewModelState = ManageAddonsViewModelState.FailedToLoadProviders;
                Logger.LogError(e, "Unable to deserialize the downloaded JSON.");
                return;
            }

            ViewModelState = ManageAddonsViewModelState.Ready;

            Logger.LogInformation("Providers list updated.");
        }

        // GetAddonsFromProviderCommand command logic.
        private async Task ExecuteGetAddonsFromProviderCommand()
        {
            Logger.LogDebug("Executing command.");

            if (SelectedProvider == null)
            {
                return;
            }

            ViewModelState = ManageAddonsViewModelState.LoadingAddonsList;

            // If cached already - load from there.
            if (CachedProvidersCollection.ContainsKey(SelectedProvider.Name))
            {
                LoadAddonsFromCache(SelectedProvider);
            }
            else
            {
                await LoadAddonsFromWebAndCache(SelectedProvider);
            }

            ViewModelState = ManageAddonsViewModelState.Ready;

            Logger.LogInformation("Addons list updated.");
        }

        // Loads a cached addons list.
        private void LoadAddonsFromCache(ProviderInfo selectedProvider)
        {
            Logger.LogDebug("Loading addons from the cache...");
            AddonsCollection = CachedProvidersCollection[selectedProvider.Name];  // key check is done beforehand
        }

        // Loads addons from web source and caches the collection.
        private async Task LoadAddonsFromWebAndCache(ProviderInfo selectedProvider)
        {
            // Request addons.
            AddonsCollection addonsCollection;
            switch (selectedProvider.Type)
            {
                case ProviderInfoHostType.GitHub:
                    Logger.LogDebug("Getting addons from a GitHub host...");
                    ViewModelState = ManageAddonsViewModelState.FailedToLoadAddons;
                    var provider = RegistryProviderFactory.GetProvider(ProviderInfoHostType.GitHub);  // TODO: for now we use only one entry point
                    addonsCollection = await provider.GetAddonsFromAsync(selectedProvider);
                    break;
                case ProviderInfoHostType.Standalone:
                    Logger.LogDebug("Getting addons from a standalone host...");
                    throw new NotSupportedException();  // TODO: implementation
                default:
                    ViewModelState = ManageAddonsViewModelState.FailedToLoadAddons;
                    Logger.LogError("Unsupported host type.");
                    return;
            }

            // Ensure data was deserialized normally.
            if (addonsCollection.Data == null || addonsCollection.Schema == null)
            {
                ViewModelState = ManageAddonsViewModelState.FailedToLoadAddons;
                Logger.LogError($"{nameof(addonsCollection)} returned invalid data or schema (null value).");
                return;
            }

            // Sort and add with animation delay.
            var sorted = await SortAddonsCollection(addonsCollection.Data);
            foreach (var item in sorted)
            {
                if (item != null)
                {
                    AddonsCollection.Add(new AddonItemModel(item));
                    await Task.Delay(50);  // for animation purposes
                    Logger.LogDebug($"Addon with InternalName={item.InternalName} added.");
                }
            }

            // Cache addons.
            var isCached = CachedProvidersCollection.TryAdd(selectedProvider.Name, AddonsCollection);
            if (!isCached)
            {
                Logger.LogError("Unable to cache the addons collection.");
            }
        }

        // Sorts the addon collection.
        private async Task<IEnumerable<AddonData>> SortAddonsCollection(IEnumerable<AddonData> collection)
        {
            Logger.LogDebug("Sorting...");
            return await Task.Run(() => collection.OrderBy(x => x.DisplayName).ToList());
        }

        #endregion Commdans Logic

        #region Methods

        // Updates VM value whenever the selected addon changes.
        private void InstallAddonsPageViewModel_SelectedAddonChanged(object? sender, PropertyChangedEventArgs? e)
        {
            ArgumentNullException.ThrowIfNull(sender, nameof(sender));
            ArgumentNullException.ThrowIfNull(e, nameof(e));

            OnPropertyChanged(nameof(SelectedAddonDescription));
            OnPropertyChanged(nameof(SelectedAddonWebsite));
            OnPropertyChanged(nameof(SelectedAddonAuthors));
            OnPropertyChanged(nameof(SelectedAddonRequired));
            OnPropertyChanged(nameof(SelectedAddonConflicts));

            Logger.LogDebug($"Handled.");
        }

        #endregion Methods
    }
}
