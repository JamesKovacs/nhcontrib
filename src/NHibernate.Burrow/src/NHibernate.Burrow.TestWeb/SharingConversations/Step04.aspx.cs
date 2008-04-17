using System;
using System.Web.UI;
using NHibernate.Burrow;
using NHibernate.Burrow.Util;

public partial class SharingConversations_Step04 : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BurrowFramework bf = new BurrowFramework();
            IConversation conversation = bf.CurrentConversation;
            Checker.CheckSpanningConversations(0);

            if (conversation == null)
                throw new Exception("The page doesn't have conversation");

            conversation.SpanWithPostBacks();
            Checker.CheckSpanningConversations(1);

            Session["conversationId"] = conversation.Id;
            lblConversationId.Text = "Current: " + conversation.Id;

            WebUtil utils = new WebUtil();
            frameChild.Attributes["src"] = utils.WrapUrlWithConversationInfo("Step04Child.aspx");
        }
    }

    protected void btnNextStep_Click(object sender, EventArgs e)
    {
        BurrowFramework bf = new BurrowFramework();
        IConversation conversation = bf.CurrentConversation;
        Checker.CheckSpanningConversations(1);

        Session["conversationId"] = conversation.Id;
        //using string.Empty as workspace name will span the conversation over all pages, NOT recommendded though
        conversation.SpanWithCookie(String.Empty);
        Checker.CheckSpanningConversations(1);

        Response.Redirect("Step05.aspx");
    }
}