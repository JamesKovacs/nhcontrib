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
            Facade facade = new Facade();
            IConversation conversation = facade.CurrentConversation;

            if (conversation == null)
                throw new Exception("The page doesn't have conversation");

            conversation.SpanWithPostBacks();

            Session["conversationId"] = conversation.Id;
            lblConversationId.Text = "Current: " + conversation.Id;

            UrlUtil utils = new UrlUtil();
            frameChild.Attributes["src"] = utils.WrapUrlWithConversationInfo("Step04Child.aspx");
        }
    }

    protected void btnNextStep_Click(object sender, EventArgs e)
    {
        Facade facade = new Facade();
        IConversation conversation = facade.CurrentConversation;

        Session["conversationId"] = conversation.Id;
        //Facade.addPageToAllUseCase("/Step06.aspx");
        //conversation.addPageToUseCase("/Step05.aspx");
        conversation.SpanWithHttpSession();
        Response.Redirect("Step05.aspx");
    }
}