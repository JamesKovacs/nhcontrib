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

public partial class StatefulFieldInDynamicallyLoadControls_Default : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{

	}

	protected override void OnInit(EventArgs e)
	{
		PlaceHolder1.Controls.Add(LoadControl("DLoadControl.ascx"));
		base.OnInit(e);
	}
}
