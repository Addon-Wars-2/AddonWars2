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
    /// <remarks>
    /// The main purpose if <see cref="InstallResult"/> is to log the installation process
    /// and provide necessary information for cleanup if the installation was interrupted.
    /// </remarks>
    public class InstallResult
    {
        #region Fields

        private readonly InstallResultStatus _status;
        private readonly List<InstallResultFile> _installedFiles;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallResult"/> class.
        /// </summary>
        /// <param name="status">Installation result status.</param>
        public InstallResult(InstallResultStatus status)
            : this(new List<InstallResultFile>(), status)
        {
            // Blank.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallResult"/> class.
        /// </summary>
        /// <param name="installedFiles">A collection of installed files.</param>
        public InstallResult(List<InstallResultFile> installedFiles)
            : this(installedFiles, InstallResultStatus.Success)
        {
            // Blank.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallResult"/> class.
        /// </summary>
        /// <param name="installedFiles">A collection of installed files.</param>
        /// <param name="status">Installation result status.</param>
        public InstallResult(List<InstallResultFile> installedFiles, InstallResultStatus status)
        {
            _installedFiles = installedFiles;
            _status = status;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets an installation status.
        /// </summary>
        public InstallResultStatus Status => _status;

        /// <summary>
        /// Gets a list of installed files.
        /// </summary>
        public List<InstallResultFile> InstalledFiles => _installedFiles;

        #endregion Properties
    }
}
