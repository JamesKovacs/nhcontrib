<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditCustomer.ascx.cs"
    Inherits="EditCustomer" %>
<%@ Register Assembly="wwDataBinder" Namespace="MsdnMag.Web.Controls" TagPrefix="ww" %>
<h2>Edit Customer</h2>
Customer ID:
<asp:Label ID="lblCustomerID" runat="server" /><br />
Company Name:
<asp:TextBox ID="txtCompanyName" runat="server" /><br />
Contact Name:
<asp:TextBox ID="txtContactName" runat="server" /><br />
Address:
<asp:TextBox ID="txtAddress" runat="server" /><br />
Sales Potential:
<asp:TextBox ID="txtSalesPotential" runat="server" /><br />
Activated:
<asp:CheckBox ID="cbActivated" runat="server" /><br />
<ww:wwDataBinder ID="DataBinder" runat="server" DefaultBindingSource="customer">
    <DataBindingItems>
        <ww:wwDataBindingItem ID="dbiId" runat="server" BindingMode="oneway" ControlId="lblCustomerID"
            BindingSourceMember="Id" />
        <ww:wwDataBindingItem ID="dbiCompanyName" BindingSourceMember="CompanyName" ControlId="txtCompanyName"
            runat="server" />
        <ww:wwDataBindingItem ID="dbiContactName" BindingSourceMember="ContactName" ControlId="txtContactName"
            runat="server" />
        <ww:wwDataBindingItem ID="dbiAddress" BindingSourceMember="Address" ControlId="txtAddress"
            runat="server" />
        <ww:wwDataBindingItem ID="dbiSP" BindingSourceMember="SalesPotential" ControlId="txtSalesPotential"
            DisplayFormat="{0:C}" runat="server" />
        <ww:wwDataBindingItem ID="dbiIA" BindingSourceMember="IsActivated" ControlId="cbActivated"
            BindingProperty="Checked" runat="server" />
    </DataBindingItems>
</ww:wwDataBinder>
<asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_OnClick" Text="Update" />
<asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_OnClick" Text="Cancel" />