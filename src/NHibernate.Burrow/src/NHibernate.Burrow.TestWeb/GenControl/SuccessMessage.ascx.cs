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

public partial class GenControl_SuccessMessage : System.Web.UI.UserControl
{
    protected override void OnInit(EventArgs e)
    {
        if(!IsPostBack)
        {
            Visible = false;
        }
        base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    public void Show()
    {
        Visible = true;
    }
}
