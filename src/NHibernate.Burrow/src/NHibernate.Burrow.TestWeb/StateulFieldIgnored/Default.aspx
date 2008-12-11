<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="StatefulFieldPerformance_Default" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    This is a page to test ignoring of StatefulField management <br />
        <asp:Label ID="lSuccess" runat="server" Text="Congratulations! Test passed." Visible="false"></asp:Label>
        <asp:Button ID="btnContinue" OnClick="Continue" runat="server" Text="Continue" />
    </div>
       
    </form>
</body>
</html>
