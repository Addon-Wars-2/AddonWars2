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
    using System.Reflection;
    using System.Windows;
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
        private string _GW2DirectoryLocation;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeViewModel"/> class.
        /// </summary>
        public HomeViewModel()
        {
            ResetWelcomeMessagesOnLoad();

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
        /// Gets the package version with suffix included.
        /// </summary>
        public string PackageVersionWithSuffix => Assembly.GetExecutingAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

        /// <summary>
        /// Gets or sets GW2 executable file location.
        /// </summary>
        public string GW2DirectoryLocation
        {
            get => _GW2DirectoryLocation;
            set => SetProperty(ref _GW2DirectoryLocation, value);
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

        #endregion Methods
    }
}
