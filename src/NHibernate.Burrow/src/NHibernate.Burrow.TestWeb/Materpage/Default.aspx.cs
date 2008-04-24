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

public partial class Materpage_Default : System.Web.UI.Page
{
	[StatefulField]
	protected int count;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
			count = 1;
	}

    protected void Button1_Click(object sender, EventArgs e)
	{
		Checker.AssertEqual(1, count);
		Label1.Text = "Congratulations, test passed";

	}
}
