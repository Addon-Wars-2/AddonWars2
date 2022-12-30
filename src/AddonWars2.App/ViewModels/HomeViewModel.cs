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
        private string _displayedWelcomeMessage;
        private string _gw2ExecPath;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeViewModel"/> class.
        /// </summary>
        public HomeViewModel()
        {
            ResetWelcomeMessagesOnLoad();

            PropertyChangedEventManager.AddHandler(this, HomeViewModel_ConfigPropertyChanged, "GW2ExecPath");

            UpdateWelcomeMessageCommand = new RelayCommand(ExecuteUpdateWelcomeMessage);
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
        public string GW2ExecPath
        {
            get => _gw2ExecPath;
            set
            {
                ApplicationGlobal.AppConfig.GW2ExecInfo.FilePath = value;
                Logger.Info($"Selected the GW2 executable location: {value}");
                SetProperty(ref _gw2ExecPath, value);
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

        #endregion Commands

        #region Commands Logic

        // OpenUrlCommand command logic.
        private void ExecuteUpdateWelcomeMessage()
        {
            Logger.Debug("Executing command.");

            ResetWelcomeMessagesOnLoad();

            // Should not happen normally.
            if (WelcomeMessages == null || WelcomeMessages.Count == 0)
            {
                DisplayedWelcomeMessage = string.Empty;
                return;
            }

            int i = _random.Next(0, WelcomeMessages.Count);
            DisplayedWelcomeMessage = WelcomeMessages[i];
        }

        #endregion Commands Logic

        #region Methods

        // We need to refresh the list on ResourceDictionary changes.
        private void ResetWelcomeMessagesOnLoad()
        {
            WelcomeMessages = new ObservableCollection<string>()
            {
                ResourcesHelper.GetApplicationResource<string>("S.HomePage.Welcome.Message_01"),
                ResourcesHelper.GetApplicationResource<string>("S.HomePage.Welcome.Message_02"),
                ResourcesHelper.GetApplicationResource<string>("S.HomePage.Welcome.Message_03"),
                ResourcesHelper.GetApplicationResource<string>("S.HomePage.Welcome.Message_04"),
            };
        }

        // Updates config if a property specified in ctor was changed.
        private void HomeViewModel_ConfigPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IOHelper.SerializeXml(ApplicationGlobal.AppConfig, ApplicationGlobal.ConfigFilePath);
            Logger.Debug($"Config file updated.");
        }

        #endregion Methods
    }
}
