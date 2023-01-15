// ==================================================================================================
// <copyright file="NewsViewModel.cs" company="Addon-Wars-2 ">
// Copyright (c) Addon-Wars-2 . All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Xml.Linq;
    using AddonWars2.App.Commands;
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
        /// Ready state.
        /// </summary>
        Ready,

        /// <summary>
        /// View model is fetching RSS data from a source.
        /// </summary>
        Fetching,

        /// <summary>
        /// View model is loading the content into its RSS collection.
        /// </summary>
        Updating,

        /// <summary>
        /// View model couldn't retrieve RSS collection.
        /// </summary>
        FailedToUpdate,
    }

    /// <summary>
    /// View model used by news view.
    /// </summary>
    public class NewsViewModel : BaseViewModel
    {
        // TODO: There is quite a lot of logic inside this VM.
        //       Maybe should we consider adding another layer between "dumb VM"
        //       and "dumb model" layers? Or separate UI logic from business one
        //       more explicitly?

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
            ////RefreshNewsCommand = new RelayCommand(ExecuteReloadNewsAsync, () => ViewModelStateInternal == NewsViewModelState.Ready);
            RefreshNewsCommand = new RelayCommand(
                ExecuteReloadNewsAsync,
                () => ViewModelStateInternal == NewsViewModelState.Ready || ViewModelStateInternal == NewsViewModelState.FailedToUpdate);
            LoadRssItemContentCommand = new RelayCommand(ExecuteLoadRssItemContentCommand);

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
                Logger.LogDebug($"Property set: {value}");
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

        #region ReloadNewsCommand Logic

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
                Logger.LogError($"No internet connection.");
                SetState(NewsViewModelState.FailedToUpdate);
                UpdateErrorCode = e.Message;
                return;
            }

            // Bad code returned.
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError($"Bad code: {(int)response.StatusCode} {response.StatusCode}");
                SetState(NewsViewModelState.FailedToUpdate);
                UpdateErrorCode = $"{response}";
                return;
            }

            // TODO: "Root element missing" exception is thrown from XDocument.LoadAsync(...) method
            //       in some scenarios when the internet connection is interrupted.
            //       Need to figure out what's the problem with the HTTP content stream (if there is any).
            ObservableCollection<RssFeedItem> feed;
            Logger.LogDebug("Parsing response data.");
            try
            {
                var stream = await response.Content.ReadAsStreamAsync();
                var xml = await WebHelper.LoadXmlAsync(stream);
                feed = await ParseRssFeedXmlAsync(xml);
                Logger.LogDebug($"Parsed items: {feed.Count}");
            }
            catch (Exception e)
            {
                Logger.LogError($"Failed to parse data.");
                SetState(NewsViewModelState.FailedToUpdate);
                UpdateErrorCode = $"{e.Message}";
                return;
            }

            SetState(NewsViewModelState.Updating);

            foreach (var item in feed)
            {
                await WriteRssItemContentAsync(item);
            }

            var sticky = feed.Where(x => x.IsSticky).ToList();
            foreach (var item in sticky)
            {
                RssFeedCollection.Add(item);
                await Task.Delay(50);  // for animation purposes
            }

            var normal = feed.Where(x => !x.IsSticky).ToList();
            foreach (var item in normal)
            {
                RssFeedCollection.Add(item);
                await Task.Delay(50);  // for animation purposes
            }

            SetState(NewsViewModelState.Ready);

            Logger.LogInformation("News feed updated.");
        }

        // Async call for ParseRssFeedXml.
        private Task<ObservableCollection<RssFeedItem>> ParseRssFeedXmlAsync(XDocument xml)
        {
            var feed = Task.Run(() => ParseRssFeedXml(xml));
            return feed;
        }

        // Parses the GW2 RSS feed.
        private ObservableCollection<RssFeedItem> ParseRssFeedXml(XDocument xml)
        {
            if (xml == null)
            {
                throw new NullReferenceException(nameof(xml));
            }

            var feed = new ObservableCollection<RssFeedItem>();
            var nsContent = xml.Root.GetNamespaceOfPrefix("content");

            foreach (var item in xml.Descendants("item"))
            {
                var isSticky = (from cat in item.Elements("category")
                                where cat.Value.ToLower() == "sticky"
                                select cat).Any();

                var entry = new RssFeedItem()
                {
                    Title = item.Element("title")?.Value,
                    Link = item.Element("link")?.Value,
                    PublishDate = DateTime.Parse(item.Element("pubDate")?.Value),
                    Guid = item.Element("guid")?.Value.Split("=").Last(),
                    Description = item.Element("description")?.Value,
                    ContentEncoded = item.Element(nsContent + "encoded")?.Value.Replace(@"""//", @"""https://"),  // HACK: Is there a batter way rather than editing strings?
                    IsSticky = isSticky,
                };

                feed.Add(entry);
            }

            return feed;
        }

        // Asynchronously writes RSS item HTML content to a file.
        private async Task WriteRssItemContentAsync(RssFeedItem item)
        {
            // TODO: Should we cache items or merely map the GW2 RSS feed as is?

            var content = item.ContentEncoded;
            var guid = item.Guid;
            var extension = ".html";
            var dirpath = Path.Join(AppConfig.AppDataDir, AppConfig.RssFeedDirName);
            var filepath = Path.Join(dirpath, guid) + extension;

            if (!Directory.Exists(dirpath))
            {
                Directory.CreateDirectory(dirpath);
            }

            await File.WriteAllTextAsync(filepath, content);

            Logger.LogDebug($"HTML filed saved: {filepath}");
        }

        #endregion ReloadNewsCommand Logic

        // LoadRssItemContentCommand command logic.
        private void ExecuteLoadRssItemContentCommand()
        {
            var extension = ".html";
            var dirpath = Path.Join(AppConfig.AppDataDir, AppConfig.RssFeedDirName);
            var filepath = Path.Join(dirpath, DisplayedRssFeedItem?.Guid) + extension;

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
            Logger.LogDebug($"ViewModel state set: {state}");
            ViewModelStateInternal = state;
            ViewModelState = Enum.GetName(typeof(NewsViewModelState), state);
        }

        #endregion Methods
    }
}
