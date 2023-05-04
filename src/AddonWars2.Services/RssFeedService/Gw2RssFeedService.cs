// ==================================================================================================
// <copyright file="Gw2RssFeedService.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Services.RssFeedService
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using AddonWars2.Services.HttpClientWrapper.Interfaces;
    using AddonWars2.Services.RssFeedService.Interfaces;
    using AddonWars2.Services.RssFeedService.Models;
    using AddonWars2.Services.XmlReadWriteService.Interfaces;

    /// <summary>
    /// Represents Guild Wars 2 RSS feed service.
    /// </summary>
    public class Gw2RssFeedService : IRssFeedService<Gw2RssFeedItem>
    {
        #region Fields

        private readonly IHttpClientWrapper _httpClientService;
        private readonly IXmlReaderService _xmlReaderService;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Gw2RssFeedService"/> class.
        /// </summary>
        /// <param name="httpClientService">A reference to <see cref="IHttpClientWrapper"/> instance.</param>
        /// <param name="xmlReaderService">A rerence to <see cref="IXmlReaderService"/> service.</param>
        public Gw2RssFeedService(IHttpClientWrapper httpClientService, IXmlReaderService xmlReaderService)
        {
            _httpClientService = httpClientService;
            _xmlReaderService = xmlReaderService;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the web client service.
        /// </summary>
        protected IHttpClientWrapper HttpClientService => _httpClientService;

        /// <summary>
        /// Gets the web client service.
        /// </summary>
        protected IXmlReaderService XmlReaderService => _xmlReaderService;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="stream"/> is <see langword="null"/>.</exception>
        public async Task<IList<Gw2RssFeedItem>> ReadXmlStreamAsync(Stream stream)
        {
            ArgumentNullException.ThrowIfNull(stream, nameof(stream));

            var xml = await XmlReaderService.LoadXmlAsync(stream);
            var feed = await Task.Run(() => ParseRssFeedXml(xml));

            return feed;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="item"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="filename"/> is <see langword="null"/> or empty.</exception>
        public async Task WriteRssItemAsync(Gw2RssFeedItem item, string filename)
        {
            ArgumentNullException.ThrowIfNull(item, nameof(item));
            ArgumentException.ThrowIfNullOrEmpty(filename, nameof(filename));

            var content = item.ContentEncoded;

            // Create all required directories.
            var parentDirPath = Path.GetDirectoryName(filename);
            if (parentDirPath == null)
            {
                throw new NullReferenceException(nameof(filename));
            }

            Directory.CreateDirectory(parentDirPath);

            await File.WriteAllTextAsync(filename, content);
        }

        /// <summary>
        /// Parses the XML document representing an RSS feedand returns it
        /// as a collection of <see cref="IRssFeedItem"/> objects.
        /// </summary>
        /// <param name="xml">The XML document representing some RSS feed to parse.</param>
        /// <returns>A collection of <see cref="IRssFeedItem"/>.</returns>
        public IList<Gw2RssFeedItem> ParseRssFeedXml(XDocument xml)
        {
            ArgumentNullException.ThrowIfNull(xml, nameof(xml));

            var feed = new List<Gw2RssFeedItem>();
            var nsContent = xml.Root?.GetNamespaceOfPrefix("content");
            if (nsContent == null)
            {
                return feed;
            }

            foreach (var item in xml.Descendants("item"))
            {
                if (item == null)
                {
                    continue;
                }

                var isSticky = (from category in item.Elements("category")
                                where category.Value.ToLower() == "sticky"
                                select category).Any();

                var entry = new Gw2RssFeedItem()
                {
                    Title = item.Element("title")?.Value ?? string.Empty,
                    Link = item.Element("link")?.Value ?? string.Empty,
                    PublishDate = DateTime.Parse(item.Element("pubDate")?.Value ?? string.Empty),  // TODO: convert to local time, since the original dt is PDT
                    Guid = item.Element("guid")?.Value?.Split("=").Last() ?? string.Empty,
                    Description = item.Element("description")?.Value ?? string.Empty,
                    ContentEncoded = item.Element(nsContent + "encoded")?.Value?.Replace(@"""//", @"""https://") ?? string.Empty,
                    IsSticky = isSticky,
                };

                feed.Add(entry);
            }

            return feed;
        }

        /// <summary>
        /// "Fixes" some image URLs with missing prefixes.
        /// </summary>
        /// <param name="html">HTML file as a string.</param>
        /// <param name="prefix">Prefix to be added.</param>
        /// <returns>A new HTML string.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="html"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="prefix"/> is <see langword="null"/>.</exception>
        public string AddProtocolPrefixesToHtml(string html, string prefix)
        {
            ArgumentNullException.ThrowIfNull(html, nameof(html));
            ArgumentNullException.ThrowIfNull(prefix, nameof(prefix));

            return html.Replace(@"""//", $@"""{prefix}://");
        }

        /// <summary>
        /// Injects a CSS into the specified HTML string.
        /// </summary>
        /// <param name="html">HTML file as a string.</param>
        /// <param name="cssFilePath">CSS file path.</param>
        /// <returns>A new HTML string.</returns>
        public string InjectCssIntoHtml(string html, string cssFilePath)
        {
            var cssInjection = $"<head><link rel=\"stylesheet\" href=\"{cssFilePath}\"></head>";
            return $"{cssInjection}\n" + html;
        }

        #endregion Methods
    }
}
