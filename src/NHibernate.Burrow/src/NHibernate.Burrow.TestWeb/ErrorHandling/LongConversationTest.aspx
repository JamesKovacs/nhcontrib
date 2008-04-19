<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LongConversationTest.aspx.cs" Inherits="ErrorHandling_ConversationErrorHandling" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Click the link to start testing
        <a href="ChildrenPages/BreakConversation.aspx" target="_blank" >BreakConversation </a> &nbsp;&nbsp; <a
            href="ChildrenPages/BreakConversationAjax.aspx"  target="_blank" > BreakConversation Ajax </a>
        <br />Status:  <asp:Literal ID="lStatus" runat="server"></asp:Literal>
        <br />
        <asp:Button ID="btnRefresh" runat="server" Text="Refresh" />
        <asp:Literal ID="lMessage" runat="server"></asp:Literal>
    </div>
    </form>
</body>
</html>
