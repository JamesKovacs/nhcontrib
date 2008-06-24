<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Propagation_Default" %>

<%@ Register Assembly="NHibernate.Burrow.WebUtil" Namespace="NHibernate.Burrow.WebUtil.Controls"
    TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <cc1:BurrowLink ID="BurrowLink1" runat="server" NavigateUrl="Result.aspx"  >Test propagation through Burrow Link to other page</cc1:BurrowLink>
    </div>
    </form>
</body>
</html>
