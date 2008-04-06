<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Step03.aspx.cs" Inherits="SharingConversations_Step03" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <p>We have successfully checked the change of conversation at do a hyperlink</p>
        <h2>The next step will check the change of conversation at do clic on hyperlink with target="_blank"</h2>
        <p>You should do clic on hyperlink, then will open other IE's window. After, you have to press button "refresh" and then you will view button "next", you press its!</p>
        <asp:Label ID="lblMessage" runat="server" />
        <asp:HyperLink ID="lnkStep03" runat="server" Text="Step 3.a" NavigateUrl="Step03a.aspx" Target="_blank" />
        <asp:Button ID="btnRefresh" runat="server" Text="Refresh" />
        <asp:Button ID="btnNextStep" runat="server" OnClick="btnNextStep_Click" Text="next" Visible="false" />
    </div>
    </form>
</body>
</html>
