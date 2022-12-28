// ==================================================================================================
// <copyright file="LocalizationHelper.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Helpers
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using AddonWars2.App.Models.Application;

    /// <summary>
    /// Provides various methods to handle localization problems within the application.
    /// </summary>
    public static class LocalizationHelper
    {
        #region Properties

        #endregion Properties

        #region Methods

        /// <summary>
        /// Sets the application culture.
        /// </summary>
        /// <param name="culture">The culture string to be set.</param>
        /// <param name="fallback">Fallback culture string.</param>
        /// <returns>The actually selected culture.</returns>
        public static string SelectCulture(CultureInfo culture, string fallback = "en-US")
        {
            // TODO: This is very bad. We need to either inject app settings instance
            //       to access default culture value, or move away from static approach.

            // See for some details.
            // https://github.com/NickeManarin/ScreenToGif/blob/867c5f6ebc2044bd9ea4da4ed4905c3d832982b5/ScreenToGif.Util/LocalizationHelper.cs

            // Fallback to the default culture in none is set.
            var cultureString = string.Empty;
            if (culture == null)
            {
                cultureString = fallback;
            }
            else
            {
                cultureString = culture.Culture;
            }

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
