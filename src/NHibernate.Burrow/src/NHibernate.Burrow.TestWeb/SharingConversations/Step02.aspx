<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Step02.aspx.cs" Inherits="SharingConversations_Step02" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <p>We have successfully checked the change of conversation at do a redirect</p>
        <h2>The next step will check the change of conversation at do clic on hyperlink</h2>
        <asp:HyperLink ID="lnkStep03" runat="server" Text="Next" NavigateUrl="Step03.aspx" />
    </div>
    </form>
</body>
</html>
