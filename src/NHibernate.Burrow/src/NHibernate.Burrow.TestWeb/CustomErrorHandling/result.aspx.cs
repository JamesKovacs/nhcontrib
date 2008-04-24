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

public partial class CustomerErrorHandling_result : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack) {
			Checker.AssertEqual(0, MockEntityDAO.Instance.FindAll().Count);
			CustomHTTPModule.Enabled = false;
		}
	}
}
