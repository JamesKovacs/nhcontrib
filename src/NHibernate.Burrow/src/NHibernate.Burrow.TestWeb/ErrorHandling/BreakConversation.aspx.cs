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

public partial class ErrorHandling_BreakConversation : System.Web.UI.Page
{
    ErrorTestStatus Status
    {
        get { return (ErrorTestStatus) Session["Status"]; }
        set { Session["Status"] = value; }
    }

    Facade f = new Facade();
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            Session["Status"] = ErrorTestStatus.ConversationStarted;
            f.CurrentConversation.SpanWithPostBacks();
        }
    }
    protected void btnBreak_Click(object sender, EventArgs e)
    {
        Status = ErrorTestStatus.ErrorOccurred;
        throw new Exception("Exception thrown to break conversation");
    }
}
