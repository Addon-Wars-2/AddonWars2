// ==================================================================================================
// <copyright file="ErrorDialogTypeLocator.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.UIServices
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using MvvmDialogs.DialogTypeLocators;

    /// <summary>
    /// Represents a custom type locator for error dialogs.
    /// </summary>
    public class ErrorDialogTypeLocator : IDialogTypeLocator
    {
        #region Methods

        /// <inheritdoc/>
        public Type Locate(INotifyPropertyChanged viewModel)
        {
            var viewModelType = viewModel.GetType();
            var viewModelTypeName = viewModelType.FullName ?? throw new NullReferenceException("Type full name is null.");
            var dialogTypeName = viewModelTypeName.Substring(0, viewModelTypeName.Length - "ViewModel".Length).Replace("ViewModels", "Views");

            return
                Assembly.GetExecutingAssembly().GetType(dialogTypeName)
                ?? throw new NullReferenceException($"Unable to locate view type {dialogTypeName}. Provided view model type: {viewModelTypeName}.");
        }

        #endregion Methods
    }
}
