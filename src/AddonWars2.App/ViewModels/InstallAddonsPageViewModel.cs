// ==================================================================================================
// <copyright file="InstallAddonsPageViewModel.cs" company="Addon-Wars-2">
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
    using AddonWars2.App.Models.Application;
    using AddonWars2.App.ViewModels.Commands;
    using AddonWars2.SharedData;
    using CommunityToolkit.Mvvm.Input;
    using Microsoft.Extensions.Logging;
    using Octokit;

    /// <summary>
    /// Represents <see cref="InstallAddonsPageViewModel"/> states.
    /// </summary>
    public enum InstallAddonsViewModelState
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
    /// View model used by install addons view.
    /// </summary>
    public class InstallAddonsPageViewModel : BaseViewModel
    {
        #region Fields

        private readonly ApplicationConfig _applicationConfig;
        private readonly CommonCommands _commonCommands;
        private readonly IWebStaticData _webStaticData;
        private readonly IRegistryProviderFactory _registryProviderFactory;

        private InstallAddonsViewModelState _viewModelState = InstallAddonsViewModelState.Ready;
        private bool _isActuallyLoaded = false;
        private ObservableCollection<ProviderInfo> _providers;
        private ProviderInfo? _selectedProvider;
        private ObservableCollection<AddonItemModel> _addonInfoDataCollection;
        private AddonItemModel? _selectedAddonInfoData;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallAddonsPageViewModel"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/> instance.</param>
        /// <param name="appConfig">A reference to <see cref="ApplicationConfig"/> instance.</param>
        /// <param name="commonCommands">A reference to <see cref="Commands.CommonCommands"/> instance.</param>
        /// <param name="webStaticData">A reference to <see cref="IWebStaticData"/> instance.</param>
        /// <param name="registryProviderFactory">A reference to <see cref="Addons.RegistryProvider.GithubRegistryProvider"/> instance.</param>
        public InstallAddonsPageViewModel(
            ILogger<NewsPageViewModel> logger,
            ApplicationConfig appConfig,
            CommonCommands commonCommands,
            IWebStaticData webStaticData,
            IRegistryProviderFactory registryProviderFactory)
            : base(logger)
        {
            _applicationConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            _commonCommands = commonCommands ?? throw new ArgumentNullException(nameof(commonCommands));
            _webStaticData = webStaticData ?? throw new ArgumentNullException(nameof(webStaticData));
            _registryProviderFactory = registryProviderFactory ?? throw new ArgumentNullException(nameof(registryProviderFactory));
            _providers = new ObservableCollection<ProviderInfo>();
            _addonInfoDataCollection = new ObservableCollection<AddonItemModel>();

            GetProvidersListCommand = new AsyncRelayCommand(ExecuteGetProvidersListCommand);
            GetAddonsFromProviderCommand = new AsyncRelayCommand(ExecuteGetAddonsFromProviderCommand);

            PropertyChangedEventManager.AddHandler(this, InstallAddonsPageViewModel_SelectedAddonChanged, nameof(SelectedAddonInfoData));

            Logger.LogDebug("Instance initialized.");
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a reference to the application config.
        /// </summary>
        public ApplicationConfig AppConfig => _applicationConfig;

        /// <summary>
        /// Gets a reference to a common commands class.
        /// </summary>
        public CommonCommands CommonCommands => _commonCommands;

        /// <summary>
        /// Gets a reference to the application web-related static data.
        /// </summary>
        public IWebStaticData WebStaticData => _webStaticData;

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
        /// Gets or sets a list of addon info providers.
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
        /// Gets or sets the selected addon info provider.
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
        public ObservableCollection<AddonItemModel> AddonInfoDataCollection
        {
            get => _addonInfoDataCollection;
            set
            {
                SetProperty(ref _addonInfoDataCollection, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets or sets the selected addon.
        /// </summary>
        public AddonItemModel? SelectedAddonInfoData
        {
            get => _selectedAddonInfoData;
            set
            {
                SetProperty(ref _selectedAddonInfoData, value);
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
                if (SelectedAddonInfoData == null || string.IsNullOrEmpty(SelectedAddonInfoData.Data.Description))
                {
                    return "None";
                }

                return SelectedAddonInfoData.Data.Description;
            }
        }

        /// <summary>
        /// Gets the selected addon website.
        /// </summary>
        public string SelectedAddonWebsite
        {
            get
            {
                if (SelectedAddonInfoData == null || string.IsNullOrEmpty(SelectedAddonInfoData.Data.Website))
                {
                    return "None";
                }

                return SelectedAddonInfoData.Data.Website;
            }
        }

        /// <summary>
        /// Gets the selected addon authors.
        /// </summary>
        public string SelectedAddonAuthors
        {
            get
            {
                if (SelectedAddonInfoData == null || string.IsNullOrEmpty(SelectedAddonInfoData.Data.Authors))
                {
                    return "None";
                }

                return SelectedAddonInfoData.Data.Authors;
            }
        }

        /// <summary>
        /// Getsa a list of required addons for the selected addon.
        /// </summary>
        public string SelectedAddonRequired
        {
            get
            {
                if (SelectedAddonInfoData == null
                    || SelectedAddonInfoData.Data.RequiredAddons == null
                    || SelectedAddonInfoData.Data.RequiredAddons.Count() == 0)
                {
                    return "None";
                }

                return string.Join(", ", SelectedAddonInfoData.Data.RequiredAddons) ?? "None";
            }
        }

        /// <summary>
        /// Gets a list of conflicts with the selected addon authors.
        /// </summary>
        public string SelectedAddonConflicts
        {
            get
            {
                if (SelectedAddonInfoData == null
                    || SelectedAddonInfoData.Data.Conflicts == null
                    || SelectedAddonInfoData.Data.Conflicts.Count() == 0)
                {
                    return "None";
                }

                return string.Join(", ", SelectedAddonInfoData.Data.Conflicts) ?? "None";
            }
        }

        /// <summary>
        /// Gets or sets the view model state.
        /// </summary>
        public InstallAddonsViewModelState ViewModelState
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
            // TODO: Do we need the ability to refresh the list similar to news feed?

            Logger.LogDebug("Executing command.");

            // Update only once when empty.
            if (ProvidersCollection?.Count != 0)
            {
                return;
            }

            ProvidersCollection.Clear();

            ViewModelState = InstallAddonsViewModelState.RequestingApprovedProviders;

            var id = WebStaticData.GitHubAddonsLibRepositoryId;
            var path = WebStaticData.GitHubAddonsLibApprovedProviders;

            try
            {
                Logger.LogDebug("Getting providers...");
                var provider = RegistryProviderFactory.GetProvider(ProviderInfoHostType.GitHub);  // TODO: for now we use only one entry point
                var providers = await provider.GetApprovedProvidersAsync(id, path);
                ProvidersCollection = new ObservableCollection<ProviderInfo>(providers);
            }
            catch (NotFoundException e)
            {
                // Repo or branch is not found.
                ViewModelState = InstallAddonsViewModelState.FailedToLoadProviders;
                Logger.LogError(e, $"GitHub API returned 404 NotFound -- repository id={id} or branch \"{Addons.AddonLibProvider.RegistryProviderBase.ApprovedProvidersBranchName}\" is not found.");
                return;
            }
            catch (HttpRequestException e)
            {
                // Bad code from download URL request.
                ViewModelState = InstallAddonsViewModelState.FailedToLoadProviders;
                Logger.LogError(e, "Unable to download the list of approved providers.");
                return;
            }
            catch (JsonException e)
            {
                // Deserialization error.
                ViewModelState = InstallAddonsViewModelState.FailedToLoadProviders;
                Logger.LogError(e, "Unable to deserialize the downloaded JSON.");
                return;
            }

            ViewModelState = InstallAddonsViewModelState.Ready;

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

            ViewModelState = InstallAddonsViewModelState.LoadingAddonsList;

            AddonInfo addonInfo;
            switch (SelectedProvider.Type)
            {
                case ProviderInfoHostType.GitHub:
                    Logger.LogDebug("Getting addons from a GitHub host...");
                    ViewModelState = InstallAddonsViewModelState.FailedToLoadAddons;
                    var provider = RegistryProviderFactory.GetProvider(ProviderInfoHostType.GitHub);  // TODO: for now we use only one entry point
                    addonInfo = await provider.GetAddonsFromAsync(SelectedProvider);
                    break;
                case ProviderInfoHostType.Standalone:
                    Logger.LogDebug("Getting addons from a standalone host...");
                    throw new NotSupportedException();  // TODO: implementation
                default:
                    ViewModelState = InstallAddonsViewModelState.FailedToLoadAddons;
                    Logger.LogWarning("Unsupported host type.");
                    return;
            }

            if (addonInfo.Data == null || addonInfo.Schema == null)
            {
                ViewModelState = InstallAddonsViewModelState.FailedToLoadAddons;
                Logger.LogWarning($"{nameof(addonInfo)} returned invalid data or schema (null value).");
                return;
            }

            var sorted = await SortAddonsCollection(addonInfo.Data);
            foreach (var item in sorted)
            {
                if (item != null)
                {
                    AddonInfoDataCollection.Add(new AddonItemModel(item));
                    await Task.Delay(50);  // for animation purposes
                    Logger.LogDebug($"Addon with InternalName={item.InternalName} added.");
                }
            }

            ViewModelState = InstallAddonsViewModelState.Ready;

            Logger.LogInformation("Addons list updated.");
        }

        // Sorts the addon collection.
        private async Task<IEnumerable<AddonInfoData>> SortAddonsCollection(IEnumerable<AddonInfoData> collection)
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
