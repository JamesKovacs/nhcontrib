using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NHibernate.Burrow.WebUtil.Attributes;

namespace NHibernate.Burrow.WebUtil.Impl {
	internal class StatefulFieldsControlFilter {
		private IList<Type> FilteredTypes 
		{
			get 
			{
				return new Type[] {
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
				                  };
			}
		}

		private IList<Type> AllowedTypes 
		{
			get
			{
				return new Type[] {
				                  	typeof (UserControl),
				                  	typeof (HtmlForm),
				                  	typeof (HtmlTable),
				                  	typeof (HtmlTableCell),
				                  	typeof (HtmlTableRow)
				                  };
			}
		}

		private IList<Type> FilteredBaseTypes 
		{
			get { return new Type[] {typeof (HtmlControl), typeof (BaseValidator)}; }
		}

		public bool CanHaveStatefulFields(Control control)
		{
			Type t = control.GetType();
			
			foreach (Type at in AllowedTypes)
				if (t.IsSubclassOf(at) || t.Equals(at))
					return true; 
			if (FilteredTypes.Contains(t))
				return false;
			foreach (Type ft in FilteredBaseTypes)
				if (t.IsSubclassOf(ft))
					return  false; 
			//this line took too much resources:
			//return Attribute.GetCustomAttribute(control.GetType(), typeof (HasStatefulField)) != null;
			return true;
		}
	}
}