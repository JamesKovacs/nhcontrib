<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ListSuppliers.aspx.cs" Inherits="ListSuppliers" %>
<%@ Import namespace="BasicSample.Core.Domain"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>List Suppliers</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>Supplier Listing</h2>
        Click here to <a href="Default.aspx">return to the homepage</a>.<br />
        <br />
        <asp:GridView ID="grdSuppliers" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="Supplier ID" />
                <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                <asp:BoundField DataField="ContactName" HeaderText="Contact Name" />
                <asp:TemplateField HeaderText="Products">
                    <ItemTemplate>
                        <asp:GridView runat="server" DataSource="<%# ((Supplier) Container.DataItem).Products %>" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
