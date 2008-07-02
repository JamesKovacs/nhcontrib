<%@ Page Language="C#" MasterPageFile="~/Materpage/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Materpage_Default" Title="Untitled Page" %>

<%@ Register Src="../GenControl/SuccessMessage.ascx" TagName="SuccessMessage" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
    <uc1:SuccessMessage ID="SuccessMessage1" runat="server" />
</asp:Content>

