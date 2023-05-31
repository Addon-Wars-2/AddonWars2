// ==================================================================================================
// <copyright file="DGraph.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.DependencyResolver.Models
{
    using System.Collections.ObjectModel;
    using AddonWars2.DependencyResolver.Interfaces;

    /// <summary>
    /// Represents a dependency graph.
    /// </summary>
    /// <typeparam name="T">Nodes type.</typeparam>
    public class DGraph<T>
        where T : class, IDNode
    {
        #region Fields

        private readonly List<T> _nodes = new List<T>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DGraph{T}"/> class.
        /// </summary>
        public DGraph()
        {
            // Blank.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DGraph{T}"/> class.
        /// </summary>
        /// <param name="nodes">A collection of nodes to add to the graph.</param>
        public DGraph(IEnumerable<T> nodes)
        {
            foreach (var node in nodes)
            {
                _nodes.Add(node);
            }
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a list of nodes added to the graph.
        /// </summary>
        /// <remarks>
        /// This is a <see cref="ReadOnlyCollection{T}"/> colection. Attempting to add, insert, remove or clear the collection
        /// will always throw <see cref="InvalidOperationException"/>. Use the appropriate methods instead.
        /// </remarks>
        public IList<T> Nodes => _nodes.AsReadOnly();

        /// <summary>
        /// Gets a value indicating whether the graph is empty.
        /// </summary>
        public bool IsEmpty => _nodes.Count == 0;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds a new node to this graph.
        /// </summary>
        /// <param name="node">A graph node to add.</param>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="node"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Is thrown if <paramref name="node"/> already exists in the graph.</exception>
        public void AddNode(T node)
        {
            ArgumentNullException.ThrowIfNull(nameof(node));

            var isAlreadyAdded = _nodes.Any(n => n.Name == node.Name);
            if (isAlreadyAdded)
            {
                throw new InvalidOperationException($"The node with the name \"{node.Name}\" is already in the graph.");
            }

            _nodes.Add(node);
        }

        /// <summary>
        /// Performs an attempt to add a new node to this graph.
        /// </summary>
        /// <remarks>
        /// This operation doesn't throw <see cref="InvalidOperationException"/> if <paramref name="node"/> was already added.
        /// </remarks>
        /// <param name="node">A graph node to add.</param>
        /// <returns>
        /// <see langword="true"/> if a node was successfully added, otherwise returns <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="node"/> is <see langword="null"/>.</exception>
        public bool TryAddNode(T node)
        {
            ArgumentNullException.ThrowIfNull(nameof(node));

            var isAlreadyAdded = _nodes.Any(n => n.Name == node.Name);
            if (isAlreadyAdded)
            {
                return false;
            }

            _nodes.Add(node);
            return true;
        }

        /// <summary>
        /// Removes a given node from this graph.
        /// </summary>
        /// <remarks>
        /// This operation doesn't throw <see cref="InvalidOperationException"/> if <paramref name="node"/> is not presented in the graph.
        /// </remarks>
        /// <param name="node">A graph node to remove.</param>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="node"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Is thrown if <paramref name="node"/> doesn't exist in the graph.</exception>
        public void RemoveNode(T node)
        {
            ArgumentNullException.ThrowIfNull(nameof(node));

            var index = _nodes.FindIndex(n => n.Name == node.Name);
            if (index < 0)
            {
                throw new InvalidOperationException($"The specified node with the name \"{node.Name}\" wasn't found in the graph.");
            }

            _nodes.RemoveAt(index);
        }

        /// <summary>
        /// Performs an attempt to remove a given node from this graph.
        /// </summary>
        /// <remarks>
        /// This operation doesn't throw <see cref="InvalidOperationException"/> if <paramref name="node"/> is not presented in the graph.
        /// </remarks>
        /// <param name="node">A graph node to remove.</param>
        /// <returns>
        /// <see langword="true"/> if a node was successfully removed, otherwise returns <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="node"/> is <see langword="null"/>.</exception>
        public bool TryRemoveNode(T node)
        {
            ArgumentNullException.ThrowIfNull(nameof(node));

            var index = _nodes.FindIndex(n => n.Name == node.Name);
            if (index < 0)
            {
                return false;
            }

            _nodes.RemoveAt(index);
            return true;
        }

        #endregion Methods
    }
}
