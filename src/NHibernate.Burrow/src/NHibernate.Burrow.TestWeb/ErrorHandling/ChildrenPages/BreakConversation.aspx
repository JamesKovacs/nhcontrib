<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BreakConversation.aspx.cs" Inherits="ErrorHandling_BreakConversation" %>

<%@ Register Src="../Controls/ConversationBreaker.ascx" TagName="ConversationBreaker"
    TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:ConversationBreaker id="ConversationBreaker1" runat="server">
        </uc1:ConversationBreaker></div>
    </form>
</body>
</html>
