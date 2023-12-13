// ==================================================================================================
// <copyright file="ObservableCollectionExtensions.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.Core.Extensions
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents an extension class for <see cref="ObservableCollection{T}"/>.
    /// </summary>
    public static class ObservableCollectionExtensions
    {
        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        /// and returns the zero-based index of the first occurrence within the entire <see cref="ObservableCollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">A type of the collection items.</typeparam>
        /// <param name="items"><see cref="ObservableCollection{T}"/> object to search in.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="collection"/>, if found; otherwise, -1.</returns>
        public static int FindIndex<T>(this ObservableCollection<T> items, Predicate<T> match)
        {
            return items.FindIndex(0, items.Count, match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        /// and returns the zero-based index of the first occurrence within the range of elements
        /// in the <see cref="ObservableCollection{T}"/> that extends from the specified index to the last element.
        /// </summary>
        /// <typeparam name="T">A type of the collection items.</typeparam>
        /// <param name="items"><see cref="ObservableCollection{T}"/> object to search in.</param>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="items"/>, if found; otherwise, -1.</returns>
        public static int FindIndex<T>(this ObservableCollection<T> items, int startIndex, Predicate<T> match)
        {
            return items.FindIndex(startIndex, items.Count, match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        /// and returns the zero-based index of the first occurrence within the range of elements
        /// in the <see cref="ObservableCollection{T}"/> that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <typeparam name="T">A type of the collection items.</typeparam>
        /// <param name="items"><see cref="ObservableCollection{T}"/> object to search in.</param>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="items"/>, if found; otherwise, -1.</returns>
        public static int FindIndex<T>(this ObservableCollection<T> items, int startIndex, int count, Predicate<T> match)
        {
            if ((uint)startIndex > items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if (count < 0 || startIndex > items.Count - count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (match == null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            var endIndex = startIndex + count;
            for (int i = startIndex; i < endIndex; i++)
            {
                if (match(items[i]))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
