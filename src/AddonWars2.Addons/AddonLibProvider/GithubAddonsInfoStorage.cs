﻿// ==================================================================================================
// <copyright file="GithubAddonsInfoStorage.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons.AddonLibProvider
{
    using AddonWars2.Addons.Models.AddonInfo;
    using Octokit;

    /// <summary>
    /// Represents a Github storage that keeps the information about addons.
    /// </summary>
    public class GithubAddonsInfoStorage : AddonsInfoStorageBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GithubAddonsInfoStorage"/> class.
        /// </summary>
        /// <param name="gitHubClient">A reference to <see cref="GitHubClient"/> instance.</param>
        public GithubAddonsInfoStorage(GitHubClient gitHubClient)
            : base(gitHubClient)
        {
            // Blank.
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc/>
        public override IEnumerable<AddonInfo> GetAddonsInfo()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}
