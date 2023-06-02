// ==================================================================================================
// <copyright file="LoadedProviderDataViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels.SubViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using AddonWars2.DependencyResolvers.Interfaces;
    using AddonWars2.DependencyResolvers.Models;
    using AddonWars2.Providers.DTO;
    using AddonWars2.Providers.Enums;
    using CommunityToolkit.Mvvm.ComponentModel;

    /// <summary>
    /// Represents a provider data loaded from some source.
    /// Maps <see cref="ProviderInfo"/> and exposes bindable data.
    /// </summary>
    public class LoadedProviderDataViewModel : ObservableObject
    {
        #region Fields

        private readonly ProviderInfo _data;
        private readonly ObservableCollection<LoadedAddonDataViewModel> _addons = new ObservableCollection<LoadedAddonDataViewModel>();
        private readonly DGraph<IDNode> _dependencyGraph = new DGraph<IDNode>();

        private bool _resolveRequired = false;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadedProviderDataViewModel"/> class.
        /// </summary>
        /// <param name="providerInfo">Deserialized provider info.</param>
        public LoadedProviderDataViewModel(ProviderInfo providerInfo)
        {
            _data = providerInfo ?? throw new ArgumentNullException(nameof(providerInfo));

            _addons.CollectionChanged += Addons_CollectionChanged;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a collection of addons which belong to this provider.
        /// </summary>
        public ObservableCollection<LoadedAddonDataViewModel> Addons => _addons;

        /// <summary>
        /// Gets the addon provider name.
        /// </summary>
        public string Name
        {
            get
            {
                if (_data == null || string.IsNullOrEmpty(_data.Name))
                {
                    return string.Empty;
                }

                return _data.Name;
            }
        }

        /// <summary>
        /// Gets the addon registry type.
        /// </summary>
        public ProviderInfoHostType Type => _data.Type;

        /// <summary>
        /// Gets the addon provider link.
        /// </summary>
        /// <remarks>
        /// If a provider is <see cref="ProviderInfoHostType.Local"/> then <see cref="Link"/>
        /// will contain a file path pointing to a local library.
        /// </remarks>
        public string Link
        {
            get
            {
                if (_data == null || string.IsNullOrEmpty(_data.Link))
                {
                    return string.Empty;
                }

                return _data.Link;
            }
        }

        /// <summary>
        /// Gets a dependency graph represeting the addon collection for this provider.
        /// </summary>
        public DGraph<IDNode> DependencyGraph => _dependencyGraph;

        /// <summary>
        /// Gets a value indicating whether the addons data has changed
        /// and the dependency graph has to be resolved again.
        /// </summary>
        public bool ResolveRequired => _resolveRequired;

        /// <summary>
        /// Gets a raw data object. Do NOT use for binding.
        /// </summary>
        internal ProviderInfo Data => _data;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Generates a new dependency graph for a collection of addons.
        /// </summary>
        public void GenerateDependencyGraph()
        {
            foreach (var addon in Addons)
            {
                var node = new DNode(addon.InternalName);
                foreach (var required in addon.RequiredCollection)
                {
                    var dependency = _dependencyGraph.TryGetNode(required) ?? new DNode(required);
                    _dependencyGraph.TryAddNode(dependency);
                    node.AddDependency(dependency);
                }

                _dependencyGraph.AddNode(node);
            }

            _resolveRequired = false;
        }

        // Handles CollectionChanged event for the addons collection.
        private void Addons_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            _resolveRequired = true;
        }

        #endregion Methods
    }
}
