// ==================================================================================================
// <copyright file="ApprovedProviders.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Provider.DTO
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents a serializable collection of approved addon providers.
    /// </summary>
    [Serializable]
    public class ApprovedProviders
    {
        #region Fields

        private IEnumerable<ProviderInfo> _approvedProvidersCollection = new List<ProviderInfo>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApprovedProviders"/> class.
        /// </summary>
        public ApprovedProviders()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets a collection of approved providers.
        /// </summary>
        [JsonPropertyName("approved")]
        public IEnumerable<ProviderInfo> ApprovedProvidersCollection
        {
            get => _approvedProvidersCollection;
            set => _approvedProvidersCollection = value;
        }

        #endregion Properties
    }
}
