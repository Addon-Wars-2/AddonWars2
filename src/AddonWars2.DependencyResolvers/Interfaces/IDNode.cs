namespace AddonWars2.DependencyResolvers.Interfaces
{
    /// <summary>
    /// Specifies a contract for dependency graph nodes.
    /// </summary>
    public interface IDNode
    {
        /// <summary>
        /// Gets a unique node name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the node edges.
        /// Adding a new node (or forming an edge) would mean that the current node
        /// is dependent on the added one.
        /// </summary>
        public IList<IDNode> Edges { get; }
    }
}
