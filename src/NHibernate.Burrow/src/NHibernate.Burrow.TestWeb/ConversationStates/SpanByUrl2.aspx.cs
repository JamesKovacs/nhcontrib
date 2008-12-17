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

public partial class ConversationalField_SpanByUrl2 : System.Web.UI.Page
{
	
	[StatefulField]
	protected string returnUrl ;
	protected void Page_Load(object sender, EventArgs e)
	{
		if(!IsPostBack)
			returnUrl = Request["returnUrl"];

	}
	protected void Button1_Click(object sender, EventArgs e)
	{
		Response.Redirect(returnUrl);
	}
}
