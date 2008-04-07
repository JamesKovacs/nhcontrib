using System;
using System.Web.UI;
using NHibernate.Burrow;

public partial class SharingConversations_Step03a : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Facade facade = new Facade();
            IConversation conversation = facade.CurrentConversation;

            if (conversation == null)
                throw new Exception("The page doesn't have conversation");
            Checker.CheckSpanningConversations(1);

            conversation.SpanWithPostBacks();

            Checker.CheckSpanningConversations(2);

            //if (Facade.ActiveConversations.Count != 2)
            //    throw new Exception("There are more conversations that the expected");            }
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        Facade facade = new Facade();
        IConversation conversation = facade.CurrentConversation;
        Session["continue"] = true;

        conversation.FinishSpan();
        Checker.CheckSpanningConversations(1);

        hdClose.Value = "1";
    }
}