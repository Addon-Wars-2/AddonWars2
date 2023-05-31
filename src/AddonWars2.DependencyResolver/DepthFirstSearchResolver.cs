// ==================================================================================================
// <copyright file="DepthFirstSearchResolver.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.DependencyResolver
{
    using AddonWars2.DependencyResolver.Interfaces;
    using AddonWars2.DependencyResolver.Models;

    /// <summary>
    /// Represents a dependency resolver which implements Depth-First Search (DFS) algorithm.
    /// </summary>
    /// <typeparam name="T">Nodes type.</typeparam>
    public class DepthFirstSearchResolver<T> : IDependencyResolver<T>
        where T : class, IDNode
    {
        #region Fields

        private readonly DGraph<T> _graph;
        private readonly IList<T> _resolved = new List<T>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthFirstSearchResolver{T}"/> class.
        /// </summary>
        /// <param name="graph">A dependency graph to resolve.</param>
        public DepthFirstSearchResolver(DGraph<T> graph)
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a reference to the added graph.
        /// </summary>
        public DGraph<T> Graph => _graph;

        /// <inheritdoc/>
        public IList<T> Resolved => _resolved.AsReadOnly();

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public void Resolve(T node)
        {
            if (Graph.IsEmpty)
            {
                throw new InvalidOperationException("The graph is empty and cannot be resolved.");
            }

            var nodes = Graph.Nodes;
            var startIndex = nodes.IndexOf(node);
            if (startIndex < 0)
            {
                throw new InvalidOperationException("The graph doesn't contain a given node.");
            }

            var startNode = nodes[startIndex];
            ResolveInternal(startNode);
        }

        // Resolve(...) main logic.
        private void ResolveInternal(T node)
        {
            foreach (var edge in node.Edges)
            {
                ResolveInternal((T)edge);
            }

            _resolved.Add(node);
        }

        #endregion Methods
    }
}
