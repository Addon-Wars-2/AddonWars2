// ==================================================================================================
// <copyright file="ValidPathRule.cs" company="Addon-Wars-2">
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
    /// Implements a <see cref="ValidationRule"/> to check if a given string is a valid path.
    /// </summary>
    public class ValidPathRule : ValidationRule
    {
        #region Fields

        // Backing field for the validation error message.
        private readonly string _errorMessage = ResourcesHelper.GetApplicationResource<string>("S.Common.ValidationText.ValidPath");

        #endregion Fields

        #region Methods

        /// <inheritdoc/>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var stringValue = Convert.ToString(value);

            if (string.IsNullOrEmpty(stringValue) || stringValue.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                return new ValidationResult(false, _errorMessage);
            }

            return ValidationResult.ValidResult;
        }

        #endregion Methods
    }
}
