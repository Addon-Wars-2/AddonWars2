// ==================================================================================================
// <copyright file="DownloadProgressEventArgs.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloader.Events
{
    /// <summary>
    /// Encapsulates download progress event arguments.
    /// </summary>
    public class DownloadProgressEventArgs : EventArgs
    {
        #region Fields

        private readonly long _totalBytesToReceive;
        private readonly long _bytesReceived;
        private readonly double _progress;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadProgressEventArgs"/> class.
        /// </summary>
        /// <param name="totalBytesToReceive">The total number of bytes in data download operation.</param>
        /// <param name="bytesReceived">The number of bytes received.</param>
        public DownloadProgressEventArgs(long totalBytesToReceive, long bytesReceived)
        {
            _totalBytesToReceive = totalBytesToReceive;
            _bytesReceived = bytesReceived;
            _progress = bytesReceived / _totalBytesToReceive * 100;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the total number of bytes in data download operation.
        /// </summary>
        public long TotalBytesToReceive => _totalBytesToReceive;

        /// <summary>
        /// Gets the number of bytes received.
        /// </summary>
        public long BytesReceived => _bytesReceived;

        /// <summary>
        /// Gets the download progress percentage locked between 0 and 100.
        /// </summary>
        public double Progress => _progress;

        #endregion Properties
    }
}
