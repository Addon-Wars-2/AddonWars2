// ==================================================================================================
// <copyright file="IAddonExtractorFactory.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Extractors.Interfaces
{
    using AddonWars2.Core.Enums;

    /// <summary>
    /// Specifies a contract for addon extractors.
    /// </summary>
    public interface IAddonExtractorFactory
    {
        /// <summary>
        /// Creates a new extractor based on download type.
        /// </summary>
        /// <param name="downloadType">The addon download type used to determine the extractor type.</param>
        /// <returns>A new extractor.</returns>
        public IAddonExtractor GetExtractor(DownloadType downloadType);
    }
}
