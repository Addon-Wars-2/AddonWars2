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
    using NLog;

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

        // Gets the current logger instance.
        private static Logger Logger => LogManager.GetCurrentClassLogger();

        #endregion Properties

        #region Methods

        /// <summary>
        /// Opens a new file dialog on button click.
        /// </summary>
        public void OpenFileDialog_OnClick()
        {
            Logger.Debug("New dialog requested.");
            var ds = DialogAssist.GetDialogService(this);
            if (ds == null)
            {
                Logger.Debug("Couldn't get the service (null).");
                return;
            }

            var paths = ds.OpenFileDialog(
                defaultExt: ".exe",
                filter: "GW2 Executable|*.exe|All Files|*.*",
                multiselect: false);

            DialogAssist.SetSelectedPaths(this, paths);

            Logger.Debug("Dialog closed.");
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Returned paths:");
                foreach (var item in paths)
                {
                    Logger.Debug(item);
                }
            }
        }

        #endregion Methods
    }
}
