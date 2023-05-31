// ==================================================================================================
// <copyright file="HomePageViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using AddonWars2.App.Configuration;
    using AddonWars2.App.Utils.Helpers;
    using AddonWars2.Core.DTO;
    using AddonWars2.SharedData.Interfaces;
    using CommunityToolkit.Mvvm.Input;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// View model used by home view.
    /// </summary>
    public class HomePageViewModel : BaseViewModel
    {
        #region Fields

        private static readonly string _fileNotExistsErrorMsg = ResourcesHelper.GetApplicationResource<string>("S.Common.ValidationText.FileExists");
        private static readonly string _fileNotGw2ExeErrorMsg = ResourcesHelper.GetApplicationResource<string>("S.Common.ValidationText.GW2Exe");
        private readonly IApplicationConfig _applicationConfig;
        private readonly IGameSharedData _gameSharedData;

        private bool _isActuallyLoaded = false;
        private AddonData? _selectedAddon;
        private string _gw2ExecPath = string.Empty;
        private string _gw2DirPath = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HomePageViewModel"/> class.
        /// </summary>
        /// <param name="logger">A referemnce to <see cref="ILogger"/>.</param>
        /// <param name="appConfig">A reference to <see cref="IApplicationConfig"/>.</param>
        /// <param name="gameSharedData">A reference to <see cref="IGameSharedData"/> instance.</param>
        public HomePageViewModel(
            ILogger<HomePageViewModel> logger,
            IApplicationConfig appConfig,
            IGameSharedData gameSharedData)
            : base(logger)
        {
            _applicationConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            _gameSharedData = gameSharedData ?? throw new ArgumentNullException(nameof(gameSharedData));

            TryFindGw2ExeCommand = new RelayCommand(ExecuteTryFindGw2ExeCommand, () => IsActuallyLoaded == false);
            UpdateGw2ExePathCommand = new RelayCommand<string[]>(ExecuteUpdateGw2ExePathCommand);

            Logger.LogDebug("Instance initialized.");
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a reference to the application config.
        /// </summary>
        public IApplicationConfig AppConfig => _applicationConfig;

        /// <summary>
        /// Gets the game-related static data.
        /// </summary>
        public IGameSharedData GameSharedData => _gameSharedData;

        /// <summary>
        /// Gets or sets GW2 executable location.
        /// </summary>
        [CustomValidation(typeof(HomePageViewModel), nameof(ValidateGw2ExecPath_FileExists))]
        [CustomValidation(typeof(HomePageViewModel), nameof(ValidateGw2ExecPath_IsGw2Executable))]
        public string Gw2ExecPath
        {
            get => _gw2ExecPath;
            set
            {
                SetProperty(ref _gw2ExecPath, value, validate: true);

                if (!GetErrors(nameof(Gw2ExecPath)).Any())
                {
                    AppConfig.UserData.Gw2FilePath = value;
                    Logger.LogDebug($"Property set: {value}");
                    Gw2DirPath = Path.GetDirectoryName(value) ?? string.Empty;
                    return;
                }

                Logger.LogDebug($"Property is invalid and only changed in the UI: {value}");
            }
        }

        /// <summary>
        /// Gets or sets GW2 directory location.
        /// </summary>
        public string Gw2DirPath
        {
            get => _gw2DirPath;
            set
            {
                SetProperty(ref _gw2DirPath, value);
                AppConfig.UserData.Gw2DirPath = value;
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets a collection of installed add-ons.
        /// </summary>
        public ObservableCollection<AddonData> InstalledAddonsCollection => new ObservableCollection<AddonData>(); // TODO: replace

        /// <summary>
        /// Gets or sets the currently selected add-on.
        /// </summary>
        public AddonData? SelectedAddon
        {
            get => _selectedAddon;
            set
            {
                SetProperty(ref _selectedAddon, value);
                Logger.LogDebug($"Property set: {value}");
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
            var regKey = GameSharedData.Gw2RegistryKey;
            var gw2exe = IOHelper.SearchRegistryKey(regKey, "DisplayIcon") as string;
            if (string.IsNullOrEmpty(gw2exe))
            {
                Logger.LogWarning("Couldn't find GW2 string in the registry.");
                gw2exe = string.Empty;
            }

            Gw2DirPath = Path.GetDirectoryName(gw2exe) ?? string.Empty;
            Gw2ExecPath = gw2exe;

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
            OnPropertyChanged(nameof(Gw2ExecPath));  // TODO: is it still a thing?
        }

        #endregion Commands Logic

        #region Methods

        #region Validation

        /// <summary>
        /// Validates <see cref="Gw2ExecPath"/> property.
        /// </summary>
        /// <param name="path">GW2 executable path.</param>
        /// <param name="context">Validation context.</param>
        /// <returns><see cref="ValidationResult"/> object.</returns>
        public static ValidationResult? ValidateGw2ExecPath_FileExists(string path, ValidationContext context)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                return new ValidationResult(_fileNotExistsErrorMsg);
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Validates <see cref="Gw2ExecPath"/> property.
        /// </summary>
        /// <param name="path">GW2 executable path.</param>
        /// <param name="context">Validation context.</param>
        /// <returns><see cref="ValidationResult"/> object.</returns>
        public static ValidationResult? ValidateGw2ExecPath_IsGw2Executable(string path, ValidationContext context)
        {
            var instance = (HomePageViewModel)context.ObjectInstance;

            if (string.IsNullOrEmpty(path) ||
                !(Path.GetExtension(path) == instance.GameSharedData.Gw2ExecutableExtension) ||
                !(FileVersionInfo.GetVersionInfo(path)?.ProductName?.ToString() == instance.GameSharedData.Gw2ProductName) ||
                !(FileVersionInfo.GetVersionInfo(path)?.FileDescription?.ToString() == instance.GameSharedData.Gw2FileDescription))
            {
                return new ValidationResult(_fileNotGw2ExeErrorMsg);
            }

            return ValidationResult.Success;
        }

        #endregion Validation

        #endregion Methods
    }
}
