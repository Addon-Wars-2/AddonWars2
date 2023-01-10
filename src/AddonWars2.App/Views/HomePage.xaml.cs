// ==================================================================================================
// <copyright file="HomePage.xaml.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Views
{
    using System.Diagnostics;
    using System.Windows.Controls;
    using AddonWars2.App.Extensions.Assists;

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
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Methods

        /// <summary>
        /// Opens a new file dialog on button click.
        /// </summary>
        public void OpenFileDialog_OnClick()
        {
            var ds = DialogAssist.GetDialogService(this);
            if (ds == null)
            {
                return;
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
