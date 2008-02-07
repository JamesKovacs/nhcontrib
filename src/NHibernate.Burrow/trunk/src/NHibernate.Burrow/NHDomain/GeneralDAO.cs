using System;
using Type = System.Type;
using NHibernate;

namespace NHibernate.Burrow.NHDomain {
    /// <summary>
    /// Non-generic GeneralDAO
    /// </summary>
    public class GeneralDAO {
        private System.Type entityType;

        private SessionManager sm;

        public GeneralDAO(System.Type t)
        {
            EntityType = t;
            sm = PersistantUnitRepo.Instance.GetPUOfDomainAssembly(EntityType.Assembly).SessionManager;
        }

        public System.Type EntityType
        {
            get { return entityType; }
            private set { entityType = value; }
        }

        private ISession sess {
            get { return sm.GetSession(); }
        }

        ///<summary>
        ///</summary>
        ///<param name="id"></param>
        ///<returns></returns>
        public object Get(object id) {
            return sess.Get(EntityType, id);
        }
    }
}