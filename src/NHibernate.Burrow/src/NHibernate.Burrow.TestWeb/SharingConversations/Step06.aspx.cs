using System;
using System.Web.UI;
using NHibernate.Burrow;

public partial class SharingConversations_Step06 : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Facade.ActiveConversations.Count != (int) Session["UseCaseCount"])
        //    throw new Exception("There are more conversations that the expected");            }
    }

    protected void btnNextStep_Click(object sender, EventArgs e)
    {
        Response.Redirect("TheEnd.aspx");
    }
}