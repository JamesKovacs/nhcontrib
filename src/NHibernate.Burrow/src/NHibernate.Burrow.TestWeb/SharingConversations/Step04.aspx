<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Step04.aspx.cs" Inherits="SharingConversations_Step04" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <p>We have successfully checked the shared a conversation between a father page and a child page</p>
        <p>Press button to go next step</p>
        <asp:Label ID="lblConversationId" runat="server" /><br />
        <iframe id="frameChild" runat="server" /><br />
        <h2>Now, we will check if shared conversation when adding a page to conversation</h2>
        <asp:Button ID="btnNextStep" runat="server" OnClick="btnNextStep_Click" Text="next" />
    </div>
    </form>
</body>
</html>
