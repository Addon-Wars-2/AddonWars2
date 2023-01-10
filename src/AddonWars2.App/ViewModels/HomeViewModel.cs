// ==================================================================================================
// <copyright file="HomeViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Windows;
    using AddonWars2.App.Helpers;
    using AddonWars2.App.Models.Application;
    using AddonWars2.App.Utils.Helpers;
    using CommunityToolkit.Mvvm.Input;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// View model used by home view.
    /// </summary>
    public class HomeViewModel : BaseViewModel
    {
        #region Fields

        private static readonly Random _random = new Random();
        private bool _isActuallyLoaded = false;
        private string _displayedWelcomeMessage;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeViewModel"/> class.
        /// </summary>
        /// <param name="logger">A referemnce to <see cref="ILogger"/>.</param>
        /// <param name="appConfig">A reference to <see cref="ViewModels.AppConfig"/>.</param>
        public HomeViewModel(
            ILogger<HomeViewModel> logger,
            ApplicationConfig appConfig)
            : base(logger)
        {
            AppConfig = appConfig;

            ////PropertyChangedEventManager.AddHandler(this, HomeViewModel_ConfigPropertyChanged, nameof(Gw2ExecPath));

            TryFindGw2ExeCommand = new RelayCommand(ExecuteTryFindGw2Exe, () => IsActuallyLoaded == false);
            UpdateGw2ExeCommand = new RelayCommand<string[]>(ExecuteUpdateGw2Exe);
            UpdateWelcomeMessageCommand = new RelayCommand(ExecuteUpdateWelcomeMessage, () => IsActuallyLoaded == false);

            Logger.LogDebug("Instance initialized.");
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a reference to the application config.
        /// </summary>
        public ApplicationConfig AppConfig { get; private set; }

        /// <summary>
        /// Gets the displayed welcome message.
        /// </summary>
        public string DisplayedWelcomeMessage
        {
            get => _displayedWelcomeMessage;
            private set
            {
                SetProperty(ref _displayedWelcomeMessage, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets or sets GW2 executable location.
        /// </summary>
        public string Gw2ExecPath
        {
            get => AppConfig.LocalData.Gw2FilePath;
            set
            {
                SetProperty(AppConfig.LocalData.Gw2FilePath, value, AppConfig.LocalData, (model, filepath) => model.Gw2FilePath = filepath);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets the GW2 exe file extension from the config object.
        /// </summary>
        public string Gw2FileExtension => AppConfig.LocalData.Gw2FileExtension;

        /// <summary>
        /// Gets the GW2 exe product name from the config object.
        /// </summary>
        public string Gw2ProductName => AppConfig.LocalData.Gw2ProductName;

        /// <summary>
        /// Gets the GW2 exe file description from the config object.
        /// </summary>
        public string Gw2FileDescription => AppConfig.LocalData.Gw2FileDescription;

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

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets a command that will try to locate GW2 executable and set it as a <see cref="Gw2ExecPath"/>.
        /// </summary>
        public RelayCommand TryFindGw2ExeCommand { get; private set; }

        /// <summary>
        /// Gets a command that updates <see cref="Gw2ExecPath"/>.
        /// </summary>
        public RelayCommand<string[]> UpdateGw2ExeCommand { get; private set; }

        /// <summary>
        /// Gets a command that updates a welcome message.
        /// </summary>
        public RelayCommand UpdateWelcomeMessageCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        // TryFindGw2ExeCommand command logic.
        private void ExecuteTryFindGw2Exe()
        {
            Logger.LogDebug("Executing command.");

            // First try to get the file location from the registry.
            var regDir = "Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Guild Wars 2";
            var gw2exe = IOHelper.SearchRegistryKey(regDir, "DisplayIcon") as string;
            if (string.IsNullOrEmpty(gw2exe))
            {
                Logger.LogWarning("Couldn't find GW2 string in the registry.");
                gw2exe = string.Empty;
            }

            Gw2ExecPath = gw2exe;

            Logger.LogInformation("GW2 executable location was set automatically.");
        }

        // UpdateGw2ExeCommand command logic.
        private void ExecuteUpdateGw2Exe(string[] paths)
        {
            Logger.LogDebug("Executing command.");

            if (paths.Length == 0)
            {
                Logger.LogDebug("No paths were selected.");
                return;
            }

            var path = paths[0];
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                Logger.LogDebug("Path is null, empty or not valid.");
                return;
            }

            Gw2ExecPath = path;
        }

        // UpdateWelcomeMessageCommand command logic.
        private void ExecuteUpdateWelcomeMessage()
        {
            Logger.LogDebug("Executing command.");

            var messages = new ObservableCollection<string>()
            {
                ResourcesHelper.GetApplicationResource<string>("S.HomePage.Welcome.Message_01"),
                ResourcesHelper.GetApplicationResource<string>("S.HomePage.Welcome.Message_02"),
                ResourcesHelper.GetApplicationResource<string>("S.HomePage.Welcome.Message_03"),
                ResourcesHelper.GetApplicationResource<string>("S.HomePage.Welcome.Message_04"),
            };

            // Should not happen normally.
            if (messages == null || messages.Count == 0)
            {
                Logger.LogWarning($"No welcome message found.");
                DisplayedWelcomeMessage = string.Empty;
                return;
            }

            int i = _random.Next(0, messages.Count);
            DisplayedWelcomeMessage = messages[i];

            Logger.LogDebug("Display messages loaded.");
        }

        #endregion Commands Logic

        #region Methods

        #endregion Methods
    }
}
