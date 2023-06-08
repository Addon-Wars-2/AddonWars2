// ==================================================================================================
// <copyright file="NewsPageViewModel.cs" company="Addon-Wars-2">
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
    using AddonWars2.App.Configuration;
    using AddonWars2.App.UIServices.Enums;
    using AddonWars2.App.Utils.Helpers;
    using AddonWars2.App.ViewModels.Factories;
    using AddonWars2.Services.HttpClientWrapper.Interfaces;
    using AddonWars2.Services.RssFeedService;
    using AddonWars2.Services.RssFeedService.Interfaces;
    using AddonWars2.Services.RssFeedService.Models;
    using AddonWars2.SharedData.Interfaces;
    using CommunityToolkit.Mvvm.Input;
    using Microsoft.Extensions.Logging;
    using MvvmDialogs;

    /// <summary>
    /// Represents <see cref="NewsPageViewModel"/> states.
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
        /// View model failed to update its data.
        /// Similar to Ready, but is used to indicate there is an error occured.
        /// </summary>
        Error,
    }

    /// <summary>
    /// View model used by news view.
    /// </summary>
    public class NewsPageViewModel : BaseViewModel
    {
        #region Fields

        private const string URI_DEFAULT = "about:blank";

        private static readonly string _networkConnectionErrorTitle = ResourcesHelper.GetApplicationResource<string>("S.NewsPage.NewsList.Errors.NoInternetConnection.Title");
        private static readonly string _networkConnectionErrorMessage = ResourcesHelper.GetApplicationResource<string>("S.NewsPage.NewsList.Errors.NoInternetConnection.Message");
        private static readonly string _httpClientBadCodeErrorTitle = ResourcesHelper.GetApplicationResource<string>("S.NewsPage.NewsList.Errors.BadCode.Title");
        private static readonly string _httpClientBadCodeErrorMessage = ResourcesHelper.GetApplicationResource<string>("S.NewsPage.NewsList.Errors.BadCode.Message");
        private static readonly string _failedParseRssErrorTitle = ResourcesHelper.GetApplicationResource<string>("S.NewsPage.NewsList.Errors.FiledToParseRss.Title");
        private static readonly string _failedParseRssErrorMessage = ResourcesHelper.GetApplicationResource<string>("S.NewsPage.NewsList.Errors.FiledToParseRss.Message");

        private readonly IDialogService _dialogService;
        private readonly IErrorDialogViewModelFactory _errorDialogViewModelFactory;
        private readonly IApplicationConfig _applicationConfig;
        private readonly IAppSharedData _appStaticData;
        private readonly Gw2RssFeedService _rssFeedService;
        private readonly IHttpClientWrapper _httpClientService;

        private NewsViewModelState _viewModelState = NewsViewModelState.Ready;
        private bool _isActuallyLoaded = false;
        private ObservableCollection<Gw2RssFeedItem> _rssFeedCollection = new ObservableCollection<Gw2RssFeedItem>();
        private Gw2RssFeedItem? _displayedRssFeedItem;
        private Uri _displayedRssFeedContent = new Uri(URI_DEFAULT);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsPageViewModel"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/>.</param>
        /// <param name="dialogService">A reference to <see cref="IErrorDialogService"/>.</param>
        /// <param name="errorDialogViewModelFactory">A reference to <see cref="IErrorDialogViewModelFactory"/>.</param>
        /// <param name="appConfig">A reference to <see cref="IApplicationConfig"/>.</param>
        /// <param name="appStaticData">A reference to <see cref="IAppSharedData"/>.</param>
        /// <param name="rssFeedService">A referemnce to <see cref="Gw2RssFeedService"/>.</param>
        /// <param name="httpClientService">A referemnce to <see cref="IHttpClientWrapper"/>.</param>
        public NewsPageViewModel(
            ILogger<NewsPageViewModel> logger,
            IDialogService dialogService,
            IErrorDialogViewModelFactory errorDialogViewModelFactory,
            IApplicationConfig appConfig,
            IAppSharedData appStaticData,
            IRssFeedService<Gw2RssFeedItem> rssFeedService,
            IHttpClientWrapper httpClientService)
            : base(logger)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _errorDialogViewModelFactory = errorDialogViewModelFactory ?? throw new ArgumentNullException(nameof(errorDialogViewModelFactory));
            _applicationConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            _appStaticData = appStaticData ?? throw new ArgumentNullException(nameof(appStaticData));
            _rssFeedService = rssFeedService as Gw2RssFeedService ?? throw new ArgumentNullException(nameof(rssFeedService));
            _httpClientService = httpClientService ?? throw new ArgumentNullException(nameof(httpClientService));

            LoadNewsCommand = new AsyncRelayCommand(ExecuteReloadNewsAsync, () => IsActuallyLoaded == false);
            RefreshNewsCommand = new AsyncRelayCommand(
                ExecuteReloadNewsAsync,
                () => ViewModelState == NewsViewModelState.Ready || ViewModelState == NewsViewModelState.Error);
            LoadRssItemContentCommand = new RelayCommand(
                ExecuteLoadRssItemContentCommand,
                () => ViewModelState == NewsViewModelState.Ready || ViewModelState == NewsViewModelState.Error);

            Logger.LogDebug("Instance initialized.");
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a reference to <see cref="IDialogService"/> service.
        /// </summary>
        public IDialogService DialogService => _dialogService;

        /// <summary>
        /// Gets a reference to the error dialog view model.
        /// </summary>
        public IErrorDialogViewModelFactory ErrorDialogViewModelFactory => _errorDialogViewModelFactory;

        /// <summary>
        /// Gets a reference to the application config.
        /// </summary>
        public IApplicationConfig AppConfig => _applicationConfig;

        /// <summary>
        /// Gets the application static data.
        /// </summary>
        public IAppSharedData AppStaticData => _appStaticData;

        /// <summary>
        /// Gets a reference to the RSS feed service.
        /// </summary>
        public Gw2RssFeedService RssFeedService => _rssFeedService;

        /// <summary>
        /// Gets a reference to the HTTP client service.
        /// </summary>
        public IHttpClientWrapper HttpClientService => _httpClientService;

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
        public ObservableCollection<Gw2RssFeedItem> RssFeedCollection
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
        public Gw2RssFeedItem? DisplayedRssFeedItem
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
        /// Gets or sets the view model state.
        /// </summary>
        public NewsViewModelState ViewModelState
        {
            get => _viewModelState;
            set
            {
                SetProperty(ref _viewModelState, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets a command that updates news list on the first load.
        /// </summary>
        public AsyncRelayCommand LoadNewsCommand { get; private set; }

        /// <summary>
        /// Gets a command that forces the news list to update itself.
        /// </summary>
        public AsyncRelayCommand RefreshNewsCommand { get; private set; }

        /// <summary>
        /// Gets a command that updates the content of a selected RSS item.
        /// </summary>
        public RelayCommand LoadRssItemContentCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        #region LoadNewsCommand Logic

        // TODO: Move out logic to services. This VM does way too much.

        // LoadNewsCommand command logic.
        private async Task ExecuteReloadNewsAsync()
        {
            Logger.LogDebug("Executing command.");

            ViewModelState = NewsViewModelState.Fetching;
            RssFeedCollection.Clear();

            HttpResponseMessage? response = null;
            try
            {
                Logger.LogInformation("Requesting RSS data.");

                if (!HttpClientService.IsNetworkAvailable())
                {
                    ViewModelState = NewsViewModelState.Error;

                    Logger.LogError($"{_networkConnectionErrorTitle}");

                    ShowErrorDialog(_networkConnectionErrorTitle, _networkConnectionErrorMessage);

                    return;
                }

                response = await HttpClientService.GetAsync(AppConfig.UserData.Gw2Rss);
            }
            catch (HttpRequestException e)
            {
                ViewModelState = NewsViewModelState.Error;

                if (response != null)
                {
                    Logger.LogError($"Bad code: {(int)response.StatusCode} {response.StatusCode}");
                    Logger.LogError($"{e.Message}");
                    Logger.LogError($"Exception: {e}");
                    ShowErrorDialog(_httpClientBadCodeErrorTitle, _httpClientBadCodeErrorMessage, $"GET {AppConfig.UserData.Gw2Rss}\n{(int)response.StatusCode} {response.StatusCode}\n{e.Message}");
                }
                else
                {
                    Logger.LogError($"{e.Message}");
                    Logger.LogError($"Exception: {e}");
                    ShowErrorDialog(_httpClientBadCodeErrorTitle, _httpClientBadCodeErrorMessage, $"GET {AppConfig.UserData.Gw2Rss}\n{e.Message}");
                }

                return;
            }

            // Read response content and store as a list of RssFeedItem.
            var feed = await ParseResponseDataAsync(response);
            if (feed == null)
            {
                return;
            }

            // Sort by date while keeping the sticky item on top.
            feed = await SortRssFeedCollectionAsync(feed); // TODO: Don't create a new one after sorting?

            // Access content and inject style.
            var cssFileName = "style.css";  // TODO: move out hardcoding
            foreach (var item in feed)
            {
                item.ContentEncoded = RssFeedService.AddProtocolPrefixesToHtml(item.ContentEncoded ?? string.Empty, "https");
                item.ContentEncoded = RssFeedService.InjectCssIntoHtml(item.ContentEncoded ?? string.Empty, cssFileName);
            }

            // Copy CSS file into the RSS feed directory.
            var rssDirPath = Path.Combine(AppConfig.SessionData.AppDataDir ?? string.Empty, AppStaticData.RssFeedDirName ?? string.Empty);
            var rssFilePath = Path.Combine(rssDirPath, cssFileName);
            await IOHelper.ResourceCopyToAsync($"AddonWars2.App.Resources.{cssFileName}", rssFilePath);  // TODO: Do not copy if exists?

            // Save HTML pages into the RSS feed directory.
            await WriteRssItemsAsync(feed, rssDirPath);

            // Add to the observable collection.
            await FillRssItemsAsync(feed, RssFeedCollection);

            ViewModelState = NewsViewModelState.Ready;

            Logger.LogInformation("News feed updated.");
        }

        // Parse response data.
        private async Task<IList<Gw2RssFeedItem>?> ParseResponseDataAsync(HttpResponseMessage response)
        {
            // TODO: "Root element missing" exception is thrown from XDocument.LoadAsync(...) method
            //       in some scenarios when the internet connection is interrupted.
            //       Need to figure out what's the problem with the HTTP content stream (if there is any).

            Logger.LogDebug("Parsing response data.");

            try
            {
                var stream = await response.Content.ReadAsStreamAsync();
                var feed = await RssFeedService.ReadXmlStreamAsync(stream);

                Logger.LogDebug($"Parsed items: {feed.Count}");

                return feed;
            }
            catch (Exception e)
            {
                ViewModelState = NewsViewModelState.Error;

                Logger.LogError($"Failed to parse data.");

                ShowErrorDialog(_failedParseRssErrorTitle, _failedParseRssErrorMessage, e.Message);

                return null;
            }
        }

        // Sort the list, so the "sticky" item will be always above (the first one),
        // and the rest will be sorted by date. Even though RSS feed already contains items in a
        // historical order, we run this sort just to make sure.
        private async Task<IList<Gw2RssFeedItem>> SortRssFeedCollectionAsync(IList<Gw2RssFeedItem> collection)
        {
            ArgumentNullException.ThrowIfNull(collection, nameof(collection));

            Logger.LogDebug("Sorting...");
            return await Task.Run(() => collection.OrderByDescending(x => x.IsSticky).ThenByDescending(x => x.PublishDate).ToList());
        }

        // Write HTML files locally, so WebView can read them later.
        private async Task WriteRssItemsAsync(IList<Gw2RssFeedItem> collection, string directory)
        {
            foreach (var item in collection)
            {
                var filepath = Path.Combine(directory, item.Guid ?? string.Empty) + ".html";
                await RssFeedService.WriteRssItemAsync(item, filepath);
                Logger.LogDebug($"HTML file saved: {filepath}");
            }
        }

        // Update the destination list with new items.
        private async Task FillRssItemsAsync(IList<Gw2RssFeedItem> source, IList<Gw2RssFeedItem> destination)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));
            ArgumentNullException.ThrowIfNull(destination, nameof(destination));

            foreach (var item in source)
            {
                destination.Add(item);
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
            var dirpath = Path.Combine(AppConfig.SessionData.AppDataDir ?? string.Empty, AppStaticData.RssFeedDirName);
            var filepath = Path.Combine(dirpath, DisplayedRssFeedItem.Guid ?? string.Empty) + extension;

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
                    DisplayedRssFeedContent = new Uri(URI_DEFAULT);
                    Logger.LogError($"An exception occured: {e.Message}");
                    return;
                }
            }

            DisplayedRssFeedContent = new Uri(URI_DEFAULT);
            Logger.LogError($"Failed to load WebView2 content from HTML file.");
        }

        #endregion Commands Logic

        #region Methods

        /// <summary>
        /// Shows an error dialog.
        /// </summary>
        /// <param name="title">Dialog window title.</param>
        /// <param name="message">Dialog message.</param>
        /// <param name="details">Dialog additional details.</param>
        /// <param name="buttons">Dialog buttons to show.</param>
        /// <returns>Dialog result.</returns>
        protected bool? ShowErrorDialog(string title, string message, string? details = null, ErrorDialogButtons buttons = ErrorDialogButtons.OK)
        {
            var vm = ErrorDialogViewModelFactory.Create(title, message, details, buttons);
            var result = DialogService.ShowDialog(this, vm);

            return result;
        }

        #endregion Methods
    }
}
