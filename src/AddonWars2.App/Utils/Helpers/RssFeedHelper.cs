// ==================================================================================================
// <copyright file="RssFeedHelper.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Utils.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using AddonWars2.App.Models.GuildWars2;

    /// <summary>
    /// Represents a Guild Wars 2 RSS feed as a collection of <see cref="RssFeedItem"/> objects.
    /// </summary>
    public static class RssFeedHelper
    {
        #region Methods

        /// <summary>
        /// Parses an XML stream representing a Guild Wars 2 RSS feed.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> object of an RSS XML.</param>
        /// <returns>A collection of <see cref="RssFeedItem"/> objects.</returns>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="stream"/> is <see langword="null"/>.</exception>
        public static async Task<List<RssFeedItem>> ParseXmlStreamAsync(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var xml = await WebHelper.LoadXmlAsync(stream);
            var feed = await Task.Run(() => ParseRssFeedXml(xml));

            return feed;
        }

        /// <summary>
        /// Asynchronously writes RSS item to a file.
        /// </summary>
        /// <param name="item"><see cref="RssFeedItem"/> object to be written.</param>
        /// <param name="filename">The full file path.</param>
        /// <returns><see cref="Task"/> object.</returns>
        public static async Task WriteRssItemContentAsync(RssFeedItem item, string filename)
        {
            var content = item.ContentEncoded;

            // Create all required directories.
            var parentDirPath = Path.GetDirectoryName(filename);
            Directory.CreateDirectory(parentDirPath);

            await File.WriteAllTextAsync(filename, content);
        }

        /// <summary>
        /// "Fixes" some image URLs with missing prefixes.
        /// </summary>
        /// <param name="html">HTML file as a string.</param>
        /// <param name="prefix">Prefix to be added.</param>
        /// <returns>A new HTML string.</returns>
        public static string AddProtocolPrefixesToHtml(string html, string prefix)
        {
            return html.Replace(@"""//", $@"""{prefix}://");
        }

        /// <summary>
        /// Injects a CSS into the specified HTML string.
        /// </summary>
        /// <param name="html">HTML file as a string.</param>
        /// <param name="cssFilePath">CSS file path.</param>
        /// <returns>A new HTML string.</returns>
        public static string InjectCssIntoHtml(string html, string cssFilePath)
        {
            var cssInjection = $"<head><link rel=\"stylesheet\" href=\"{cssFilePath}\"></head>";
            return $"{cssInjection}\n" + html;
        }

        // Parses the RSS feed (XML document) and returns it as a collection of RssFeedItem objects.
        private static List<RssFeedItem> ParseRssFeedXml(XDocument xml)
        {
            if (xml == null)
            {
                throw new NullReferenceException(nameof(xml));
            }

            var feed = new List<RssFeedItem>();
            var nsContent = xml.Root.GetNamespaceOfPrefix("content");

            foreach (var item in xml.Descendants("item"))
            {
                var isSticky = (from cat in item.Elements("category")
                                where cat.Value.ToLower() == "sticky"
                                select cat).Any();

                var entry = new RssFeedItem()
                {
                    Title = item.Element("title")?.Value,
                    Link = item.Element("link")?.Value,
                    PublishDate = DateTime.Parse(item.Element("pubDate")?.Value),
                    Guid = item.Element("guid")?.Value.Split("=").Last(),
                    Description = item.Element("description")?.Value,
                    ContentEncoded = item.Element(nsContent + "encoded")?.Value.Replace(@"""//", @"""https://"),
                    IsSticky = isSticky,
                };

                feed.Add(entry);
            }

            return feed;
        }

        #endregion Methods
    }
}
