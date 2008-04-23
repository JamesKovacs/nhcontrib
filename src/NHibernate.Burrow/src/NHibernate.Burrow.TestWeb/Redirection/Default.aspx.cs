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
using NHibernate.Burrow.Test.MockEntities;

public partial class Redirection_Default : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if(!IsPostBack) {
			Util.ResetEnvironment();
			Session["mo"] = null;
		}
	}
	protected void btnRedirect_Click(object sender, EventArgs e)
	{
		MockEntity mo = new MockEntity();
	    new MockEntityDAO().Save(mo);
		Session["mo"] = mo;

		Response.Redirect("result.aspx");
	}
}
