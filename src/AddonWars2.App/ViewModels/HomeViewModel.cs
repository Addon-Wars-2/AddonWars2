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
        private bool _actuallyLoaded = false;
        private string _displayedWelcomeMessage;
        private string _gw2ExecPath;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeViewModel"/> class.
        /// </summary>
        public HomeViewModel()
        {
            WelcomeMessages = new ObservableCollection<string>()
            {
                ResourcesHelper.GetApplicationResource<string>("S.HomePage.Welcome.Message_01"),
                ResourcesHelper.GetApplicationResource<string>("S.HomePage.Welcome.Message_02"),
                ResourcesHelper.GetApplicationResource<string>("S.HomePage.Welcome.Message_03"),
                ResourcesHelper.GetApplicationResource<string>("S.HomePage.Welcome.Message_04"),
            };

            PropertyChangedEventManager.AddHandler(this, HomeViewModel_ConfigPropertyChanged, nameof(Gw2ExecPath));

            UpdateWelcomeMessageCommand = new RelayCommand(ExecuteUpdateWelcomeMessage);
            TryFindGw2ExeCommand = new RelayCommand(ExecuteTryFindGw2Exe);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the displayed welcome message.
        /// </summary>
        public string DisplayedWelcomeMessage
        {
            get => _displayedWelcomeMessage;
            private set => SetProperty(ref _displayedWelcomeMessage, value);
        }

        /// <summary>
        /// Gets or sets GW2 executable location.
        /// </summary>
        public string Gw2ExecPath
        {
            get => _gw2ExecPath;
            set
            {
                ApplicationGlobal.AppConfig.GW2ExecInfo.FilePath = value;
                Logger.Info($"Selected the GW2 executable location: {value}");
                SetProperty(ref _gw2ExecPath, value);
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
        public bool ActuallyLoaded
        {
            get => _actuallyLoaded;
            set
            {
                if (_actuallyLoaded == false)
                {
                    Logger.Info($"View model is loaded for the first time.");
                    SetProperty(ref _actuallyLoaded, value);
                }
            }
        }

        // Holds a collection of welcome message displayed randomly on window update (Loaded event).
        private ObservableCollection<string> WelcomeMessages { get; set; }

        // Gets the current logger instance.
        private static Logger Logger => LogManager.GetCurrentClassLogger();

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets a command which is invoked when the parent page is loaded.
        /// Updates welcome message by replacing it with a random one taken from a
        /// list of pre-defined strings.
        /// </summary>
        public RelayCommand UpdateWelcomeMessageCommand { get; private set; }

        /// <summary>
        /// Gets a command which is invoked when the parent page is loaded.
        /// This command will try to locate gw2 executable and set it as a <see cref="Gw2ExecPath"/>.
        /// </summary>
        public RelayCommand TryFindGw2ExeCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        // OpenUrlCommand command logic.
        private void ExecuteUpdateWelcomeMessage()
        {
            Logger.Debug("Executing command.");

            // Should not happen normally.
            if (WelcomeMessages == null || WelcomeMessages.Count == 0)
            {
                DisplayedWelcomeMessage = string.Empty;
                return;
            }

            int i = _random.Next(0, WelcomeMessages.Count);
            DisplayedWelcomeMessage = WelcomeMessages[i];
        }

        // TryFindGw2ExeCommand command logic.
        private void ExecuteTryFindGw2Exe()
        {
            Logger.Debug("Executing command.");

            // First try to get the file location from the registry.
            var gw2exe = IOHelper.TryFindGw2Exe();
            if (string.IsNullOrEmpty(gw2exe))
            {
                Logger.Debug("Couldn't find GW2 string in the registry.");
                return;
            }

            Gw2ExecPath = gw2exe;
        }

        #endregion Commands Logic

        #region Methods

        // Updates config if a property specified in ctor was changed.
        private void HomeViewModel_ConfigPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IOHelper.SerializeXml(ApplicationGlobal.AppConfig, ApplicationGlobal.ConfigFilePath);
            Logger.Debug($"Config file updated.");
        }

        #endregion Methods
    }
}
