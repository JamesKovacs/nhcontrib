using System;
using System.Web.UI;
using NHibernate.Burrow.Test.MockEntities;
using NHibernate.Burrow.WebUtil.Attributes;

public partial class CRUDtest : Page
{
    [EntityFieldDeletionSafe] protected MockEntity me;

    protected void Page_Load(object sender, EventArgs e)
    {
        Checker.AssertEqual(Session["CRUDtestME"], me);
    }

    protected override void OnInit(EventArgs e)
    {
        if (!IsPostBack)
        {
            Util.ResetEnvironment();
            Session["CRUDtestME"] = null;
        }

        base.OnInit(e);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        me.Name = tbName.Text;
        me.Number = Convert.ToInt32(tbNumber.Text);
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        me = new MockEntity();
        me.Name = "New Mock entity";
        me.Save();
        Session["CRUDtestME"] = me;
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (me != null)
        {
            me.Delete();
        }
        Session["CRUDtestME"] = null;
        me = new MockEntityDAO().Get(me.Id);
    }

    protected override void OnPreRender(EventArgs e)
    {
        bool hasData = me != null;
        lName.Text = hasData ? me.Name : string.Empty;
        lNumber.Text = hasData ? me.Number.ToString() : string.Empty;
        btnDelete.Enabled = hasData;
        btnSave.Enabled = hasData;
        base.OnPreRender(e);
    }

    protected void btnRefresh_Click(object sender, EventArgs e) {}
}