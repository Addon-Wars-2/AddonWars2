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
    using System.Xml.Serialization;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Holds user settings used by the application.
    /// </summary>
    [Serializable]
    [XmlRoot("ApplicationConfig")]
    public class ApplicationConfig
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationConfig"/> class.
        /// </summary>
        public ApplicationConfig()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a default instance version of <see cref="ApplicationException"/>.
        /// </summary>
        [XmlIgnore]
        public static ApplicationConfig Default
        {
            get
            {
                var cfg = Services.GetRequiredService<ApplicationConfig>();
                cfg.SetDefaultValues();
                return cfg;
            }
        }

        /// <summary>
        /// Gets or sets a list of available cultures.
        /// </summary>
        [XmlArray("AvailableCultures")]
        public ObservableCollection<CultureInfo> AvailableCultures { get; set; }

        /// <summary>
        /// Gets or sets the default application culture.
        /// </summary>
        [XmlElement("DefaultCulture")]
        public CultureInfo DefaultCulture { get; set; }

        /// <summary>
        /// Gets or sets the selected application culture.
        /// </summary>
        [XmlElement("SelectedCulture")]
        public CultureInfo SelectedCulture { get; set; }

        /// <summary>
        /// Gets or sets <see cref="IServiceProvider"/> provider reference.
        /// </summary>
        [XmlIgnore]
        internal static IServiceProvider Services { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Determines if the current config is fully set and valid.
        /// </summary>
        /// <param name="cfg"><see cref="ApplicationConfig"/> reference to check for validity.</param>
        /// <returns><see langword="true"/> if valid, otherwise <see langword="false"/>.</returns>
        public static bool IsValid(ApplicationConfig cfg)
        {
            return
                cfg != null &&
                cfg.AvailableCultures != null &&
                cfg.AvailableCultures.Count > 0 &&
                cfg.SelectedCulture != null &&
                cfg.DefaultCulture != null;
        }

        // Serializer calls default ctor, so we want to avoid setting anything in there
        // to prevent setting data incorrectly.
        private void SetDefaultValues()
        {
            AvailableCultures = ApplicationConfigDefaultState.AvailableCultures;
            DefaultCulture = ApplicationConfigDefaultState.DefaultCulture;
            SelectedCulture = ApplicationConfigDefaultState.SelectedCulture;
        }

        #endregion Methods

        #region Inner Classes

        // Encapsulates the default state of the class. It simply maps the main class.
        private static class ApplicationConfigDefaultState
        {
            internal static ObservableCollection<CultureInfo> AvailableCultures { get; set; } = new ObservableCollection<CultureInfo>()
            {
                new CultureInfo("en-US", "EN", "English"),
                new CultureInfo("ru-RU", "RU", "Russian"),
            };

            internal static CultureInfo DefaultCulture { get; set; } = new CultureInfo("en-US", "EN", "English");

            internal static CultureInfo SelectedCulture { get; set; } = new CultureInfo("en-US", "EN", "English");
        }

        #endregion Inner Classes
    }
}
