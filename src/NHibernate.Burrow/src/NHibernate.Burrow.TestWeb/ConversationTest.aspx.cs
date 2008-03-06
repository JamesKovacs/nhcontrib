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
using NHibernate.Burrow.Test.MockEntities;
using NHibernate.Burrow.WebUtil;

public partial class ConversationTest : System.Web.UI.Page
{
    [ConversationalField] protected MockEntity me;
    [EntityFieldNullSafe] protected MockEntity meInDb;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected override void OnPreRender(EventArgs e)
    {
        lStatus.Text = "MockEntity In Conversation: " + GetNumber(me) + 
                        "<br /> MockEntity in DB: " + GetNumber(meInDb);
        btnUpdate.Enabled = me != null;
        btnCommit.Enabled = me != null;
        base.OnPreRender(e);
    }

    private string GetNumber(MockEntity m)
    {
        if (m != null)
            return m.Number.ToString();
        else return "NULL";
    }
    protected void btnStart_Click(object sender, EventArgs e)
    {
        me = new MockEntity();
        Facade.StarLongConversation(); 

        
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        me.Number++;
    }
    protected void btnCommit_Click(object sender, EventArgs e)
    {
        me.Save();
        meInDb = me;
        me = null;
        Facade.FinishOverSpanConversation();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        me = null;
        Facade.CancelConversation();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {

    }
}
