using System;
using System.Web.UI;
using NHibernate.Burrow;
using NHibernate.Burrow.Test.MockEntities;
using NHibernate.Burrow.WebUtil.Attributes;

public partial class MultipleConversation_Control_MultipleConversationTest : UserControl
{
    private BurrowFramework bf = new BurrowFramework();
    [ConversationalField] protected MockEntity me;

    private int currentStep
    {
        get
        {
            if (ViewState["currentStep"] == null)
            {
                ViewState["currentStep"] = 0;
            }
            return (int) ViewState["currentStep"];
        }
        set { ViewState["currentStep"] = value; }
    }

    public bool currentStepDone
    {
        get
        {
            if (ViewState["currentStepDone"] == null)
            {
                ViewState["currentStepDone"] = false;
            }
            return (bool) ViewState["currentStepDone"];
        }
        set { ViewState["currentStepDone"] = value; }
    }

    private int currentStepProgress
    {
        get
        {
            return
                Session["MultipleConversationStep" + currentStep] is int
                    ? (int) Session["MultipleConversationStep" + currentStep]
                    : 0;
        }
        set { Session["MultipleConversationStep" + currentStep] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lPage.Text = Request.Path;
        }
    }

    protected void btnStart_Click(object sender, EventArgs e)
    {
        if (currentStepDone)
        {
            if (currentStepProgress < 4)
            {
                lStatus.Text = "Step " + currentStep + " is not done by other pages yet, finish them first";
                return;
            }
            else
            {
                currentStep++;
                currentStepDone = false;
            }
        }

        if (currentStep == 0)
        {
            bf.CurrentConversation.SpanWithPostBacks();
            me = new MockEntity();
            Checker.CheckSpanningConversations(currentStepProgress + 1);
        }

        if (currentStep < 3)
        {
            Checker.AssertEqual(me.Number, currentStep);
            me.Number++;
            lStatus.Text = "Step " + currentStep + " is done for this page";
        }
        else
        {
            me.Save();
            me = null;
            bf.CurrentConversation.FinishSpan();
            lStatus.Text = "This page is finished! ";
            btnStart.Visible = false;
        }
        currentStepDone = true;
        currentStepProgress++;
        btnStart.Text = "Go Step " + (currentStep + 1);
    }
}