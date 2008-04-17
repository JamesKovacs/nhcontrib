using System;
using System.Web.UI;
using NHibernate.Burrow;
using NHibernate.Burrow.Util;

public partial class SharingConversations_Step03 : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BurrowFramework bf = new BurrowFramework();
        IConversation conversation = bf.CurrentConversation;

        if (!IsPostBack)
        {
            if (conversation == null)
                throw new Exception("The page doesn't have conversation");
            Checker.CheckSpanningConversations(0);
            conversation.SpanWithPostBacks();
            Checker.CheckSpanningConversations(1);
            Session["continue"] = false;
            //if (BurrowFramework.ActiveConversations.Count != 1)
            //    throw new Exception("There are more conversations that the expected");
        }
            //else if (conversation.Items.ContainsKey("continue"))
        else if (Session["continue"] != null && (bool) Session["continue"])
        {
            Session.Remove("continue");
            btnNextStep.Visible = true;
        }
        else
            lblMessage.Text = "You should complete the step 3 a before continue";
    }

    protected void btnNextStep_Click(object sender, EventArgs e)
    {
        Checker.CheckSpanningConversations(1);

        new BurrowFramework().CurrentConversation.FinishSpan();
        Checker.CheckSpanningConversations(0);

        Response.Redirect("Step04.aspx");
    }

    
}