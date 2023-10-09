// ==================================================================================================
// <copyright file="ProvidersCollection.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Providers.DTO
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Represents a serializable collection of addon providers.
    /// </summary>
    [Serializable]
    public class ProvidersCollection
    {
        #region Fields

        private IEnumerable<ProviderInfo> _providers = new List<ProviderInfo>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvidersCollection"/> class.
        /// </summary>
        public ProvidersCollection()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets a collection of providers.
        /// </summary>
        [JsonPropertyName("providers")]
        public IEnumerable<ProviderInfo> Providers
        {
            get => _providers;
            set => _providers = value;
        }

        #endregion Properties
    }
}
