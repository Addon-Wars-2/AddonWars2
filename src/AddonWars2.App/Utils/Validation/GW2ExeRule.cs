// ==================================================================================================
// <copyright file="GW2ExeRule.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Utils.Validation
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Windows.Controls;
    using AddonWars2.App.Utils.Helpers;

    /// <summary>
    /// Implements a <see cref="ValidationRule"/> to check if a given string is a GW2 executable.
    /// </summary>
    /// <remarks>
    /// This rule merely checks the GW2 file name, product name and file description.
    /// </remarks>
    public class GW2ExeRule : ValidationRule
    {
        // TODO: refactor to a separate struct/class with gw2-64.exe signature.

        // Backing field for the validation error message.
        private readonly string _errorMessage = ResourcesHelper.GetApplicationResource<string>("S.Common.ValidationText.GW2Exe");
        private readonly string _gw2ExeFileName = "gw2-64.exe";
        private readonly string _gw2ProductName = "Guild Wars 2";
        private readonly string _gw2FileDescription = "Guild Wars 2 Game Client";

        /// <inheritdoc/>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var stringValue = Convert.ToString(value);

            // TODO: "    .txt" is okay, while "    " is an illegal file path.
            //       Change to IsNullOrWhiteSpace?
            if (string.IsNullOrEmpty(stringValue) ||
                !(Path.GetFileName(stringValue) == _gw2ExeFileName) ||
                !(System.Diagnostics.FileVersionInfo.GetVersionInfo(stringValue).ProductName.ToString() == _gw2ProductName) ||
                !(System.Diagnostics.FileVersionInfo.GetVersionInfo(stringValue).FileDescription.ToString() == _gw2FileDescription))
            {
                return new ValidationResult(false, _errorMessage);
            }

            return ValidationResult.ValidResult;
        }
    }
}
