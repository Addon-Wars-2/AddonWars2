// ==================================================================================================
// <copyright file="BulkAddonDownloader.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloaders
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using AddonWars2.Core.DTO;
    using AddonWars2.Downloaders.Exceptions;
    using AddonWars2.Downloaders.Interfaces;
    using AddonWars2.Downloaders.Models;

    /// <summary>
    /// Represents a class to download multiple addons asynchronously.
    /// </summary>
    public class BulkAddonDownloader
    {
        #region Fields

        private readonly IAddonDownloaderFactory _addonDownloaderFactory;
        private readonly Dictionary<string, IProgress<double>> _progressCollection = new Dictionary<string, IProgress<double>>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkAddonDownloader"/> class.
        /// </summary>
        /// <param name="addonDownloaderFactory">A reference to <see cref="IAddonDownloaderFactory"/> instance.</param>
        public BulkAddonDownloader(IAddonDownloaderFactory addonDownloaderFactory)
        {
            _addonDownloaderFactory = addonDownloaderFactory ?? throw new ArgumentNullException(nameof(addonDownloaderFactory));
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Is raised whenever the bulk download process is about to start.
        /// This event can be used to handle some stuff like injecting
        /// into <see cref="ProgressCollection"/>.
        /// </summary>
        public event EventHandler DownloadStarting;

        /// <summary>
        /// Is raised whenever one of the download tasks is executed.
        /// </summary>
        public event EventHandler DownloadStarted;

        /// <summary>
        /// Is invoked when all addons are downloaded.
        /// </summary>
        public event EventHandler DownloadCompleted;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets a collection of <see cref="IProgress{T}"/> items which can be used to
        /// track the downloading progress for any of the requested addons.
        /// </summary>
        /// <remarks>
        /// Items are not added automatically and must be injected prior starting the download process.
        /// <see cref="DownloadStarting"/> event handler can be used for this purposes since
        /// <see cref="DownloadStarting"/> event is fired right before executing all download tasks.
        /// </remarks>
        public Dictionary<string, IProgress<double>> ProgressCollection => _progressCollection;

        /// <summary>
        /// Gets the instance of <see cref="IAddonDownloaderFactory"/> factory.
        /// </summary>
        protected IAddonDownloaderFactory AddonDownloaderFactory => _addonDownloaderFactory;

        #endregion Properties

        #region Methods

        public async Task<IEnumerable<DownloadedObject>> DownloadBulkAsync(params AddonData[] addonDataItems)
        {
            return await DownloadBulkAsync(addonDataItems.ToList());
        }

        public async Task<IEnumerable<DownloadedObject>> DownloadBulkAsync(IEnumerable<AddonData> addonDataItems)
        {
            ArgumentNullException.ThrowIfNull(addonDataItems, nameof(addonDataItems));

            var taskQuery = addonDataItems
                .Select(x => DownloadAsync(x))
                .ToList();

            var results = new DownloadedObject[taskQuery.Count];

            OnDownloadStarting();

            results = await Task.WhenAll(taskQuery);

            OnDownloadCompleted();

            ClearProgressCollection();

            return results;
        }

        /// <summary>
        /// Raises <see cref="DownloadStarting"/> event to inform subscribers the bulk download is about to start.
        /// </summary>
        protected virtual void OnDownloadStarting()
        {
            var handler = DownloadStarting;
            handler?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises <see cref="DownloadStarted"/> event to inform subscribers a particular addon is started to download.
        /// </summary>
        protected virtual void OnDownloadStarted()
        {
            var handler = DownloadStarted;
            handler?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises <see cref="DownloadCompleted"/> event to inform subscribers the bulk download is completed.
        /// </summary>
        protected virtual void OnDownloadCompleted()
        {
            var handler = DownloadCompleted;
            handler?.Invoke(this, EventArgs.Empty);
        }

        private async Task<DownloadedObject> DownloadAsync(AddonData addonData)
        {
            ArgumentNullException.ThrowIfNull(addonData, nameof(addonData));

            var exceptions = new List<Exception>();
            DownloadedObject result = new DownloadedObject(string.Empty, Array.Empty<byte>());

            // Iterate through all hosts and try to download. Stop if managed to download from at least one host.
            // An exception will be thrown only if it failed to download from every host.
            foreach (var host in addonData.Hosts)
            {
                var downloader = AddonDownloaderFactory.GetDownloader(host.HostType);
                downloader.DownloadProgressChanged += (sender, e) =>
                {
                    ProgressCollection.TryGetValue(addonData.InternalName, out var progress);
                    progress?.Report(e.Progress);
                };

                try
                {
                    result = await BeginDownload(downloader, host.HostUrl);
                    break;
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                    continue;
                }
            }

            // If failed to download, throw an exception with a complete list of stack traces from the previously thrown exceptions.
            if (exceptions.Count > 0)
            {
                ClearProgressCollection();
                var stackTraces = BuildStackTracesString(exceptions);
                throw new AddonDownloaderException($"The downloader has failed to download the {addonData.InternalName} from available hosts. See a complete list of stack traces below.\n{stackTraces}");
            }

            return result;
        }

        // Executes download process.
        private async Task<DownloadedObject> BeginDownload(IAddonDownloader downloader, string host)
        {
            OnDownloadStarted();
            return await downloader.DownloadAsync(host);
        }

        // Cleans up the progress collection.
        private void ClearProgressCollection()
        {
            ProgressCollection.Clear();
        }

        // Builds a stack trace string which will be included into the exception message
        // if any of the requested addons has failed to be downloaded.
        private string BuildStackTracesString(IEnumerable<Exception> exceptions)
        {
            var stackTraces = string.Empty;
            foreach (var e in exceptions)
            {
                stackTraces += $"{e.Message}\n{e.StackTrace}\n";
            }

            return stackTraces;
        }

        #endregion Methods
    }
}
