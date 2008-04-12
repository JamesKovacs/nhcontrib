using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for ErrorTestStatus
/// </summary>
public enum ErrorTestStatus
{
    Unknown = 0,
    ConversationStarted = 1,
    ErrorOccurred = 2
}
