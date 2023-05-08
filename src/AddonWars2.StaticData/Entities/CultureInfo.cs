// ==================================================================================================
// <copyright file="CultureInfo.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.SharedData.Entities
{
    using System;

    /// <summary>
    /// Represents an object that encapsulated culture information.
    /// </summary>
    public class CultureInfo
    {
        #region Fields

        private readonly string _culture;
        private readonly string _shortName;
        private readonly string _fullName;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureInfo"/> class.
        /// </summary>
        /// <param name="culture">Culture string in a standard format "en-EN".</param>
        /// <param name="shortName">Culture short name.</param>
        /// <param name="fullName">Culture full name.</param>
        public CultureInfo(
            string culture,
            string shortName,
            string fullName)
        {
            ArgumentException.ThrowIfNullOrEmpty(culture, nameof(culture));
            ArgumentException.ThrowIfNullOrEmpty(shortName, nameof(shortName));
            ArgumentException.ThrowIfNullOrEmpty(fullName, nameof(fullName));

            _culture = culture;
            _shortName = shortName;
            _fullName = fullName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureInfo"/> class.
        /// </summary>
        public CultureInfo()
            : this(string.Empty, string.Empty, string.Empty)
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets culture string using ISO 639 language codes, i.e. "en-US".
        /// </summary>
        public string Culture => _culture;

        /// <summary>
        /// Gets culture short name.
        /// </summary>
        public string ShortName => _shortName;

        /// <summary>
        /// Gets culture full name.
        /// </summary>
        public string FullName => _fullName;

        #endregion Properties
    }
}
