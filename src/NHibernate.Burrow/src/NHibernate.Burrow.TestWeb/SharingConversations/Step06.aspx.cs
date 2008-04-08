using System;
using System.Web.UI;
using NHibernate.Burrow;

public partial class SharingConversations_Step06 : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack || Session["continue"] != null && (bool)Session["continue"])
        {
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

        }
        
        if (Session["continue"] != null && (bool)Session["continue"])
        {
            Session.Remove("continue");
            btnNextStep.Visible = true;
        }
        else if (IsPostBack)
            lblMessage.Text = "You should complete the step 6 a before continue";
    }

    protected void btnNextStep_Click(object sender, EventArgs e)
    {
        Response.Redirect("TheEnd.aspx");
    }
}