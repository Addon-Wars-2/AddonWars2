// ==================================================================================================
// <copyright file="ManageAddonsPage.xaml.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Views
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using AddonWars2.App.Extensions.Assists;
    using AddonWars2.App.Utils.Helpers;
    using AddonWars2.App.Views.Dialogs;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// Interaction logic for ManageAddonsPage.xaml.
    /// </summary>
    public partial class ManageAddonsPage : Page
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageAddonsPage"/> class.
        /// </summary>
        public ManageAddonsPage()
        {
            InitializeComponent();

            ShowInstallAddonDependenciesDialogCommand = new RelayCommand(ExecuteShowInstallAddonDependenciesDialogCommand);
        }

        #endregion Constructors

        #region Commands

        /// <summary>
        /// Gets a command that will open a new confirmation dialog
        /// whenever a requested addon contains at least one dependency.
        /// </summary>
        public RelayCommand ShowInstallAddonDependenciesDialogCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        // ShowInstallAddonDependenciesDialogCommand command logic.
        private void ExecuteShowInstallAddonDependenciesDialogCommand()
        {
            var ws = WindowAssist.GetWindowService(this);
            var owner = VisualTreeHelperEx.FindParent<Window>(this);
            ws.ShowModalWindow<InstallAddonDependenciesDialog>(owner);
        }

        #endregion Commands Logic

        #region Methods

        #endregion Methods
    }
}
