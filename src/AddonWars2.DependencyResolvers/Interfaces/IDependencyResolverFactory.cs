// ==================================================================================================
// <copyright file="IDependencyResolverFactory.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.DependencyResolvers.Interfaces
{
    using AddonWars2.DependencyResolvers.Enums;
    using AddonWars2.DependencyResolvers.Models;

    /// <summary>
    /// Specifies a contract for <see cref="IDependencyResolver"/> factories.
    /// </summary>
    public interface IDependencyResolverFactory
    {
        /// <summary>
        /// Creates a new resolver based on a search algorithm type.
        /// </summary>
        /// <param name="resolverType">The search algorithm type used to determine the resolver type.</param>
        /// <param name="graph">A graph to resolve.</param>
        /// <returns>A new resolver.</returns>
        IDependencyResolver<IDNode> GetDependencyResolver(GraphResolverType resolverType, DGraph<IDNode> graph);
    }
}
