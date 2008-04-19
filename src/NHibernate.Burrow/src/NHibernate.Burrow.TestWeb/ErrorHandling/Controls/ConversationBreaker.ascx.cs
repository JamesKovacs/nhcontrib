using System;
using System.Web.UI;
using NHibernate.Burrow;

public partial class Controls_ConversationBreaker : UserControl
{
    private BurrowFramework f = new BurrowFramework();

    private ErrorTestStatus Status
    {
        get { return (ErrorTestStatus) Session["Status"]; }
        set { Session["Status"] = value; }
    }

    private int conversationNum
    {
        get { return (int) Session["conversationNum"]; }
        set { Session["conversationNum"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["Status"] = ErrorTestStatus.ConversationStarted;
            f.CurrentConversation.SpanWithPostBacks();
            conversationNum++;
        }
    }

    protected void btnBreak_Click(object sender, EventArgs e)
    {
        Status = ErrorTestStatus.ErrorOccurred;
        conversationNum--;
        throw new Exception("Exception thrown to break conversation");
    }
}