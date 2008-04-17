using System;
using System.Web.UI;
using NHibernate.Burrow;

public partial class SharingConversations_Step01 : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BurrowFramework bf = new BurrowFramework();
        IConversation conversation = bf.CurrentConversation;

        if (conversation == null)
            throw new Exception("The page doesn't have current conversation!");
       

        if (!IsPostBack)
        {
            Checker.CheckSpanningConversations(0);
            conversation.SpanWithPostBacks();
            Checker.CheckSpanningConversations(1);

            //conversation.Items["step"] = 0;
            Session["conversationId"] = conversation.Id;
            Session["step"] = 0;
            btnNextStep.Text = "step 1";
        }
        else
        {
            if (!conversation.Id.Equals(Session["conversationId"]))
                throw new Exception("The current conversation isn't the correct");

            //if (conversation.Items["step"] == null)
            //    throw new Exception("The current conversation has lost state");

            lblMessage.Text = "We have successfully checked that the conversation is shared between postbacks";
        }
    }

    protected void btnNextStep_Click(object sender, EventArgs e)
    {
        BurrowFramework bf = new BurrowFramework();
        IConversation conversation = bf.CurrentConversation;
       
        //int step = (int)conversation.Items["step"];
        int step = (int) Session["step"];

        step++;
        //conversation.Items["step"] = step;
        Session["step"] = step;

        if (step < 5)
        {
            btnNextStep.Text = "step 1 + " + step + "/5";
            Checker.CheckSpanningConversations(1);

        }
        else
        {
            conversation.FinishSpan();
            Checker.CheckSpanningConversations(0);

            Response.Redirect("Step02.aspx");
        }
    }
}