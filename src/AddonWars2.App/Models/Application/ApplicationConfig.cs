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
    using AddonWars2.Services.XmlSerializerService.Interfaces;
    using CommunityToolkit.Mvvm.ComponentModel;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents the application configuration and holds the local data.
    /// </summary>
    public class ApplicationConfig : ObservableObject
    {
        #region Fields

        private static ILogger _logger;

        private readonly IXmlSerializationService _xmlSerializationService;

        private bool _isDebugMode;
        private DateTime _startupDateTime;
        private string _appDataDir = string.Empty;
        private string _logFileFullPath = string.Empty;
        private CultureInfo _selectedCulture;
        private LocalData _localData;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationConfig"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/> instance.</param>
        /// <param name="xmlSerializationService">A reference to <see cref="IXmlSerializationService"/> instance.</param>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="logger"/> is <see langword="null"/>.</exception>
        public ApplicationConfig(ILogger<ApplicationConfig> logger, IXmlSerializationService xmlSerializationService)
        {
            _logger = logger;
            _xmlSerializationService = xmlSerializationService;

            _selectedCulture = DefaultCulture;
            _localData = LocalData.Default;

            PropertyChangedEventManager.AddHandler(LocalData, ApplicationConfig_LocalDataPropertyChanged, string.Empty);
            PropertyChangedEventManager.AddHandler(this, ApplicationConfig_LocalDataChanged, nameof(LocalData));
            PropertyChangedEventManager.AddHandler(this, ApplicationConfig_SelectedCultureChanged, nameof(SelectedCulture));

            Logger.LogDebug("Instance initialized.");
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the application
        /// was executed in debug or normal mode.
        /// </summary>
        public bool IsDebugMode
        {
            get => _isDebugMode;
            internal set
            {
                SetProperty(ref _isDebugMode, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets the application startup date and time.
        /// </summary>
        public DateTime StartupDateTime
        {
            get => _startupDateTime;
            internal set
            {
                SetProperty(ref _startupDateTime, value);
                Logger.LogDebug($"Property set: {value}");
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
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets the application log directory name.
        /// </summary>
        public string LogDirName => "logs";

        /// <summary>
        /// Gets the application log file prefix.
        /// </summary>
        public string LogPrefix
        {
            get
            {
                var debugSuffix = IsDebugMode ? "debug_" : string.Empty;
                return "aw2_log_" + debugSuffix;
            }
        }

        /// <summary>
        /// Gets or sets the full log file path.
        /// </summary>
        public string LogFileFullPath
        {
            get => _logFileFullPath;
            set
            {
                SetProperty(ref _logFileFullPath, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets the application config file name.
        /// </summary>
        public string ConfigFileName => "aw2_config.xml";

        /// <summary>
        /// Gets the application config file path.
        /// </summary>
        public string ConfigFilePath => Path.Join(AppDataDir, ConfigFileName);

        /// <summary>
        /// Gets the extension of GW2 executable.
        /// </summary>
        public string Gw2FileExtension => ".exe";

        /// <summary>
        /// Gets the product name of GW2 executable.
        /// </summary>
        public string Gw2ProductName => "Guild Wars 2";

        /// <summary>
        /// Gets the file description of GW2 executable.
        /// </summary>
        public string Gw2FileDescription => "Guild Wars 2 Game Client";

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
        /// <remarks>
        /// <para>
        /// Changes in this property will internally invoke a handling method, which in turn will update <see cref="LocalData"/> properties.
        /// Any changes inside the <see cref="LocalData"/> will be automatically handled by another handling method and force the config file
        /// to be updated. No explicit save required.
        /// </para>
        /// <para>
        /// Handlers are re-attached automatically to ensure they keep a reference to the current property instance.
        /// </para>
        /// </remarks>
        public CultureInfo SelectedCulture
        {
            get => _selectedCulture;
            set
            {
                SetProperty(ref _selectedCulture, value);
                Logger.LogDebug($"Property set: {value}. Culture: {value.Culture}");
            }
        }

        /// <summary>
        /// Gets or sets the part of application data which is stored locally.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Changes in <see cref="LocalData"/> inner properties internally invoke a handling method,
        /// which in turn will write <see cref="LocalData"/> to a local storage. No explicit save required.
        /// </para>
        /// <para>
        /// Handlers are re-attached automatically to ensure they keep a reference to the current property instance.
        /// </para>
        /// </remarks>
        public LocalData LocalData
        {
            get => _localData;
            set
            {
                SetProperty(ref _localData, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets the current logger instance.
        /// </summary>
        protected static ILogger Logger => _logger;

        /// <summary>
        /// Gets the XML serialization service instance.
        /// </summary>
        protected IXmlSerializationService XmlSerializationService => _xmlSerializationService;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Serializes a given <see cref="LocalData"/> object and writes it using <paramref name="path"/> value.
        /// </summary>
        /// <param name="path">A path data is written to.</param>
        /// <param name="data">Data to be written.</param>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="path"/> is <see langword="null"/> or empty.</exception>
        public void WriteLocalDataAsXml(string path, LocalData? data)
        {
            ArgumentNullException.ThrowIfNull(path, nameof(path));

            _ = XmlSerializationService.SerializeXml(data, path);  // TODO: check the returned result

            Logger.LogDebug($"Local data saved.");
        }

        /// <summary>
        /// Deserializes a given XML file and returns it as a <see cref="LocalData"/> object.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <returns><see cref="LocalData"/> object.</returns>
        public LocalData? LoadLocalDataFromXml(string path)
        {
            var data = XmlSerializationService.DeserializeXml<LocalData>(path);

            Logger.LogDebug($"Local data loaded.");

            return data;
        }

        // Is invoked whenever the selected culture property is changed.
        private void ApplicationConfig_SelectedCultureChanged(object? sender, PropertyChangedEventArgs? e)
        {
            ArgumentNullException.ThrowIfNull(sender, nameof(sender));
            ArgumentNullException.ThrowIfNull(e, nameof(e));

            // Set the requested culture string inside the local data.
            var culture = AvailableCultures.FirstOrDefault(x => x.Culture == SelectedCulture.Culture, DefaultCulture);

            if (LocalData == null)
            {
                throw new NullReferenceException(nameof(LocalData));
            }

            LocalData.SelectedCultureString = culture.Culture;

            Logger.LogInformation($"Selected culture was changed to: {culture.Culture}");

            // ANet webside supports only several languages. If the requested culture is not supported by ANet,
            // then use global (en) version of ANet services.
            if (!ArenaNetSupportedCultures.Any(x => x.Culture == culture.Culture))
            {
                culture = DefaultCulture;
                Logger.LogWarning($"The requested culture is not supported by ANet services. The default one ({culture.Culture}) will be set for them.");
            }

            LocalData.Gw2Home = string.Format(LocalData.Gw2HomeTemplate, culture.ShortName.ToLower());
            LocalData.Gw2Rss = string.Format(LocalData.Gw2RssTemplate, culture.ShortName.ToLower());
            LocalData.Gw2WikiHome = string.Format(LocalData.Gw2WikiHomeTemplate, culture.ShortName.ToLower());

            // Re-attach handlers to keep a reference to the current property instance.
            PropertyChangedEventManager.RemoveHandler(this, ApplicationConfig_SelectedCultureChanged, nameof(SelectedCulture));
            PropertyChangedEventManager.AddHandler(this, ApplicationConfig_SelectedCultureChanged, nameof(SelectedCulture));

            Logger.LogDebug($"PropertyChangedEventManager | ApplicationConfig_SelectedCultureChanged handler re-attached.");
            Logger.LogDebug($"Handled.");
        }

        // Is invoked whenever the local data property is changed.
        private void ApplicationConfig_LocalDataPropertyChanged(object? sender, PropertyChangedEventArgs? e)
        {
            ArgumentNullException.ThrowIfNull(sender, nameof(sender));
            ArgumentNullException.ThrowIfNull(e, nameof(e));

            if (LocalData == null)
            {
                throw new NullReferenceException(nameof(LocalData));
            }

            WriteLocalDataAsXml(ConfigFilePath, LocalData);

            Logger.LogDebug($"Handled.");
        }

        // Is invoked whenever a local data inner property is changed.
        private void ApplicationConfig_LocalDataChanged(object? sender, PropertyChangedEventArgs? e)
        {
            ArgumentNullException.ThrowIfNull(sender, nameof(sender));
            ArgumentNullException.ThrowIfNull(e, nameof(e));

            if (LocalData == null)
            {
                throw new NullReferenceException(nameof(LocalData));
            }

            WriteLocalDataAsXml(ConfigFilePath, LocalData);

            PropertyChangedEventManager.RemoveHandler(LocalData, ApplicationConfig_LocalDataPropertyChanged, string.Empty);
            PropertyChangedEventManager.AddHandler(LocalData, ApplicationConfig_LocalDataPropertyChanged, string.Empty);

            Logger.LogDebug($"PropertyChangedEventManager | ApplicationConfig_LocalDataPropertyChanged handler re-attached.");
            Logger.LogDebug($"Handled.");
        }

        #endregion Methods
    }
}
