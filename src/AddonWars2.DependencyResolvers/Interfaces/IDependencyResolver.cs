// ==================================================================================================
// <copyright file="IDependencyResolver.cs" company="Addon-Wars-2">
// Copyright (c) Addon-Wars-2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ==================================================================================================

namespace AddonWars2.DependencyResolvers.Interfaces
{
    /// <summary>
    /// Specifies a contract for addon dependency resolvers.
    /// </summary>
    /// <typeparam name="T">Nodes type.</typeparam>
    public interface IDependencyResolver<T>
        where T : class, IDNode
    {
        /// <summary>
        /// Gets a collection of resolved items.
        /// </summary>
        public IList<T> Resolved { get; }

        /// <summary>
        /// Resolves dependencies.
        /// </summary>
        /// <param name="node">A node the resolution starts from.</param>
        public void Resolve(T node);
    }
}
