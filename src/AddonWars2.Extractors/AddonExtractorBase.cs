// ==================================================================================================
// <copyright file="AddonExtractorBase.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Extractors
{
    using AddonWars2.Core.Events;
    using AddonWars2.Extractors.Interfaces;
    using AddonWars2.Extractors.Models;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents the <see cref="ExtractProgressEventArgs"/> event handler.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void ExtractProgressChangedEventHandler(object? sender, ProgressEventArgs e);

    /// <summary>
    /// Represents a base class for addon extractors.
    /// </summary>
    public abstract class AddonExtractorBase : IAddonExtractor
    {
        #region Fields

        private static ILogger _logger;
        private readonly Dictionary<string, IProgress<double>> _progressCollection = new Dictionary<string, IProgress<double>>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonExtractorBase"/> class.
        /// </summary>
        /// <param name="logger">A reference to <see cref="ILogger"/>.</param>
        public AddonExtractorBase(ILogger<AddonExtractorBase> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            ExtractProgressChanged += AddonExtractorBase_ExtractProgressChanged;
            ExtractionStarted += AddonExtractorBase_ExtractionStarted;
            ExtractionCompleted += AddonExtractorBase_ExtractionCompleted;
        }

        #endregion Constructors

        #region Events

        /// <inheritdoc/>
        public event ExtractProgressChangedEventHandler? ExtractProgressChanged;

        /// <inheritdoc/>
        public event EventHandler ExtractionStarted;

        /// <inheritdoc/>
        public event EventHandler ExtractionCompleted;

        #endregion Events

        #region Properties

        /// <inheritdoc/>
        public Dictionary<string, IProgress<double>> ProgressCollection => _progressCollection;

        /// <summary>
        /// Gets the current logger instance.
        /// </summary>
        protected static ILogger Logger => _logger;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public abstract Task<ExtractionResult> ExtractAsync(ExtractionRequest request, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public void AttachProgressItem(string token, IProgress<double> progress)
        {
            ProgressCollection.Add(token, progress);
        }

        /// <summary>
        /// Raises <see cref="ExtractProgressChanged"/> event to inform subscribers the extraction progress value has changed.
        /// </summary>
        /// <param name="totalItemsToExtract">The total number of items in data extraction operation.</param>
        /// <param name="itemsExtracted">The number of items extracted.</param>
        protected virtual void OnExtractProgressChanged(int totalItemsToExtract, int itemsExtracted)
        {
            var handler = ExtractProgressChanged;
            handler?.Invoke(this, new ProgressEventArgs(totalItemsToExtract, itemsExtracted));
        }

        /// <summary>
        /// Raises <see cref="ExtractionStarted"/> event to inform subscribers the extraction process has started.
        /// </summary>
        protected virtual void OnExtractionStarted()
        {
            var handler = ExtractionStarted;
            handler?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises <see cref="ExtractionCompleted"/> event to inform subscribers the extraction process has completed.
        /// </summary>
        protected virtual void OnExtractionCompleted()
        {
            var handler = ExtractionCompleted;
            handler?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Updates all items in the progress collection.
        /// </summary>
        /// <param name="sender">Event source.</param>
        /// <param name="e">Event arguments.</param>
        private void AddonExtractorBase_ExtractProgressChanged(object? sender, ProgressEventArgs e)
        {
            if (ProgressCollection.Count > 0)
            {
                foreach (var key in ProgressCollection.Keys)
                {
                    ProgressCollection.TryGetValue(key, out var progress);
                    progress?.Report(e.Progress);
                }
            }
        }

        private void AddonExtractorBase_ExtractionStarted(object? sender, EventArgs e)
        {
            // Blank.
        }

        private void AddonExtractorBase_ExtractionCompleted(object? sender, EventArgs e)
        {
            ExtractProgressChanged -= AddonExtractorBase_ExtractProgressChanged;
            ExtractionStarted -= AddonExtractorBase_ExtractionStarted;
        }

        #endregion Methods
    }
}
