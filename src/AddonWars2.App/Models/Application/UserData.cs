﻿// ==================================================================================================
// <copyright file="UserData.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Models.Application
{
    using System;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;
    using AddonWars2.App.Models.GuildWars2;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Holds user settings used by the application.
    /// </summary>
    [Serializable]
    [XmlRoot("userdata")]
    public class UserData
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserData"/> class.
        /// </summary>
        public UserData()
        {
            // Blank.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserData"/> class.
        /// </summary>
        /// <param name="serviceProvider">A reference to a <see cref="IServiceProvider"/> instance.</param>
        public UserData(IServiceProvider serviceProvider)
        {
            Services = serviceProvider;
            SetDefaultValues();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a default instance version of <see cref="ApplicationException"/>.
        /// </summary>
        [XmlIgnore]
        public static UserData Default
        {
            get
            {
                var cfg = Services.GetRequiredService<UserData>();
                cfg.SetDefaultValues();
                return cfg;
            }
        }

        /// <summary>
        /// Gets or sets a list of available cultures.
        /// </summary>
        [XmlArray("cultures")]
        public ObservableCollection<CultureInfo> AvailableCultures { get; set; }

        /// <summary>
        /// Gets or sets the default application culture.
        /// </summary>
        [XmlElement("default")]
        public CultureInfo DefaultCulture { get; set; }

        /// <summary>
        /// Gets or sets the selected application culture.
        /// </summary>
        [XmlElement("selected")]
        public CultureInfo SelectedCulture { get; set; }

        /// <summary>
        /// Gets or sets the information about GW2 executable.
        /// </summary>
        [XmlElement("gw2execinfo")]
        public Gw2ExecSignature Gw2ExecInfo { get; set; } = new Gw2ExecSignature();

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
        /// <param name="cfg"><see cref="UserData"/> reference to check for validity.</param>
        /// <returns><see langword="true"/> if valid, otherwise <see langword="false"/>.</returns>
        public static bool IsValid(UserData cfg)
        {
            // TODO: Implement through attributes maybe?
            //       Otherwise eventually we'll end up with a looooong and ugly method call.

            return
                cfg != null &&
                cfg.AvailableCultures != null &&
                cfg.AvailableCultures.Count > 0 &&
                cfg.SelectedCulture != null &&
                cfg.DefaultCulture != null &&
                cfg.Gw2ExecInfo != null;
        }

        // Serializer calls default ctor, so we want to avoid setting anything in there.
        private void SetDefaultValues()
        {
            AvailableCultures = ApplicationConfigDefaultState.AvailableCultures;
            DefaultCulture = ApplicationConfigDefaultState.DefaultCulture;
            SelectedCulture = ApplicationConfigDefaultState.SelectedCulture;
            Gw2ExecInfo = ApplicationConfigDefaultState.GW2ExecInfo;
        }

        #endregion Methods

        #region Inner Classes

        // Encapsulates the default state of the class. It simply maps the main class.
        private static class ApplicationConfigDefaultState
        {
            internal static ObservableCollection<CultureInfo> AvailableCultures => new ObservableCollection<CultureInfo>()
            {
                new CultureInfo("en-US", "EN", "English"),
                new CultureInfo("ru-RU", "RU", "Russian"),
            };

            internal static CultureInfo DefaultCulture => new CultureInfo("en-US", "EN", "English");

            internal static CultureInfo SelectedCulture => new CultureInfo("en-US", "EN", "English");

            internal static Gw2ExecSignature GW2ExecInfo => new Gw2ExecSignature();
        }

        #endregion Inner Classes
    }
}
