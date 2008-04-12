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
    Facade f = new Facade();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            Status = ErrorTestStatus.Unknown;
            f.CloseDomain();
            f.BurrowEnvironment.ShutDown();
            f.BurrowEnvironment.Start();
            f.InitializeDomain();
        }
    }

    protected ErrorTestStatus Status
    {
        get { return (ErrorTestStatus)Session["Status"]; }
        set { Session["Status"] = value; }
    }


    protected override void OnPreRender(EventArgs e)
    {
        int expectedConversation;
        switch(Status)
        {
           
            case ErrorTestStatus.ConversationStarted:
                expectedConversation = 1;
                lMessage.Text = "Test started";
                break;
            case ErrorTestStatus.ErrorOccurred:
                expectedConversation = 0;
                lMessage.Text = "Test passed"; //it has not passed at this point, but this won't show up if it does not.
                break;
            default:
                expectedConversation = 0;
                break;
        }
        Checker.CheckSpanningConversations(expectedConversation);

        base.OnPreRender(e);
    }
}
