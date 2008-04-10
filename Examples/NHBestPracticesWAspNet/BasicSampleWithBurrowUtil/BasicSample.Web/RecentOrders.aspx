<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RecentOrders.aspx.cs" Inherits="RecentOrders" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Order placed today<br />
           This is a sortable paginated gridview with some searching criteria. 
            <asp:GridView ID="grdOrders" runat="server" AutoGenerateColumns="false" DataSourceID="odsRecentOrders"
             AllowPaging="true" PageSize="5" AllowSorting="true" >
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="Order ID" SortExpression="ID" />
                    <asp:BoundField DataField="OrderedBy" HeaderText="Customer"  SortExpression="OrderedBy" />
                    <asp:BoundField DataField="ShipToName" HeaderText="Shipped To" SortExpression="ShipToName"  />
                    <asp:BoundField DataField="OrderDate" HeaderText="Order Date"  SortExpression="OrderDate" />
                </Columns>
                <EmptyDataTemplate>
                    There is no order placed today
                </EmptyDataTemplate>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsRecentOrders" runat="server" TypeName="BasicSample.Data.OrderDao"
                EnablePaging="True" MaximumRowsParameterName="pageSize" StartRowIndexParameterName="startRow"
                SortParameterName="sortExpression" SelectMethod="GetOrdersPlacedBetween" SelectCountMethod="CountOrdersPlacedBetween">
                <SelectParameters>
                    <asp:Parameter name="startDate"  Type="datetime" />
                    <asp:Parameter name="endDate"  Type="datetime" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </form>
</body>
</html>
