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

public partial class Controls_ConversationBreaker : System.Web.UI.UserControl
{
    ErrorTestStatus Status
    {
        get { return (ErrorTestStatus)Session["Status"]; }
        set { Session["Status"] = value; }
    }
     
    int conversationNum
    {
        get { return (int)Session["conversationNum"]; }
        set { Session["conversationNum"] = value; }
    }



    Facade f = new Facade();
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
