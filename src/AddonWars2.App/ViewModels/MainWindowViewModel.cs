// ==================================================================================================
// <copyright file="MainWindowViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Windows.Controls;
    using AddonWars2.App.Models.Application;
    using AddonWars2.App.ViewModels.Commands;
    using AddonWars2.SharedData;
    using AddonWars2.SharedData.Entities;
    using CommunityToolkit.Mvvm.Input;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The main view model used by application main window.
    /// </summary>
    public class MainWindowViewModel : WindowBaseViewModel
    {
        #region Fields

        private readonly ApplicationConfig _applicationConfig;
        private readonly CommonCommands _commonCommands;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="logger">A referemnce to <see cref="ILogger"/>.</param>
        /// <param name="appConfig">A reference to <see cref="ApplicationConfig"/>.</param>
        /// <param name="commonCommands">A reference to a common commands class.</param>
        public MainWindowViewModel(
            ILogger<MainWindowViewModel> logger,
            ApplicationConfig appConfig,
            CommonCommands commonCommands)
            : base(logger)
        {
            _applicationConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            _commonCommands = commonCommands ?? throw new ArgumentNullException(nameof(commonCommands));

            ChangeLanguageCommand = new RelayCommand<SelectionChangedEventArgs>(ExecuteChangeLanguageCommand);

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
        /// Gets a list of available cultures.
        /// </summary>
        public ObservableCollection<CultureInfo> AvailableCultures => new ObservableCollection<CultureInfo>(AppStaticData.APP_SUPPORTED_CULTURES);

        /// <summary>
        /// Gets a value indicating whether the application
        /// was executed in debug or normal mode.
        /// </summary>
        public bool IsDebugMode => AppConfig.IsDebugMode;

        /// <summary>
        /// Gets the GW2 website URL.
        /// </summary>
        public string Gw2HomeLink => AppConfig.UserData.Gw2Home;

        /// <summary>
        /// Gets the GW2 wiki URL.
        /// </summary>
        public string Gw2WikiLink => AppConfig.UserData.Gw2WikiHome;

        /// <summary>
        /// Gets the project URL.
        /// </summary>
        public string ProjectLink => WebStaticData.PROJECT_REPOSITORY_URL_GITHUB;

        /// <summary>
        /// Gets the project wiki URL.
        /// </summary>
        public string ProjectWikiLink => WebStaticData.PROJECT_WIKI_URL_GITHUB;

        /// <summary>
        /// Gets or sets the selected culture.
        /// </summary>
        public CultureInfo SelectedCulture
        {
            get => AppConfig.SelectedCulture;
            set
            {
                SetProperty(AppConfig.SelectedCulture, value, AppConfig, (model, culture) => model.SelectedCulture = culture);
                Logger.LogDebug($"Property set: {value}. Culture: {value.Culture}");
            }
        }

        /// <summary>
        /// Gets the package version with suffix included.
        /// </summary>
        public string AssemblyFileVersion => Assembly.GetExecutingAssembly().GetName()?.Version?.ToString() ?? string.Empty;

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets a command which is invoked after another language is selected.
        /// </summary>
        public RelayCommand<SelectionChangedEventArgs> ChangeLanguageCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        // ChangeLanguageCommand command logic.
        private void ExecuteChangeLanguageCommand(SelectionChangedEventArgs? e)
        {
            // TODO: Doesn't really belong to VM since it does nothing with data (models).
            //       More naturally to put it to a code-behind.

            Logger.LogDebug("Executing command.");

            ArgumentNullException.ThrowIfNull(e, nameof(e));

            // The SelectionChangedEventArgs is fired twice: when its data is loaded (attached by the binding)
            // and we edit its value. So we just ignore the first call until it's loaded completely.
            ComboBox comboBox = (ComboBox)e.Source;
            if (!comboBox.IsLoaded)
            {
                return;
            }

            AW2Application.Current.Restart();
        }

        #endregion Commands Logic

        #region Methods

        #endregion Methods
    }
}
