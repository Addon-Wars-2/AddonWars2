// ==================================================================================================
// <copyright file="CultureInfo.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Models.Application
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents an object that encapsulated culture information.
    /// </summary>
    [Serializable]
    public class CultureInfo
    {
        #region Fields

        private string _culture;
        private string _shortName;
        private string _fullName;

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
            ArgumentNullException.ThrowIfNull(culture, nameof(culture));
            ArgumentNullException.ThrowIfNull(shortName, nameof(shortName));
            ArgumentNullException.ThrowIfNull(fullName, nameof(fullName));

            _culture = culture;
            _shortName = shortName;
            _fullName = fullName;
        }

        private CultureInfo()
            : this(string.Empty, string.Empty, string.Empty)
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets culture string using ISO 639 language codes,
        /// i.e. "en-US".
        /// </summary>
        [XmlElement("culture")]
        public string Culture
        {
            get => _culture;
            set => _culture = value;
        }

        /// <summary>
        /// Gets or sets culture short name.
        /// </summary>
        [XmlElement("short")]
        public string ShortName
        {
            get => _shortName;
            set => _shortName = value;
        }

        /// <summary>
        /// Gets or sets culture full name.
        /// </summary>
        [XmlElement("full")]
        public string FullName
        {
            get => _fullName;
            set => _fullName = value;
        }

        #endregion Properties
    }
}
