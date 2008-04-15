<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditCustomerView.ascx.cs" Inherits="Views_EditCustomerView" %>

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
        <td colspan="3" align="center">
            <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_OnClick" Text="Update" /> 
            <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_OnClick" Text="Cancel" CausesValidation="false" />
        </td>
    </tr>
</table>