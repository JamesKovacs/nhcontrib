using System;
using NHibernate.Burrow.Util.DAOBases;

namespace NHibernate.Burrow.WebUtil {
	internal abstract class EntityVSFInterceptorBase : IStatefulFieldInterceptor
	{
        public EntityVSFInterceptorBase() {}

        protected abstract bool UseLoad { get; }

        #region IStatefulFieldInterceptor Members

        public object OnSave(object toSave, object objectInStateContainer) {
            if (toSave == null) return null;
            object id = EntityLoader.Instance.GetId(toSave);
            Type t = toSave.GetType();
            return new object[] {id, t};
        }

        public object OnLoad(object objectOriginallyLoaded) {
            if (objectOriginallyLoaded == null)
                return null;

            object[] vs = (object[]) objectOriginallyLoaded;
            if (UseLoad)
                return EntityLoader.Instance.Load((Type) vs[1], vs[0]);
            else
                return EntityLoader.Instance.Get((Type) vs[1], vs[0]);
        }

        #endregion
    }

	internal class GetEntityVSFInterceptor : EntityVSFInterceptorBase
	{
        protected override bool UseLoad {
            get { return false; }
        }
    }

	internal class LoadEntityVSFInterceptor : EntityVSFInterceptorBase
	{
        protected override bool UseLoad {
            get { return true; }
        }
    }
}