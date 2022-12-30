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
    /// Represents a single culture with its additional information,
    /// encapsulated inside <see cref="CultureMap"/> object.
    /// </summary>
    [Serializable]
    public class CultureInfo
    {
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
            Culture = culture;
            ShortName = shortName;
            FullName = fullName;
        }

        // For serializer.
        private CultureInfo()
            : this(string.Empty, string.Empty, string.Empty)
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        // Publuc setters are okay - we need them for serialization.

        /// <summary>
        /// Gets or sets culture string in a standard format, i.e. "en-US".
        /// </summary>
        [XmlElement("culture")]
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets culture short name.
        /// </summary>
        [XmlElement("short")]
        public string ShortName { get; set; }

        /// <summary>
        /// Gets or sets culture full name.
        /// </summary>
        [XmlElement("full")]
        public string FullName { get; set; }

        #endregion Properties
    }
}
