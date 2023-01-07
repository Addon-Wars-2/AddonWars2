// ==================================================================================================
// <copyright file="WindowBaseViewModel.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels
{
    using System.Windows;
    using CommunityToolkit.Mvvm.Input;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A base view model used by all application windows.
    /// Provides basic commands such as minimize, maximize, restore down and close.
    /// </summary>
    public class WindowBaseViewModel : BaseViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowBaseViewModel"/> class.
        /// </summary>
        /// <param name="logger">A referemnce to <see cref="ILogger"/>.</param>
        public WindowBaseViewModel(ILogger<WindowBaseViewModel> logger)
            : base(logger)
        {
            // Commands.
            MinimizeWindowCommand = new RelayCommand<Window>(ExecuteMinimizeWindow);
            MaximizeWindowCommand = new RelayCommand<Window>(ExecuteMaximizeWindow);
            RestoreWindowCommand = new RelayCommand<Window>(ExecuteRestoreWindow);
            CloseWindowCommand = new RelayCommand<Window>(ExecuteCloseWindow);
            DragMoveWindowCommand = new RelayCommand<Window>(ExecuteDragMoveWindow);
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Commands

        // TODO: These commands belong to UI logic only and don't deal with data.
        //       However it's also desired to avoid code duplication, i.e. put it
        //       to a code-behind for every Window. Probably a custom Window
        //       control with this logic implemented in there would be better.
        //       That would also eliminate the need of deriving all Window VM from
        //       this class.

        /// <summary>
        /// Gets a command which minimizes the window.
        /// </summary>
        public RelayCommand<Window> MinimizeWindowCommand { get; private set; }

        /// <summary>
        /// Gets a command which maximizes the window.
        /// </summary>
        public RelayCommand<Window> MaximizeWindowCommand { get; private set; }

        /// <summary>
        /// Gets a command which restores the window.
        /// </summary>
        public RelayCommand<Window> RestoreWindowCommand { get; private set; }

        /// <summary>
        /// Gets a command which closes the window.
        /// </summary>
        public RelayCommand<Window> CloseWindowCommand { get; private set; }

        /// <summary>
        /// Gets a command which drags and moves the window.
        /// </summary>
        public RelayCommand<Window> DragMoveWindowCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        // MinimizeWindowCommand command logic.
        private void ExecuteMinimizeWindow(Window window)
        {
            Logger.LogDebug("Executing command.");
            SystemCommands.MinimizeWindow(window);
        }

        // MaximizeWindowCommand command logic.
        private void ExecuteMaximizeWindow(Window window)
        {
            Logger.LogDebug("Executing command.");
            SystemCommands.MaximizeWindow(window);
        }

        // RestoreWindowCommand command logic.
        private void ExecuteRestoreWindow(Window window)
        {
            Logger.LogDebug("Executing command.");
            SystemCommands.RestoreWindow(window);
        }

        // CloseWindowCommand command logic.
        private void ExecuteCloseWindow(Window window)
        {
            Logger.LogDebug("Executing command.");
            SystemCommands.CloseWindow(window);
        }

        // Performs Drag and Move on a given window.
        private void ExecuteDragMoveWindow(Window window)
        {
            Logger.LogDebug("Executing command.");
            window?.DragMove();
        }

        #endregion Commands Logic
    }
}
