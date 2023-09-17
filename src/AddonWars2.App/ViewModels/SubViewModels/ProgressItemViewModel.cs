// ==================================================================================================
// <copyright file="ProgressItemViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels.SubViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;

    /// <summary>
    /// Represents a view model for a progress item.
    /// </summary>
    public class ProgressItemViewModel : ObservableObject
    {
        #region Fields

        private string _displayName = string.Empty;
        private string _token = string.Empty;
        private double _progressValue = 0d;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the addon's display name.
        /// </summary>
        public string DisplayName
        {
            get => _displayName;
            set => SetProperty(ref _displayName, value);
        }

        /// <summary>
        /// Gets or sets the addon's unique token used to distinguish downloading addons.
        /// Typically equal to the addon's internal name.
        /// </summary>
        public string Token
        {
            get => _token;
            set => SetProperty(ref _token, value);
        }

        /// <summary>
        /// Gets or sets the progress value.
        /// </summary>
        public double ProgressValue
        {
            get => _progressValue;
            set => SetProperty(ref _progressValue, value);
        }

        #endregion Properties
    }
}
