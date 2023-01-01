// ==================================================================================================
// <copyright file="Gw2ExecSignature.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Models.GuildWars2
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// Represetns a pre-defined information about GW2 executable file.
    /// </summary>
    [Serializable]
    public class Gw2ExecSignature
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Gw2ExecSignature"/> class.
        /// </summary>
        public Gw2ExecSignature()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the name of GW2 executable.
        /// </summary>
        [XmlElement("exepath")]
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the extension of GW2 executable.
        /// </summary>
        [XmlElement("extension")]
        public string FileExtension { get; set; } = ".exe";

        /// <summary>
        /// Gets or sets the product name of GW2 executable.
        /// </summary>
        [XmlElement("prodname")]
        public string ProductName { get; set; } = "Guild Wars 2";

        /// <summary>
        /// Gets or sets the file description of GW2 executable.
        /// </summary>
        [XmlElement("filedesc")]
        public string FileDescription { get; set; } = "Guild Wars 2 Game Client";

        #endregion Properties
    }
}
