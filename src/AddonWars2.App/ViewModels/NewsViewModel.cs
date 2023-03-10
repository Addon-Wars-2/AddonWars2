// ==================================================================================================
// <copyright file="NewsViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows;
    using AddonWars2.App.Commands;
    using AddonWars2.App.Helpers;
    using AddonWars2.App.Models.Application;
    using AddonWars2.App.Models.GuildWars2;
    using AddonWars2.App.Utils.Helpers;
    using CommunityToolkit.Mvvm.Input;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents <see cref="NewsViewModel"/> states.
    /// </summary>
    public enum NewsViewModelState
    {
        /// <summary>
        /// View model is ready. Default state.
        /// </summary>
        Ready,

        /// <summary>
        /// View model is fetching RSS data from a source (web or local).
        /// </summary>
        Fetching,

        /// <summary>
        /// View model is updating its content to be presented in View.
        /// </summary>
        Updating,

        /// <summary>
        /// View model failed to update its data.
        /// Similar to Ready, but is used to indicate there is an error occured.
        /// </summary>
        FailedToUpdate,
    }

    /// <summary>
    /// View model used by news view.
    /// </summary>
    public class NewsViewModel : BaseViewModel
    {
        #region Fields

        private string _updateErrorCode;
        private string _viewModelState;
        private NewsViewModelState _viewModelStateInternal;
        private bool _isActuallyLoaded = false;
        private ObservableCollection<RssFeedItem> _rssFeedCollection;
        private RssFeedItem _displayedRssFeedItem;
        private Uri _displayedRssFeedContent;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsViewModel"/> class.
        /// </summary>
        /// <param name="logger">A referemnce to <see cref="ILogger"/>.</param>
        /// <param name="appConfig">A reference to <see cref="ViewModels.AppConfig"/>.</param>
        /// <param name="commonCommands">A reference to a common commands class.</param>
        public NewsViewModel(
            ILogger<NewsViewModel> logger,
            ApplicationConfig appConfig,
            CommonCommands commonCommands)
            : base(logger)
        {
            AppConfig = appConfig;
            CommonCommands = commonCommands;
            RssFeedCollection = new ObservableCollection<RssFeedItem>();
            SetState(NewsViewModelState.Ready);

            LoadNewsCommand = new RelayCommand(ExecuteReloadNewsAsync, () => IsActuallyLoaded == false);
            RefreshNewsCommand = new RelayCommand(
                ExecuteReloadNewsAsync,
                () => ViewModelStateInternal == NewsViewModelState.Ready || ViewModelStateInternal == NewsViewModelState.FailedToUpdate);
            LoadRssItemContentCommand = new RelayCommand(
                ExecuteLoadRssItemContentCommand,
                () => ViewModelStateInternal == NewsViewModelState.Ready || ViewModelStateInternal == NewsViewModelState.FailedToUpdate);

            Logger.LogDebug("Instance initialized.");
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a reference to the application config.
        /// </summary>
        public ApplicationConfig AppConfig { get; private set; }

        /// <summary>
        /// Gets a reference to a common commands class.
        /// </summary>
        public CommonCommands CommonCommands { get; private set; }

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
        /// Gets or sets a collection of GW2 RSS items.
        /// </summary>
        public ObservableCollection<RssFeedItem> RssFeedCollection
        {
            get => _rssFeedCollection;
            set
            {
                SetProperty(ref _rssFeedCollection, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets or sets the currently displayed RSS item.
        /// </summary>
        public RssFeedItem DisplayedRssFeedItem
        {
            get => _displayedRssFeedItem;
            set
            {
                SetProperty(ref _displayedRssFeedItem, value);
                Logger.LogDebug($"Property set: {value}, guild={value?.Guid}");
            }
        }

        /// <summary>
        /// Gets or sets the currently displayed RSS item content.
        /// </summary>
        public Uri DisplayedRssFeedContent
        {
            get => _displayedRssFeedContent;
            set
            {
                SetProperty(ref _displayedRssFeedContent, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets or sets the error code (including error message)
        /// if an error occured on news feed update.
        /// </summary>
        public string UpdateErrorCode
        {
            get => _updateErrorCode;
            set
            {
                SetProperty(ref _updateErrorCode, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets or sets the view model state as a string representation
        /// of <see cref="NewsViewModelState"/> value.
        /// </summary>
        public string ViewModelState
        {
            get => _viewModelState;
            set
            {
                SetProperty(ref _viewModelState, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets or sets the view model state.
        /// </summary>
        internal NewsViewModelState ViewModelStateInternal
        {
            get => _viewModelStateInternal;
            set
            {
                SetProperty(ref _viewModelStateInternal, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets a command that updates news list on the first load.
        /// </summary>
        public RelayCommand LoadNewsCommand { get; private set; }

        /// <summary>
        /// Gets a command that forces the news list to update itself.
        /// </summary>
        public RelayCommand RefreshNewsCommand { get; private set; }

        /// <summary>
        /// Gets a command that updates the content of a selected RSS item.
        /// </summary>
        public RelayCommand LoadRssItemContentCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        #region LoadNewsCommand Logic

        // LoadNewsCommand command logic.
        private async void ExecuteReloadNewsAsync()
        {
            Logger.LogDebug("Executing command.");
            Logger.LogInformation("Fetching news feed.");

            SetState(NewsViewModelState.Fetching);

            RssFeedCollection.Clear();

            Logger.LogDebug("Requesting RSS data.");

            HttpResponseMessage response;
            try
            {
                response = await WebHelper.GetResponseAsync(AppConfig.LocalData.Gw2Rss, HttpCompletionOption.ResponseHeadersRead);
            }
            catch (HttpRequestException e)
            {
                // No internet connection.
                SetState(NewsViewModelState.FailedToUpdate);
                UpdateErrorCode = e.Message;
                Logger.LogError($"No internet connection.");
                return;
            }

            // Bad code returned.
            if (!response.IsSuccessStatusCode)
            {
                SetState(NewsViewModelState.FailedToUpdate);
                UpdateErrorCode = $"{response}";
                Logger.LogError($"Bad code: {(int)response.StatusCode} {response.StatusCode}");
                return;
            }

            SetState(NewsViewModelState.Updating);

            var feed = await ParseResponseDataAsync(response);
            feed = SortRssFeedCollection(feed);
            var cssFileName = "style.css";
            foreach (var item in feed)
            {
                item.ContentEncoded = RssFeedHelper.AddProtocolPrefixesToHtml(item.ContentEncoded, "https");
                item.ContentEncoded = RssFeedHelper.InjectCssIntoHtml(item.ContentEncoded, cssFileName);
            }

            var rssDirPath = Path.Combine(AppConfig.AppDataDir, AppConfig.RssFeedDirName);
            var rssFilePath = Path.Combine(rssDirPath, cssFileName);
            await IOHelper.ResourceCopyToAsync($"AddonWars2.App.Resources.{cssFileName}", rssFilePath);  // copy CSS embedded resource

            await WriteRssItemsAsync(feed, rssDirPath);
            await FillRssItemsAsync(feed, RssFeedCollection);

            SetState(NewsViewModelState.Ready);

            Logger.LogInformation("News feed updated.");
        }

        // Parse response data.
        private async Task<IList<RssFeedItem>> ParseResponseDataAsync(HttpResponseMessage response)
        {
            // TODO: "Root element missing" exception is thrown from XDocument.LoadAsync(...) method
            //       in some scenarios when the internet connection is interrupted.
            //       Need to figure out what's the problem with the HTTP content stream (if there is any).
            Logger.LogDebug("Parsing response data.");
            try
            {
                var stream = await response.Content.ReadAsStreamAsync();
                var feed = await RssFeedHelper.ParseXmlStreamAsync(stream);
                Logger.LogDebug($"Parsed items: {RssFeedCollection.Count}");
                return feed;
            }
            catch (Exception e)
            {
                SetState(NewsViewModelState.FailedToUpdate);
                UpdateErrorCode = $"{e.Message}";
                Logger.LogError($"Failed to parse data.");
                return null;
            }
        }

        // Sort the list, so the "sticky" item will be always above (the first one),
        // and the rest will be sorted by date. Even though RSS feed already contains items in a
        // historical order, we run this sort just to make sure.
        private IList<RssFeedItem> SortRssFeedCollection(IList<RssFeedItem> collection)
        {
            return collection.OrderByDescending(x => x.IsSticky).ThenByDescending(x => x.PublishDate).ToList();
        }

        // Write HTML files locally, so WebView can read them later.
        private async Task WriteRssItemsAsync(IList<RssFeedItem> collection, string directory)
        {
            foreach (var item in collection)
            {
                var filepath = Path.Combine(directory, item.Guid) + ".html";
                await RssFeedHelper.WriteRssItemContentAsync(item, filepath);
                Logger.LogDebug($"HTML file saved: {filepath}");
            }
        }

        // Update the destination list with new items using some delay.
        private async Task FillRssItemsAsync(IList<RssFeedItem> source, IList<RssFeedItem> destination, int delay = 50)
        {
            foreach (var item in source)
            {
                destination.Add(item);
                await Task.Delay(delay);  // for animation purposes
                Logger.LogDebug($"RSS item with guid={item.Guid} added.");
            }
        }

        #endregion LoadNewsCommand Logic

        // LoadRssItemContentCommand command logic.
        private void ExecuteLoadRssItemContentCommand()
        {
            Logger.LogDebug("Executing command.");

            if (DisplayedRssFeedItem == null)
            {
                Logger.LogDebug($"{nameof(DisplayedRssFeedItem)} is null.");
                return;
            }

            var extension = ".html";
            var dirpath = Path.Combine(AppConfig.AppDataDir, AppConfig.RssFeedDirName);
            var filepath = Path.Combine(dirpath, DisplayedRssFeedItem?.Guid) + extension;

            if (File.Exists(filepath))
            {
                try
                {
                    DisplayedRssFeedContent = new Uri(filepath);
                    Logger.LogDebug($"WebView2 content loaded from: {filepath}");
                    return;
                }
                catch (Exception e)
                {
                    DisplayedRssFeedContent = null;
                    Logger.LogError($"An exception occured: {e.Message}");
                    return;
                }
            }

            DisplayedRssFeedContent = new Uri("about:blank");
            Logger.LogDebug($"Failed to load WebView2 content from HTML file.");
        }

        #endregion Commands Logic

        #region Methods

        // Sets the view model state.
        private void SetState(NewsViewModelState state)
        {
            ViewModelStateInternal = state;
            ViewModelState = Enum.GetName(typeof(NewsViewModelState), state);
            Logger.LogDebug($"ViewModel state set: {state}");
        }

        #endregion Methods
    }
}
