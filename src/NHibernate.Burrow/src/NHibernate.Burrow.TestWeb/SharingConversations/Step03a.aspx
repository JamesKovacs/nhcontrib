<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Step03a.aspx.cs" Inherits="SharingConversations_Step03a" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script language="javascript" type="text/javascript">
    function checkCerrar()
    {
        var cerrar = document.getElementById("hdClose");
        if (cerrar.value==1)
            window.close();
        
    }
    </script>
</head>
<body onload="checkCerrar()">
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField ID="hdClose" runat="server" Value="0" />
        <p>We have successfully checked the change of conversation at do a hyperlink with target="_blank"</p>
        <p>Press button to continue</p>
        <asp:Button ID="btnClose" runat="server" OnClick="btnClose_Click" Text="Clic here to continue" />
    </div>
    </form>
</body>
</html>
