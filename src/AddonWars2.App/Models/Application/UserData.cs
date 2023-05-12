// ==================================================================================================
// <copyright file="UserData.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Models.Application
{
    using System;
    using System.Xml.Serialization;
    using AddonWars2.SharedData;
    using CommunityToolkit.Mvvm.ComponentModel;

    /// <summary>
    /// Prepresents user data that is stored locally.
    /// </summary>
    public class UserData : ObservableObject
    {
        #region Fields

        private static readonly Lazy<UserDataDefaultState> _defaultStateLazy = new Lazy<UserDataDefaultState>(() => InitializeDefaultStateLazy(AppStaticData, WebStaticData));
        private static IAppStaticData? _appStaticData;
        private static IWebStaticData? _webStaticData;
        private string _selectedCultureString = string.Empty;
        private string _anetHome = string.Empty;
        private string _gw2Home = string.Empty;
        private string _gw2WikiHome = string.Empty;
        private string _gw2Rss = string.Empty;
        private string _gw2Api2 = string.Empty;
        private string _gw2FilePath = string.Empty;
        private string _gw2DirPath = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserData"/> class.
        /// </summary>
        public UserData()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a default instance version of <see cref="UserData"/>.
        /// </summary>
        /// <remarks>
        /// The default version of <see cref="UserData"/> will contain
        /// pre-defined properties hardcoded internally.
        /// </remarks>
        [XmlIgnore]
        public static UserData Default
        {
            get
            {
                // TODO: This is not the best approach, since it enforces to set defaults first.
                //       At the same time, we don't gulp the runtime error and indicate that default
                //       data must be injected (configured) before calling this property.

                if (AppStaticData == null)
                {
                    throw new InvalidOperationException($"App static data is not initialized. Use {nameof(SetDefaultConfiguration)} method to set a default configuration.");
                }

                if (WebStaticData == null)
                {
                    throw new InvalidOperationException($"Web static data is not initialized.  Use {nameof(SetDefaultConfiguration)} method to set a default configuration.");
                }

                return new UserData()
                {
                    SelectedCultureString = UserDataDefaultStateInstance.SelectedCultureString,
                    AnetHome = UserDataDefaultStateInstance.AnetHome,
                    Gw2Home = UserDataDefaultStateInstance.Gw2Home,
                    Gw2Rss = UserDataDefaultStateInstance.Gw2Rss,
                    Gw2WikiHome = UserDataDefaultStateInstance.Gw2WikiHome,
                    Gw2Api2 = UserDataDefaultStateInstance.Gw2Api2,
                    Gw2FilePath = UserDataDefaultStateInstance.Gw2FilePath,
                    Gw2DirPath = UserDataDefaultStateInstance.Gw2DirPath,
                };
            }
        }

        /// <summary>
        /// Gets or sets the selected application culture string.
        /// </summary>
        [XmlElement("SelectedCulture")]
        public string SelectedCultureString
        {
            get => _selectedCultureString;
            set => SetProperty(ref _selectedCultureString, value);
        }

        /// <summary>
        /// Gets or sets the ArenaNet homepage URL.
        /// </summary>
        [XmlElement("AnetHome")]
        public string AnetHome
        {
            get => _anetHome;
            set => SetProperty(ref _anetHome, value);
        }

        /// <summary>
        /// Gets or sets the GW2 homepage URL.
        /// </summary>
        [XmlElement("Gw2Home")]
        public string Gw2Home
        {
            get => _gw2Home;
            set => SetProperty(ref _gw2Home, value);
        }

        /// <summary>
        /// Gets or sets the GW2 wiki homepage URL.
        /// </summary>
        [XmlElement("Gw2Wiki")]
        public string Gw2WikiHome
        {
            get => _gw2WikiHome;
            set => SetProperty(ref _gw2WikiHome, value);
        }

        /// <summary>
        /// Gets or sets the GW2 RSS feed URL.
        /// </summary>
        [XmlElement("Gw2Rss")]
        public string Gw2Rss
        {
            get => _gw2Rss;
            set => SetProperty(ref _gw2Rss, value);
        }

        /// <summary>
        /// Gets or sets the GW2 APIv2 endpoint URL.
        /// </summary>
        [XmlElement("Gw2Api")]
        public string Gw2Api2
        {
            get => _gw2Api2;
            set => SetProperty(ref _gw2Api2, value);
        }

        /// <summary>
        /// Gets or sets the location of GW2 executable.
        /// </summary>
        [XmlElement("Gw2FilePath")]
        public string Gw2FilePath
        {
            get => _gw2FilePath;
            set => SetProperty(ref _gw2FilePath, value);
        }

        /// <summary>
        /// Gets or sets the location of GW2 directory.
        /// </summary>
        [XmlElement("Gw2DirPath")]
        public string Gw2DirPath
        {
            get => _gw2DirPath;
            set => SetProperty(ref _gw2DirPath, value);
        }

        /// <summary>
        /// Gets a template string that contanins a single format item that can be
        /// substituted with a culture string in a short format, i.e. "en", "de", etc.
        /// </summary>
        [XmlIgnore]
        internal static string AnetHomeTemplate => UserDataDefaultStateInstance.Gw2HomeTemplate;

        /// <summary>
        /// Gets a template string that contanins a single format item that can be
        /// substituted with a culture string in a short format, i.e. "en", "de", etc.
        /// </summary>
        [XmlIgnore]
        internal static string Gw2HomeTemplate => UserDataDefaultStateInstance.Gw2HomeTemplate;

        /// <summary>
        /// Gets a template string that contanins a single format item that can be
        /// substituted with a culture string in a short format, i.e. "en", "de", etc.
        /// </summary>
        [XmlIgnore]
        internal static string Gw2RssTemplate => UserDataDefaultStateInstance.Gw2RssTemplate;

        /// <summary>
        /// Gets a template string that contanins a single format item that can be
        /// substituted with a culture string in a short format, i.e. "en", "de", etc.
        /// </summary>
        [XmlIgnore]
        internal static string Gw2WikiHomeTemplate => UserDataDefaultStateInstance.Gw2WikiHomeTemplate;

        // Application static data.
        [XmlIgnore]
        private static IAppStaticData? AppStaticData => _appStaticData;

        // Application web-related static data.
        [XmlIgnore]
        private static IWebStaticData? WebStaticData => _webStaticData;

        // Default state.
        [XmlIgnore]
        private static UserDataDefaultState UserDataDefaultStateInstance => _defaultStateLazy.Value;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Sets the default values for all new <see cref="UserData"/> instances.
        /// </summary>
        /// <param name="appStaticData"><see cref="IAppStaticData"/> reference.</param>
        /// <param name="webStaticData"><see cref="IWebStaticData"/> reference.</param>
        public static void SetDefaultConfiguration(IAppStaticData appStaticData, IWebStaticData webStaticData)
        {
            _appStaticData = appStaticData ?? throw new ArgumentNullException(nameof(appStaticData));
            _webStaticData = webStaticData ?? throw new ArgumentNullException(nameof(webStaticData));
        }

        /// <summary>
        /// Determines if the current state is valid.
        /// </summary>
        /// <remarks>
        /// This methods should be used after deserialization to ensure the input file contains
        /// the required and valid data.
        /// </remarks>
        /// <param name="obj"><see cref="UserData"/> reference to check for validity.</param>
        /// <returns><see langword="true"/> if valid, otherwise <see langword="false"/>.</returns>
        public static bool IsValid(UserData? obj)
        {
            // TODO: Implement through attributes maybe?
            //       Otherwise eventually we'll end up with a looooong and ugly method call.

            return
                obj != null &&
                !string.IsNullOrEmpty(obj.SelectedCultureString) &&
                !string.IsNullOrEmpty(obj.AnetHome) &&
                !string.IsNullOrEmpty(obj.Gw2Home) &&
                !string.IsNullOrEmpty(obj.Gw2WikiHome) &&
                !string.IsNullOrEmpty(obj.Gw2Rss) &&
                !string.IsNullOrEmpty(obj.Gw2Api2);
        }

        // Initialized a new instance of a default state and returns a reference to it.
        private static UserDataDefaultState InitializeDefaultStateLazy(IAppStaticData appStaticData, IWebStaticData webStaticData)
        {
            return new UserDataDefaultState(appStaticData, webStaticData);
        }

        #endregion Methods

        #region Inner Classes

        // Encapsulates the default state of the class.
        private class UserDataDefaultState
        {
            #region Fields

            private readonly IAppStaticData _appStaticData;
            private readonly IWebStaticData _webStaticData;

            #endregion Fields

            #region Constructors

            internal UserDataDefaultState(IAppStaticData appStaticData, IWebStaticData webStaticData)
            {
                _appStaticData = appStaticData ?? throw new ArgumentNullException(nameof(appStaticData));
                _webStaticData = webStaticData ?? throw new ArgumentNullException(nameof(webStaticData));
            }

            #endregion Constructors

            #region Properties

            internal string AnetHomeTemplate => _webStaticData.AnetHomeTemplate;

            internal string Gw2HomeTemplate => _webStaticData.Gw2HomeTemplate;

            internal string Gw2RssTemplate => _webStaticData.Gw2RssHomeTemplate;

            internal string Gw2WikiHomeTemplate => _webStaticData.Gw2WikiHomeTemplate;

            internal string SelectedCultureString => _appStaticData.DefaultCulture.Culture;

            internal string AnetHome => string.Format(AnetHomeTemplate, _appStaticData.DefaultCulture.ShortName.ToLower());

            internal string Gw2Home => string.Format(Gw2HomeTemplate, _appStaticData.DefaultCulture.ShortName.ToLower());

            internal string Gw2WikiHome => string.Format(Gw2WikiHomeTemplate, _appStaticData.DefaultCulture.ShortName.ToLower());

            internal string Gw2Rss => string.Format(Gw2RssTemplate, _appStaticData.DefaultCulture.ShortName.ToLower());

            internal string Gw2Api2 => _webStaticData.Gw2ApiV2Endpoint;

            internal string Gw2FilePath => string.Empty;

            internal string Gw2DirPath => string.Empty;

            #endregion Properties
        }

        #endregion Inner Classes
    }
}
