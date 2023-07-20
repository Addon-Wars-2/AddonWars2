// ==================================================================================================
// <copyright file="AddonDownloadProgressToken.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.App.Messaging.Tokens
{
    using System;

    /// <summary>
    /// Represents a message token used to report download progress status between view models.
    /// </summary>
    public class AddonDownloadProgressToken : IEquatable<AddonDownloadProgressToken>
    {
        #region Fields

        private readonly string _internalName;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonDownloadProgressToken"/> class.
        /// </summary>
        /// <param name="internalToken">A message unique token (typically addon intenral name).</param>
        public AddonDownloadProgressToken(string internalToken)
        {
            _internalName = internalToken ?? throw new ArgumentNullException(nameof(internalToken));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a unique token of this message.
        /// </summary>
        public string InternalName => _internalName;

        #endregion Properties

        #region Operators

        /// <summary>
        /// Returns <see langword="true"/> if the left <see cref="AddonDownloadProgressToken"/> is equal to the right one,
        /// otherwise returns <see langword="false"/>.
        /// </summary>
        /// <param name="left">The left <see cref="AddonDownloadProgressToken"/> to compare.</param>
        /// <param name="right">The right <see cref="AddonDownloadProgressToken"/> to compare.</param>
        /// <returns><see langword="true"/> if both <see cref="AddonDownloadProgressToken"/> are equal, otherwise returns <see langword="false"/>.</returns>
        public static bool operator ==(AddonDownloadProgressToken left, AddonDownloadProgressToken right)
        {
            if (left is null)
            {
                if (right is null)
                {
                    return true;
                }

                return false;
            }

            return left.Equals(right);
        }

        /// <summary>
        /// Returns <see langword="true"/> if the left <see cref="AddonDownloadProgressToken"/> is NOT equal to the right one,
        /// otherwise returns <see langword="false"/>.
        /// </summary>
        /// <param name="left">The left <see cref="AddonDownloadProgressToken"/> to compare.</param>
        /// <param name="right">The right <see cref="AddonDownloadProgressToken"/> to compare.</param>
        /// <returns><see langword="true"/> if both <see cref="AddonDownloadProgressToken"/> are NOT equal, otherwise returns <see langword="false"/>.</returns>
        public static bool operator !=(AddonDownloadProgressToken left, AddonDownloadProgressToken right) => !(left == right);

        #endregion Operators

        #region Methods

        /// <inheritdoc/>
        public bool Equals(AddonDownloadProgressToken? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (_internalName == other.InternalName)
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return Equals(obj as AddonDownloadProgressToken);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion Methods
    }
}
