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

public partial class RedirectingA : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Redirect(object sender, EventArgs e)
    {
        MockEntity me = new MockEntity();
        me.Save(); 
        //Trying to create a dead lock here.
        Response.Redirect("RedirectingB.aspx");
    }
}
