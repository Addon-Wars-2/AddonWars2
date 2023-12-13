// ==================================================================================================
// <copyright file="InstallRequest.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Installers.Models
{
    using AddonWars2.Extractors.Models;

    /// <summary>
    /// Represents a wrapped request for addon installation.
    /// </summary>
    public class InstallRequest
    {
        #region Fields

        private readonly string _entrypoint;
        private readonly List<InstallRequestFile> _installItems;
        private readonly InstallInstructions _instructions;
        private readonly double _requiredDiskSpace = 0d;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallRequest"/> class.
        /// </summary>
        /// <param name="root">The installation root path.</param>
        /// <param name="extractionResult">The extraction result the request will be created from.</param>
        public InstallRequest(string root, ExtractionResult extractionResult)
            : this(root, extractionResult, new InstallInstructions())
        {
            // Blank.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstallRequest"/> class.
        /// </summary>
        /// <param name="entrypoint">The installation entypoint path.</param>
        /// <param name="extractionResult">The extraction result the request will be created from.</param>
        /// <param name="installInstructions">The addon installation instruction.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="entrypoint"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="extractionResult"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="installInstructions"/> is <see langword="null"/>.</exception>
        public InstallRequest(string entrypoint, ExtractionResult extractionResult, InstallInstructions installInstructions)
        {
            ArgumentException.ThrowIfNullOrEmpty(entrypoint, nameof(entrypoint));
            ArgumentNullException.ThrowIfNull(extractionResult, nameof(extractionResult));
            ArgumentNullException.ThrowIfNull(installInstructions, nameof(installInstructions));

            _entrypoint = entrypoint;
            _installItems = new List<InstallRequestFile>();
            foreach (var item in extractionResult.ExtractedFiles)
            {
                _installItems.Add(new InstallRequestFile(item.Name, item.Content, item.RelativePath));
            }

            _instructions = installInstructions ?? throw new ArgumentNullException(nameof(installInstructions));
            _requiredDiskSpace = extractionResult.TotalSize;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the entrypoint path for the installation.
        /// </summary>
        /// <remarks>
        /// Each <see cref="InstallRequestFile"/> has <see cref="InstallRequestFile.RelativePath"/> property,
        /// which should be combined with the entrypoint installation path to generate a complete installation path.
        /// </remarks>
        public string Entrypoint => _entrypoint;

        /// <summary>
        /// Gets a list of items to install.
        /// </summary>
        public List<InstallRequestFile> InstallItems => _installItems;

        /// <summary>
        /// Gets the addon installation instructions.
        /// </summary>
        public InstallInstructions Instructions => _instructions;

        /// <summary>
        /// Gets the required disk space to install the files contained in <see cref="InstallItems"/>.
        /// </summary>
        public double RequiredDiskSpace => _requiredDiskSpace;

        #endregion Properties
    }
}
