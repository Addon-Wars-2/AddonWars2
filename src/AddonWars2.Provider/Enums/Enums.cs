// ==================================================================================================
// <copyright file="Enums.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Provider.Enums
{
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Specified a type of the registry host.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProviderInfoHostType
    {
        /// <summary>
        /// This provider points to a GitHub repository.
        /// </summary>
        [EnumMember(Value = "github")]
        GitHub,

        /// <summary>
        /// This provider points to some standalone source.
        /// </summary>
        [EnumMember(Value = "standalone")]
        Standalone,

        /// <summary>
        /// This provider points to some local (cached) source.
        /// </summary>
        [EnumMember(Value = "local")]
        Local,
    }
}
