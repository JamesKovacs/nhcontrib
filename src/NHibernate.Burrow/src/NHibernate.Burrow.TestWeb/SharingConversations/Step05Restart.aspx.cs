using System;
using System.Web.UI;
using NHibernate.Burrow;

public partial class SharingConversations_Step05Restart : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       Util.ResetEnvironment();
        
    }

    protected void btnNextStep_Click(object sender, EventArgs e)
    {
    	IConversation conversation = new Facade().CurrentConversation;
    	conversation.SpanWithCookie("WorkSpaceStep06");
		 Session["conversationId"] = conversation.Id;
        Response.Redirect("Step06.aspx");
    }
}