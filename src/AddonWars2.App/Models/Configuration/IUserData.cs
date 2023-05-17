// ==================================================================================================
// <copyright file="IUserData.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Models.Configuration
{
    using System.ComponentModel;
    using Config.Net;

    /// <summary>
    /// Reprsents a contract for user data config.
    /// </summary>
    public interface IUserData : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets a selected culture as a string.
        /// </summary>
        [Option(Alias = "SelectedCulture", DefaultValue = "")]
        public string SelectedCultureString { get; set; }

        /// <summary>
        /// Gets or sets ANet website URL.
        /// </summary>
        [Option(Alias = "AnetHome", DefaultValue = "")]
        public string AnetHome { get; set; }

        /// <summary>
        /// Gets or sets GW2 website URL.
        /// </summary>
        [Option(Alias = "Gw2Home", DefaultValue = "")]
        public string Gw2Home { get; set; }

        /// <summary>
        /// Gets or sets GW2 wiki homepage URL.
        /// </summary>
        [Option(Alias = "Gw2WikiHome", DefaultValue = "")]
        public string Gw2WikiHome { get; set; }

        /// <summary>
        /// Gets or sets GW2 RSS feed URL.
        /// </summary>
        [Option(Alias = "Gw2Rss", DefaultValue = "")]
        public string Gw2Rss { get; set; }

        /// <summary>
        /// Gets or sets GW2 filepath.
        /// </summary>
        [Option(Alias = "Gw2FilePath", DefaultValue = "")]
        public string Gw2FilePath { get; set; }

        /// <summary>
        /// Gets or sets GW2 directory path.
        /// </summary>
        [Option(Alias = "Gw2DirPath", DefaultValue = "")]
        public string Gw2DirPath { get; set; }
    }
}
