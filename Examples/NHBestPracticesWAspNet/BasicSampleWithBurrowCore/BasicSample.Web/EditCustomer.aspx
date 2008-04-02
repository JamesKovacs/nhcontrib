<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditCustomer.aspx.cs" Inherits="EditCustomer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Edit Customer</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>Edit Customer</h2>
        Use this form to update the customer's information.<br />
        <br />
        
        <table>
            <tr>
                <td>Customer ID:</td>
                <td>
                    <asp:Label ID="lblCustomerID" runat="server" />
                    <input type="hidden" id="hidCustomerID" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Company Name:</td>
                <td>
                    <asp:TextBox ID="txtCompanyName" runat="server" MaxLength="40" />
                    <asp:RequiredFieldValidator ControlToValidate="txtCompanyName" runat="server"
                        ErrorMessage="Company name must be provided" />
                </td>
            </tr>
            <tr>
                <td>Contact Name:</td>
                <td>
                    <asp:TextBox ID="txtContactName" runat="server" MaxLength="30" />
                    <asp:RequiredFieldValidator ControlToValidate="txtContactName" runat="server"
                        ErrorMessage="Company name must be provided" />
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                    <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_OnClick" Text="Update" /> 
                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_OnClick" Text="Cancel" CausesValidation="false" />
                </td>
            </tr>
        </table>

        <hr />

        <h2>Past Orders</h2>
        <asp:GridView ID="grdOrders" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="Order ID" />
                <asp:BoundField DataField="ShipToName" HeaderText="Shipped To" />
                <asp:BoundField DataField="OrderDate" HeaderText="Order Date" />
            </Columns>
        </asp:GridView>

        <hr />

        <h2>Products Ordered</h2>
        <asp:GridView ID="grdProductsOrdered" runat="server" />
    </div>
    </form>
</body>
</html>
