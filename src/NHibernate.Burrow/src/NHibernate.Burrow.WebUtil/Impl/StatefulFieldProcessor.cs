using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using log4net;
using NHibernate.Burrow.WebUtil.Attributes;

namespace NHibernate.Burrow.WebUtil.Impl {
	internal abstract class StatefulFieldProcessor {
		
		private static readonly IDictionary<Type, IDictionary<FieldInfo, StatefulField>> fieldInfoCache =
			new Dictionary<Type, IDictionary<FieldInfo, StatefulField>>();

		private readonly Control ctl;
		private readonly StatefulFieldPageModule pageModule;
		protected IDictionary<FieldInfo, StatefulField> statefulfields;
		private string stateKeyPrefix;
		private StateBag states;
		 
		private readonly Assembly webAssembly = Assembly.GetAssembly(typeof(System.Web.UI.Control));

		public StatefulFieldProcessor(Control c, StatefulFieldPageModule sfpm) {
			ctl = c;
			statefulfields = GetStatefulFields();
			pageModule = sfpm;
			states = pageModule.GetControlState(Control.UniqueID);
		 
		}

		protected StateBag States {
			get { return states; }
		}

		public Control Control {
			get { return ctl; }
		}
		

		private bool HasStatefulField {
			get { return statefulfields != null && statefulfields.Count > 0; }
		}

 
		public void Process() {
			if (HasStatefulField)
				ProcessFields();

			foreach (Control control in Control.Controls)
				if (new StatefulFieldsControlFilter().CanHaveStatefulFields(control))
					CreateSubProcessor(control, pageModule).Process();
		}


		protected abstract StatefulFieldProcessor CreateSubProcessor(Control c, StatefulFieldPageModule sfpm);

		protected abstract void ProcessFields();

		/// <summary>
		/// Get the FieldInfo - Attribute pairs that have the customer attribute of type <typeparamref name="AT"/> 
		/// </summary>
		/// <typeparam name="AT"></typeparam>
		/// <returns></returns>
		protected IDictionary<FieldInfo, AT> GetFieldInfo<AT>() where AT : Attribute {
			IDictionary<FieldInfo, AT> retVal = new Dictionary<FieldInfo, AT>();
			foreach (FieldInfo fi in GetFields())
				foreach (AT a in Attribute.GetCustomAttributes(fi, typeof (AT)))
					retVal.Add(fi, a);
			return retVal;
		}

		protected IDictionary<FieldInfo, StatefulField> GetStatefulFields() {
			IDictionary<FieldInfo, StatefulField> retVal;
			Type controlType = Control.GetType();
			if(controlType.Assembly == webAssembly)
				return null;
			if (!fieldInfoCache.TryGetValue(controlType, out retVal))
				fieldInfoCache[controlType] = retVal = GetFieldInfo<StatefulField>();
			return retVal;
		}

		private FieldInfo[] GetFields() {
			return Control.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		}

 
	}
}