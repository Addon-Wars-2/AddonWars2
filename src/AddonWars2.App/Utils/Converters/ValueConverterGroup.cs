// ==================================================================================================
// <copyright file="ValueConverterGroup.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Utils.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Data;

    // Source:
    // https://stackoverflow.com/a/48503354

    /// <summary>
    /// Provides the ability of chaining multiple converters.
    /// </summary>
    public partial class ValueConverterGroup : List<IValueConverter>, IValueConverter
    {
        private string[] _parameters;

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null)
            {
                _parameters = ParamsRegex().Split(parameter.ToString());
            }

            return this.Aggregate(value, (current, converter) => converter.Convert(current, targetType, GetParameter(converter), culture));
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        [GeneratedRegex(@"(?<!\\),")]
        private static partial Regex ParamsRegex();

        private string GetParameter(IValueConverter converter)
        {
            if (_parameters == null)
            {
                return null;
            }

            var index = IndexOf(converter);
            string parameter;

            try
            {
                parameter = _parameters[index];
            }
            catch (IndexOutOfRangeException)
            {
                parameter = null;
            }

            if (parameter != null)
            {
                parameter = Regex.Unescape(parameter);
            }

            return parameter;
        }
    }
}
