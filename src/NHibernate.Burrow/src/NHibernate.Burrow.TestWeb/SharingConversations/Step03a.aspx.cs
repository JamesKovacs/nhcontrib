using System;
using System.Web.UI;
using NHibernate.Burrow;

public partial class SharingConversations_Step03a : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BurrowFramework bf = new BurrowFramework();
            IConversation conversation = bf.CurrentConversation;

            if (conversation == null)
            {
                throw new Exception("The page doesn't have conversation");
            }
            Checker.CheckSpanningConversations(1);

            conversation.SpanWithPostBacks();

            Checker.CheckSpanningConversations(2);

            //if (BurrowFramework.ActiveConversations.Count != 2)
            //    throw new Exception("There are more conversations that the expected");            }
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        BurrowFramework bf = new BurrowFramework();
        IConversation conversation = bf.CurrentConversation;
        Session["continue"] = true;

        conversation.FinishSpan();
        Checker.CheckSpanningConversations(1);

        hdClose.Value = "1";
    }
}