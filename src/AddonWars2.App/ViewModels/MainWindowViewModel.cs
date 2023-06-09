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
    using System.Linq;
    using System.Reflection;
    using System.Windows.Controls;
    using AddonWars2.App.Configuration;
    using AddonWars2.App.ViewModels.Commands;
    using AddonWars2.SharedData.Interfaces;
    using AddonWars2.SharedData.Models;
    using CommunityToolkit.Mvvm.Input;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The main view model used by application main window.
    /// </summary>
    public class MainWindowViewModel : WindowBaseViewModel
    {
        #region Fields

        private readonly IApplicationConfig _applicationConfig;
        private readonly CommonCommands _commonCommands;
        private readonly IAppSharedData _appStaticData;
        private readonly IWebSharedData _webStaticData;

        private CultureInfo _selectedCulture;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="logger">A referemnce to <see cref="ILogger"/>.</param>
        /// <param name="appConfig">A reference to <see cref="IApplicationConfig"/>.</param>
        /// <param name="commonCommands">A reference to <see cref="Commands.CommonCommands"/>.</param>
        /// <param name="appStaticData">A reference to <see cref="IAppSharedData"/>.</param>
        /// <param name="webStaticData">A reference to <see cref="IWebSharedData"/> instance.</param>
        public MainWindowViewModel(
            ILogger<MainWindowViewModel> logger,
            IApplicationConfig appConfig,
            CommonCommands commonCommands,
            IAppSharedData appStaticData,
            IWebSharedData webStaticData)
            : base(logger)
        {
            _applicationConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            _commonCommands = commonCommands ?? throw new ArgumentNullException(nameof(commonCommands));
            _appStaticData = appStaticData ?? throw new ArgumentNullException(nameof(appStaticData));
            _webStaticData = webStaticData ?? throw new ArgumentNullException(nameof(webStaticData));

            _selectedCulture = appStaticData.AppSupportedCultures.First(x => x.Culture == appConfig.UserData.SelectedCultureString);

            Logger.LogDebug("Instance initialized.");
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a reference to the application config.
        /// </summary>
        public IApplicationConfig AppConfig => _applicationConfig;

        /// <summary>
        /// Gets a reference to a common commands class.
        /// </summary>
        public CommonCommands CommonCommands => _commonCommands;

        /// <summary>
        /// Gets the application static data.
        /// </summary>
        public IAppSharedData AppStaticData => _appStaticData;

        /// <summary>
        /// Gets a reference to the application web-related static data.
        /// </summary>
        public IWebSharedData WebStaticData => _webStaticData;

        /// <summary>
        /// Gets a list of available cultures.
        /// </summary>
        public ObservableCollection<CultureInfo> AvailableCultures => new ObservableCollection<CultureInfo>(AppStaticData.AppSupportedCultures);

        /// <summary>
        /// Gets a value indicating whether the application
        /// was executed in debug or normal mode.
        /// </summary>
        public bool IsDebugMode => AppConfig.SessionData.IsDebugMode;

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
        public string ProjectLink => WebStaticData.GitHubProjectRepositoryUrl;

        /// <summary>
        /// Gets the project wiki URL.
        /// </summary>
        public string ProjectWikiLink => WebStaticData.GitHubProjectWikiUrl;

        /// <summary>
        /// Gets or sets the selected culture.
        /// </summary>
        public CultureInfo SelectedCulture
        {
            get => _selectedCulture;
            set
            {
                SetProperty(ref _selectedCulture, value);
                AppConfig.UserData.SelectedCultureString = value.Culture;
                Logger.LogDebug($"Property set: {value}. Culture: {value.Culture}");
            }
        }

        /// <summary>
        /// Gets the package version with suffix included.
        /// </summary>
        public string AssemblyFileVersion => Assembly.GetExecutingAssembly().GetName()?.Version?.ToString() ?? string.Empty;

        #endregion Properties

        #region Commands

        #endregion Commands

        #region Commands Logic

        #endregion Commands Logic

        #region Methods

        #endregion Methods
    }
}
