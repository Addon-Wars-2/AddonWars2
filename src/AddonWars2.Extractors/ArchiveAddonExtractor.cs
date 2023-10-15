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
        public override async Task<ExtractionResult> ExtractAsync(ExtractionRequest request)
        {
            var extractedFiles = new List<ExtractedFile>();

            using (MemoryStream ms = new MemoryStream(request.Content))
            {
                using (ZipArchive zipArchive = new ZipArchive(ms, ZipArchiveMode.Read))
                {
                    var zipArchiveFileEntries = zipArchive.Entries.Where(x => IsZipEntryFile(x));
                    var itemsExtracted = 0;
                    var itemsTotal = zipArchiveFileEntries.Count();

                    foreach (var entry in zipArchiveFileEntries)
                    {
                        var extracted = await ExtractZipArchiveEntryAsync(entry);
                        extractedFiles.Add(extracted);

                        itemsExtracted += 1;
                        OnExtractProgressChanged(itemsTotal, itemsExtracted);

                        if (UseFakeDelay)
                        {
                            await DelayAsync(FAKE_DELAY);
                        }
                    }
                }
            }

            return new ExtractionResult(extractedFiles);
        }

        // Extracts the specified entry.
        private async Task<ExtractedFile> ExtractZipArchiveEntryAsync(ZipArchiveEntry zipArchiveEntry)
        {
            using (Stream es = zipArchiveEntry.Open())
            {
                using (MemoryStream ems = new MemoryStream())
                {
                    await es.CopyToAsync(ems);

                    var name = zipArchiveEntry.Name;
                    var relativePath = zipArchiveEntry.FullName;
                    var content = ems.ToArray();

                    return new ExtractedFile(name, content, relativePath);
                }
            }
        }

        // See for more details: https://github.com/icsharpcode/SharpZipLib/blob/master/src/ICSharpCode.SharpZipLib/Zip/ZipEntry.cs (MIT)
        private bool IsZipEntryFile(ZipArchiveEntry entry)
        {
            return !IsZipEntryDirectory(entry) && entry.ExternalAttributes != 8;
        }

        // See for more details: https://github.com/icsharpcode/SharpZipLib/blob/master/src/ICSharpCode.SharpZipLib/Zip/ZipEntry.cs (MIT)
        private bool IsZipEntryDirectory(ZipArchiveEntry entry)
        {
            var nameLength = entry.Name.Length;
            return (nameLength > 0 && (entry.Name.EndsWith("/") || entry.Name.EndsWith("\\"))) || entry.ExternalAttributes == 16;
        }

        #endregion Methods
    }
}
