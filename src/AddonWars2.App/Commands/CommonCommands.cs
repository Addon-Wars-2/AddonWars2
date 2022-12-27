// ==================================================================================================
// <copyright file="CommonCommands.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Commands
{
    using System.Diagnostics;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// Provides a set of common <see cref="IRelayCommand"/> commands, which do not belong
    /// to any particular view model and can be used from any of them.
    /// </summary>
    public class CommonCommands
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommonCommands"/> class.
        /// </summary>
        public CommonCommands()
        {
            OpenUrlCommand = new RelayCommand<string>(ExecuteOpenUrlCommand);
        }

        #region Commands

        /// <summary>
        /// Gets a command which is invoked directly after <see cref="Window.Close"/> is called.
        /// </summary>
        public RelayCommand<string> OpenUrlCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        // OpenUrlCommand command logic.
        private void ExecuteOpenUrlCommand(string url)
        {
            // https://learn.microsoft.com/en-us/dotnet/core/compatibility/fx-core#change-description
            // UseShellExecute = false is a default behavior for .NET Core and on, while it's set to true for .NET Framework.
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            ////e.Handled = true;
        }

        #endregion Commands Logic
    }
}
