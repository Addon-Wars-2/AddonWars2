// ==================================================================================================
// <copyright file="LibraryManagerException.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.LibraryManager.Exceptions
{
    public class LibraryManagerException : Exception
    {
        public LibraryManagerException()
        {

        }

        public LibraryManagerException(string? message)
            : base(message)
        {

        }

        public LibraryManagerException(string? message, Exception? innerException)
            : base(message, innerException)
        {

        }
    }
}
