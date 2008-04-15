using ProjectBase.Utils;

namespace EnterpriseSample.Core.Domain
{
    /// <summary>
    /// For a discussion of this object, see 
    /// http://devlicio.us/blogs/billy_mccafferty/archive/2007/04/25/using-equals-gethashcode-effectively.aspx
    /// </summary>
    public abstract class DomainObject<IdT>
    {
        /// <summary>
        /// ID may be of type string, int, custom type, etc.
        /// Setter is protected to allow unit tests to set this property via reflection and to allow 
        /// domain objects more flexibility in setting this for those objects with assigned IDs.
        /// </summary>
        public IdT ID {
            get { return id; }
            protected set { id = value; }
        }

        public override sealed bool Equals(object obj) {
            DomainObject<IdT> compareTo = obj as DomainObject<IdT>;

            return (compareTo != null) &&
                   (HasSameNonDefaultIdAs(compareTo) ||
                    // Since the IDs aren't the same, either of them must be transient to 
                    // compare business value signatures
                    (((IsTransient()) || compareTo.IsTransient()) &&
                     HasSameBusinessSignatureAs(compareTo)));
        }

        /// <summary>
        /// Transient objects are not associated with an item already in storage.  For instance,
        /// a <see cref="Customer" /> is transient if its ID is 0.
        /// </summary>
        public bool IsTransient() {
            return ID == null || ID.Equals(default(IdT));
        }

        /// <summary>
        /// Must be provided to properly compare two objects
        /// </summary>
        public abstract override int GetHashCode();

        private bool HasSameBusinessSignatureAs(DomainObject<IdT> compareTo) {
            Check.Require(compareTo != null, "compareTo may not be null");

            return GetHashCode().Equals(compareTo.GetHashCode());
        }

        /// <summary>
        /// Returns true if self and the provided persistent object have the same ID values 
        /// and the IDs are not of the default ID value
        /// </summary>
        private bool HasSameNonDefaultIdAs(DomainObject<IdT> compareTo) {
            Check.Require(compareTo != null, "compareTo may not be null");

            return (ID != null && ! ID.Equals(default(IdT))) &&
                   (compareTo.ID != null && ! compareTo.ID.Equals(default(IdT))) &&
                   ID.Equals(compareTo.ID);
        }

        private IdT id = default(IdT);
    }
}
