// ==================================================================================================
// <copyright file="DialogService.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.UIServices
{
    using AddonWars2.App.UIServices.Interfaces;
    using Microsoft.Win32;

    /// <summary>
    /// Contains various methods to create new dialogs, such as <see cref="OpenFileDialog"/>.
    /// </summary>
    public class DialogService : IDialogService
    {
        #region Methods

        /// <inheritdoc/>
        public string[] OpenFileDialog(
            string? title = "",
            string? filename = "",
            string? defaultExt = "",
            string? filter = "",
            bool multiselect = false)
        {
            var dialog = new OpenFileDialog()
            {
                Title = title,
                FileName = filename,
                DefaultExt = defaultExt,
                Filter = filter,
                Multiselect = multiselect,
            };

            var isOkClicked = dialog.ShowDialog();
            if (isOkClicked == true)
            {
                return dialog.FileNames;
            }

            return System.Array.Empty<string>();
        }

        #endregion Methods
    }
}
