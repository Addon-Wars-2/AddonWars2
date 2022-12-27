// ==================================================================================================
// <copyright file="VisibilityToBooleanConverter.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Utils.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Converts <see cref="Visibility"/> to boolean.
    /// Returns  <see langword="true"/> if <see cref="Visibility.Visible"/>, otherwise <see langword="false"/>.
    /// </summary>
    public class VisibilityToBooleanConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
            {
                throw new InvalidOperationException("Target type must be a type of boolean. Received: {targetType.GetType}");
            }

            return true ? (Visibility)value == Visibility.Visible : false;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
