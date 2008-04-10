<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditCustomer.ascx.cs" Inherits="EditCustomer" %>
  <%@ Register Assembly="wwDataBinder" Namespace="MsdnMag.Web.Controls" TagPrefix="ww" %>
 <h2>Edit Customer</h2>
        Use this form to update the customer's information.<br />
        <br />
        
        <table>
            <tr>
                <td>Customer ID:</td>
                <td>
                    <asp:Label ID="lblCustomerID" runat="server" />
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
the wwDataBinder can be used for two way databinding, but it's totally optional
<ww:wwDataBinder ID="DataBinder" runat="server" DefaultBindingSource="customer" >
    <DataBindingItems>
        <ww:wwDataBindingItem ID="dbiName" runat="server"  BindingMode="oneway"
            ControlId="lblCustomerID" BindingProperty="Text" BindingSourceMember="ID" />
        <ww:wwDataBindingItem ID="dbiCompanyName" runat="server"  
            ControlId="txtCompanyName" BindingProperty="Text" BindingSourceMember="CompanyName" /> 
        <ww:wwDataBindingItem ID="WwDataBindingItem1"  runat="server" 
            ControlId="txtContactName" BindingProperty="Text" BindingSourceMember="ContactName" />
    </DataBindingItems> 
</ww:wwDataBinder>

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
        
