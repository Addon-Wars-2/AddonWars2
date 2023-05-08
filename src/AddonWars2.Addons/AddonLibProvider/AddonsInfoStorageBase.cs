// ==================================================================================================
// <copyright file="AddonsInfoStorageBase.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons.AddonLibProvider
{
    using AddonWars2.Addons.AddonLibProvider.Interfaces;
    using AddonWars2.Addons.Models.AddonInfo;
    using Octokit;

    /// <summary>
    /// Represents a base class for addons storage.
    /// </summary>
    public abstract class AddonsInfoStorageBase : IAddonsInfoStorage
    {
        #region Fields
        private readonly GitHubClient _gitHubClient;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonsInfoStorageBase"/> class.
        /// </summary>
        /// <param name="gitHubClient">A reference to <see cref="GitHubClient"/> instance.</param>
        public AddonsInfoStorageBase(GitHubClient gitHubClient)
        {
            _gitHubClient = gitHubClient;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets GitHub client.
        /// </summary>
        protected GitHubClient GitHubClient => _gitHubClient;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public abstract IEnumerable<AddonInfo> GetAddonsInfo();

        #endregion Methods
    }
}
