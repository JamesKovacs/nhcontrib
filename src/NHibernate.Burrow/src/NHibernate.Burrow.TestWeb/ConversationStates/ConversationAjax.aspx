<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConversationAjax.aspx.cs" Inherits="ConversationAjax" %>

<%@ Register Src="Controls/ConversationStates.ascx" TagName="ConversationStates"
    TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1"   runat="server" EnablePartialRendering="true">
        </asp:ScriptManager>
    <div>
       <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
           <uc1:ConversationStates ID="ConversationStates1" runat="server" />
    
       </ContentTemplate>
</asp:UpdatePanel>

    </div>
    </form>
</body>
</html>
