using System;
using System.Web.UI;
using NHibernate.Burrow;

public partial class SharingConversations_Step03 : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Facade facade = new Facade();
        IConversation conversation = facade.CurrentConversation;

        if (!IsPostBack)
        {
            if (conversation == null)
                throw new Exception("The page doesn't have conversation");

            conversation.SpanWithPostBacks();

            //if (Facade.ActiveConversations.Count != 1)
            //    throw new Exception("There are more conversations that the expected");
        }
            //else if (conversation.Items.ContainsKey("continue"))
        else if (Session["continue"] != null)
            btnNextStep.Visible = true;
        else
            lblMessage.Text = "You should complete the step 3 a before continue";
    }

    protected void btnNextStep_Click(object sender, EventArgs e)
    {
        Response.Redirect("Step04.aspx");
    }
}