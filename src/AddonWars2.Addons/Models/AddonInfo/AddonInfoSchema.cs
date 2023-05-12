// ==================================================================================================
// <copyright file="AddonInfoSchema.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Addons.Models.AddonInfo
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// Encapsulates "modinfo" schema information.
    /// </summary>
    [Serializable]
    public class AddonInfoSchema
    {
        #region Fields

        private string _templateName = string.Empty;
        private uint _templateVersion = 1;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonInfoSchema"/> class.
        /// </summary>
        public AddonInfoSchema()
        {
            // Blank.
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the template schema name.
        /// </summary>
        [JsonPropertyName("name")]
        public string TemplateName
        {
            get => _templateName;
            set => _templateName = value;
        }

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
