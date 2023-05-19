// ==================================================================================================
// <copyright file="IUserSettings.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Models.Configuration
{
    using System.ComponentModel;
    using Config.Net;

    /// <summary>
    /// Represents a contract for user settings config.
    /// </summary>
    public interface IUserSettings : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets user settings from General section.
        /// </summary>
        [Option(Alias = "General")]
        IUserSettingsGeneral UserSettingsGeneral { get; set; }

        /// <summary>
        /// Gets or sets user settings from API section.
        /// </summary>
        [Option(Alias = "API")]
        IUserSettingsApi UserSettingsApi { get; set; }
    }
}
