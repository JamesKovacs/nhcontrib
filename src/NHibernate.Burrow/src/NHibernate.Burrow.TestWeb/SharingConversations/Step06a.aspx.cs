using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NHibernate.Burrow;

public partial class SharingConversations_Step07 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Facade facade = new Facade();
            IConversation conversation = facade.CurrentConversation;

            if (conversation == null)
                throw new Exception("The page doesn't have conversation");
            
            object lastConversationId = Session["conversationId"];
            if (lastConversationId == null)
                throw new Exception("We haven't found the Id of previous conversation");

            if (conversation.Id.Equals(lastConversationId))
                throw new Exception("The conversation is same that previous, the new conversation was not created");

            Checker.CheckSpanningConversations(1);

            conversation.SpanWithPostBacks();
            
            Checker.CheckSpanningConversations(2);

            conversation.FinishSpan();
            Checker.CheckSpanningConversations(1);
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
