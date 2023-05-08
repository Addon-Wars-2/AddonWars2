// ==================================================================================================
// <copyright file="RequestHeaders.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons.Shared
{
    using System.Reflection;
    using AddonWars2.SharedData;

    /// <summary>
    /// Stores default request headers used by this application.
    /// </summary>
    public static class RequestHeaders
    {
        #region Fields

        private static readonly string _userAgent = string.Empty;

        #endregion Fields

        #region Constructors

        static RequestHeaders()
        {
            var productName = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? string.Empty;
            var productVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version ?? string.Empty;
            var productComment = $"+{WebStaticData.PROJECT_WIKI_URL_GITHUB}";
            _userAgent = $"{productName}/{productVersion} (+{productComment})";
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a User-Agent string.
        /// </summary>
        /// <remarks>
        /// This value is constrcuted from <see cref="_defaultProductName"/>, <see cref="DefaultProductVersion"/>
        /// and <see cref="DefaultProductComment"/> values and gets updated internally every time any of these
        /// properties are changed.
        /// </remarks>
        public static string UserAgent => _userAgent;

        #endregion Properties
    }
}
