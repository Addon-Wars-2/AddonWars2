// ==================================================================================================
// <copyright file="IAttachableProgress.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Core.Interfaces
{
    /// <summary>
    /// Specifies a contract for classes which can attach one or more
    /// <see cref="IProgress{double}"/> objects, which in turn can be used
    /// to track the progress of some operations perfomed by a class
    /// that implements this interface.
    /// </summary>
    public interface IAttachableProgress
    {
        /// <summary>
        /// Gets a collection of <see cref="IProgress{double}"/> items which can be used to
        /// track the downloading progress for an operation.
        /// </summary>
        public Dictionary<string, IProgress<double>> ProgressCollection { get; }

        /// <summary>
        /// Attaches a new <see cref="IProgress{double}"/> item using a unique string token.
        /// </summary>
        /// <remarks>
        /// <see cref="IProgress{double}"/> object should be created outside background thread, typically inside a method
        /// called on a UI thread to capture sync context.
        /// </remarks>
        /// <param name="token">A unique string token (addon internal name).</param>
        /// <param name="progress">A progress item to add.</param>
        public void AttachProgressItem(string token, IProgress<double> progress);
    }
}
