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
using NHibernate.Burrow.WebUtil.Attributes;

[WorkSpaceInfo("WorkSpaceStep06")]
public partial class SharingConversations_Step06b : System.Web.UI.Page
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

            if (!conversation.Id.Equals(lastConversationId))
                throw new Exception("Failed to join the conversation in the same workspace, a new conversation was  created");
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
    
        Session["continue_b"] = true;
        hdClose.Value = "1";
    }
    
}
