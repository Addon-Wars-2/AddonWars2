// ==================================================================================================
// <copyright file="ResourcesHelper.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Utils.Helpers
{
    using System;
    using System.Windows;

    /// <summary>
    /// Provides helping methods for application scope resources.
    /// </summary>
    public static class ResourcesHelper
    {
        /// <summary>
        /// Returns the requested application scope resource.
        /// </summary>
        /// <typeparam name="T">Resource type.</typeparam>
        /// <param name="uri">Resource Uniform Resource Identifier.</param>
        /// <returns>The requested resource.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="uri"/> is <see langword="null"/>.</exception>
        public static T GetApplicationResource<T>(string uri)
        {
            ArgumentNullException.ThrowIfNull(uri, nameof(uri));

            return (T)Application.Current.Resources[uri];
        }
    }
}
