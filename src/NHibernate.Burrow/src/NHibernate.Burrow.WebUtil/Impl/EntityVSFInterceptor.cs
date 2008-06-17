using System;
using NHibernate.Burrow.Util;

namespace NHibernate.Burrow.WebUtil.Impl {
	using Type = System.Type;
	internal abstract class EntityVSFInterceptorBase : IStatefulFieldInterceptor {
		public EntityVSFInterceptorBase() {}

		protected abstract bool UseLoad { get; }

		#region IStatefulFieldInterceptor Members

		public object OnSave(object toSave, object objectInStateContainer) {
			if (toSave == null)
				return null;
			object id = EntityLoader.Instance.GetId(toSave);
			Type t = toSave.GetType();
			return new object[] {id, t};
		}

		public object OnLoad(object objectOriginallyLoaded) {
			if (objectOriginallyLoaded == null)
				return null;

			object[] vs = (object[]) objectOriginallyLoaded;

			if (vs[0] is int && (int) vs[0] == 0)
				return null;

			if (UseLoad)
				return EntityLoader.Instance.Load((Type) vs[1], vs[0]);
			else
				return EntityLoader.Instance.Get((Type) vs[1], vs[0]);
		}

		#endregion
	}

	internal class GetEntityVSFInterceptor : EntityVSFInterceptorBase {
		protected override bool UseLoad {
			get { return false; }
		}
	}

	internal class LoadEntityVSFInterceptor : EntityVSFInterceptorBase {
		protected override bool UseLoad {
			get { return true; }
		}
	}
}