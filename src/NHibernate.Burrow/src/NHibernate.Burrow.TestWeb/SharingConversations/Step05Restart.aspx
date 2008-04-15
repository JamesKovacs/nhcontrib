<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Step05Restart.aspx.cs" Inherits="SharingConversations_Step05Restart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>Restart from Step 6</h2>
        <p>Now, we check if shared conversation when adding a page to all (any) conversation</p>
        <asp:Label ID="lblUseCaseId" runat="server" />
        <asp:Button ID="btnNextStep" runat="server" OnClick="btnNextStep_Click" Text="next" />
    </div>
    </form>
</body>
</html>
