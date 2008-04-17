using System;
using System.Web.UI;
using NHibernate.Burrow;
using NHibernate.Burrow.WebUtil.Attributes;

[WorkSpaceInfo("WorkSpaceStep06")]
public partial class SharingConversations_Step06 : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack || Session["continue"] != null && (bool)Session["continue"])
        {
            BurrowFramework bf = new BurrowFramework();
            IConversation conversation = bf.CurrentConversation;

            if (conversation == null)
                throw new Exception("The page doesn't have conversation");
            Checker.CheckSpanningConversations(1);

            object lastConversationId = Session["conversationId"];
            if (lastConversationId == null)
                throw new Exception("We haven't found the Id of previous conversation");

            if (!conversation.Id.Equals(lastConversationId))
                throw new Exception("The conversation isn't same that previous, the new conversation was created");
        
        }

		if (StepCompleted("a") && StepCompleted("b"))
        {
            Session.Remove("continue");
            btnNextStep.Visible = true;
        }
        else if (IsPostBack)
            lblMessage.Text = "You should complete the step 6a, 6b, 6c a before continue";
    }

	private bool StepCompleted(string step) {
		return Session["continue_" + step] != null && (bool)Session["continue_" + step];
	}

	protected void btnNextStep_Click(object sender, EventArgs e)
    {
		new BurrowFramework().CurrentConversation.FinishSpan();
        Response.Redirect("TheEnd.aspx");
    }
}