// ==================================================================================================
// <copyright file="ErrorDialogService.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.UIServices
{
    using System;
    using System.Windows;
    using AddonWars2.App.UIServices.Enums;
    using AddonWars2.App.UIServices.Interfaces;
    using AddonWars2.App.ViewModels.Dialogs;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents a service to show error dialogs.
    /// </summary>
    public class ErrorDialogService : IErrorDialogService
    {
        #region Fields

        private readonly ILogger<ErrorDialogViewModel> _logger;
        private readonly IWindowLocator _windowLocator;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorDialogService"/> class.
        /// </summary>
        /// <param name="logger">A referemnce to <see cref="ILogger"/>.</param>
        /// <param name="windowLocator">A reference to <see cref="IWindowLocator"/> instance.</param>
        public ErrorDialogService(
            ILogger<ErrorDialogViewModel> logger,
            IWindowLocator windowLocator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _windowLocator = windowLocator ?? throw new ArgumentNullException(nameof(windowLocator));
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc/>
        public void Show<T>(Window? owner, string? title, string? message, string? details, ErrorDialogButtons buttons)
            where T : Window
        {
            var vm = new ErrorDialogViewModel(_logger)
            {
                Title = title ?? string.Empty,
                Message = message ?? string.Empty,
                Details = details ?? string.Empty,
                Flags = buttons,
            };

            var dialog = _windowLocator.FindWindow<T>();
            dialog.DataContext = vm;
            dialog.Owner = owner;
            dialog.Show();
        }

        /// <inheritdoc/>
        public void Show<T>(Window? owner, string? title, string? message, string? details, ErrorDialogButtons buttons, Action<ErrorDialogResult> callback)
            where T : Window
        {
            var vm = new ErrorDialogViewModel(_logger)
            {
                Title = title ?? string.Empty,
                Message = message ?? string.Empty,
                Details = details ?? string.Empty,
                Flags = buttons,
            };

            var dialog = _windowLocator.FindWindow<T>();
            dialog.DataContext = vm;
            dialog.Owner = owner;

            void ClosedEventHandler(object? sender, EventArgs args)
            {
                callback?.Invoke(vm.Result);
                dialog.Closed -= ClosedEventHandler;
            }

            dialog.Closed += ClosedEventHandler;

            dialog.ShowDialog();
        }

        #endregion Methods
    }
}
