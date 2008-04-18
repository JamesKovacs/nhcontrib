<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ListCustomers.aspx.cs" Inherits="ListCustomers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>List Customers</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>Customer Listing</h2>
        <asp:Label ID="lblMessage" runat="server" /> <a href="AddCustomer.aspx">Click here</a> to add a new customer.  You can also <a href="Default.aspx">return to the homepage</a>.<br />
        <br />
        <asp:GridView ID="grdEmployees" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderText="Customer ID">
                    <ItemTemplate>
                        <a href="EditCustomer.aspx?customerID=<%# Eval("ID") %>"><%# Eval("ID") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                <asp:BoundField DataField="ContactName" HeaderText="Contact Name" />
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
