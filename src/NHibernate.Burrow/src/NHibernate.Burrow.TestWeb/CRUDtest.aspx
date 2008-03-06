<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CRUDtest.aspx.cs" Inherits="CRUDtest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CRUD Test</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td>
                        Name</td>
                    <td>
                        <asp:TextBox ID="tbName" runat="server"></asp:TextBox></td>
                    <td>
                        Value:</td>
                    <td>
                        <asp:Literal ID="lName" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td>
                        Number</td>
                    <td>
                        <asp:TextBox ID="tbNumber" runat="server"></asp:TextBox></td>
                    <td>
                        Value:</td>
                    <td>
                        <asp:Literal ID="lNumber" runat="server"></asp:Literal></td>
                </tr>
            </table>
            <asp:Button ID="btnSave" runat="server" Text="Update" OnClick="btnSave_Click" /> <br  /> 
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click" /> <br  /> 
            <asp:Button ID="btnCreate" runat="Server" Text="Create New" OnClick="btnCreate_Click" /><br />
            <asp:Button ID="btnDelete" runat="Server" Text="Delete" OnClick="btnDelete_Click" /><br />
            
        </div>
    </form>
</body>
</html>
