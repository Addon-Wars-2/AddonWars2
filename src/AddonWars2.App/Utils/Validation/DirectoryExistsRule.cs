// ==================================================================================================
// <copyright file="DirectoryExistsRule.cs" company="Addon-Wars-2">
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
    /// Implements a <see cref="ValidationRule"/> to check if a given string is a valid directory.
    /// </summary>
    public class DirectoryExistsRule : ValidationRule
    {
        // Backing field for the validation error message.
        private readonly string _errorMessage = ResourcesHelper.GetApplicationResource<string>("S.Common.ValidationText.DirectoryExists");

        /// <inheritdoc/>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var stringValue = Convert.ToString(value);

            // TODO: "    .txt" is okay, while "    " is an illegal file path.
            //       Change to IsNullOrWhiteSpace?
            if (string.IsNullOrEmpty(stringValue) || !Directory.Exists(stringValue))
            {
                return new ValidationResult(false, _errorMessage);
            }

            return ValidationResult.ValidResult;
        }
    }
}
