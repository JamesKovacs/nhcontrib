<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConversationNormal.aspx.cs" Inherits="ConversationTest" %>

<%@ Register Src="Controls/ConversationStates.ascx" TagName="ConversationStates"
    TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:ConversationStates id="ConversationStates1" runat="server">
        </uc1:ConversationStates></div>
    </form>
</body>
</html>
