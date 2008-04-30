<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="StatefulFieldPerformance_Default" %>

<%@ Register Src="WebUserControl.ascx" TagName="WebUserControl" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    This is a page to profile StatefulField management 
    <br />
    <asp:Repeater ID="Repeater1" runat="server">
    <ItemTemplate>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
        <asp:LinkButton ID="LinkButton1" runat="server">LinkButton</asp:LinkButton>
        <asp:Literal   ID="Literal1" runat="server"></asp:Literal>
    </ItemTemplate>
        </asp:Repeater>
        <uc1:WebUserControl ID="WebUserControl1" runat="server" />
    </div>
       
    </form>
</body>
</html>
