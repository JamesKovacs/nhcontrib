<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ConversationBreaker.ascx.cs" Inherits="Controls_ConversationBreaker" %>
    <script language="javascript" type="text/javascript">
    function checkCerrar()
    {
        var cerrar = document.getElementById("hdClose");
        if (cerrar.value==1)
            window.close();
        
    }
    </script>
        <asp:HiddenField ID="hdClose" runat="server" Value="0" />
    
        <asp:Button ID="btnBreak" runat="server" Text="BreakConversation By exception" OnClick="btnBreak_Click" />
