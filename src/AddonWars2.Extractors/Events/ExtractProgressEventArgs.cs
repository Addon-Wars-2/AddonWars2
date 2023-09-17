// ==================================================================================================
// <copyright file="ExtractProgressEventArgs.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Extractors.Events
{
    /// <summary>
    /// Encapsulates extraction progress event arguments.
    /// </summary>
    public class ExtractProgressEventArgs : EventArgs
    {
        #region Fields

        private readonly int _totalItemsToExtract;
        private readonly int _itemsExtracted;
        private readonly double _progress;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtractProgressEventArgs"/> class.
        /// </summary>
        /// <param name="totalItemsToExtract">The total number of items in data extraction operation.</param>
        /// <param name="itemsExtracted">The number of items extracted.</param>
        public ExtractProgressEventArgs(int totalItemsToExtract, int itemsExtracted)
        {
            _totalItemsToExtract = totalItemsToExtract;
            _itemsExtracted = itemsExtracted;
            _progress = totalItemsToExtract == 0 ? 0 : (double)itemsExtracted / (double)totalItemsToExtract * 100;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the total number of items in data extraction operation.
        /// </summary>
        public long TotalItemsToExtract => _totalItemsToExtract;

        /// <summary>
        /// Gets the number of items extracted.
        /// </summary>
        public long ItemsExtracted => _itemsExtracted;

        /// <summary>
        /// Gets the extraction progress percentage locked between 0 and 100.
        /// </summary>
        public double Progress => _progress;

        #endregion Properties
    }
}
