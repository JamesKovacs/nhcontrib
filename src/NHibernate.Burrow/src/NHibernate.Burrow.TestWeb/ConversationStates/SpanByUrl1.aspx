<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SpanByUrl1.aspx.cs" Inherits="ConversationalField_SpanByUrl1" %>

<%@ Register Src="../Controls/FailedInfo.ascx" TagName="FailedInfo" TagPrefix="uc2" %>

<%@ Register Src="../Controls/SuccessInfo.ascx" TagName="SuccessInfo" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnStart" runat="server" Text="Start" OnClick="btnStart_Click" />
        <uc1:SuccessInfo ID="SuccessInfo1" runat="server" Visible="false" />
        <uc2:FailedInfo ID="FailedInfo1" runat="server" Visible="false" />
    </div>
    </form>
</body>
</html>
