// ==================================================================================================
// <copyright file="LoadedAddonDataViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels.SubViewModels
{
    using System;
    using System.Collections.Generic;
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
    /// Represents an addon item loaded from some source.
    /// Maps <see cref="AddonData"/> object and exposes bindable data.
    /// </summary>
    public class LoadedAddonDataViewModel : ObservableObject
    {
        #region Fields

        private readonly string _sourceProvider;
        private readonly AddonData _data;
        private bool _isInstalled;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadedAddonDataViewModel"/> class.
        /// </summary>
        /// <param name="sourceProvider">A source name this addon was loaded from.</param>
        /// <param name="data">Deserialized addon info.</param>
        public LoadedAddonDataViewModel(string sourceProvider, AddonData data)
        {
            _sourceProvider = string.IsNullOrEmpty(sourceProvider) ? throw new ArgumentException($"{nameof(sourceProvider)} cannot be null or empty.", nameof(sourceProvider)) : sourceProvider;
            _data = data ?? throw new ArgumentNullException(nameof(data));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a source provider name this addon was loaded from.
        /// </summary>
        public string SourceProvider => _sourceProvider;

        /// <summary>
        /// Gets the addon internal name.
        /// </summary>
        public string InternalName
        {
            get
            {
                if (_data == null || string.IsNullOrEmpty(_data.InternalName))
                {
                    return string.Empty;
                }

                return _data.InternalName;
            }
        }

        /// <summary>
        /// Gets the addon display name.
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (_data == null || string.IsNullOrEmpty(_data.DisplayName))
                {
                    return string.Empty;
                }

                return _data.DisplayName;
            }
        }

        /// <summary>
        /// Gets the addon description.
        /// </summary>
        public string Description
        {
            get
            {
                if (_data == null || string.IsNullOrEmpty(_data.Description))
                {
                    return string.Empty;
                }

                return _data.Description;
            }
        }

        /// <summary>
        /// Gets the addon tooltip.
        /// </summary>
        public string Tooltip
        {
            get
            {
                if (_data == null || string.IsNullOrEmpty(_data.Tooltip))
                {
                    return string.Empty;
                }

                return _data.Tooltip;
            }
        }

        /// <summary>
        /// Gets the addon website.
        /// </summary>
        public string Website
        {
            get
            {
                if (_data == null || string.IsNullOrEmpty(_data.Website))
                {
                    return string.Empty;
                }

                return _data.Website;
            }
        }

        /// <summary>
        /// Gets the addon authors.
        /// </summary>
        public string Authors
        {
            get
            {
                if (_data == null || string.IsNullOrEmpty(_data.Authors))
                {
                    return string.Empty;
                }

                return _data.Authors;
            }
        }

        /// <summary>
        /// Getsa a list of required addons for the addon represented as a single string
        /// of entries separated with commas.
        /// </summary>
        public string Required
        {
            get
            {
                if (_data == null
                    || _data.RequiredAddons == null
                    || !_data.RequiredAddons.Any())
                {
                    return string.Empty;
                }

                return string.Join(", ", _data.RequiredAddons) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets a a list of required addons.
        /// </summary>
        public IEnumerable<string> RequiredCollection => _data.RequiredAddons ?? Enumerable.Empty<string>();

        /// <summary>
        /// Gets a list of conflicts for the addon.
        /// </summary>
        public string Conflicts
        {
            get
            {
                if (_data == null
                    || _data.Conflicts == null
                    || !_data.Conflicts.Any())
                {
                    return string.Empty;
                }

                return string.Join(", ", _data.Conflicts) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the addon is installed.
        /// </summary>
        public bool IsInstalled
        {
            get => _isInstalled;
            set => _isInstalled = value;
        }

        /// <summary>
        /// Gets a raw data object. Do NOT use for binding.
        /// </summary>
        internal AddonData Data => _data;

        #endregion Properties
    }
}
