using System;
using NHibernate.Burrow.NHDomain;

namespace NHibernate.Burrow.WebUtil {
    public abstract class EntityVSFInterceptorBase : IStatefulFieldInterceptor {
        public EntityVSFInterceptorBase() {}

        protected abstract bool UseLoad { get; }

        #region IStatefulFieldInterceptor Members

        public object OnSave(object toSave, object objectInStateContainer) {
            if (toSave == null) return null;
            object id = Loader.Instance.GetId(toSave);
            Type t = toSave.GetType();
            return new object[] {id, t};
        }

        public object OnLoad(object objectOriginallyLoaded) {
            if (objectOriginallyLoaded == null)
                return null;

            object[] vs = (object[]) objectOriginallyLoaded;
            if (UseLoad)
                return Loader.Instance.Load((Type) vs[1], vs[0]);
            else
                return Loader.Instance.Get((Type) vs[1], vs[0]);
        }

        #endregion

    }

    public class GetEntityVSFInterceptor : EntityVSFInterceptorBase {
        protected override bool UseLoad {
            get { return false; }
        }
    }

    public class LoadEntityVSFInterceptor : EntityVSFInterceptorBase {
        protected override bool UseLoad {
            get { return true; }
        }
    }
}