using System;
using System.Web.UI;
using NHibernate.Burrow;

public partial class SharingConversations_Step05 : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BurrowFramework bf = new BurrowFramework();
            IConversation conversation = bf.CurrentConversation;
            Checker.CheckSpanningConversations(1);

            if (conversation == null)
            {
                throw new Exception("The page doesn't have conversation");
            }

            object lastConversationId = Session["conversationId"];
            if (lastConversationId == null)
            {
                throw new Exception("We haven't found the Id of previous conversation");
            }

            if (!conversation.Id.Equals(lastConversationId))
            {
                throw new Exception("The conversation isn't same that previous, the new conversation was created");
            }

            conversation.FinishSpan();
        }
    }

    protected void btnNextStep_Click(object sender, EventArgs e)
    {
        IConversation conversation = new BurrowFramework().CurrentConversation;
        conversation.SpanWithCookie("WorkSpaceStep06"); //Span the conversation in WorkSpace "WorkSpaceStep06"
        Session["conversationId"] = conversation.Id;
        Response.Redirect("Step06.aspx");
    }
}