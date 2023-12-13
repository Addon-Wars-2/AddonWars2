// ==================================================================================================
// <copyright file="AddonActionEnums.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Core.Enums
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Specifies an order in which an action should be applied in an addon installation process.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum WhenApplyAddonAction
    {
        /// <summary>
        /// A rule doesn't apply. Default value for all actions.
        /// </summary>
        None,

        /// <summary>
        /// An action is applied at the beginning of the installation process
        /// before any of the items is installed.
        /// </summary>
        PreInstall,

        /// <summary>
        /// An action is applied after all items are installed.
        /// </summary>
        PostInstall,
    }
}
