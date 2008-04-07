using System;
using System.Web.UI;
using NHibernate.Burrow;

public partial class SharingConversations_Step05 : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Facade facade = new Facade();
        IConversation conversation = facade.CurrentConversation;
        Checker.CheckSpanningConversations(1);
        
        if (conversation == null)
            throw new Exception("The page doesn't have conversation");

        object lastConversationId = Session["conversationId"];
        if (lastConversationId == null)
            throw new Exception("We haven't found the Id of previous conversation");

        if (!conversation.Id.Equals(lastConversationId))
            throw new Exception("The conversation isn't same that previous, the new conversation was created");
        
    }

    protected void btnNextStep_Click(object sender, EventArgs e)
    {
        //Session["UseCaseCount"] = Facade.ActiveConversations.Count;
        Response.Redirect("Step06.aspx");
    }
}