// ==================================================================================================
// <copyright file="NewsViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
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
        /// Normal state.
        /// </summary>
        Normal,

        /// <summary>
        /// View model is updating its RSS collection.
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
        //       Maybe I should consider adding another layer between "dumb VM"
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
            SetState(NewsViewModelState.Normal);

            // TODO: uncomment after implementing refresh button.
            ////ReloadNewsCommand = new RelayCommand(ExecuteReloadNewsAsync, () => IsActuallyLoaded == false);
            ReloadNewsCommand = new RelayCommand(ExecuteReloadNewsAsync);
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

        public string UpdateErrorCode
        {
            get => _updateErrorCode;
            set
            {
                SetProperty(ref _updateErrorCode, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        public string ViewModelState
        {
            get => _viewModelState;
            set
            {
                SetProperty(ref _viewModelState, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

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
        /// Gets a command that updates news list.
        /// </summary>
        public RelayCommand ReloadNewsCommand { get; private set; }

        /// <summary>
        /// Gets a command that updates the content of a selected RSS item.
        /// </summary>
        public RelayCommand LoadRssItemContentCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        // ReloadNewsCommand command logic.
        private async void ExecuteReloadNewsAsync()
        {
            Logger.LogDebug("Executing command.");
            Logger.LogInformation("Updating news feed.");

            ViewModelStateInternal = NewsViewModelState.Updating;

            RssFeedCollection.Clear();

            SetState(NewsViewModelState.Updating);

            // GW2 RSS feed falls back to EN version if the selected culture is unknown.
            Logger.LogDebug("Requesting RSS data.");
            var response = await WebHelper.GetResponseAsync(AppConfig.LocalData.Gw2Rss);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError($"Bad code: {(int)response.StatusCode} {response.StatusCode}");
                SetState(NewsViewModelState.FailedToUpdate);
                UpdateErrorCode = $"{(int)response.StatusCode} {response.StatusCode}";
                return;
            }

            Logger.LogDebug("Parsing response data.");
            var stream = await response.Content.ReadAsStreamAsync();
            var xml = await WebHelper.LoadXmlAsync(stream);
            var feed = await ParseRssFeedXmlAsync(xml);
            Logger.LogDebug($"Parsed items: {feed.Count}");

            foreach (var item in feed)
            {
                await WriteRssItemContentAsync(item);
            }

            SetState(NewsViewModelState.Normal);

            // Instead or replacing the whole collection, we add items one by one for animation purposes.
            foreach (var item in feed)
            {
                RssFeedCollection.Add(item);
                await Task.Delay(50);  // TODO: Delay feels wrong here, it belongs to UI.
            }

            Logger.LogInformation("News list updated.");
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
                    ContentEncoded = item.Element(nsContent + "encoded")?.Value.Replace(@"""//", @"""https://"),  // TODO: Move "coerce" part elsewhere?
                    IsSticky = isSticky,
                };

                feed.Add(entry);
            }

            return feed;
        }

        private async Task WriteRssItemContentAsync(RssFeedItem item)
        {
            // TODO: Cache all items or cleanup?

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

        // Updates config if a property specified in the even manager params was changed.
        private void NewsViewModel_ConfigPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ApplicationConfig.WriteLocalDataAsXml(AppConfig.ConfigFilePath, AppConfig.LocalData);
            Logger.LogDebug($"Config file updated.");
        }

        private void SetState(NewsViewModelState state)
        {
            Logger.LogDebug($"ViewModel state set: {state}");
            ViewModelStateInternal = state;
            ViewModelState = Enum.GetName(typeof(NewsViewModelState), state);
        }

        #endregion Methods
    }
}
