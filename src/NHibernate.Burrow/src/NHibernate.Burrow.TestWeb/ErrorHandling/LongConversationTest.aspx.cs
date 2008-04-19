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

public partial class ErrorHandling_ConversationErrorHandling : System.Web.UI.Page
{
    BurrowFramework f = new BurrowFramework();

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

    protected ErrorTestStatus Status
    {
        get { return (ErrorTestStatus)Session["Status"]; }
        set { Session["Status"] = value; }
    }

    int conversationNum
    {
        get { return Session["conversationNum"] != null ? (int)Session["conversationNum"] : 0; }
        set { Session["conversationNum"] = value; }
    }



    protected override void OnPreRender(EventArgs e)
    {
        if(Status == ErrorTestStatus.ErrorOccurred)
            lMessage.Text = "Succeed";
        Checker.CheckSpanningConversations(conversationNum);

        base.OnPreRender(e);
    }
}
