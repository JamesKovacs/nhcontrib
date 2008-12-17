using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web.UI;
using NHibernate.Burrow.WebUtil.Attributes;

namespace NHibernate.Burrow.WebUtil.Impl
{

	internal class StatefulFieldSaver : StatefulFieldProcessor
	{
		public StatefulFieldSaver(Control c, StatefulFieldPageModule sfpm) : base(c, sfpm) { }

		protected override void ProcessFields()
		{
			foreach (KeyValuePair<FieldInfo, StatefulField> p in statefulfields)
			{
				StatefulField vsf = p.Value;
				object toSave = p.Key.GetValue(Control);
				object objectInViewState = States[p.Key.Name];
				if (vsf.Interceptor != null)
					toSave = vsf.Interceptor.OnSave(toSave, objectInViewState);
				States[p.Key.Name] = toSave;
				if (toSave != null)
					LogFactory.Log.Debug(p.Value.GetType() + " \"" + p.Key.Name + "\"'s value" + toSave + " is retrieved from control " +
									 Control.ID);
			}

		}

		protected override StatefulFieldProcessor CreateSubProcessor(System.Web.UI.Control c, StatefulFieldPageModule sfpm)
		{
			return new StatefulFieldSaver(c, sfpm);
		}
	}
}
