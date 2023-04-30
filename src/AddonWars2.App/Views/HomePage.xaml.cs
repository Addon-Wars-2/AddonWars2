// ==================================================================================================
// <copyright file="HomePage.xaml.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Views
{
    using System.Windows.Controls;
    using AddonWars2.App.Extensions.Assists;
    using AddonWars2.App.Services;
    using AddonWars2.App.Utils.Helpers;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// Interaction logic for HomePage.xaml.
    /// </summary>
    public partial class HomePage : Page
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HomePage"/> class.
        /// </summary>
        public HomePage()
        {
            InitializeComponent();

            // Belongs purely to UI, not data.
            SwitchToInstallAddonsTabCommand = new RelayCommand<int>(ExecuteSwitchToInstallAddonsTabCommand);
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets a command that will search for a parent <see cref="TabControl"/>
        /// and switch to the invisible "Install Addons" tab.
        /// </summary>
        public RelayCommand<int> SwitchToInstallAddonsTabCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        private void ExecuteSwitchToInstallAddonsTabCommand(int tabIndex)
        {
            var tabControl = VisualTreeHelperEx.FindParent<TabControl>(this);
            if (tabControl != null && tabIndex >= 0)
            {
                tabControl.SelectedIndex = tabIndex;
            }
        }

        #endregion Commands Logic

        #region Methods

        /// <summary>
        /// Opens a new file dialog on button click.
        /// </summary>
        public void OpenFileDialog_OnClick()
        {
            var ds = DialogAssist.GetDialogService(this);
            if (ds == null)
            {
                DialogAssist.SetDialogService(this, new DialogService());
            }

            var paths = ds.OpenFileDialog(
                defaultExt: ".exe",
                filter: "GW2 .exe|*.exe|All Files|*.*",
                multiselect: false);

            DialogAssist.SetSelectedPaths(this, paths);
        }

        #endregion Methods
    }
}
