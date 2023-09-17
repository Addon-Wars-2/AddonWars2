// ==================================================================================================
// <copyright file="InstallResult.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers.Models
{
    using AddonWars2.Installers.Enums;

    /// <summary>
    /// Represents a result of an installation operation.
    /// </summary>
    public class InstallResult
    {
        #region Fields

        private readonly InstallStatus _status;

        #endregion Fields

        #region Constructors

        public InstallResult()
        {

        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets an installation status.
        /// </summary>
        public InstallStatus Status => _status;

        #endregion Properties
    }
}
