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

    ///////// <summary>
    ///////// Specifies addon installation status.
    ///////// </summary>
    ////public enum AddonInstalledStatus
    ////{
    ////    /// <summary>
    ////    /// The addon is installed.
    ////    /// </summary>
    ////    Installed,

    ////    /// <summary>
    ////    /// The addon is not installed.
    ////    /// </summary>
    ////    NotInstalled,

    ////    /// <summary>
    ////    /// One or more of the addon files were modified.
    ////    /// </summary>
    ////    Modified,
    ////}

    /// <summary>
    /// Represents an addon item loaded from a source provider.
    /// Is intended to map <see cref="AddonData"/> object and exposes bindable data.
    /// </summary>
    public class LoadedAddonDataViewModel : ObservableObject
    {
        #region Fields

        private readonly string _sourceProvider;
        private readonly AddonData _model;
        private bool _isInstalled;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadedAddonDataViewModel"/> class.
        /// </summary>
        /// <param name="sourceProvider">A source name this addon was loaded from.</param>
        /// <param name="model">Deserialized addon info.</param>
        public LoadedAddonDataViewModel(string sourceProvider, AddonData model)
        {
            _sourceProvider = string.IsNullOrEmpty(sourceProvider) ? throw new ArgumentException($"{nameof(sourceProvider)} cannot be null or empty.", nameof(sourceProvider)) : sourceProvider;
            _model = model ?? throw new ArgumentNullException(nameof(model));
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
                if (_model == null || string.IsNullOrEmpty(_model.InternalName))
                {
                    return string.Empty;
                }

                return _model.InternalName;
            }
        }

        /// <summary>
        /// Gets the addon display name.
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (_model == null || string.IsNullOrEmpty(_model.DisplayName))
                {
                    return string.Empty;
                }

                return _model.DisplayName;
            }
        }

        /// <summary>
        /// Gets the addon description.
        /// </summary>
        public string Description
        {
            get
            {
                if (_model == null || string.IsNullOrEmpty(_model.Description))
                {
                    return string.Empty;
                }

                return _model.Description;
            }
        }

        /// <summary>
        /// Gets the addon tooltip.
        /// </summary>
        public string Tooltip
        {
            get
            {
                if (_model == null || string.IsNullOrEmpty(_model.Tooltip))
                {
                    return string.Empty;
                }

                return _model.Tooltip;
            }
        }

        /// <summary>
        /// Gets the addon website.
        /// </summary>
        public string Website
        {
            get
            {
                if (_model == null || string.IsNullOrEmpty(_model.Website))
                {
                    return string.Empty;
                }

                return _model.Website;
            }
        }

        /// <summary>
        /// Gets the addon authors.
        /// </summary>
        public string Authors
        {
            get
            {
                if (_model == null || string.IsNullOrEmpty(_model.Authors))
                {
                    return string.Empty;
                }

                return _model.Authors;
            }
        }

        /// <summary>
        /// Gets a list of required addons for the addon represented as a single string
        /// of entries separated with commas.
        /// </summary>
        public string Required
        {
            get
            {
                if (_model == null
                    || _model.RequiredAddons == null
                    || !_model.RequiredAddons.Any())
                {
                    return string.Empty;
                }

                return string.Join(", ", _model.RequiredAddons) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets a a list of required addons.
        /// </summary>
        public IEnumerable<string> RequiredCollection => _model.RequiredAddons ?? Enumerable.Empty<string>();

        /// <summary>
        /// Gets a list of conflicts for the addon.
        /// </summary>
        public string Conflicts
        {
            get
            {
                if (_model == null
                    || _model.Conflicts == null
                    || !_model.Conflicts.Any())
                {
                    return string.Empty;
                }

                return string.Join(", ", _model.Conflicts) ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the addon is installed.
        /// </summary>
        public bool IsInstalled
        {
            get => _isInstalled;
            set => SetProperty(ref _isInstalled, value);
        }

        /// <summary>
        /// Gets a raw data object. Do NOT use for binding.
        /// </summary>
        internal AddonData Model => _model;

        #endregion Properties
    }
}
