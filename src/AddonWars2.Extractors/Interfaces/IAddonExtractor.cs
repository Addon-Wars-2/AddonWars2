// ==================================================================================================
// <copyright file="IAddonExtractor.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Extractors.Interfaces
{
    using AddonWars2.Core.Interfaces;
    using AddonWars2.Extractors.Models;

    /// <summary>
    /// Specifies a contract for addon extractors.
    /// </summary>
    public interface IAddonExtractor : IAttachableProgress
    {
        /// <summary>
        /// Is raised whenever the extraction progress has changed.
        /// </summary>
        public event ExtractProgressChangedEventHandler? ExtractProgressChanged;

        /// <summary>
        /// Is raised right before the extraction has started.
        /// </summary>
        public event EventHandler ExtractionStarted;

        /// <summary>
        /// Is raised after the extraction has completed.
        /// </summary>
        public event EventHandler ExtractionCompleted;

        /// <summary>
        /// Extracts the addon using the provided information.
        /// </summary>
        /// <param name="request">An extruction request.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>An extracted addon.</returns>
        public Task<ExtractionResult> ExtractAsync(ExtractionRequest request, CancellationToken cancellationToken);
    }
}
