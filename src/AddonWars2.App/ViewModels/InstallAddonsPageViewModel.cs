// ==================================================================================================
// <copyright file="InstallAddonsPageViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using AddonWars2.Addons.RegistryProvider;
    using AddonWars2.Addons.RegistryProvider.Models;
    using AddonWars2.App.Models.Application;
    using AddonWars2.SharedData;
    using CommunityToolkit.Mvvm.Input;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// View model used by install addons view.
    /// </summary>
    public class InstallAddonsPageViewModel : BaseViewModel
    {
        #region Fields

        private readonly ApplicationConfig _applicationConfig;
        private readonly IWebStaticData _webStaticData;
        private readonly GithubRegistryProvider _githubRegistryProvider;
        private ObservableCollection<ProviderInfo> _providers;
        private ProviderInfo? _selectedProvider;

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

            GetProvidersListCommand = new AsyncRelayCommand(ExecuteGetProvidersListCommand);

            Logger.LogDebug("Instance initialized.");
        }

        #endregion Constructors

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
        /// Gets or sets a list of addon info providers.
        /// </summary>
        public ObservableCollection<ProviderInfo> ProvidersCollection
        {
            get => _providers;
            set => _providers = value;
        }

        /// <summary>
        /// Gets or sets the selected addon info provider.
        /// </summary>
        public ProviderInfo? SelectedProvider
        {
            get => _selectedProvider;
            set => _selectedProvider = value;
        }

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets a command which loads a list of providers.
        /// </summary>
        public AsyncRelayCommand GetProvidersListCommand { get; private set; }

        /// <summary>
        /// Gets a reference to a GitHub registry provider.
        /// </summary>
        public GithubRegistryProvider GithubRegistryProvider => _githubRegistryProvider;

        #endregion Commands

        #region Commands Logic

        // GetProvidersListCommand command logic.
        private async Task ExecuteGetProvidersListCommand()
        {
            // Update only once when empty.
            if (ProvidersCollection?.Count != 0)
            {
                Debug.WriteLine("Providers are alrady loaded...");
                return;
            }

            Debug.WriteLine("Getting providers...");

            await GithubRegistryProvider.GetApprovedProvidersAsync(WebStaticData.GitHubProjectRepositoryId, WebStaticData.GitHubProjectApprovedProviders);
        }

        #endregion Commdans Logic

        #region Methods

        #endregion Methods
    }
}
