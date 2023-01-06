﻿// ==================================================================================================
// <copyright file="LocalizationHelper.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Helpers
{
    using System.Linq;
    using System.Threading;
    using AddonWars2.App.Models.Application;

    /// <summary>
    /// Provides various methods to handle localization problems within the application.
    /// </summary>
    public static class LocalizationHelper
    {
        #region Methods

        /// <summary>
        /// Sets the application culture.
        /// </summary>
        /// <param name="culture">The culture to be set.</param>
        /// <param name="fallback">Fallback culture string.</param>
        /// <returns>The actually selected culture.</returns>
        public static string SelectCulture(CultureInfo culture, string fallback = "en-US")
        {
            // TODO: We need to either inject app settings instance
            //       to access default culture value, or move away from static approach.

            // Fallback to the default culture in none is set.
            string cultureString;
            if (culture == null)
            {
                cultureString = fallback;
            }
            else
            {
                cultureString = culture.Culture;
            }

            return SelectCultureInternal(cultureString, fallback);
        }

        /// <summary>
        /// Sets the application culture.
        /// </summary>
        /// <param name="culture">The culture string to be set.</param>
        /// <param name="fallback">Fallback culture string.</param>
        /// <returns>The actually selected culture.</returns>
        public static string SelectCulture(string culture, string fallback = "en-US")
        {
            // Fallback to the default culture in none is set.
            string cultureString;
            if (string.IsNullOrEmpty(culture))
            {
                cultureString = fallback;
            }
            else
            {
                cultureString = culture;
            }

            return SelectCultureInternal(cultureString, fallback);
        }

        // Performs the actual selection of a culture.
        private static string SelectCultureInternal(string cultureString, string fallback = "en-US")
        {
            // Copy all merged dictionaries to a temp list (TODO: memory footprint?).
            var dictionaryList = AW2Application.Current.Resources.MergedDictionaries.ToList();
            var requestedCulture = $"pack://application:,,,/Resources/Localization/StringResources_{cultureString}.xaml";
            var requestedResource = dictionaryList.FirstOrDefault(d => d.Source?.OriginalString == requestedCulture);

            // If no requested resource (culture) found - fallback to the default one.
            if (requestedResource == null)
            {
                cultureString = fallback;
                requestedCulture = $"pack://application:,,,/Resources/Localization/StringResources_{cultureString}.xaml";
                requestedResource = dictionaryList.FirstOrDefault(d => d.Source?.OriginalString == requestedCulture);
            }

            // We remove the requested resource and place it at the end => binding will use it instead.
            AW2Application.Current.Resources.MergedDictionaries.Remove(requestedResource);
            AW2Application.Current.Resources.MergedDictionaries.Add(requestedResource);

            // Inform the threads about the new culture.
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureString);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return cultureString;
        }

        #endregion Methods
    }
}
