using System.Reflection;

namespace NHibernate.Burrow.Util.DAOBases {
    /// <summary>
    /// A concrete simple inheritance of the GeneraticDAOBase for generic use.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class GenericDAO<T> : GenericDAOBase<T> {
        public GenericDAO(Assembly a) : base(a) {}
        public GenericDAO() {}

        ///<summary>
        /// Load the id using Type <paramref name="t"/>
        ///</summary>
        ///<param name="t"></param>
        ///<param name="id"></param>
        ///<returns></returns>
        public T Load(System.Type t, object id) {
            return (T) Sess.Get(t, id);
        }
    }
}