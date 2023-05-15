// ==================================================================================================
// <copyright file="EnumToStringConverter.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Utils.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Converts a given enumeration to string.
    /// </summary>
    public class EnumToStringConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? enumString;
            try
            {
                enumString = Enum.GetName(value.GetType(), value);
                if (string.IsNullOrEmpty(enumString))
                {
                    return string.Empty;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return enumString;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
