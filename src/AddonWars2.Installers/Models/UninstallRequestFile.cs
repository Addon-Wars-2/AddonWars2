// ==================================================================================================
// <copyright file="UninstallRequestFile.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers.Models
{
    /// <summary>
    /// Represents an object that encapsulates data for a file which was requested for uninstallation.
    /// </summary>
    public readonly struct UninstallRequestFile
    {
        #region Fields

        private readonly string _filepath;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UninstallRequestFile"/> struct.
        /// </summary>
        /// <param name="filepath">The installed file full path.</param>
        /// v<exception cref="ArgumentNullException">If <paramref name="filepath"/> is <see langword="null"/>.</exception>
        public UninstallRequestFile(string filepath)
        {
            ArgumentException.ThrowIfNullOrEmpty(filepath, nameof(filepath));

            _filepath = filepath;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the file path of the item to uninstall.
        /// </summary>
        public string FilePath => _filepath;

        #endregion Properties
    }
}
