// ==================================================================================================
// <copyright file="MainWindow.xaml.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            ChangeLanguageCommand = new RelayCommand<SelectionChangedEventArgs>(ExecuteChangeLanguageCommand);
        }

        #endregion Constructors

        #region Commands

        /// <summary>
        /// Gets a command which is invoked after another language is selected.
        /// </summary>
        public RelayCommand<SelectionChangedEventArgs> ChangeLanguageCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        // ChangeLanguageCommand command logic.
        private void ExecuteChangeLanguageCommand(SelectionChangedEventArgs? e)
        {
            ArgumentNullException.ThrowIfNull(e, nameof(e));

            // The SelectionChangedEventArgs is fired twice: when its data is loaded (attached by the binding)
            // and we edit its value. So we just ignore the first call until it's loaded completely.
            ComboBox comboBox = (ComboBox)e.Source;
            if (!comboBox.IsLoaded)
            {
                return;
            }

            AW2Application.Current.Restart();
        }

        #endregion Commands Logic

        #region Methods

        /// <summary>
        /// Brings the window to foreground.
        /// </summary>
        public void BringToForeground()
        {
            // Source: https://stackoverflow.com/a/23730146
            // TODO: move out of code-behind?

            if (WindowState == WindowState.Minimized || Visibility == Visibility.Hidden)
            {
                Show();
                WindowState = WindowState.Normal;
            }

            // According to some sources these steps gurantee that an app will be brought to foreground.
            Activate();
            Topmost = true;
            Topmost = false;
            Focus();
        }

        #endregion Methods
    }
}
