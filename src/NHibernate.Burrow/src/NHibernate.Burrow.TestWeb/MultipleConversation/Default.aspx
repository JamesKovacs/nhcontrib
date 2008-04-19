<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="MultipleConversation_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Multiple conversation test</title>
</head>
<body>
  
    <form id="form1" runat="server">
        <div>
         
            <table  Width="100%">
            <tr><td colspan="2">  <iframe id="Iframe4" runat="server"  src="Monitor.aspx" />
           </td></tr>
                <tr>
                    <td> <iframe id="frame1" runat="server" Width="100%" src="Conversation1.aspx" />
                    </td>
                    <td> <iframe id="Iframe1" runat="server"  Width="100%" src="Conversation2.aspx" />
                    </td>
                </tr>
                <tr>
                    <td> <iframe id="Iframe2" runat="server"  Width="100%" src="Conversation3.aspx" />
                    </td>
                    <td> <iframe id="Iframe3" runat="server"  Width="100%"  src="Conversation4.aspx" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
