// ==================================================================================================
// <copyright file="ApplicationConfig.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Models.Application
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using AddonWars2.Services.XmlSerializerService.Interfaces;
    using AddonWars2.SharedData;
    using AddonWars2.SharedData.Entities;
    using CommunityToolkit.Mvvm.ComponentModel;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents the application configuration and holds the local data.
    /// </summary>
    [XmlRoot("UserData")]
    public class ApplicationConfig : ObservableObject
    {
        #region Fields

        private static ILogger _logger;

        private readonly IAppStaticData _appStaticData;
        private readonly IWebStaticData _webStaticData;
        private readonly IXmlSerializationService _xmlSerializationService;

        private bool _isDebugMode;
        private DateTime _startupDateTime;
        private string _appDataDir = string.Empty;
        private string _logFilePath = string.Empty;
        private CultureInfo _selectedCulture;
        private UserData _userData;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationConfig"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/> instance.</param>
        /// <param name="appStaticData">A reference to <see cref="IAppStaticData"/>.</param>
        /// <param name="webStaticData">A reference to <see cref="IWebStaticData"/> instance.</param>
        /// <param name="xmlSerializationService">A reference to <see cref="IXmlSerializationService"/> instance.</param>
        public ApplicationConfig(
            ILogger<ApplicationConfig> logger,
            IAppStaticData appStaticData,
            IWebStaticData webStaticData,
            IXmlSerializationService xmlSerializationService)
        {
            _logger = logger;
            _appStaticData = appStaticData ?? throw new ArgumentNullException(nameof(appStaticData));
            _webStaticData = webStaticData ?? throw new ArgumentNullException(nameof(webStaticData));
            _xmlSerializationService = xmlSerializationService;

            _selectedCulture = AppStaticData.DefaultCulture;

            UserData.SetDefaultConfiguration(AppStaticData, WebStaticData);  // IMPORTANT! Configure default data first. Should be done once.
            _userData = UserData.Default;

            // To track culture change.
            PropertyChangedEventManager.AddHandler(this, ApplicationConfig_UserDataChanged, nameof(UserData));
            PropertyChangedEventManager.AddHandler(UserData, ApplicationConfig_UserDataPropertyChanged, string.Empty);
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
        /// Gets or sets the full log file path.
        /// </summary>
        public string LogFilePath
        {
            get => _logFilePath;
            set
            {
                SetProperty(ref _logFilePath, value);
                Logger.LogDebug($"Property set: {value}");
            }
        }

        /// <summary>
        /// Gets the application config file path.
        /// </summary>
        public string ConfigFilePath => Path.Join(AppDataDir, AppStaticData.ConfigFileName);

        /// <summary>
        /// Gets or sets the selected application culture.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Changes in this property will internally invoke a handling method, which in turn will update <see cref="UserData"/> properties.
        /// Any changes inside the <see cref="UserData"/> will be automatically handled by another handling method and force the config file
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
        /// Changes in <see cref="UserData"/> inner properties internally invoke a handling method,
        /// which in turn will write <see cref="UserData"/> to a local storage. No explicit save required.
        /// </para>
        /// <para>
        /// Handlers are re-attached automatically to ensure they keep a reference to the current property instance.
        /// </para>
        /// </remarks>
        [XmlElement("UserData")]
        public UserData UserData
        {
            get => _userData;
            set
            {
                ArgumentNullException.ThrowIfNull(value, nameof(value));

                WriteLocalDataAsXml(ConfigFilePath, UserData);

                SetProperty(ref _userData, value);
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

        /// <summary>
        /// Gets the application static data.
        /// </summary>
        protected IAppStaticData AppStaticData => _appStaticData;

        /// <summary>
        /// Gets a reference to the application web-related static data.
        /// </summary>
        protected IWebStaticData WebStaticData => _webStaticData;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Serializes a given <see cref="UserData"/> object and writes it using <paramref name="path"/> value.
        /// </summary>
        /// <param name="path">A path data is written to.</param>
        /// <param name="data">Data to be written.</param>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="path"/> is <see langword="null"/> or empty.</exception>
        public void WriteLocalDataAsXml(string path, UserData? data)
        {
            ArgumentNullException.ThrowIfNull(path, nameof(path));

            _ = XmlSerializationService.SerializeXml(data, path);  // TODO: check the returned result

            Logger.LogDebug($"Local data saved.");
        }

        /// <summary>
        /// Deserializes a given XML file and returns it as a <see cref="UserData"/> object.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <returns><see cref="UserData"/> object.</returns>
        public UserData? LoadLocalDataFromXml(string path)
        {
            var data = XmlSerializationService.DeserializeXml<UserData>(path);

            Logger.LogDebug($"Local data loaded.");

            return data;
        }

        // Is invoked whenever the selected culture property is changed.
        private void ApplicationConfig_SelectedCultureChanged(object? sender, PropertyChangedEventArgs? e)
        {
            ArgumentNullException.ThrowIfNull(sender, nameof(sender));
            ArgumentNullException.ThrowIfNull(e, nameof(e));

            // Set the requested culture string inside the local data.
            var culture = AppStaticData.AppSupportedCultures.FirstOrDefault(x => x.Culture == SelectedCulture.Culture, AppStaticData.DefaultCulture);
            UserData.SelectedCultureString = culture.Culture;

            Logger.LogInformation($"Selected culture was changed to: {culture.Culture}");

            // ANet webside supports only several languages. If the requested culture is not supported by ANet,
            // then use global (en) version of ANet services.
            if (!AppStaticData.AnetSupportedCultures.Any(x => x.Culture == culture.Culture))
            {
                culture = AppStaticData.DefaultCulture;
                Logger.LogWarning($"The requested culture is not supported by ANet services. The default one ({culture.Culture}) will be set for them.");
            }

            UserData.Gw2Home = string.Format(UserData.Gw2HomeTemplate, culture.ShortName.ToLower());
            UserData.Gw2Rss = string.Format(UserData.Gw2RssTemplate, culture.ShortName.ToLower());
            UserData.Gw2WikiHome = string.Format(UserData.Gw2WikiHomeTemplate, culture.ShortName.ToLower());

            // Re-attach handlers to keep a reference to the current property instance.
            PropertyChangedEventManager.RemoveHandler(this, ApplicationConfig_SelectedCultureChanged, nameof(SelectedCulture));
            PropertyChangedEventManager.AddHandler(this, ApplicationConfig_SelectedCultureChanged, nameof(SelectedCulture));

            Logger.LogDebug($"PropertyChangedEventManager | ApplicationConfig_SelectedCultureChanged handler re-attached.");
            Logger.LogDebug($"Handled.");
        }

        // Is invoked whenever the local data property is changed.
        private void ApplicationConfig_UserDataPropertyChanged(object? sender, PropertyChangedEventArgs? e)
        {
            ArgumentNullException.ThrowIfNull(sender, nameof(sender));
            ArgumentNullException.ThrowIfNull(e, nameof(e));

            WriteLocalDataAsXml(ConfigFilePath, UserData);

            Logger.LogDebug($"Handled.");
        }

        // Is invoked whenever a local data inner property is changed.
        private void ApplicationConfig_UserDataChanged(object? sender, PropertyChangedEventArgs? e)
        {
            ArgumentNullException.ThrowIfNull(sender, nameof(sender));
            ArgumentNullException.ThrowIfNull(e, nameof(e));

            if (UserData == null)
            {
                throw new NullReferenceException(nameof(UserData));
            }

            WriteLocalDataAsXml(ConfigFilePath, UserData);

            PropertyChangedEventManager.RemoveHandler(UserData, ApplicationConfig_UserDataPropertyChanged, string.Empty);
            PropertyChangedEventManager.AddHandler(UserData, ApplicationConfig_UserDataPropertyChanged, string.Empty);

            Logger.LogDebug($"PropertyChangedEventManager | ApplicationConfig_UserDataPropertyChanged handler re-attached.");
            Logger.LogDebug($"Handled.");
        }

        #endregion Methods
    }
}
