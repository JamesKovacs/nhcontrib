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
using NHibernate.Burrow.WebUtil.Attributes;

public partial class ConversationalField_SpanByUrl1 : System.Web.UI.Page
{
	BurrowFramework bf = new BurrowFramework(); 
	private const string spanbyurlmessage = "SpanByUrlMessage";
	protected string message
	{

		get {
			
			return (string)bf.CurrentConversation.Items[spanbyurlmessage];
		}
		set
		{
			bf.CurrentConversation.Items[spanbyurlmessage] = value;
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack ) {
			if (string.IsNullOrEmpty(Request["returned"]))
				bf.CurrentConversation.SpanWithPostBacks();
			else  {
				Checker.AssertEqual(message, "1");
				SuccessInfo1.Visible = true;
				btnStart.Visible = false;
				bf.CurrentConversation.FinishSpan();
			}
			
		}
	}
	protected void btnStart_Click(object sender, EventArgs e)
	{
		message = "1";
		string backUrl = new NHibernate.Burrow.Util.WebUtil().WrapUrlWithConversationInfo("SpanByUrl1.aspx");
		Response.Redirect("SpanByUrl2.aspx?returnUrl=" + HttpUtility.UrlEncode( backUrl+"&returned=1") );
	}
}
