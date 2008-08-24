<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ListCustomers.aspx.cs" Inherits="ListCustomers"
    Theme="Theme1" %>
<%@ Register Src="EditCustomer.ascx" TagName="EditCustomer" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>List Customers</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Customer Listing</h2>
            <a href="Default.aspx">return to the homepage</a>.<br />
            <asp:Button ID="btnCreate" runat="server" Text="Create New Customer" OnClick="btnCreate_Click" /><br />
            All customers
            <asp:GridView ID="grdCustomers" runat="server" AutoGenerateColumns="false" AllowPaging="True"
                AllowSorting="True" PageSize="5" DataSourceID="odsAll">
                <Columns>
                    <asp:TemplateField HeaderText="Customer ID" SortExpression="Id">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtSelect" OnCommand="lbt_SelectCommand" CommandArgument='<%# Eval("Id") %>'
                                runat="server"><%# Eval("Id") %></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CompanyName" HeaderText="Company Name" SortExpression="CompanyName" />
                    <asp:BoundField DataField="ContactName" HeaderText="Contact Name" SortExpression="ContactName" />
                    <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                    <asp:BoundField DataField="SalesPotential" HeaderText="Sales Potential"
                         HtmlEncodeFormatString="true" DataFormatString="{0:C}" SortExpression="SalesPotential" />
                    <asp:BoundField DataField="IsActivated" HeaderText="Activated" SortExpression="IsActivated" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsAll" runat="server" TypeName="BasicThreeTier.Core.Dao.CustomerDAO"
                EnablePaging="True" MaximumRowsParameterName="pageSize" StartRowIndexParameterName="startRow"
                SortParameterName="sortExpression" SelectMethod="FindAll" SelectCountMethod="CountAll">
            </asp:ObjectDataSource>
            <br />
            <uc1:EditCustomer ID="EditCustomer1" runat="server" Visible="false" OnUpdated="EditCustomer_Updated" />
        </div>
    </form>
</body>
</html>
