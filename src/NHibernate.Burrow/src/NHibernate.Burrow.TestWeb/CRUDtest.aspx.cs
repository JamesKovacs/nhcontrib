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
using NHibernate.Burrow.Test.MockEntities;
using NHibernate.Burrow.Test.UtilTests.DAO;
using NHibernate.Burrow.WebUtil.Attributes;

public partial class CRUDtest : System.Web.UI.Page
{
    [EntityFieldDeletionSafe] protected MockEntity me;

    protected void Page_Load(object sender, EventArgs e)
    {
       
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
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if(me != null)
            me.Delete();
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

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
    }
}
