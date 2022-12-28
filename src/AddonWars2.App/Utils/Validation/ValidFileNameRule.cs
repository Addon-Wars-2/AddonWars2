// ==================================================================================================
// <copyright file="ValidFileNameRule.cs" company="Addon-Wars-2">
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
    /// Implements a <see cref="ValidationRule"/> to check if a given string is a valid file name.
    /// </summary>
    public class ValidFileNameRule : ValidationRule
    {
        // Backing field for the validation error message.
        private readonly string _errorMessage = ResourcesHelper.GetApplicationResource<string>("S.Common.ValidationText.ValidFileName");

        /// <inheritdoc/>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var stringValue = Convert.ToString(value);

            // TODO: "    .txt" is okay, while "    " is an illegal file name.
            //       Change to IsNullOrWhiteSpace?
            if (string.IsNullOrEmpty(stringValue) || stringValue.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                return new ValidationResult(false, _errorMessage);
            }

            return ValidationResult.ValidResult;
        }
    }
}
