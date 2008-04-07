using System;
using System.Web.UI;
using NHibernate.Burrow;

public partial class SharingConversations_Step06 : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            Facade facade = new Facade();
            IConversation conversation = facade.CurrentConversation;

            if (conversation == null)
                throw new Exception("The page doesn't have conversation");
            Checker.CheckSpanningConversations(1);

            object lastConversationId = Session["conversationId"];
            if (lastConversationId == null)
                throw new Exception("We haven't found the Id of previous conversation");

            if (!conversation.Id.Equals(lastConversationId))
                throw new Exception("The conversation isn't same that previous, the new conversation was created");
            conversation.FinishSpan();
            Checker.CheckSpanningConversations(0);

        }
    }

    protected void btnNextStep_Click(object sender, EventArgs e)
    {
        Response.Redirect("TheEnd.aspx");
    }
}