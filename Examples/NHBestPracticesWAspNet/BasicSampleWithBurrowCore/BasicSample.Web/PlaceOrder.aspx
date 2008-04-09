<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlaceOrder.aspx.cs" Inherits="PlaceOrder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:PlaceHolder ID="phSelectCustomer" runat="server" Visible="true">
            Select a customer who wants to place an order (will be the OrderBy)<br /> 
                <asp:DropDownList ID="ddlCustomers" runat="server" DataTextField="ContactName" DataValueField="ID">
                </asp:DropDownList>
                <asp:Button ID="btnSelectCustomer" runat="server" Text="Next" OnClick="btnSelectCustomer_Click" />
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phEnterShipToName" runat="server" Visible="false">Please enter
                your ship to information:
                <asp:TextBox ID="tbShipToName" runat="server"></asp:TextBox>
                <asp:Button ID="btnEnterShipTo" runat="server" Text="Next" OnClick="btnEnterShipTo_Click" /><br />
                <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="cancel"></asp:Button>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phConfirm" runat="server" Visible="false">Customer:
                <asp:Literal ID="lCustomer" runat="server"></asp:Literal>
                <br />
                Ship to:
                <asp:Literal ID="lShipTo" runat="server"></asp:Literal>
                <br />
                <asp:Button ID="btnConfirm" runat="server" Text="Confirm" OnClick="btnConfirm_Click" />
            </asp:PlaceHolder>
            <br /><br /><br />
            <asp:HyperLink ID="hlOrders" Target="_blank" NavigateUrl="~/recentorders.aspx" runat="server">Show recent orders</asp:HyperLink>
        </div>
    </form>
</body>
</html>
