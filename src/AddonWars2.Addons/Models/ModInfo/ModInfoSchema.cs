// ==================================================================================================
// <copyright file="ModInfoSchema.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons.Models.ModInfo
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Encapsulates "modinfo" schema information.
    /// </summary>
    public class ModInfoSchema
    {
        #region Fields

        private uint _templateVersion = 1;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModInfoSchema"/> class.
        /// </summary>
        public ModInfoSchema()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the template schema version.
        /// </summary>
        [JsonPropertyName("template_ver")]
        public uint TemplateVersion
        {
            get => _templateVersion;
            set => _templateVersion = value;
        }

        #endregion Properties
    }
}
