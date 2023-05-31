// ==================================================================================================
// <copyright file="DNode.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.DependencyResolver.Models
{
    using System.Collections.ObjectModel;
    using AddonWars2.DependencyResolver.Interfaces;

    /// <summary>
    /// Represents a node in a dependency graph.
    /// </summary>
    public class DNode : IDNode, IEquatable<IDNode>
    {
        #region Fields

        private readonly string _name;
        private readonly IList<IDNode> _edges = new List<IDNode>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DNode"/> class.
        /// </summary>
        /// <param name="name">The node name.</param>
        public DNode(string name)
        {
            _name = string.IsNullOrEmpty(name) ? throw new ArgumentException("Name cannot be null or empty.", nameof(name)) : name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DNode"/> class.
        /// </summary>
        /// <remarks>
        /// Always throws <see cref="NotSupportedException"/>. Do not use.
        /// </remarks>
        private DNode()
        {
            throw new NotSupportedException();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the node name.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets the node edges.
        /// Adding a new node (or forming an edge) would mean that the current node
        /// is dependent on the added one.
        /// </summary>
        /// <remarks>
        /// This is a <see cref="ReadOnlyCollection{T}"/> colection. Attempting to add, insert, remove or clear the collection
        /// will always throw <see cref="InvalidOperationException"/>. Use the appropriate methods instead.
        /// </remarks>
        public IList<IDNode> Edges => _edges.AsReadOnly();

        #endregion Properties

        #region Operators

        public static bool operator ==(DNode left, DNode right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DNode left, DNode right)
        {
            return !left.Equals(right);
        }

        #endregion Operators

        #region Methods

        /// <summary>
        /// Adds a new dependency to this node.
        /// </summary>
        /// <param name="edge">A graph edge to add.</param>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="edge"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Is thrown if <paramref name="edge"/> already exists in the dependency list.</exception>
        public void AddDependency(IDNode edge)
        {
            ArgumentNullException.ThrowIfNull(nameof(edge));

            var isAlreadyAdded = _edges.Contains(edge);
            if (isAlreadyAdded)
            {
                throw new InvalidOperationException($"This node already has a dependency with the name \"{edge.Name}\".");
            }

            _edges.Add(edge);
        }

        /// <summary>
        /// Performs an attempt to add a new dependency to this node.
        /// </summary>
        /// <remarks>
        /// This operation doesn't throw <see cref="InvalidOperationException"/> if <paramref name="edge"/> was already added.
        /// </remarks>
        /// <param name="edge">A graph edge to add.</param>
        /// <returns>
        /// <see langword="true"/> if an edge was successfully added, otherwise returns <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="edge"/> is <see langword="null"/>.</exception>
        public bool TryAddDependency(IDNode edge)
        {
            ArgumentNullException.ThrowIfNull(nameof(edge));

            var isAlreadyAdded = _edges.Contains(edge);
            if (isAlreadyAdded)
            {
                return false;
            }

            _edges.Add(edge);

            return true;
        }

        /// <summary>
        /// Removes a given dependency from this node.
        /// </summary>
        /// <remarks>
        /// This operation doesn't throw <see cref="InvalidOperationException"/> if <paramref name="edge"/> is not presented in the list.
        /// </remarks>
        /// <param name="edge">A graph edge to remove.</param>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="edge"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Is thrown if <paramref name="edge"/> doesn't exist in the dependency list.</exception>
        public void RemoveDependency(IDNode edge)
        {
            ArgumentNullException.ThrowIfNull(nameof(edge));

            var isFound = _edges.Contains(edge);
            if (!isFound)
            {
                throw new InvalidOperationException($"The specified edge with the name \"{edge.Name}\" wasn't found in the dependecies.");
            }

            _edges.Remove(edge);
        }

        /// <summary>
        /// Performs an attempt to remove a given dependency from this node.
        /// </summary>
        /// <remarks>
        /// This operation doesn't throw <see cref="InvalidOperationException"/> if <paramref name="edge"/> is not presented in the list.
        /// </remarks>
        /// <param name="edge">A graph edge to remove.</param>
        /// <returns>
        /// <see langword="true"/> if an edge was successfully removed, otherwise returns <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="edge"/> is <see langword="null"/>.</exception>
        public bool TryRemoveDependency(IDNode edge)
        {
            ArgumentNullException.ThrowIfNull(nameof(edge));

            var isFound = _edges.Contains(edge);
            if (!isFound)
            {
                return false;
            }

            _edges.Remove(edge);

            return true;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is DNode other && Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(_name, _edges, Name, Edges);
        }

        /// <inheritdoc/>
        public bool Equals(IDNode? other)
        {
            return other != null
                && ReferenceEquals(this, other)
                && Name == other.Name
                && Edges.SequenceEqual(other.Edges);
        }

        #endregion Methods
    }
}
