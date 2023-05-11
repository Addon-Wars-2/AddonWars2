// ==================================================================================================
// <copyright file="CommonCommands.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.ViewModels.Commands
{
    using System;
    using System.Diagnostics;
    using System.Windows.Navigation;
    using CommunityToolkit.Mvvm.Input;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Provides a set of common <see cref="IRelayCommand"/> commands, which do not belong
    /// to any particular view model and can be used from any of them.
    /// </summary>
    public class CommonCommands
    {
        #region Fields

        private static ILogger _logger;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonCommands"/> class.
        /// </summary>
        /// <param name="logger">A referemnce to <see cref="ILogger"/>.</param>
        public CommonCommands(ILogger<CommonCommands> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            OpenUrlCommand = new RelayCommand<RequestNavigateEventArgs>(ExecuteOpenUrlCommand);
        }

        #endregion Properties

        #region Properties

        /// <summary>
        /// Gets the current logger instance.
        /// </summary>
        protected static ILogger Logger => _logger;

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets a command which is invoked directly after <see cref="Window.Close"/> is called.
        /// </summary>
        public RelayCommand<RequestNavigateEventArgs> OpenUrlCommand { get; private set; }

        #endregion Commands

        #region Commands Logic

        // OpenUrlCommand command logic.
        private void ExecuteOpenUrlCommand(RequestNavigateEventArgs? e)
        {
            Logger.LogDebug("Executing command.");

            ArgumentNullException.ThrowIfNull(e, nameof(e));

            // https://learn.microsoft.com/en-us/dotnet/core/compatibility/fx-core#change-description
            // UseShellExecute = false is a default behavior for .NET Core and on, while it's set to true for .NET Framework.
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri)
            {
                UseShellExecute = true,
            });

            e.Handled = true;
        }

        #endregion Commands Logic
    }
}
