using System;
using System.Web.UI;

public partial class SharingConversations_TheEnd : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Checker.CheckSpanningConversations(0);
    }
}