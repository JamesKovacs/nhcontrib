<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RecentOrders.aspx.cs" Inherits="RecentOrders" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Order placed today 
         <asp:GridView ID="grdOrders" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="Order ID" />
                <asp:BoundField DataField="OrderedBy" HeaderText="Customer" />
                <asp:BoundField DataField="ShipToName" HeaderText="Shipped To" />
                <asp:BoundField DataField="OrderDate" HeaderText="Order Date" />
            </Columns>
            <EmptyDataTemplate>
            There is no order placed today
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
