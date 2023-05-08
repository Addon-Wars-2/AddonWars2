// ==================================================================================================
// <copyright file="GithubAddonsInfoStorage.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons.AddonLibProvider
{
    using AddonWars2.Addons.AddonLibProvider.Interfaces;
    using AddonWars2.Addons.Models.AddonInfo;

    /// <summary>
    /// Represents a Github storage that keeps the information about addons.
    /// </summary>
    public class GithubAddonsInfoStorage : IAddonsInfoStorage
    {
        #region Methods

        /// <inheritdoc/>
        public IEnumerable<AddonInfo> GetAddonsInfo()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}
