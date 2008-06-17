using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Iesi.Collections.Generic;

namespace NHibernate.Burrow.WebUtil.Impl {
	using Type = System.Type;

	internal class StatefulFieldsControlFilter {
		private static readonly StatefulFieldsControlFilter instance = new StatefulFieldsControlFilter();

		private readonly HashedSet<Type> allowedTypes = new HashedSet<Type>(new Type[] {
		                                                                               	typeof (HtmlForm),
		                                                                               	typeof (HtmlTable),
		                                                                               	typeof (HtmlTableCell),
		                                                                               	typeof (HtmlTableRow)
		                                                                               });

		private readonly Type[] filteredBaseTypes = new Type[] {typeof (HtmlControl), typeof (BaseValidator)};

		private readonly HashedSet<Type> filteredTypes = new HashedSet<Type>(new Type[] {
		                                                                                	typeof (FileUpload),
		                                                                                	typeof (Label),
		                                                                                	typeof (Button),
		                                                                                	typeof (CheckBox),
		                                                                                	typeof (Image),
		                                                                                	typeof (LinkButton),
		                                                                                	typeof (TextBox),
		                                                                                	typeof (HyperLink),
		                                                                                	typeof (ValidationSummary),
		                                                                                	typeof (FileUpload),
		                                                                                	typeof (Literal),
		                                                                                	typeof (LiteralControl)
		                                                                                });

		private StatefulFieldsControlFilter() {}

		public static StatefulFieldsControlFilter Instance {
			get { return instance; }
		}

		private ISet<Type> FilteredTypes {
			get { return filteredTypes; }
		}

		private ISet<Type> AllowedTypes {
			get { return allowedTypes; }
		}

		private IList<Type> FilteredBaseTypes {
			get { return filteredBaseTypes; }
		}

		public bool CanHaveStatefulFields(Control control) {
			if (control is LiteralControl) //quickly removed the LiteralControls
				return false;
			if (control is UserControl)
				return true;
			Type t = control.GetType();
			LogFactory.Log.Debug("inspecting " + t + control.GetType() + control.ID + "(" + control.UniqueID + ")");
			if (AllowedTypes.Contains(t))
				return true;
			if (FilteredTypes.Contains(t))
				return false;
			foreach (Type ft in FilteredBaseTypes)
				if (t.IsSubclassOf(ft)) {
					FilteredTypes.Add(t);
					return false;
				}
			//this line took too much resources:
			//return Attribute.GetCustomAttribute(control.GetType(), typeof (HasStatefulField)) != null;
			return true;
		}
	}
}