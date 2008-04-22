<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="GridViewEvents_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>     <asp:Literal ID="Literal1" runat="server"></asp:Literal>
		<asp:GridView ID="GridView1" runat="server">
			<Columns>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:LinkButton runat="server" ID="LinkButton1" OnClick="LinkButton1_Click" Text="LinkButton" />
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</asp:GridView>
    </div>
   
    </form>
</body>
</html>
