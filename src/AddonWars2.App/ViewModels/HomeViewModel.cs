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
    using System.Windows;
    using AddonWars2.App.Helpers;
    using AddonWars2.App.Models.Application;
    using AddonWars2.App.Utils.Helpers;
    using CommunityToolkit.Mvvm.Input;
    using NLog;

    /// <summary>
    /// View model used by home view.
    /// </summary>
    public class HomeViewModel : BaseViewModel
    {
        #region Fields

        private static readonly Random _random = new Random();
        private bool _isActuallyLoaded = false;
        private string _displayedWelcomeMessage;
        private string _gw2ExecPath;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeViewModel"/> class.
        /// </summary>
        /// <param name="appConfig">A reference to <see cref="ViewModels.AppConfig"/>.</param>
        public HomeViewModel(
            ApplicationConfig appConfig)
        {
            AppConfig = appConfig;

            PropertyChangedEventManager.AddHandler(this, HomeViewModel_ConfigPropertyChanged, nameof(Gw2ExecPath));

            TryFindGw2ExeCommand = new RelayCommand(ExecuteTryFindGw2Exe, () => _isActuallyLoaded == false);
            UpdateWelcomeMessageCommand = new RelayCommand(ExecuteUpdateWelcomeMessage, () => _isActuallyLoaded == false);
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
                Logger.Debug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets or sets GW2 executable location.
        /// </summary>
        public string Gw2ExecPath
        {
            get => _gw2ExecPath;
            set
            {
                AppConfig.UserData.Gw2ExecInfo.FilePath = value;
                SetProperty(ref _gw2ExecPath, value);
                Logger.Debug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets the GW2 exe file extension from the config object.
        /// </summary>
        public string Gw2FileExtension => AppConfig.UserData.Gw2ExecInfo.FileExtension;

        /// <summary>
        /// Gets the GW2 exe product name from the config object.
        /// </summary>
        public string Gw2ProductName => AppConfig.UserData.Gw2ExecInfo.ProductName;

        /// <summary>
        /// Gets the GW2 exe file description from the config object.
        /// </summary>
        public string Gw2FileDescription => AppConfig.UserData.Gw2ExecInfo.FileDescription;

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
                    Logger.Debug($"Property set: {value}");
                }
            }
        }

        // Gets the current logger instance.
        private static Logger Logger => LogManager.GetCurrentClassLogger();

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets a command that will try to locate GW2 executable and set it as a <see cref="Gw2ExecPath"/>.
        /// </summary>
        public RelayCommand TryFindGw2ExeCommand { get; private set; }

        /// <summary>
        /// Gets a command that updates a welcome message.
        /// </summary>
        public RelayCommand UpdateWelcomeMessageCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        // TryFindGw2ExeCommand command logic.
        private void ExecuteTryFindGw2Exe()
        {
            Logger.Debug("Executing command.");

            // First try to get the file location from the registry.
            var regDir = "Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Guild Wars 2";
            var gw2exe = IOHelper.SearchRegistryKey(regDir, "DisplayIcon") as string;
            if (string.IsNullOrEmpty(gw2exe))
            {
                Logger.Debug("Couldn't find GW2 string in the registry.");
                gw2exe = string.Empty;
            }

            Gw2ExecPath = gw2exe;
        }

        // UpdateWelcomeMessageCommand command logic.
        private void ExecuteUpdateWelcomeMessage()
        {
            Logger.Debug("Executing command.");

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
                Logger.Warn($"No welcome message found.");
                DisplayedWelcomeMessage = string.Empty;
                return;
            }

            int i = _random.Next(0, messages.Count);
            DisplayedWelcomeMessage = messages[i];
        }

        #endregion Commands Logic

        #region Methods

        // Updates config if a property specified in ctor was changed.
        private void HomeViewModel_ConfigPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IOHelper.SerializeXml(AppConfig.UserData, AppConfig.ConfigFilePath);
            Logger.Debug($"Config file updated.");
        }

        #endregion Methods
    }
}
