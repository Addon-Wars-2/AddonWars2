// ==================================================================================================
// <copyright file="BulkDownloadRequest.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloaders.Models
{
    using AddonWars2.Core.DTO;

    /// <summary>
    /// Encapsulates a bulk download request.
    /// </summary>
    public class BulkDownloadRequest
    {
        #region Fields

        private readonly string _internalName = string.Empty;
        private readonly IEnumerable<AddonHost> _hosts;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkDownloadRequest"/> class.
        /// </summary>
        /// <param name="addonData">An addon data object the request will be created from.</param>
        public BulkDownloadRequest(AddonData addonData)
        {
            _internalName = addonData.InternalName;
            _hosts = addonData.Hosts;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the addon internal name.
        /// </summary>
        public string InternalName => _internalName;

        /// <summary>
        /// Gets the addon host list.
        /// </summary>
        public IEnumerable<AddonHost> Hosts => _hosts;

        #endregion Properties
    }
}
