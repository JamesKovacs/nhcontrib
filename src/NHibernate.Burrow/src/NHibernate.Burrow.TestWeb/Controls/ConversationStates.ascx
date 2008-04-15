<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ConversationStates.ascx.cs" Inherits="Controls_ConversationStates" %>
    <asp:Literal ID="lStatus" runat="server"></asp:Literal><br />
        <asp:Button ID="btnStart" runat="server" Text="Start" OnClick="btnStart_Click" />
        <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" />
        <asp:Button ID="btnCommit" runat="server" Text="Commit" OnClick="btnCommit_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
        <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click" /><br />
        <asp:Label ID="lConversationStatus" runat="server" Text=""></asp:Label>