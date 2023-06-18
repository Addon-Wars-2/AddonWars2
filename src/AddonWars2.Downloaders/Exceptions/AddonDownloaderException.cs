// ==================================================================================================
// <copyright file="AddonDownloaderException.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloaders.Exceptions
{
    /// <summary>
    /// Represents a generic exception thrown by addon downloaders.
    /// </summary>
    public class AddonDownloaderException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddonDownloaderException"/> class.
        /// </summary>
        public AddonDownloaderException()
        {
            // Blank.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonDownloaderException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public AddonDownloaderException(string? message)
            : base(message)
        {
            // Blank.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonDownloaderException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public AddonDownloaderException(string? message, Exception? innerException)
            : base(message, innerException)
        {
            // Blank.
        }
    }
}
