using System;
using System.Collections.Generic;
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

public partial class Redirection_result : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if(!IsPostBack) {
			try {
				IList<MockEntity> result = MockEntityDAO.Instance.FindAll();
			if (result.Contains((MockEntity)Session["mo"]))
				Literal1.Text = "congratulations, test passed";
			else
				Literal1.Text = "rediection failed, transaction in the last page isn't committed ";
			}
			catch (System.Data.SqlClient.SqlException e1)
			{
				Literal1.Text = "Redirection failed - " + e1.Message + " the last transaction is still alive.";
			}


		}
	}
}
