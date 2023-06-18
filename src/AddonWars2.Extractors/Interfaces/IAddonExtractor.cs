// ==================================================================================================
// <copyright file="IAddonExtractor.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Extractors.Interfaces
{
    using AddonWars2.Extractors.Models;

    public interface IAddonExtractor
    {
        public Task<ExtractedObject> Extract();
    }
}
