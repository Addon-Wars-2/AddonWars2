// ==================================================================================================
// <copyright file="AddonDownloaderException.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Downloaders.Exceptions
{
    public class AddonDownloaderException : Exception
    {
        public AddonDownloaderException()
        {

        }

        public AddonDownloaderException(string? message)
            : base(message)
        {

        }

        public AddonDownloaderException(string? message, Exception? innerException)
            : base(message, innerException)
        {

        }
    }
}
