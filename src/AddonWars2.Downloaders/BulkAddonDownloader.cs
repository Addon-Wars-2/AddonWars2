﻿// ==================================================================================================
// <copyright file="BulkAddonDownloader.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloaders
{
    using System.Collections.Generic;
    using AddonWars2.Core.Events;
    using AddonWars2.Core.Interfaces;
    using AddonWars2.Downloaders.Exceptions;
    using AddonWars2.Downloaders.Interfaces;
    using AddonWars2.Downloaders.Models;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents a downloader that allows to download multiple addons asynchronously.
    /// </summary>
    public class BulkAddonDownloader : IAttachableProgress
    {
        #region Fields

        private static ILogger _logger;
        private readonly IAddonDownloaderFactory _addonDownloaderFactory;
        private readonly Dictionary<string, IProgress<double>> _progressCollection = new Dictionary<string, IProgress<double>>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkAddonDownloader"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/>.</param>
        /// <param name="addonDownloaderFactory">A reference to <see cref="IAddonDownloaderFactory"/> instance.</param>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="logger"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="addonDownloaderFactory"/> is <see langword="null"/>.</exception>
        public BulkAddonDownloader(ILogger<BulkAddonDownloader> logger, IAddonDownloaderFactory addonDownloaderFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _addonDownloaderFactory = addonDownloaderFactory ?? throw new ArgumentNullException(nameof(addonDownloaderFactory));
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Is raised whenever one of the download tasks is executed.
        /// </summary>
        public event EventHandler? DownloadStarted;

        /// <summary>
        /// Is invoked when all addons are downloaded.
        /// </summary>
        public event EventHandler? DownloadCompleted;

        /// <summary>
        /// Is invoked when at least one addon could not be downloaded.
        /// </summary>
        public event EventHandler? DownloadFailed;

        /// <summary>
        /// Is invoked when an operation cancellation was requested.
        /// </summary>
        public event EventHandler? DownloadAborted;

        #endregion Events

        #region Properties

        /// <inheritdoc/>
        public Dictionary<string, IProgress<double>> ProgressCollection => _progressCollection;

        /// <summary>
        /// Gets the instance of <see cref="IAddonDownloaderFactory"/> factory.
        /// </summary>
        protected IAddonDownloaderFactory AddonDownloaderFactory => _addonDownloaderFactory;

        /// <summary>
        /// Gets the current logger instance.
        /// </summary>
        protected static ILogger Logger => _logger;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Asynchronously downloads the requested addons.
        /// </summary>
        /// <param name="cancellationToken">A task cancellation token.</param>
        /// <param name="requests">A sequence of download requests.</param>
        /// <exception cref="ArgumentNullException">If thrown if <paramref name="requests"/> is <see langword="null"/>.</exception>
        /// <returns>A collection of <see cref="Task{TResult}"/> items.</returns>
        public async Task<IEnumerable<DownloadResult>> DownloadBulkAsync(CancellationToken cancellationToken = default, params BulkDownloadRequest[] requests)
        {
            return await DownloadBulkAsync(requests.ToList(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously downloads the requested addons.
        /// </summary>
        /// <param name="requests">A collection of download requests.</param>
        /// <param name="cancellationToken">A task cancellation token.</param>
        /// <returns>A collection of <see cref="Task{TResult}"/> items.</returns>
        public async Task<IEnumerable<DownloadResult>> DownloadBulkAsync(IEnumerable<BulkDownloadRequest> requests, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(requests, nameof(requests));

            var taskQuery = requests
                .Select(x => DownloadAsync(x, cancellationToken))
                .ToList();

            var results = new DownloadResult[taskQuery.Count];

            OnDownloadStarted();

            try
            {
                results = await Task.WhenAll(taskQuery).WaitAsync(cancellationToken);
            }
            catch (TaskCanceledException)
            {
                OnDownloadAborted();
                ProgressCollection.Clear();
                throw;
            }

            OnDownloadCompleted();

            ProgressCollection.Clear();

            return results;
        }

        /// <inheritdoc/>
        public void AttachProgressItem(string token, IProgress<double> progress)
        {
            ProgressCollection.Add(token, progress);
        }

        /// <summary>
        /// Raises <see cref="DownloadStarted"/> event to inform subscribers a particular addon is started to download.
        /// </summary>
        protected virtual void OnDownloadStarted()
        {
            var handler = DownloadStarted;
            handler?.Invoke(this, EventArgs.Empty);

            Logger.LogInformation("Bulk download started.");
        }

        /// <summary>
        /// Raises <see cref="DownloadCompleted"/> event to inform subscribers the bulk download is completed.
        /// </summary>
        protected virtual void OnDownloadCompleted()
        {
            var handler = DownloadCompleted;
            handler?.Invoke(this, EventArgs.Empty);

            Logger.LogInformation("Bulk download completed.");
        }

        /// <summary>
        /// Raises <see cref="DownloadFailed"/> event to inform subscribers that at least
        /// one of the addons failed to be downloaded.
        /// </summary>
        protected virtual void OnDownloadFailed()
        {
            var handler = DownloadFailed;
            handler?.Invoke(this, EventArgs.Empty);

            Logger.LogError("Bulk download failed.");
        }

        /// <summary>
        /// Raises <see cref="DownloadFailed"/> event to inform subscribers that
        /// download operation was aborted and task cancellation was requested.
        /// </summary>
        protected virtual void OnDownloadAborted()
        {
            var handler = DownloadAborted;
            handler?.Invoke(this, EventArgs.Empty);

            Logger.LogWarning("Bulk download aborted.");
        }

        // Performs async download for the requested addons.
        private async Task<DownloadResult> DownloadAsync(BulkDownloadRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Logger.LogDebug($"Scheduling a task for \"{request.InternalName}\".");

            var exceptions = new List<Exception>();
            DownloadResult result = new DownloadResult(string.Empty, Array.Empty<byte>());

            // Iterate through all hosts and try to download. Stop if managed to download from at least one host.
            // An exception will be thrown only if it failed to download from every host.
            foreach (var host in request.Hosts)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Logger.LogWarning("A task cancellation was requested.");
                    OnDownloadAborted();
                    throw new TaskCanceledException();
                }

                var downloader = AddonDownloaderFactory.GetDownloader(host.HostType);

                downloader.DownloadProgressChanged += Downloader_DownloadProgressChanged;

                // Local func.
                void Downloader_DownloadProgressChanged(object? sender, ProgressEventArgs e)
                {
                    ProgressCollection.TryGetValue(request.InternalName, out var progress);
                    progress?.Report(e.Progress);
                }

                try
                {
                    result = await downloader.DownloadAsync(host.HostUrl, cancellationToken);
                    result.Metadata.Add("internal_name", request.InternalName);
                    Logger.LogDebug($"Download completed for \"{request.InternalName}\" using the host type \"{host.HostType}\" from {host.HostUrl}");
                    break;
                }
                catch (OperationCanceledException)
                {
                    Logger.LogWarning($"Download operation was canceled for {request.InternalName}.");
                    break;
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Failed to download \"{request.InternalName}\" using the host type \"{host.HostType}\" from {host.HostUrl}\nThe next host will be used if available.");
                    exceptions.Add(ex);
                    continue;
                }
                finally
                {
                    downloader.DownloadProgressChanged -= Downloader_DownloadProgressChanged;
                }
            }

            // If failed to download, throw an exception with a complete list of stack traces from the previously thrown exceptions.
            if (exceptions.Count >= request.Hosts.Count() || result.Content.Length == 0)
            {
                OnDownloadFailed();
                ProgressCollection.Clear();

                var stackTraces = BuildStackTracesString(exceptions);
                var ex = new AddonDownloaderException($"The bulk downloader has failed to download the {request.InternalName} from available hosts. See a complete list of stack traces below.\n{stackTraces}");
                Logger.LogError(ex, message: string.Empty);
                throw ex;
            }

            // If the content length was not available beforehand, a downloader will always report zero
            // for total bytes to receive and for an overall progress in percents, but will still update
            // the amount of bytes received. Thus we forcibly update the progress to 100% once downloaded.
            ProgressCollection.TryGetValue(request.InternalName, out var progress);
            progress?.Report(100);

            return result;
        }

        // Builds a stack trace string which will be included into the exception message
        // if any of the requested addons has failed to be downloaded.
        private string BuildStackTracesString(IEnumerable<Exception> exceptions)
        {
            var stackTraces = string.Empty;
            foreach (var ex in exceptions)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                {
                    stackTraces += $"{ex.Message}\n";
                }

                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    stackTraces += $"{ex.StackTrace}\n";
                }
            }

            return stackTraces;
        }

        #endregion Methods
    }
}
