<%@ Page Language="C#" AutoEventWireup="true" CodeFile="result.aspx.cs" Inherits="CustomerErrorHandling_result" %>

<%@ Register Src="../GenControl/SuccessMessage.ascx" TagName="SuccessMessage" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div> 
        <uc1:SuccessMessage ID="SuccessMessage1" runat="server" />
    </div>
    </form>
</body>
</html>
