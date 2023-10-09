// ==================================================================================================
// <copyright file="AddonExtractorBase.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Extractors
{
    using AddonWars2.Extractors.Events;
    using AddonWars2.Extractors.Interfaces;
    using AddonWars2.Extractors.Models;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents the ExtractProgressEventArgs event handler.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void ExtractProgressChangedEventHandler(object? sender, ExtractProgressEventArgs e);

    /// <summary>
    /// Represents a base class for addon extractors.
    /// </summary>
    public abstract class AddonExtractorBase : IAddonExtractor
    {
        #region Fields

        /// <summary>
        /// Fake delay value that can be optionally used as a delay between
        /// every extraction entry.
        /// </summary>
        protected static readonly int FAKE_DELAY = 10;

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
        }

        #endregion Constructors

        #region Events

        /// <inheritdoc/>
        public event ExtractProgressChangedEventHandler? ExtractProgressChanged;

        #endregion Events

        #region Properties

        /// <inheritdoc/>
        public Dictionary<string, IProgress<double>> ProgressCollection => _progressCollection;

        /// <summary>
        /// Gets or sets a value indicating whether an artificial delay must be added
        /// between each extraction entry.
        /// </summary>
        /// <remarks>
        /// <para>
        /// For smaller files the extraction process feels to be super fast causing
        /// progress bars to fill up instantly. To improve user experience and
        /// to increase their serotonin level a delay between every extraction entry
        /// can be used.
        /// </para>
        /// <para>
        /// Works good with smaller archives, but it's recommended to disable
        /// it when extracting large archives.
        /// </para>
        /// </remarks>
        public bool UseFakeDelay { get; set; } = true;

        /// <summary>
        /// Gets the current logger instance.
        /// </summary>
        protected static ILogger Logger => _logger;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public abstract Task<ExtractionResult> ExtractAsync(ExtractionRequest request);

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
            handler?.Invoke(this, new ExtractProgressEventArgs(totalItemsToExtract, itemsExtracted));
        }

        // Updates all items in the progress collection.
        private void AddonExtractorBase_ExtractProgressChanged(object? sender, ExtractProgressEventArgs e)
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

        // Sets a delay.
        protected async Task DelayAsync(int milliseconds)
        {
            await Task.Run(async () => await Task.Delay(milliseconds));
        }

        #endregion Methods
    }
}
