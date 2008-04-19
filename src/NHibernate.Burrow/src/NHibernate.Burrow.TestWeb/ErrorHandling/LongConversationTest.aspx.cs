using System;
using System.Web.UI;
using NHibernate.Burrow;

public partial class ErrorHandling_ConversationErrorHandling : Page
{
    private BurrowFramework f = new BurrowFramework();

    protected ErrorTestStatus Status
    {
        get { return (ErrorTestStatus) Session["Status"]; }
        set { Session["Status"] = value; }
    }

    private int conversationNum
    {
        get { return Session["conversationNum"] != null ? (int) Session["conversationNum"] : 0; }
        set { Session["conversationNum"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Status = ErrorTestStatus.Unknown;
            conversationNum = 0;
            Util.ResetEnvironment();
        }
        lStatus.Text = Status.ToString();
    }

    protected override void OnPreRender(EventArgs e)
    {
        if (Status == ErrorTestStatus.ErrorOccurred)
        {
            lMessage.Text = "Succeed";
        }
        Checker.CheckSpanningConversations(conversationNum);

        base.OnPreRender(e);
    }
}