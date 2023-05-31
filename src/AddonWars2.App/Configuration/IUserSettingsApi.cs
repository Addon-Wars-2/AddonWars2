// ==================================================================================================
// <copyright file="IUserSettingsApi.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Configuration
{
    using Config.Net;

    /// <summary>
    /// Represents a contract for user settings config (API section).
    /// </summary>
    public interface IUserSettingsApi
    {
        /// <summary>
        /// Gets or sets GitHub API token.
        /// </summary>
        [Option(Alias = "GitHubApiToken", DefaultValue = "")]
        public string GitHubApiToken { get; set; }
    }
}
