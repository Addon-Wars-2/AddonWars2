// ==================================================================================================
// <copyright file="UninstallResult.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers.Models
{
    using AddonWars2.Installers.Enums;

    /// <summary>
    /// Represents a result of an uninstallation operation.
    /// </summary>
    public class UninstallResult
    {
        #region Fields

        private readonly UninstallResultStatus _status;

        #endregion Fields

        #region Constructors

        public UninstallResult()
        {

        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets an uninstallation status.
        /// </summary>
        public UninstallResultStatus Status => _status;

        #endregion Properties
    }
}
