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
using NHibernate.Burrow.WebUtil.Attributes;

[IgnoreStatefulFields]
public partial class StatefulFieldPerformance_Default : System.Web.UI.Page
{

    [StatefulField] protected int start = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            start = 1;
    }

    protected void Continue(object sender, EventArgs e)
    {
        Checker.AssertEqual(0, start);
        lSuccess.Visible = true;
    }
}
