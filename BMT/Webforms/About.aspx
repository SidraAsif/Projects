<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="BMT.Webforms.About" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
<%@ Register Src="~/UserControls/LoadingPanel.ascx" TagName="LoadingPanel" TagPrefix="ucl" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="MainHead" runat="server">
    <link href="../Themes/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="mainForm" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <div>
        <asp:UpdatePanel ID="upnlMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <br />
                <p>
                    <asp:Label ID="lblAboutUs" runat="server"></asp:Label>
                </p>
                <br />
                <%--<div>
                    <asp:Button ID="btnOk" runat="server" Text="OK" />
                </div>--%>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
