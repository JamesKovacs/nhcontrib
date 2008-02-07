using NHibernate.Burrow.Util.DAOBases;

namespace NHibernate.Burrow.Util.EntityBases {
    /// <summary>
    /// A object that has a <see cref="IObjectDAOHelper"/>
    /// </summary>
    public interface IPersistantObjWithDAO : IWithId {
        /// <summary>
        /// the helper<see cref="IObjectDAOHelper"/> for doing the DAO related work
        /// </summary>
        IObjectDAOHelper DAO { get; }
    }
}