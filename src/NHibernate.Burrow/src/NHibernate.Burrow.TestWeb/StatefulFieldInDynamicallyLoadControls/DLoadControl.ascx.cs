using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NHibernate.Burrow.WebUtil.Attributes;

public partial class StatefulFieldInDynamicallyLoadControls_DLoadControl : System.Web.UI.UserControl
{
	[StatefulField] protected int i = 0;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
			i = 2;
	}

	protected void btnClick(object sender, EventArgs e) {
		Checker.AssertEqual(2, i);
		btn.Visible = false;
        SuccessMessage1.Show();
	}
}
