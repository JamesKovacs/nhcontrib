using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using NHibernate.Burrow.WebUtil.Attributes;

namespace NHibernate.Burrow.WebUtil.Impl {
	internal class StatefulFieldLoader : StatefulFieldProcessor {
		public StatefulFieldLoader(Control c, StatefulFieldPageModule sfpm) : base(c, sfpm) { }

		protected override void ProcessFields() {
			foreach (KeyValuePair<FieldInfo, StatefulField> p in statefulfields) {
				StatefulField vsf = p.Value;
				object toSet = States[p.Key.Name];
				if (vsf.Interceptor != null)
					toSet = vsf.Interceptor.OnLoad(toSet);
				p.Key.SetValue(Control, toSet);
				if(toSet != null)
					LogFactory.Log.Debug(p.Value.GetType() + " \"" + p.Key.Name + "\"'s value " + toSet +
				                     " has been restored in control " + Control.ID);
			}
		}

		protected override StatefulFieldProcessor CreateSubProcessor(Control c, StatefulFieldPageModule sfpm)
		{
			return new StatefulFieldLoader(c, sfpm);
		}
	}
}