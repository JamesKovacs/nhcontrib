using System;

namespace NHibernate.Burrow.WebUtil.Attributes {
	[AttributeUsage(AttributeTargets.Field)]
	public class EntityFieldNullSafe : StatefulField {
		public EntityFieldNullSafe()
			: base(typeof (GetEntityVSFInterceptor).AssemblyQualifiedName) {}
	}
}