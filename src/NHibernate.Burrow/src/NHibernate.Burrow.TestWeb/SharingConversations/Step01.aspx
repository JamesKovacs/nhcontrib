<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Step01.aspx.cs" Inherits="SharingConversations_Step01" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>We are checking that been shared the conversation between postbacks</h2>
        <asp:Label ID="lblMessage" runat="server" />
        <p>Press 'next' button until change to next step</p>
        <asp:Button ID="btnNextStep" runat="server" OnClick="btnNextStep_Click" Text="Next" />
    </div>
    </form>
</body>
</html>
