// ==================================================================================================
// <copyright file="IGameSharedData.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.SharedData
{
    /// <summary>
    /// Represents a contract for game-related static data.
    /// </summary>
    public interface IGameSharedData
    {
        #region Fields and Constants

        private const string _gw2ExecutableExtension = ".exe";
        private const string _gw2ProductName = "Guild Wars 2";
        private const string _gw2FileDescription = "Guild Wars 2 Game Client";

        #endregion Fields and Constants.

        #region Properties

        /// <summary>
        /// Gets the extension of GW2 executable.
        /// </summary>
        public string Gw2ExecutableExtension => _gw2ExecutableExtension;

        /// <summary>
        /// Gets the product name of GW2 executable.
        /// </summary>
        public string Gw2ProductName => _gw2ProductName;

        /// <summary>
        /// Gets the file description of GW2 executable.
        /// </summary>
        public string Gw2FileDescription => _gw2FileDescription;

        #endregion Properties
    }
}
