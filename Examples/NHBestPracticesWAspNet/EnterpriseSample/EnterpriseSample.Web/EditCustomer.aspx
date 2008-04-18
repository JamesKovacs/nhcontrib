<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditCustomer.aspx.cs" Inherits="EditCustomer" %>
<%@ Register Src="./Views/EditCustomerView.ascx" TagPrefix="ctrl" TagName="EditCustomerView" %>
<%@ Register Src="./Views/ListOrdersView.ascx" TagPrefix="ctrl" TagName="ListOrdersView" %>
<%@ Register Src="./Views/ListHistoricalOrderSummariesView.ascx" TagPrefix="ctrl" TagName="ListHistoricalOrderSummariesView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Edit Customer</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ctrl:EditCustomerView id="ctrlEditCustomerView" runat="server" />
        <hr />
        <ctrl:ListOrdersView id="ctrlListOrdersView" runat="server" />
        <hr />
        <ctrl:ListHistoricalOrderSummariesView id="ctrlListHistoricalOrderSummariesView" runat="server" />
    </div>
    </form>
</body>
</html>
