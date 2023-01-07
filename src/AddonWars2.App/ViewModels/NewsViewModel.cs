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
    /// View model used by news view.
    /// </summary>
    public class NewsViewModel : BaseViewModel
    {
        #region Fields

        private bool _isActuallyLoaded = false;
        private ObservableCollection<RssFeedItem> _rssFeedCollection;
        private RssFeedItem _displayedRssFeedItem;
        private Uri _displayedRssFeedContent;
        private bool _isUpdating = false;

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

            // TODO: uncomment after implementing refresh button.
            ////ReloadNewsCommand = new RelayCommand(ExecuteReloadNewsAsync, () => IsActuallyLoaded == false);
            ReloadNewsCommand = new RelayCommand(ExecuteReloadNewsAsync);
            LoadRssItemContentCommand = new RelayCommand(ExecuteLoadRssItemContentCommand);
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
        /// Gets a collection of GW2 RSS items.
        /// </summary>
        public ObservableCollection<RssFeedItem> RssFeedCollection
        {
            get => _rssFeedCollection;
            private set
            {
                SetProperty(ref _rssFeedCollection, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets the currently displayed RSS item.
        /// </summary>
        public RssFeedItem DisplayedRssFeedItem
        {
            get => _displayedRssFeedItem;
            private set
            {
                SetProperty(ref _displayedRssFeedItem, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets the currently displayed RSS item content.
        /// </summary>
        public Uri DisplayedRssFeedContent
        {
            get => _displayedRssFeedContent;
            private set
            {
                SetProperty(ref _displayedRssFeedContent, value);
                Logger.LogDebug($"Property set: {value}");
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

            IsUpdating = true;

            RssFeedCollection.Clear();

            // GW2 RSS feed falls back to EN version if the selected culture is unknown.
            Logger.LogDebug("Requesting RSS data.");
            var response = await WebHelper.GetResponseAsync(AppConfig.LocalData.Gw2Rss);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogWarning($"Bad code: {(int)response.StatusCode} {response.StatusCode}");
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

            IsUpdating = false;

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
                    ContentEncoded = item.Element(nsContent + "encoded")?.Value,
                    IsSticky = isSticky,
                };

                feed.Add(entry);
            }

            return feed;
        }

        private async Task WriteRssItemContentAsync(RssFeedItem item)
        {
            // TODO: Doesn'treally belong to VM - move to elsewhere?
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

            Logger.LogDebug($"HTML filed saved: {guid}{extension}");
        }

        // LoadRssItemContentCommand command logic.
        private void ExecuteLoadRssItemContentCommand()
        {
            var extension = ".html";
            var dirpath = Path.Join(AppConfig.AppDataDir, AppConfig.RssFeedDirName);
            var filepath = Path.Join(dirpath, DisplayedRssFeedItem.Guid) + extension;

            try
            {
                DisplayedRssFeedContent = new Uri(filepath);
                Logger.LogDebug($"WebView2 content loaded from: {filepath}");
            }
            catch (Exception)
            {
                DisplayedRssFeedContent = new Uri(string.Empty);
                Logger.LogDebug($"Failed to load WebView2 content from HTML file.");
            }
        }

        #endregion Commands Logic

        #region Methods

        // Updates config if a property specified in the even manager params was changed.
        private void NewsViewModel_ConfigPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ApplicationConfig.WriteLocalDataAsXml(AppConfig.ConfigFilePath, AppConfig.LocalData);
            Logger.LogDebug($"Config file updated.");
        }

        #endregion Methods
    }
}
