<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Frametests_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
<title>Success!</title>
</head>
<frameset rows="79,*" cols="*" frameborder="NO" border="0" framespacing="0">
  <frame src="header.aspx" name="headerFrame" scrolling="NO" noresize></frame>
  <frameset name="frameset" rows="*" cols="150,*" framespacing="0"
frameborder="NO" border="0">
    <frame src="Menu.aspx" name="menuFrame" scrolling="NO">
    <frame src="Homepage.aspx" name="contentFrame">
  </frameset>
</frameset>
</html>
