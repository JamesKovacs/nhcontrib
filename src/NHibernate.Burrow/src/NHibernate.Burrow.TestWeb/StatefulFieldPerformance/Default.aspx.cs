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

public partial class StatefulFieldPerformance_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int[] source = new int[5];
        for (int i = 0; i < source.Length; i++)
        {
            source[i] = i;
        }
        Repeater1.DataSource = source;
        Repeater1.DataBind();
    }
}
