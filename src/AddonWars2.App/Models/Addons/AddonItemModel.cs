// ==================================================================================================
// <copyright file="AddonItemModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Models.Addons
{
    using System;
    using AddonWars2.Addons.Models.AddonInfo;
    using CommunityToolkit.Mvvm.ComponentModel;

    /// <summary>
    /// Specifies addon installation status.
    /// </summary>
    public enum AddonInstalledStatus
    {
        /// <summary>
        /// The addon is installed.
        /// </summary>
        Installed,

        /// <summary>
        /// The addon is not installed.
        /// </summary>
        NotInstalled,

        /// <summary>
        /// One or more of the addon files were modified.
        /// </summary>
        Modified,
    }

    /// <summary>
    /// Represents an addon item model inside Install Addons page
    /// and acts as a wrapper around <see cref="AddonInfoData"/>.
    /// </summary>
    public class AddonItemModel : ObservableObject
    {
        #region Fields

        private readonly AddonInfoData _data;
        private bool _isMarkedToInstall = false;
        private AddonInstalledStatus _installedStatus = AddonInstalledStatus.NotInstalled;
        private string _installedStatusString = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonItemModel"/> class.
        /// </summary>
        /// <param name="data">Deserialized addon info.</param>
        public AddonItemModel(AddonInfoData data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets deserialized addon info.
        /// </summary>
        public AddonInfoData Data => _data;

        /// <summary>
        /// Gets or sets a value indicating whether the addon is marked for installation.
        /// </summary>
        public bool IsMarkedToInstall
        {
            get => _isMarkedToInstall;
            set
            {
                SetProperty(ref _isMarkedToInstall, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the addon is already installed.
        /// </summary>
        public AddonInstalledStatus InstalledStatus
        {
            get => _installedStatus;
            set
            {
                SetProperty(ref _installedStatus, value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the addon is already installed.
        /// This is a string value which mirrors <see cref="InstalledStatus"/> enum value.
        /// </summary>
        public string InstalledStatusString
        {
            get
            {
                var stringValue = Enum.GetName(typeof(AddonInstalledStatus), InstalledStatus);
                if (string.IsNullOrEmpty(stringValue))
                {
                    throw new InvalidOperationException("Invalid enum value.");
                }

                return stringValue;
            }
        }

        #endregion Properties
    }
}
