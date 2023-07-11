// ==================================================================================================
// <copyright file="ArchiveAddonExtractor.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Extractors
{
    using System.IO.Compression;
    using AddonWars2.Extractors.Models;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents an extractor for ZIP archive-type addons.
    /// </summary>
    public class ArchiveAddonExtractor : AddonExtractorBase
    {
        #region Fields

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveAddonExtractor"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/>.</param>
        public ArchiveAddonExtractor(ILogger<AddonExtractorBase> logger)
            : base(logger)
        {
            // Blank.
        }

        #endregion Constructors

        #region Events

        #endregion Events

        #region Properties

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public override async Task<ExtractionResult> Extract(ExtractionRequest request)
        {
            var extractionResult = new ExtractionResult()
            {
                Version = request.Version,
            };

            using (MemoryStream ms = new MemoryStream(request.Content))
            {
                using (ZipArchive zipArchive = new ZipArchive(ms, ZipArchiveMode.Read))
                {
                    var zipArchiveFileEntries = zipArchive.Entries.Where(x => IsZipEntryFile(x));
                    foreach (var entry in zipArchiveFileEntries)
                    {
                        using (Stream es = entry.Open())
                        {
                            using (MemoryStream ems = new MemoryStream())
                            {
                                await es.CopyToAsync(ems);

                                var name = entry.Name;
                                var relativePath = entry.FullName;
                                var content = ems.ToArray();

                                var extractedFile = new ExtractedFile(name, content, relativePath);
                                extractionResult.ExtractedFiles.Add(extractedFile);
                            }
                        }
                    }
                }
            }

            return extractionResult;
        }

        // See for details: https://github.com/icsharpcode/SharpZipLib/blob/master/src/ICSharpCode.SharpZipLib/Zip/ZipEntry.cs (MIT)
        private bool IsZipEntryFile(ZipArchiveEntry entry)
        {
            return !IsZipEntryDirectory(entry) && entry.ExternalAttributes != 8;
        }

        // See for details: https://github.com/icsharpcode/SharpZipLib/blob/master/src/ICSharpCode.SharpZipLib/Zip/ZipEntry.cs (MIT)
        private bool IsZipEntryDirectory(ZipArchiveEntry entry)
        {
            var nameLength = entry.Name.Length;
            return (nameLength > 0 && (entry.Name.EndsWith("/") || entry.Name.EndsWith("\\"))) || entry.ExternalAttributes == 16;
        }

        #endregion Methods
    }
}
