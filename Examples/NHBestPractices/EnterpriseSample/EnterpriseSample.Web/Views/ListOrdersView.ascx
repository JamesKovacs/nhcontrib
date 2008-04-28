<%@ Import namespace="System.Collections.Generic"%>
<%@ Import namespace="EnterpriseSample.Core.Domain"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ListOrdersView.ascx.cs" Inherits="Views_ListOrdersView" %>

<h2>Past Orders</h2>

<asp:GridView ID="grdOrders" runat="server" AutoGenerateColumns="false">
    <Columns>
        <asp:BoundField DataField="ID" HeaderText="Order ID" />
        <asp:BoundField DataField="ShipToName" HeaderText="Shipped To" />
        <asp:BoundField DataField="OrderDate" HeaderText="Order Date" />
    </Columns>
</asp:GridView>
