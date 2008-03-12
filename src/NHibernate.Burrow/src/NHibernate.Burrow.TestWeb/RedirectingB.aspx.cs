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
using NHibernate.Burrow.Test.PersistenceTests;

public partial class RedirectB : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        MockEntityDAO.Instance.FindAll();
    }
   
    protected void Redirect(object sender, EventArgs e)
    {
         Response.Redirect("RedirectingA.aspx");
    }
}
