// ==================================================================================================
// <copyright file="Enums.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers.Enums
{
    /// <summary>
    /// Specifies various resulting statuses for an installation operation.
    /// </summary>
    public enum InstallResultStatus
    {
        /// <summary>
        /// The installation is successfull.
        /// </summary>
        Success,

        /// <summary>
        /// The installation has failed.
        /// </summary>
        Failed,

        /// <summary>
        /// The installation was aborted.
        /// </summary>
        Aborted,
    }

    /// <summary>
    /// Specifies various resulting statuses for an uninstallation operation.
    /// </summary>
    public enum UninstallResultStatus
    {
        /// <summary>
        /// The uninstallation is successfull.
        /// </summary>
        Success,

        /// <summary>
        /// The uninstallation has failed.
        /// </summary>
        Failed,

        /// <summary>
        /// The uninstallation was aborted.
        /// </summary>
        Aborted,
    }
}
