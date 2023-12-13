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
        private readonly List<UninstallResultFile> _uninstalledFiles;
        private readonly List<UninstallResultFile> _failedToUninstallFiles;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UninstallResult"/> class.
        /// </summary>
        /// <param name="status">Uninstallation result status.</param>
        public UninstallResult(UninstallResultStatus status)
            : this(new List<UninstallResultFile>(), new List<UninstallResultFile>(), status)
        {
            // Blank.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UninstallResult"/> class.
        /// </summary>
        /// <param name="uninstalledFiles">A collection of uninstalled files.</param>
        /// <param name="failedToUninstallFiles">A collection of files failed to delete.</param>
        public UninstallResult(List<UninstallResultFile> uninstalledFiles, List<UninstallResultFile> failedToUninstallFiles)
            : this(uninstalledFiles, failedToUninstallFiles, UninstallResultStatus.Success)
        {
            // Blank.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UninstallResult"/> class.
        /// </summary>
        /// <param name="uninstalledFiles">A collection of uninstalled files.</param>
        /// <param name="failedToUninstallFiles">A collection of files failed to delete.</param>
        /// <param name="status">Uninstallation result status.</param>
        public UninstallResult(List<UninstallResultFile> uninstalledFiles, List<UninstallResultFile> failedToUninstallFiles, UninstallResultStatus status)
        {
            _uninstalledFiles = uninstalledFiles;
            _failedToUninstallFiles = failedToUninstallFiles;
            _status = status;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets an uninstallation status.
        /// </summary>
        public UninstallResultStatus Status => _status;

        /// <summary>
        /// Gets a list of uninstalled files.
        /// </summary>
        public List<UninstallResultFile> UninstalledFiles => _uninstalledFiles;

        /// <summary>
        /// Gets a list of files which were failed to uninstall.
        /// </summary>
        public List<UninstallResultFile> FailedToUninstallFiles => _failedToUninstallFiles;

        #endregion Properties
    }
}
