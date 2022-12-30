// ==================================================================================================
// <copyright file="MainWindowViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Windows.Controls;
    using AddonWars2.App.Commands;
    using AddonWars2.App.Models.Application;
    using CommunityToolkit.Mvvm.Input;
    using NLog;

    /// <summary>
    /// The main view model used by application main window.
    /// </summary>
    public class MainWindowViewModel : WindowBaseViewModel
    {
        #region Fields

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="commonCommands">A reference to a common commands class.</param>
        /// <param name="homeViewModel">A reference to <see cref="ViewModels.HomeViewModel"/>.</param>
        /// <param name="loggingViewModel">A reference to <see cref="ViewModels.LoggingViewModel"/>.</param>
        public MainWindowViewModel(
            CommonCommands commonCommands,
            HomeViewModel homeViewModel,
            LoggingViewModel loggingViewModel)
        {
            CommonCommands = commonCommands;
            HomeViewModel = homeViewModel;
            LoggingViewModel = loggingViewModel;

            ChangeLanguageCommand = new RelayCommand<SelectionChangedEventArgs>(ExecuteChangeLanguageCommand);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a reference to a common commands class.
        /// </summary>
        public CommonCommands CommonCommands { get; private set; }

        /// <summary>
        /// Gets a reference to <see cref="ViewModels.HomeViewModel"/> view model.
        /// </summary>
        public HomeViewModel HomeViewModel { get; private set; }

        /// <summary>
        /// Gets a reference to <see cref="ViewModels.LoggingViewModel"/> view model.
        /// </summary>
        public LoggingViewModel LoggingViewModel { get; private set; }

        /// <summary>
        /// Gets a list of available cultures.
        /// </summary>
        public ObservableCollection<CultureInfo> AvailableCultures => ApplicationGlobal.AppConfig.AvailableCultures;

        /// <summary>
        /// Gets or sets the selected culture.
        /// </summary>
        public CultureInfo SelectedCulture
        {
            get => ApplicationGlobal.AppConfig.SelectedCulture;
            set
            {
                SetProperty(ApplicationGlobal.AppConfig.SelectedCulture, value, ApplicationGlobal.AppConfig, (model, selected) => model.SelectedCulture = selected);
            }
        }

        /// <summary>
        /// Gets the package version with suffix included.
        /// </summary>
        public string PackageVersionWithSuffix => Assembly.GetExecutingAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

        // Gets the current logger instance.
        private static Logger Logger => LogManager.GetCurrentClassLogger();

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets a command which is invoked after another language is selected.
        /// </summary>
        public RelayCommand<SelectionChangedEventArgs> ChangeLanguageCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        // ChangeLanguageCommand command logic.
        private void ExecuteChangeLanguageCommand(SelectionChangedEventArgs e)
        {
            // TODO: Doesn't really belong to VM since it does nothing with data (models).
            //       More naturally to put it to a code-behind.

            Logger.Debug("Executing command.");

            // The SelectionChangedEventArgs is fired twice: when its data is loaded (attached by the binding)
            // and we edit its value. That leads that window redraws itself twice once
            // we select another language. So we just ignore the first call until it's loaded completely.
            ComboBox comboBox = (ComboBox)e.Source;
            if (!comboBox.IsLoaded)
            {
                return;
            }

            AW2Application.Restart();
        }

        #endregion Commands Logic

        #region Methods

        #endregion Methods
    }
}
