<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintReport.aspx.cs" Inherits="BMT.Webforms.PrintReportPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="Stylesheet" type="text/css" href="../Themes/loading-panel.css" />
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jQuery-1.7.1.min.js") %>"></script>
    <title>Report</title>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#hdnButton')[0].click();
            if (window.history) {
                window.history.forward(1);
            }
        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server" AsyncPostBackTimeout="10000000">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePnl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" Style="color: Red; font-size: 16px; font-weight: bold;"
                Text="Report could not be generated, please try again." Visible="false"></asp:Label>
            <asp:Button ID="hdnButton" runat="server" OnClick="hdnButton_Click" ClientIDMode="Static"
                Style="visibility: hidden;"></asp:Button>
            <div id="divProgress" class="UpdateProgressContent" runat="server">
                Generating Report...<br />
                <br />
                <img id="ImageLoading" src="~/Themes/Images/loading.gif" runat="server" alt="" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
