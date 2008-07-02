<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DLoadControl.ascx.cs" Inherits="StatefulFieldInDynamicallyLoadControls_DLoadControl" %>
<%@ Register Src="../GenControl/SuccessMessage.ascx" TagName="SuccessMessage" TagPrefix="uc1" %>
<asp:Button ID="btn" runat="server" Text="Click Me" OnClick="btnClick" />
<asp:Label ID="Label1" runat="server" Text=""></asp:Label>
<uc1:SuccessMessage ID="SuccessMessage1" runat="server" />
