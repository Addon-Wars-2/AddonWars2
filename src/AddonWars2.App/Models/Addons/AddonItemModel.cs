// ==================================================================================================
// <copyright file="AddonItemModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Models.Addons
{
    using System;
    using System.Linq;
    using AddonWars2.Core.DTO;
    using CommunityToolkit.Mvvm.ComponentModel;

    ///// <summary>
    ///// Specifies addon installation status.
    ///// </summary>
    //public enum AddonInstalledStatus
    //{
    //    /// <summary>
    //    /// The addon is installed.
    //    /// </summary>
    //    Installed,

    //    /// <summary>
    //    /// The addon is not installed.
    //    /// </summary>
    //    NotInstalled,

    //    /// <summary>
    //    /// One or more of the addon files were modified.
    //    /// </summary>
    //    Modified,
    //}

    /// <summary>
    /// Represents an addon item model inside Install Addons page
    /// and acts as a wrapper around <see cref="AddonData"/>.
    /// </summary>
    public class AddonItemModel : ObservableObject
    {
        #region Fields

        private readonly AddonData _data;
        private bool _isMarked = false;
        private bool _isInstalled = false;
        //private bool _canBeMarked = true;
        //private AddonInstalledStatus _installedStatus = AddonInstalledStatus.NotInstalled;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonItemModel"/> class.
        /// </summary>
        /// <param name="data">Deserialized addon info.</param>
        public AddonItemModel(AddonData data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets deserialized addon info.
        /// </summary>
        public AddonData Data => _data;

        /// <summary>
        /// Gets or sets a value indicating whether the addon is marked.
        /// </summary>
        public bool IsMarked
        {
            get => _isMarked;
            set
            {
                SetProperty(ref _isMarked, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the addon is installed.
        /// </summary>
        public bool IsInstalled
        {
            get => _isInstalled;
            set
            {
                SetProperty(ref _isInstalled, value);
            }
        }

        ///// <summary>
        ///// Gets or sets a value indicating whether the addon can be marked.
        ///// </summary>
        //public bool CanBeMarked
        //{
        //    get => _canBeMarked;
        //    set
        //    {
        //        SetProperty(ref _canBeMarked, value);
        //    }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether the addon is already installed.
        ///// </summary>
        //public AddonInstalledStatus InstalledStatus
        //{
        //    get => _installedStatus;
        //    set
        //    {
        //        SetProperty(ref _installedStatus, value);
        //    }
        //}

        /// <summary>
        /// Gets the addon description.
        /// </summary>
        public string AddonDescription
        {
            get
            {
                if (Data == null || string.IsNullOrEmpty(Data.Description))
                {
                    return string.Empty;
                }

                return Data.Description;
            }
        }

        /// <summary>
        /// Gets the addon website.
        /// </summary>
        public string AddonWebsite
        {
            get
            {
                if (Data == null || string.IsNullOrEmpty(Data.Website))
                {
                    return string.Empty;
                }

                return Data.Website;
            }
        }

        /// <summary>
        /// Gets the addon authors.
        /// </summary>
        public string AddonAuthors
        {
            get
            {
                if (Data == null || string.IsNullOrEmpty(Data.Authors))
                {
                    return string.Empty;
                }

                return Data.Authors;
            }
        }

        /// <summary>
        /// Getsa a list of required addons for the addon.
        /// </summary>
        public string AddonRequired
        {
            get
            {
                if (Data == null
                    || Data.RequiredAddons == null
                    || !Data.RequiredAddons.Any())
                {
                    return string.Empty;
                }

                return string.Join(", ", Data.RequiredAddons) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets a list of conflicts for the addon.
        /// </summary>
        public string AddonConflicts
        {
            get
            {
                if (Data == null
                    || Data.Conflicts == null
                    || !Data.Conflicts.Any())
                {
                    return string.Empty;
                }

                return string.Join(", ", Data.Conflicts) ?? string.Empty;
            }
        }

        #endregion Properties
    }
}
