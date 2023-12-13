// ==================================================================================================
// <copyright file="IGameSharedData.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.SharedData.Interfaces
{
    /// <summary>
    /// Represents a contract for game-related static data.
    /// </summary>
    public interface IGameSharedData
    {
        #region Fields and Constants

        private const string GW2_EXEC_EXTENSION_NAME = ".exe";
        private const string GW2_EXEC_PROD_NAME = "Guild Wars 2";
        private const string GW2_EXEC_DESC = "Guild Wars 2 Game Client";
        private const string GW2_REG_KEY = @"Software\Microsoft\Windows\CurrentVersion\Uninstall\Guild Wars 2";
        private const string GW2_BIN_DIR = @"\bin64";
        private const string GW2_CEF_DIR = @"\bin64\cef";
        private const string GW2_ADDONS_DIR = @"\addons";
        private const string GW2_ARCDPS_DIR = @"\addons\arcdps";

        #endregion Fields and Constants.

        #region Properties

        /// <summary>
        /// Gets the extension of GW2 executable.
        /// </summary>
        public string Gw2ExecutableExtension => GW2_EXEC_EXTENSION_NAME;

        /// <summary>
        /// Gets the product name of GW2 executable.
        /// </summary>
        public string Gw2ProductName => GW2_EXEC_PROD_NAME;

        /// <summary>
        /// Gets the file description of GW2 executable.
        /// </summary>
        public string Gw2FileDescription => GW2_EXEC_DESC;

        /// <summary>
        /// Gets the GW2 executable registry dir path.
        /// </summary>
        public string Gw2RegistryKey => GW2_REG_KEY;

        /// <summary>
        /// Gets the GW2 bin directory relative path.
        /// </summary>
        public string Gw2BinDir => GW2_BIN_DIR;

        /// <summary>
        /// Gets the GW2 cef directory relative path.
        /// </summary>
        public string Gw2CefDir => GW2_CEF_DIR;

        /// <summary>
        /// Gets the GW2 addons directory relative path.
        /// </summary>
        public string Gw2AddonsDir => GW2_ADDONS_DIR;

        /// <summary>
        /// Gets the GW2 arcdps directory relative path.
        /// </summary>
        public string Gw2ArcdpsDir => GW2_ARCDPS_DIR;

        #endregion Properties
    }
}
