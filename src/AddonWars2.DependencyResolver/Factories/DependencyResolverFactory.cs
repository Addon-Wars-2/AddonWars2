// ==================================================================================================
// <copyright file="DependencyResolverFactory.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.DependencyResolver.Factories
{
    using AddonWars2.DependencyResolver.Enums;
    using AddonWars2.DependencyResolver.Interfaces;
    using AddonWars2.DependencyResolver.Models;

    /// <summary>
    /// Represents a <see cref="IDependencyResolver"/> factory.
    /// </summary>
    public class DependencyResolverFactory : IDependencyResolverFactory
    {
        #region Methods

        /// <inheritdoc/>
        public IDependencyResolver<IDNode> GetDependencyResolver(GraphResolverType resolverType, DGraph<IDNode> graph)
        {
            switch (resolverType)
            {
                case GraphResolverType.DFS:
                    return new DepthFirstSearchResolver<IDNode>(graph);
                case GraphResolverType.BFS:
                    throw new NotImplementedException();  // TODO: implementation
                default:
                    throw new NotSupportedException($"Cannot create a resolver for the search type: {resolverType.GetType().Name}. The search type is not supported.");
            }
        }

        #endregion Methods
    }
}
