// ==================================================================================================
// <copyright file="NotEmptyFieldRule.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Utils.Validation
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using AddonWars2.App.Utils.Helpers;

    /// <summary>
    /// Implements a <see cref="ValidationRule"/> to check if a given string
    /// is not null, empty or contains only whitespaces.
    /// </summary>
    public class NotEmptyFieldRule : ValidationRule
    {
        #region Fields

        // Backing field for the validation error message.
        private readonly string _errorMessage = ResourcesHelper.GetApplicationResource<string>("S.Common.ValidationText.NotEmptyField");

        #endregion Fields

        #region Methods

        /// <inheritdoc/>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var stringValue = Convert.ToString(value);

            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return new ValidationResult(false, _errorMessage);
            }

            return ValidationResult.ValidResult;
        }

        #endregion Methods
    }
}
