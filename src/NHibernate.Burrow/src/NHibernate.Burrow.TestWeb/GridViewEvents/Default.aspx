<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="GridViewEvents_Default" %>

<%@ Register Src="../GenControl/SuccessMessage.ascx" TagName="SuccessMessage" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    Click the link button in the gridview to test if the event fires.
		<asp:GridView ID="GridView1" runat="server">
		
			<Columns>
				<asp:TemplateField>
					<ItemTemplate>
						<asp:LinkButton runat="server" ID="LinkButton1" OnClick="LinkButton1_Click" Text="LinkButton" />
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
			
		</asp:GridView>
        <uc1:SuccessMessage ID="SuccessMessage1" runat="server" />
    </div>
   
    </form>
</body>
</html>
