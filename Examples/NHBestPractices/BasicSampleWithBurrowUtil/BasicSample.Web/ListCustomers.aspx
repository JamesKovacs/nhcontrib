<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ListCustomers.aspx.cs" Inherits="ListCustomers" %>

<%@ Register Src="EditCustomer.ascx" TagName="EditCustomer" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>List Customers</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2> Customer Listing</h2>
            <a href="Default.aspx">return to the homepage</a>.<br />
            <br />
            
            <table cellpadding="0" cellspacing="5" width="100%" border="1">
                <tr>
                    <td valign="top">
                    A sortable and paginated (both at DB side) gridview with little effort 
                        <asp:GridView ID="grdEmployees" runat="server" AutoGenerateColumns="false" AllowPaging="True"
                            AllowSorting="True" PageSize="20" DataSourceID="odsAll">
                            <Columns>
                                <asp:TemplateField HeaderText="Customer ID" SortExpression="ID">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtSelect" OnCommand="lbt_SelectCommand" CommandArgument='<%# Eval("ID") %>'
                                            runat="server"><%# Eval("ID") %></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CompanyName" HeaderText="Company Name" SortExpression="CompanyName" />
                                <asp:BoundField DataField="ContactName" HeaderText="Contact Name" SortExpression="ContactName" />
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsAll" runat="server" TypeName="BasicSample.Data.CustomerDao"
                            EnablePaging="True" MaximumRowsParameterName="pageSize" StartRowIndexParameterName="startRow"
                            SortParameterName="sortExpression" SelectMethod="FindAll" SelectCountMethod="CountAll">
                        </asp:ObjectDataSource>
                    </td>
                    <td>
                        <uc1:EditCustomer ID="EditCustomer1" runat="server" Visible="false" OnUpdated="EditCustomer_Updated"></uc1:EditCustomer>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
