<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="SharingConversations_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>These pages share conversations</h1>
        <p>You have do clik on buttons and links until congratulation's page</p>
        <asp:HyperLink ID="lnkStep01" runat="server" NavigateUrl="Step01.aspx" Text="Start" />
    </div>
    </form>
</body>
</html>
