<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Step06.aspx.cs" Inherits="SharingConversations_Step06" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <p>We have successful checked that conversation is shared between two page adding the second page to all (any) conversation</p>
        <h2>Now, we will check a hyperlink with target="_blank" to other page with diferent conversation</h2>
        <br />
        Please click Step 6a, 6b, 6c and close them before you click refresh here
        <asp:Label ID="lblMessage" runat="server" />
        <asp:Hyperlink ID="lna" runat="server" Text="Step 6a" NavigateUrl="~/SharingConversations/Step06a.aspx" Target="_blank" />
        <asp:Hyperlink ID="lnb" runat="server" Text="Step 6b" NavigateUrl="~/SharingConversations/Step06b.aspx" Target="_blank" />
        <asp:Hyperlink ID="lnc" runat="server" Text="Step 6c" NavigateUrl="~/SharingConversations/Step06c.aspx" Target="_blank" />
        <asp:Button ID="btnRefresh" runat="server" Text="Refresh" />
        <asp:Button ID="btnNextStep" runat="server" OnClick="btnNextStep_Click" Text="next" Visible="false" />
    </div>
    </form>
</body>
</html>
