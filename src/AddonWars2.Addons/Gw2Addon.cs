// ==================================================================================================
// <copyright file="Gw2Addon.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons
{
    /// <summary>
    /// Represents a Guild Wars 2 add-on, which was installed by add-on manager.
    /// </summary>
    [Serializable]
    public class Gw2Addon
    {
        #region Constructor

        public Gw2Addon(string name)
        {

        }

        #endregion Constructor

        #region Properties

        public string Name { get; private set; }

        public string Description { get; private set; } = string.Empty;

        public string Version { get; private set; } = string.Empty;

        public bool IsEnabled { get; set; } = false;

        #endregion Properties
    }
}
