using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;


public partial class GridViewEvents_Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (IsPostBack == false)
			{
				IList<string> l = new List<string>();
				l.Add("string0");
				GridView1.DataSource = l;
				GridView1.DataBind(); 
			}
		}

		protected void LinkButton1_Click(object sender, EventArgs e)
		{
			GridView1.Visible = false;
			SuccessMessage1.Show();
			
		}
	}
 