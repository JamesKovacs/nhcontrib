using System;
using System.Web.UI;
using NHibernate.Burrow;
using NHibernate.Burrow.Test.MockEntities;
using NHibernate.Burrow.WebUtil.Attributes;

public partial class Controls_ConversationStates : UserControl
{
    [ConversationalField] protected MockEntity me;
    [EntityField] protected MockEntity meInDb;

    private MockEntity MEInConversation
    {
        get { return me; }
        set
        {
            me = value;
            Session["me"] = value;
        }
    }

    private MockEntity MEInDB
    {
        get { return meInDb; }
        set
        {
            meInDb = value;

            Session["meInDb"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["me"] = null;
            Session["meInDb"] = null;
            Util.ResetEnvironment();
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        lStatus.Text = "MockEntity In Conversation: " + GetNumber(me) + "<br /> MockEntity in DB: " + GetNumber(meInDb);
        bool spanning = new BurrowFramework().CurrentConversation.IsSpanning;
        btnUpdate.Enabled = spanning;
        btnCommit.Enabled = spanning;
        btnCancel.Enabled = spanning;
        btnStart.Enabled = !spanning;
        base.OnPreRender(e);
        Checker.AssertEqual(Session["me"], me);
        Checker.AssertEqual(Session["meInDb"], meInDb);
    }

    private string GetNumber(MockEntity m)
    {
        if (m != null)
        {
            return m.Number.ToString();
        }
        else
        {
            return "NULL";
        }
    }

    protected void btnStart_Click(object sender, EventArgs e)
    {
        MEInConversation = new MockEntity();
        BurrowFramework f = new BurrowFramework();
        f.CurrentConversation.SpanWithPostBacks();
        lConversationStatus.Text = f.CurrentConversation.IsSpanning.ToString();
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        me.Number++;
    }

    protected void btnCommit_Click(object sender, EventArgs e)
    {
        me.Save();
        MEInDB = me;
        MEInConversation = null;
        new BurrowFramework().CurrentConversation.FinishSpan();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        MEInConversation = null;
        new BurrowFramework().CurrentConversation.GiveUp();
    }

    protected void btnRefresh_Click(object sender, EventArgs e) {}
}