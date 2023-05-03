// ==================================================================================================
// <copyright file="IDownloadedAddon.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons.Downloader.Interfaces
{
    /// <summary>
    /// Specifies a completion status.
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// Default (initial) status.
        /// </summary>
        None,

        /// <summary>
        /// The content was downloaded successfully.
        /// </summary>
        Success,

        /// <summary>
        /// The content has failed to download.
        /// </summary>
        Failed,
    }

    /// <summary>
    /// Represents a contract for a downloaded addon.
    /// </summary>
    public interface IDownloadedAddon
    {
        /// <summary>
        /// Gets the downloaded content as a byte array.
        /// </summary>
        public byte[] Content { get; }

        /// <summary>
        /// Gets or sets the completion status.
        /// </summary>
        public Status Status { get; set; }
    }
}
