// ==================================================================================================
// <copyright file="NewsViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels
{
    using System.ComponentModel;
    using AddonWars2.App.Models.Application;
    using AddonWars2.App.Utils.Helpers;
    using CommunityToolkit.Mvvm.Input;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// View model used by news view.
    /// </summary>
    public class NewsViewModel : BaseViewModel
    {
        #region Fields

        private bool _isActuallyLoaded = false;
        private bool _isUpdating = false;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsViewModel"/> class.
        /// </summary>
        /// <param name="logger">A referemnce to <see cref="ILogger"/>.</param>
        /// <param name="appConfig">A reference to <see cref="ViewModels.AppConfig"/>.</param>
        public NewsViewModel(
            ILogger<NewsViewModel> logger,
            ApplicationConfig appConfig)
            : base(logger)
        {
            Logger = logger;
            AppConfig = appConfig;

            PropertyChangedEventManager.AddHandler(this, NewsViewModel_ConfigPropertyChanged, nameof(Gw2RssWithCulture));

            ReloadNewsCommand = new RelayCommand(ExecuteReloadNewsAsync, () => IsActuallyLoaded == false);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a reference to the application config.
        /// </summary>
        public ApplicationConfig AppConfig { get; private set; }

        /// <summary>
        /// Gets or sets the GW2 RSS feef URL using the selected culture.
        /// </summary>
        public string Gw2RssWithCulture
        {
            get => AppConfig.LocalData.Gw2Rss;
            set
            {
                SetProperty(AppConfig.LocalData.Gw2Rss, value, AppConfig.LocalData, (model, rss) => model.Gw2Rss = rss);
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

        /// <summary>
        /// Gets or sets a value indicating whether the view model is in a process
        /// of updating news feed.
        /// </summary>
        public bool IsUpdating
        {
            get => _isUpdating;
            set
            {
                SetProperty(ref _isUpdating, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        // Gets the current logger instance.
        private static ILogger Logger { get; set; }

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets a command that updates news list.
        /// </summary>
        public RelayCommand ReloadNewsCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        private async void ExecuteReloadNewsAsync()
        {
            Logger.LogDebug("Executing command.");
            Logger.LogInformation("Updating news feed.");

            IsUpdating = true;

            // GW2 RSS feed falls back to EN version if the selected culture is unknown.
            Logger.LogDebug("Requesting RSS data.");
            var response = await WebHelper.GetResponseAsync(Gw2RssWithCulture);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogWarning($"Bad code: {(int)response.StatusCode} {response.StatusCode}");
                return;
            }

            Logger.LogDebug("Parsing response data.");
            var stream = await response.Content.ReadAsStreamAsync();
            var xml = await WebHelper.LoadXmlAsync(stream);

            IsUpdating = false;

            Logger.LogInformation("News list updated.");
        }

        #endregion Commands Logic

        #region Methods

        /// <summary>
        /// Builds a GW2 RSS feed URL using the selected culture.
        /// </summary>
        public void GenerateGw2RssWithCulture()
        {
            Logger.LogDebug("Executing method.");

            //var culture = AppConfig.UserData.SelectedCultureString.ShortName.ToLower();
            //Gw2RssWithCulture = string.Format(AppConfig.UserData.Gw2RssTemplate, culture);
        }

        // Updates config if a property specified in the even manager params was changed.
        private void NewsViewModel_ConfigPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ApplicationConfig.WriteLocalDataAsXml(AppConfig.ConfigFilePath, AppConfig.LocalData);
            Logger.LogDebug($"Config file updated.");
        }

        #endregion Methods
    }
}
