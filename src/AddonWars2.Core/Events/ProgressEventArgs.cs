// ==================================================================================================
// <copyright file="ProgressEventArgs.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Core.Events
{
    /// <summary>
    /// Encapsulates progress event args.
    /// </summary>
    public class ProgressEventArgs
    {
        #region Fields

        private readonly long _totalItems;
        private readonly long _itemsProcessed;
        private readonly double _progress;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressEventArgs"/> class.
        /// </summary>
        /// <param name="totalItems">The total number of items in an operation.</param>
        /// <param name="itemsProcessed">The number of items processed.</param>
        public ProgressEventArgs(long totalItems, long itemsProcessed)
        {
            _totalItems = totalItems;
            _itemsProcessed = itemsProcessed;
            _progress = totalItems == 0L ? 0L : (double)itemsProcessed / (double)totalItems * 100;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the total number of items in an operation.
        /// </summary>
        public long TotalItems => _totalItems;

        /// <summary>
        /// Gets the number of items processed.
        /// </summary>
        public long ItemsProcessed => _itemsProcessed;

        /// <summary>
        /// Gets the progress percentage locked between 0 and 100.
        /// </summary>
        public double Progress => _progress;

        #endregion Properties
    }
}
