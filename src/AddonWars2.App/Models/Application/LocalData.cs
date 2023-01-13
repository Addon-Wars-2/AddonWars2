// ==================================================================================================
// <copyright file="LocalData.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Models.Application
{
    using System.Xml.Serialization;
    using CommunityToolkit.Mvvm.ComponentModel;

    /// <summary>
    /// Prepresents data that is stored locally.
    /// </summary>
    [XmlRoot("LocalData")]
    public class LocalData : ObservableObject
    {
        #region Fields

        private string _selectedCultureString;
        private string _anetHome;
        private string _gw2Home;
        private string _gw2WikiHome;
        private string _gw2Rss;
        private string _gw2Api2;
        private string _gw2FilePath;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalData"/> class.
        /// </summary>
        public LocalData()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a default instance version of <see cref="LocalData"/>.
        /// </summary>
        /// <remarks>
        /// The default version of <see cref="LocalData"/> will contain
        /// pre-defined properties hardcoded internally.
        /// </remarks>
        [XmlIgnore]
        public static LocalData Default
        {
            get
            {
                var obj = new LocalData();
                obj.SetDefaultValues();
                return obj;
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
        /// Gets or sets the name of GW2 executable.
        /// </summary>
        [XmlElement("Gw2FilePath")]
        public string Gw2FilePath
        {
            get => _gw2FilePath;
            set => SetProperty(ref _gw2FilePath, value);
        }

        /// <summary>
        /// Gets a template string that contanins a single format item that can be
        /// substituted with a culture string in a short format, i.e. "en", "de", etc.
        /// </summary>
        [XmlIgnore]
        internal string Gw2HomeTemplate => LocalDataDefaultState.Gw2HomeTemplate;

        /// <summary>
        /// Gets a template string that contanins a single format item that can be
        /// substituted with a culture string in a short format, i.e. "en", "de", etc.
        /// </summary>
        [XmlIgnore]
        internal string Gw2RssTemplate => LocalDataDefaultState.Gw2RssTemplate;

        /// <summary>
        /// Gets a template string that contanins a single format item that can be
        /// substituted with a culture string in a short format, i.e. "en", "de", etc.
        /// </summary>
        [XmlIgnore]
        internal string Gw2WikiHomeTemplate => LocalDataDefaultState.Gw2WikiHomeTemplate;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Determines if the current state is valid.
        /// </summary>
        /// <remarks>
        /// This methods should be used after deserialization to ensure the input file contained
        /// the required and valid data.
        /// </remarks>
        /// <param name="obj"><see cref="LocalData"/> reference to check for validity.</param>
        /// <returns><see langword="true"/> if valid, otherwise <see langword="false"/>.</returns>
        public static bool IsValid(LocalData obj)
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

        // Sets default values.
        private void SetDefaultValues()
        {
            SelectedCultureString = LocalDataDefaultState.SelectedCultureString;
            AnetHome = LocalDataDefaultState.AnetHome;
            Gw2Home = string.Format(Gw2HomeTemplate, "en");
            Gw2WikiHome = LocalDataDefaultState.Gw2WikiHome;
            Gw2Rss = string.Format(Gw2RssTemplate, "en");
            Gw2Api2 = LocalDataDefaultState.Gw2Api2;
            Gw2FilePath = LocalDataDefaultState.Gw2FilePath;
        }

        #endregion Methods

        #region Inner Classes

        // Encapsulates the default state of the class.
        private static class LocalDataDefaultState
        {
            internal static string Gw2HomeTemplate => "https://guildwars2.com/{0}";

            internal static string Gw2RssTemplate => "https://www.guildwars2.com/{0}/feed";

            internal static string Gw2WikiHomeTemplate => "https://wiki-{0}.guildwars2.com";

            internal static string SelectedCultureString => "en-US";

            internal static string AnetHome => "https://arena.net";

            internal static string Gw2Home => string.Format(Gw2HomeTemplate, "en");

            internal static string Gw2WikiHome => string.Format(Gw2WikiHomeTemplate, "en");

            internal static string Gw2Rss => string.Format(Gw2RssTemplate, "en");

            internal static string Gw2Api2 => "https://api.guildwars2.com/v2";

            internal static string Gw2FilePath => string.Empty;
        }

        #endregion Inner Classes
    }
}
