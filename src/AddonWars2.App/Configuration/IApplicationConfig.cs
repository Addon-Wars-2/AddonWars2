// ==================================================================================================
// <copyright file="IApplicationConfig.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Configuration
{
    using System.ComponentModel;

    /// <summary>
    /// Represents a contract for application config instance.
    /// </summary>
    public interface IApplicationConfig : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the recent session data.
        /// </summary>
        ISessionData SessionData { get; set; }

        /// <summary>
        /// Gets or sets user data.
        /// </summary>
        IUserData UserData { get; set; }

        /// <summary>
        /// Gets or sets user settings.
        /// </summary>
        IUserSettings UserSettings { get; set; }
    }
}
