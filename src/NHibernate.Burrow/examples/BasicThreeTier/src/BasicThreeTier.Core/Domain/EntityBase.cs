namespace BasicThreeTier.Core.Domain
{
    /// <summary> 
    /// the base class for entities
    /// </summary>
    /// <remarks>
    /// This class does not override the Equal and GetHashCode by default 
    /// since in a NHibernate.Burrow managed environment the entity memory identity is well maintained (no detach and re-merge needed). 
    /// </remarks>
    public abstract class EntityBase<IdT>
    {
        /// <summary>
        /// ID may be of type string, int, custom type, etc.
        /// Setter is protected to allow unit tests to set this property via reflection and to allow 
        /// domain objects more flexibility in setting this for those objects with assigned IDs.
        /// </summary>
        public IdT Id {
            get { return id; }
            protected set { id = value; }
        }

        private IdT id = default(IdT);
    }
}
