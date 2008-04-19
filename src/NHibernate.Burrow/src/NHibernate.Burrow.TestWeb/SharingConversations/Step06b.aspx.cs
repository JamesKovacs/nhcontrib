using System;
using System.Web.UI;
using NHibernate.Burrow;
using NHibernate.Burrow.WebUtil.Attributes;

[WorkSpaceInfo("WorkSpaceStep06")]
public partial class SharingConversations_Step06b : Page
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

            object lastConversationId = Session["conversationId"];
            if (lastConversationId == null)
            {
                throw new Exception("We haven't found the Id of previous conversation");
            }

            if (!conversation.Id.Equals(lastConversationId))
            {
                throw new Exception(
                    "Failed to join the conversation in the same workspace, a new conversation was  created");
            }
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        Session["continue_b"] = true;
        hdClose.Value = "1";
    }
}