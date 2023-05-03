// ==================================================================================================
// <copyright file="DownloadedStandaloneAddon.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons.Downloader.Models
{
    using AddonWars2.Addons.Downloader.Interfaces;

    /// <summary>
    /// Represents a downloaded addon obtained as a standalone source.
    /// </summary>
    public class DownloadedStandaloneAddon : IDownloadedAddon
    {
        #region Fields

        private readonly byte[] _content;
        private Status _status = Status.None;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadedStandaloneAddon"/> class.
        /// </summary>
        /// <param name="content">The downloaded content represented as a byte array.</param>
        public DownloadedStandaloneAddon(byte[] content)
        {
            _content = content;
        }

        #endregion Constructors

        #region Properties

        /// <inheritdoc/>
        public byte[] Content => _content;

        /// <inheritdoc/>
        public Status Status
        {
            get => _status;
            set => _status = value;
        }

        #endregion Properties
    }
}
