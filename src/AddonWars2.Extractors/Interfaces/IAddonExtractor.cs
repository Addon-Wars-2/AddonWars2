﻿// ==================================================================================================
// <copyright file="IAddonExtractor.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Extractors.Interfaces
{
    using AddonWars2.Extractors.Models;

    /// <summary>
    /// Specifies a contract for addon extractors.
    /// </summary>
    public interface IAddonExtractor
    {
        /// <summary>
        /// Extracts the addon using the provided information.
        /// </summary>
        /// <param name="request">An extruction request.</param>
        /// <returns>An extracted addon.</returns>
        public Task<ExtractionResult> Extract(ExtractionRequest request);
    }
}
