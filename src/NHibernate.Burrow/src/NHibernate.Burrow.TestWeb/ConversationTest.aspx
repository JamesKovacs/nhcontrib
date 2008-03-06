<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConversationTest.aspx.cs" Inherits="ConversationTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Literal ID="lStatus" runat="server"></asp:Literal><br />
        <asp:Button ID="btnStart" runat="server" Text="Start" OnClick="btnStart_Click" />
        <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" />
        <asp:Button ID="btnCommit" runat="server" Text="Commit" OnClick="btnCommit_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
        <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click" />
        
    </div>
    </form>
</body>
</html>
