<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddCustomer.aspx.cs" Inherits="AddCustomer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>Add Customer</h2>
        <asp:Label ID="lblMessage" runat="server" /><br />
        <br />

        <table>
            <tr>
                <td>Customer ID:</td>
                <td>
                    <asp:TextBox ID="txtCustomerID" runat="server" MaxLength="5" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtCustomerID" runat="server"
                        ErrorMessage="Customer ID must be provided" />
                </td>
            </tr>
            <tr>
                <td>Company Name:</td>
                <td>
                    <asp:TextBox ID="txtCompanyName" runat="server" MaxLength="40" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtCompanyName" runat="server"
                        ErrorMessage="Company name must be provided" />
                </td>
            </tr>
            <tr>
                <td>Contact Name:</td>
                <td>
                    <asp:TextBox ID="txtContactName" runat="server" MaxLength="30" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtContactName" runat="server"
                        ErrorMessage="Company name must be provided" />
                </td>
            </tr>
            <tr>
                <td align="center"><asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_OnClick" Text="Add Customer" /></td>
                <td><asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_OnClick" Text="Cancel" CausesValidation="false" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
