// ==================================================================================================
// <copyright file="ApplicationConfig.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Models.Application
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using AddonWars2.App.Helpers;
    using AddonWars2.App.ViewModels;
    using CommunityToolkit.Mvvm.ComponentModel;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Holds some globally available application information.
    /// </summary>
    public class ApplicationConfig : ObservableObject
    {
        #region Fields

        private DateTime _startupDateTime;
        private string _appDataDir;
        private CultureInfo _selectedCulture;
        private LocalData _localData;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationConfig"/> class.
        /// </summary>
        /// <param name="logger">A referemnce to <see cref="ILogger"/>.</param>
        public ApplicationConfig(ILogger<MainWindowViewModel> logger)
        {
            Logger = logger;
            SelectedCulture = DefaultCulture;
            LocalData = new LocalData();

            PropertyChangedEventManager.AddHandler(LocalData, ApplicationConfig_LocalDataChanged, string.Empty);
            PropertyChangedEventManager.AddHandler(this, ApplicationConfig_LocalDataChanged, nameof(LocalData));
            PropertyChangedEventManager.AddHandler(this, ApplicationConfig_SelectedCultureChanged, nameof(SelectedCulture));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the application startup date and time.
        /// </summary>
        public DateTime StartupDateTime
        {
            get => _startupDateTime;
            internal set
            {
                SetProperty(ref _startupDateTime, value);
            }
        }

        /// <summary>
        /// Gets or sets the application %APPDATA% dir.
        /// </summary>
        public string AppDataDir
        {
            get => _appDataDir;
            set
            {
                SetProperty(ref _appDataDir, value);
            }
        }

        /// <summary>
        /// Gets the application log directory name.
        /// </summary>
        public string LogDirName => "logs";

        /// <summary>
        /// Gets the application log file prefix.
        /// </summary>
        public string LogPrefix => "aw2_log_";

        /// <summary>
        /// Gets the application config file name.
        /// </summary>
        public string ConfigFileName => "aw2_config.xml";

        /// <summary>
        /// Gets the application config file path.
        /// </summary>
        public string ConfigFilePath => Path.Join(AppDataDir, ConfigFileName);

        /// <summary>
        /// Gets the directory name used to store GW2 RSS feed pages in HTML format.
        /// </summary>
        public string RssFeedDirName => "rss";

        /// <summary>
        /// Gets a list of available cultures.
        /// </summary>
        public ObservableCollection<CultureInfo> AvailableCultures { get; } = new ObservableCollection<CultureInfo>()
        {
            new CultureInfo("en-US", "EN", "English"),
            new CultureInfo("ru-RU", "RU", "Русский"),
        };

        /// <summary>
        /// Gets a list of cultures supported by ANet services.
        /// </summary>
        public ObservableCollection<CultureInfo> ArenaNetSupportedCultures { get; } = new ObservableCollection<CultureInfo>()
        {
            new CultureInfo("en-US", "EN", "English"),
            new CultureInfo("es-ES", "ES", "Español"),
            new CultureInfo("de-DE", "DE", "Deutsch"),
            new CultureInfo("fr-FR", "FR", "Français"),
        };

        /// <summary>
        /// Gets the default application culture.
        /// </summary>
        public CultureInfo DefaultCulture => AvailableCultures.First(x => x.Culture == "en-US");

        /// <summary>
        /// Gets or sets the selected application culture.
        /// </summary>
        public CultureInfo SelectedCulture
        {
            get => _selectedCulture;
            set
            {
                SetProperty(ref _selectedCulture, value);
            }
        }

        /// <summary>
        /// Gets or sets the part of application data which is stored locally.
        /// </summary>
        public LocalData LocalData
        {
            get => _localData;
            set
            {
                SetProperty(ref _localData, value);
            }
        }

        /// <summary>
        /// Gets the current logger instance.
        /// </summary>
        protected static ILogger Logger { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Serializes a given <see cref="LocalData"/> object and writes it using <paramref name="path"/> value.
        /// </summary>
        /// <param name="path">A path data is written to.</param>
        /// <param name="data">Data to be written.</param>
        public static void WriteLocalDataAsXml(string path, LocalData data)
        {
            IOHelper.SerializeXml(data, path);
        }

        /// <summary>
        /// Deserializes a given XML file and returns it as a <see cref="LocalData"/> object.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <returns><see cref="LocalData"/> object.</returns>
        public static LocalData LoadLocalDataFromXml(string path)
        {
            return IOHelper.DeserializeXml<LocalData>(path);
        }

        // Is invoked whenever selected culture property is changed.
        private void ApplicationConfig_SelectedCultureChanged(object sender, PropertyChangedEventArgs e)
        {
            // Set the requested culture string inside the local data.
            var culture = AvailableCultures.First(x => x.Culture == SelectedCulture.Culture);
            LocalData.SelectedCultureString = culture.Culture;

            // ANet webside supports only several languages. If the requested culture is not supported by ANet,
            // then use global (en) version of ANet services.
            if (!ArenaNetSupportedCultures.Any(x => x.Culture == culture.Culture))
            {
                culture = DefaultCulture;
            }

            LocalData.Gw2Home = string.Format(LocalData.Gw2HomeTemplate, culture.ShortName.ToLower());
            LocalData.Gw2Rss = string.Format(LocalData.Gw2RssTemplate, culture.ShortName.ToLower());
            LocalData.Gw2WikiHome = string.Format(LocalData.Gw2WikiHomeTemplate, culture.ShortName.ToLower());
        }

        // Is invoked whenever local data property is changed.
        private void ApplicationConfig_LocalDataChanged(object sender, PropertyChangedEventArgs e)
        {
            WriteLocalDataAsXml(ConfigFilePath, LocalData);
            Logger.LogDebug($"Config file updated.");

            // TODO: is it even legal?
            PropertyChangedEventManager.RemoveHandler(LocalData, ApplicationConfig_LocalDataChanged, string.Empty);
            PropertyChangedEventManager.AddHandler(LocalData, ApplicationConfig_LocalDataChanged, string.Empty);
            Logger.LogDebug($"PropertyChangedEventManager handler re-attached.");
        }

        #endregion Methods
    }
}
