<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeleteFiles.aspx.cs" Inherits="BMT.Webforms.DeleteFiles" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>File deleting...</title>
    <link rel="Stylesheet" type="text/css" href="../Themes/loading-panel.css" />
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jQuery-1.7.1.min.js") %>"></script>
</head>
<body>
    <form id="frmDeleteFile" runat="server">
    <div class="UpdateProgressContent" style="top: 70px; left: 55px;">
        Deleting...<br />
        <br />
        <img id="ImageLoading" src="~/Themes/Images/loading.gif" runat="server" alt="" />
    </div>
    </form>
</body>
</html>
