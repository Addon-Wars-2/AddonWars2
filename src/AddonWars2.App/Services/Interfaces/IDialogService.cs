// ==================================================================================================
// <copyright file="IDialogService.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Services.Interfaces
{
    /// <summary>
    /// Represents a contract for creating dialog windows.
    /// </summary>
    public interface IDialogService
    {
        #region Methods

        /// <summary>
        /// Opens a file dialog that allows to select one or multiple files.
        /// </summary>
        /// <param name="title">A text that appears in the title bar of a file dialog.</param>
        /// <param name="filename">A string containing the full path of the file selected in a file dialog.</param>
        /// <param name="defaultExt">A value that specifies the default extension string to use to filter the list of files that are displayed.</param>
        /// <param name="filter">A filter string that determines what types of files are displayed.</param>
        /// <param name="multiselect">An option indicating whether the dialog allows users to select multiple files.</param>
        /// <returns>An array of selected files. In none selected - return an empty array.</returns>
        public string[] OpenFileDialog(
            string? title = "",
            string? filename = "",
            string? defaultExt = "",
            string? filter = "",
            bool multiselect = false);

        #endregion Methods
    }
}
