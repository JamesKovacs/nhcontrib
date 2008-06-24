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
using NHibernate.Burrow;

public partial class Propagation_Result : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Checker.AssertEqual(new BurrowFramework().CurrentConversation.IsSpanning, true);
        Checker.AssertEqual( Session["conversationId"] , new BurrowFramework().CurrentConversation.Id);
    }
}
