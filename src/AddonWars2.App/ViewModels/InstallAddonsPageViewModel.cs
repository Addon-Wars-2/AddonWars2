// ==================================================================================================
// <copyright file="InstallAddonsPageViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using AddonWars2.Addons.Models.AddonInfo;
    using AddonWars2.Addons.RegistryProvider;
    using AddonWars2.Addons.RegistryProvider.Models;
    using AddonWars2.App.Models.Application;
    using AddonWars2.SharedData;
    using CommunityToolkit.Mvvm.Input;
    using Microsoft.Extensions.Logging;
    using Octokit;

    /// <summary>
    /// View model used by install addons view.
    /// </summary>
    public class InstallAddonsPageViewModel : BaseViewModel
    {
        #region Fields

        private readonly ApplicationConfig _applicationConfig;
        private readonly IWebStaticData _webStaticData;
        private readonly GithubRegistryProvider _githubRegistryProvider;
        private string _viewModelState = string.Empty;
        private bool _isActuallyLoaded = false;
        private InstallAddonsViewModelState _viewModelStateInternal = InstallAddonsViewModelState.Ready;
        private ObservableCollection<ProviderInfo> _providers;
        private ProviderInfo? _selectedProvider;
        private ObservableCollection<AddonInfoData> _addonInfoDataCollection;
        private AddonInfoData? _selectedAddonInfoData;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallAddonsPageViewModel"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/> instance.</param>
        /// <param name="appConfig">A reference to <see cref="ApplicationConfig"/> instance.</param>
        /// <param name="webStaticData">A reference to <see cref="IWebStaticData"/> instance.</param>
        /// <param name="githubRegistryProvider">A reference to <see cref="Addons.RegistryProvider.GithubRegistryProvider"/> instance.</param>
        public InstallAddonsPageViewModel(
            ILogger<NewsPageViewModel> logger,
            ApplicationConfig appConfig,
            IWebStaticData webStaticData,
            GithubRegistryProvider githubRegistryProvider)
            : base(logger)
        {
            _applicationConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            _webStaticData = webStaticData ?? throw new ArgumentNullException(nameof(webStaticData));
            _githubRegistryProvider = githubRegistryProvider ?? throw new ArgumentNullException(nameof(githubRegistryProvider));
            _providers = new ObservableCollection<ProviderInfo>();
            _addonInfoDataCollection = new ObservableCollection<AddonInfoData>();

            GetProvidersListCommand = new AsyncRelayCommand(ExecuteGetProvidersListCommand);
            GetAddonsFromProviderCommand = new AsyncRelayCommand(ExecuteGetAddonsFromProviderCommand);

            Logger.LogDebug("Instance initialized.");
        }

        #endregion Constructors

        #region Enums

        /// <summary>
        /// Represents <see cref="InstallAddonsPageViewModel"/> states.
        /// </summary>
        private enum InstallAddonsViewModelState
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

        #endregion Enums

        #region Properties

        /// <summary>
        /// Gets a reference to the application config.
        /// </summary>
        public ApplicationConfig AppConfig => _applicationConfig;

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
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets or sets a list of addons.
        /// </summary>
        public ObservableCollection<AddonInfoData> AddonInfoDataCollection
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
        public AddonInfoData? SelectedAddonInfoData
        {
            get => _selectedAddonInfoData;
            set
            {
                SetProperty(ref _selectedAddonInfoData, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets or sets the view model state as a string representation
        /// of <see cref="InstallAddonsViewModelState"/> value.
        /// </summary>
        public string ViewModelState
        {
            get => _viewModelState;
            set
            {
                SetProperty(ref _viewModelState, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets or sets the view model state.
        /// </summary>
        private InstallAddonsViewModelState ViewModelStateInternal
        {
            get => _viewModelStateInternal;
            set
            {
                SetProperty(ref _viewModelStateInternal, value);
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
        /// Gets a reference to a GitHub registry provider.
        /// </summary>
        public GithubRegistryProvider GithubRegistryProvider => _githubRegistryProvider;

        #endregion Commands

        #region Commands Logic

        // GetProvidersListCommand command logic.
        private async Task ExecuteGetProvidersListCommand()
        {
            // TODO: Do we need the ability to refresh the list similar to news feed?

            // Update only once when empty.
            if (ProvidersCollection?.Count != 0)
            {
                return;
            }

            ProvidersCollection.Clear();

            SetState(InstallAddonsViewModelState.RequestingApprovedProviders);

            var id = WebStaticData.GitHubAddonsLibRepositoryId;
            var path = WebStaticData.GitHubAddonsLibApprovedProviders;

            try
            {
                var providers = await GithubRegistryProvider.GetApprovedProvidersAsync(id, path);
                ProvidersCollection = new ObservableCollection<ProviderInfo>(providers);
            }
            catch (NotFoundException e)
            {
                // Repo or branch is not found.
                SetState(InstallAddonsViewModelState.FailedToLoadProviders);
                Logger.LogError(e, $"GitHub API returned 404 NotFound -- repository id={id} or branch \"{Addons.AddonLibProvider.RegistryProviderBase.ApprovedProvidersBranchName}\" is not found.");
                return;
            }
            catch (HttpRequestException e)
            {
                // Bad code from download URL request.
                SetState(InstallAddonsViewModelState.FailedToLoadProviders);
                Logger.LogError(e, "Unable to download the list of approved providers.");
                return;
            }
            catch (JsonException e)
            {
                // Deserialization error.
                SetState(InstallAddonsViewModelState.FailedToLoadProviders);
                Logger.LogError(e, "Unable to deserialize the downloaded JSON.");
                return;
            }

            SetState(InstallAddonsViewModelState.Ready);
        }

        // GetAddonsFromProviderCommand command logic.
        private async Task ExecuteGetAddonsFromProviderCommand()
        {
            if (SelectedProvider == null)
            {
                return;
            }

            SetState(InstallAddonsViewModelState.LoadingAddonsList);

            AddonInfo addonInfo;
            switch (SelectedProvider.Type)
            {
                case ProviderInfoHostType.GitHub:
                    SetState(InstallAddonsViewModelState.FailedToLoadAddons);
                    addonInfo = await GithubRegistryProvider.GetAddonsFromAsync(SelectedProvider);
                    break;
                case ProviderInfoHostType.Standalone:
                    throw new NotSupportedException();  // TODO: implementation
                default:
                    SetState(InstallAddonsViewModelState.FailedToLoadAddons);
                    throw new NotSupportedException();
            }

            if (addonInfo.Data == null || addonInfo.Schema == null)
            {
                SetState(InstallAddonsViewModelState.FailedToLoadAddons);
                Logger.LogError($"{nameof(addonInfo)} returned invalid data or schema (null value).");
                return;
            }

            var sorted = await SortAddonsCollection(addonInfo.Data);
            foreach (var item in sorted)
            {
                if (item != null)
                {
                    AddonInfoDataCollection.Add(item);
                    await Task.Delay(50);  // for animation purposes
                    Logger.LogDebug($"Addon with InternalName={item.InternalName} added.");
                }
            }

            SetState(InstallAddonsViewModelState.Ready);
        }

        // Sorts the addon collection.
        private async Task<IEnumerable<AddonInfoData>> SortAddonsCollection(IEnumerable<AddonInfoData> collection)
        {
            return await Task.Run(() => collection.OrderByDescending(x => x.DisplayName).ToList());
        }

        #endregion Commdans Logic

        #region Methods

        // Sets the view model state.
        private void SetState(InstallAddonsViewModelState state)
        {
            ViewModelStateInternal = state;

            var stateString = Enum.GetName(typeof(InstallAddonsViewModelState), state);
            if (string.IsNullOrEmpty(stateString))
            {
                throw new InvalidOperationException("Unknown view model state.");
            }

            ViewModelState = stateString;
            Logger.LogDebug($"ViewModel state set: {state}");
        }

        #endregion Methods
    }
}
