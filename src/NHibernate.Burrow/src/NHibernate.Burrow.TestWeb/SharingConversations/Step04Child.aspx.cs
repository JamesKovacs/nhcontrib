using System;
using System.Web.UI;
using NHibernate.Burrow;

public partial class SharingConversations_Step04Child : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Facade facade = new Facade();
        IConversation conversation = facade.CurrentConversation;

        if (conversation == null)
            throw new Exception("The page doesn't have conversation");

        if (Session["conversationId"] == null)
            throw new Exception("We haven't found the Id's conversation in the ASP.NET session");

        object id = Session["conversationId"];

        if (!conversation.Id.Equals(id))
            throw new Exception("The conversation in iframe isn't same that conversation in container page. Current.Id " +
                                conversation.Id);

        lblConversationId.Text = conversation.Id.ToString();
    }
}