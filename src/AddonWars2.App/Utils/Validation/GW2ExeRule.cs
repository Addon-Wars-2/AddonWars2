// ==================================================================================================
// <copyright file="GW2ExeRule.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Utils.Validation
{
    using System;
    using System.IO;
    using System.Windows.Controls;
    using AddonWars2.App.Models.Application;
    using AddonWars2.App.Utils.Helpers;

    /// <summary>
    /// Implements a <see cref="ValidationRule"/> to check if a given string is a GW2 executable.
    /// </summary>
    /// <remarks>
    /// This rule merely checks the GW2 file name, product name and file description.
    /// </remarks>
    public class GW2ExeRule : ValidationRule
    {
        #region Fields

        // Backing field for the validation error message.
        private readonly string _errorMessage = ResourcesHelper.GetApplicationResource<string>("S.Common.ValidationText.GW2Exe");

        #endregion Fields

        #region Methods

        /// <inheritdoc/>
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var stringValue = Convert.ToString(value);

            if (string.IsNullOrEmpty(stringValue) ||
                !(Path.GetExtension(stringValue) == ApplicationGlobal.AppConfig.GW2ExecInfo.FileExtension) ||
                !(System.Diagnostics.FileVersionInfo.GetVersionInfo(stringValue).ProductName.ToString() == ApplicationGlobal.AppConfig.GW2ExecInfo.ProductName) ||
                !(System.Diagnostics.FileVersionInfo.GetVersionInfo(stringValue).FileDescription.ToString() == ApplicationGlobal.AppConfig.GW2ExecInfo.FileDescription))
            {
                return new ValidationResult(false, _errorMessage);
            }

            return ValidationResult.ValidResult;
        }

        #endregion Methods
    }
}
