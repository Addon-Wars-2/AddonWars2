// ==================================================================================================
// <copyright file="HomePageViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels
{
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Windows;
    using AddonWars2.Addons.Models.AddonInfo;
    using AddonWars2.App.Models.Application;
    using AddonWars2.App.Services;
    using AddonWars2.App.Utils.Helpers;
    using AddonWars2.SharedData;
    using CommunityToolkit.Mvvm.Input;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// View model used by home view.
    /// </summary>
    public class HomePageViewModel : BaseViewModel
    {
        #region Fields

        private const string GW2_REGISTRY_DIR = @"Software\Microsoft\Windows\CurrentVersion\Uninstall\Guild Wars 2";

        private readonly ApplicationConfig _applicationConfig;

        private bool _isActuallyLoaded = false;
        private AddonInfoData? _selectedAddon;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HomePageViewModel"/> class.
        /// </summary>
        /// <param name="logger">A referemnce to <see cref="ILogger"/>.</param>
        /// <param name="appConfig">A reference to <see cref="ApplicationConfig"/>.</param>
        /// <param name="addonsManager">A reference to <see cref="AddonsService"/>.</param>
        public HomePageViewModel(
            ILogger<HomePageViewModel> logger,
            ApplicationConfig appConfig,
            AddonsService addonsManager)
            : base(logger)
        {
            _applicationConfig = appConfig;
            AddonsManager = addonsManager;

            TryFindGw2ExeCommand = new RelayCommand(ExecuteTryFindGw2ExeCommand, () => IsActuallyLoaded == false);
            UpdateGw2ExePathCommand = new RelayCommand<string[]>(ExecuteUpdateGw2ExePathCommand);

            Logger.LogDebug("Instance initialized.");
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a reference to the application config.
        /// </summary>
        public ApplicationConfig AppConfig => _applicationConfig;

        /// <summary>
        /// Gets a reference to the application config.
        /// </summary>
        public AddonsService AddonsManager { get; private set; }

        /// <summary>
        /// Gets or sets GW2 executable location.
        /// </summary>
        public string Gw2ExecPath
        {
            get => AppConfig.UserData.Gw2FilePath;
            set
            {
                SetProperty(AppConfig.UserData.Gw2FilePath, value, AppConfig.UserData, (model, filepath) => model.Gw2FilePath = filepath);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets or sets GW2 directory location.
        /// </summary>
        public string Gw2DirPath
        {
            get => AppConfig.UserData.Gw2DirPath;
            set
            {
                SetProperty(AppConfig.UserData.Gw2DirPath, value, AppConfig.UserData, (model, dirpath) => model.Gw2DirPath = dirpath);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets a collection of installed add-ons.
        /// </summary>
        public ObservableCollection<AddonInfoData> InstalledAddonsCollection => AddonsManager.InstalledAddonsCollection;

        /// <summary>
        /// Gets or sets the currently selected add-on.
        /// </summary>
        public AddonInfoData? SelectedAddon
        {
            get => _selectedAddon;
            set
            {
                SetProperty(ref _selectedAddon, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets the GW2 exe file extension from the config object.
        /// </summary>
        public string Gw2FileExtension => GameStaticData.GW2_EXECUTABLE_EXTENSION;

        /// <summary>
        /// Gets the GW2 exe product name from the config object.
        /// </summary>
        public string Gw2ProductName => GameStaticData.GW2_PRODUCT_NAME;

        /// <summary>
        /// Gets the GW2 exe file description from the config object.
        /// </summary>
        public string Gw2FileDescription => GameStaticData.GW2_FILE_DESC;

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
        public RelayCommand<string[]> UpdateGw2ExePathCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        // TryFindGw2ExeCommand command logic.
        private void ExecuteTryFindGw2ExeCommand()
        {
            // TODO: Add another step (if this one failed) to search inside the %adddata% directory
            //       and parse GW2 GFXSettings file. GW2 dir path is stored in INSTALLPATH entry.

            Logger.LogDebug("Executing command.");

            // First try to get the file location from the registry.
            var regDir = GW2_REGISTRY_DIR;
            var gw2exe = IOHelper.SearchRegistryKey(regDir, "DisplayIcon") as string;
            if (string.IsNullOrEmpty(gw2exe))
            {
                Logger.LogWarning("Couldn't find GW2 string in the registry.");
                gw2exe = string.Empty;
            }

            Gw2ExecPath = gw2exe;
            Gw2DirPath = Path.GetDirectoryName(gw2exe) ?? string.Empty;

            Logger.LogInformation("GW2 executable location was set automatically.");
        }

        // UpdateGw2ExePathCommand command logic.
        private void ExecuteUpdateGw2ExePathCommand(string[]? paths)
        {
            Logger.LogDebug("Executing command.");

            if (paths?.Length == 0)
            {
                Logger.LogWarning("No paths were selected.");
                return;
            }

            var path = paths?[0];
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                Logger.LogWarning("Path is null, empty or not valid.");
                return;
            }

            Gw2ExecPath = path;

            // Inform UI even if the property value is the same.
            // We do this, because the text box could be changed manually, but failed to pass validation.
            // The actual property value does not change in such case, and re-selecting the same file again will not
            // update the text box, since SetProperty internally compares old and new values, which are still the same.
            OnPropertyChanged(nameof(Gw2ExecPath));

            Gw2DirPath = Path.GetDirectoryName(path) ?? string.Empty;
        }

        #endregion Commands Logic

        #region Methods

        #endregion Methods
    }
}
