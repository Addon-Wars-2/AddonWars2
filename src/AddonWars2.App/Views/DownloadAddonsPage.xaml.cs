// ==================================================================================================
// <copyright file="DownloadAddonsPage.xaml.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Views
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for DownloadAddonsPage.xaml.
    /// </summary>
    public partial class DownloadAddonsPage : Page
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadAddonsPage"/> class.
        /// </summary>
        public DownloadAddonsPage()
        {
            InitializeComponent();

            //ShowInstallAddonDependenciesDialogCommand = new RelayCommand(ExecuteShowInstallAddonDependenciesDialogCommand);
        }

        #endregion Constructors

        #region Commands

        ///// <summary>
        ///// Gets a command that will open a new confirmation dialog
        ///// whenever a requested addon contains at least one dependency.
        ///// </summary>
        //public RelayCommand ShowInstallAddonDependenciesDialogCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        //// ShowInstallAddonDependenciesDialogCommand command logic.
        //private void ExecuteShowInstallAddonDependenciesDialogCommand()
        //{
        //    var ws = WindowAssist.GetWindowService(this);
        //    var owner = VisualTreeHelperEx.FindParent<Window>(this);
        //    ws.ShowModalWindow<InstallAddonDependenciesDialog>(owner);
        //}

        #endregion Commands Logic

        #region Methods

        #endregion Methods
    }
}
