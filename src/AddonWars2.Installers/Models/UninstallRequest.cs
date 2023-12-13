// ==================================================================================================
// <copyright file="UninstallRequest.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers.Models
{
    /// <summary>
    /// Represents a wrapped request for addon uninstallation.
    /// </summary>
    public class UninstallRequest
    {
        #region Fields

        private readonly List<UninstallRequestFile> _filesToUninstall;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UninstallRequest"/> class.
        /// </summary>
        public UninstallRequest()
            : this(new List<UninstallRequestFile>())
        {
            // Blank.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UninstallRequest"/> class.
        /// </summary>
        /// <param name="filesToUninstall">A collection of installed files.</param>
        public UninstallRequest(List<UninstallRequestFile> filesToUninstall)
        {
            _filesToUninstall = filesToUninstall;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a list of files to uninstall.
        /// </summary>
        public List<UninstallRequestFile> FilesToUninstall => _filesToUninstall;

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}
