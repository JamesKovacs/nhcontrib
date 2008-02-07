using System.Reflection;

namespace NHibernate.Burrow.Util.DAOBases {
    /// <summary>
    /// Generic DAO whose NHibernate Type and Return Type are the same
    /// </summary>
    /// <typeparam name="T">Type of the Entity</typeparam>
    public class GenericDAOBase<T> : AdvGenericDAO<T, T> {
        public GenericDAOBase() : base() {}
        public GenericDAOBase(Assembly a) : base(a) {}
    }
}