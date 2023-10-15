// ==================================================================================================
// <copyright file="LibraryManagerException.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.LibraryManager.Exceptions
{
    /// <summary>
    /// Represents a generic exception thrown by <see cref="LibraryManager"/>.
    /// </summary>
    public class LibraryManagerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryManagerException"/> class.
        /// </summary>
        public LibraryManagerException()
        {
            // Blank.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryManagerException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public LibraryManagerException(string? message)
            : base(message)
        {
            // Blank.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryManagerException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public LibraryManagerException(string? message, Exception? innerException)
            : base(message, innerException)
        {
            // Blank.
        }
    }
}
