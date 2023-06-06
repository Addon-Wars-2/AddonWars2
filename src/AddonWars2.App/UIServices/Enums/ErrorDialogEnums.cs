// ==================================================================================================
// <copyright file="ErrorDialogEnums.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.UIServices.Enums
{
    using System;

    /// <summary>
    /// Specifies a result returned by <see cref="Interfaces.IErrorDialogService"/> call.
    /// </summary>
    /// <remarks>
    /// Enum values map <see cref="System.Windows.MessageBoxResult"/>.
    /// </remarks>
    public enum ErrorDialogResult
    {
        /// <summary>
        /// The error dialog returns no result.
        /// </summary>
        None = 0,

        /// <summary>
        /// The result value of the error dialog is <b>OK</b>.
        /// </summary>
        OK = 1,

        /// <summary>
        /// The result value of the error dialog is <b>Cancel</b>.
        /// </summary>
        Cancel = 2,

        /// <summary>
        /// The result value of the error dialog is <b>Yes</b>.
        /// </summary>
        Yes = 6,

        /// <summary>
        /// The result value of the error dialog is <b>No</b>.
        /// </summary>
        No = 7,
    }

    /// <summary>
    /// Specifies buttons available to show.
    /// </summary>
    [Flags]
    public enum ErrorDialogButtons
    {
        /// <summary>
        /// The error dialog will show <b>OK</b> button.
        /// </summary>
        OK = 0x00000000,

        /// <summary>
        /// The error dialog will show <b>OK</b> button.
        /// </summary>
        Cancel = 0x00000001,
    }
}
