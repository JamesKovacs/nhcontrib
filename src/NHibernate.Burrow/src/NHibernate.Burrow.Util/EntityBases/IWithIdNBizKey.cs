using System;

namespace NHibernate.Burrow.Util.EntityBases {
    /// <summary>
    /// interface for object that has business Key
    /// </summary>
    public interface IWithIdNBizKey : IWithId, IComparable {
        /// <summary>
        /// A businesskey is a property that can uniquely identify the the entity from others of the same type.
        /// </summary>
        /// <remarks>
        /// It's somewhat similar to concept of primary key in relational database
        /// </remarks>
        IComparable BusinessKey { get; }
    }
}