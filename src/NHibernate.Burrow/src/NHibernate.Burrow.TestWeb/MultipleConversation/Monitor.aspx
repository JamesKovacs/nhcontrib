<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Monitor.aspx.cs" Inherits="MultipleConversation_Monitor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
       <asp:Button ID="btnRefresh" runat="server" Text="Refresh" />
              <asp:Label ID="lStatus" runat="server" Text="Click buttons in each frame to start tests"></asp:Label>
    </div>
    </form>
</body>
</html>
