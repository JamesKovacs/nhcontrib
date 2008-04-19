using System;
using System.Web.UI;

public partial class ErrorHandling : Page
{
    protected void Page_Load(object sender, EventArgs e) {}

    protected void btnException_Click(object sender, EventArgs e)
    {
        throw new Exception("Exception thrown by the btnException");
    }
}