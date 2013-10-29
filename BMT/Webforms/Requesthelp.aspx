<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Requesthelp.aspx.cs" Inherits="BMT.Webforms.Request_help" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
<%@ Register Src="~/UserControls/LoadingPanel.ascx" TagName="LoadingPanel" TagPrefix="ucl" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="MainHead" runat="server">
    <title></title>
    <link href="../Themes/style.css" rel="stylesheet" type="text/css" />
    <link rel="Stylesheet" type="text/css" href="../Themes/popup.css" />
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jQuery-1.7.1.min.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/common.js") %>"></script>
    <style type="text/css">
        .style3
        {
            width: 90px;
        }
    </style>
</head>
<body>
    <form id="mainForm" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlRequestHelp" runat="server" Style="margin-left: 5px;">
                <ucdm:DisplayMessage ID="MsgRequestHelp" runat="server" DisplayMessageWidth="390"
                    ShowCloseButton="false"></ucdm:DisplayMessage>
            </asp:Panel>
            <div>
                <asp:Label ID="lblname" CssClass="popup-info" ClientIDMode="Static" runat="server"
                    Width="395px" Text=" User: ">
                </asp:Label>
            </div>
            <table width="100%">
                <tr>
                    <td>
                        <p>
                            Subject:*</p>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSubject" runat="server" CssClass="text-field02" ClientIDMode="Static"
                            MaxLength="50" Width="287px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="DFVSubject" runat="server" ControlToValidate="txtSubject"
                            Text="*">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <p>
                            Description:</p>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDesc" runat="server" CssClass="text-field02" ClientIDMode="Static"
                            MaxLength="5000" Style="resize: none;" TextMode="MultiLine" Width="287px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <p>
                <asp:Label ID="lblDescription" runat="server">
                </asp:Label>
                <br />
                We will reply promptly to your email address as registered in the application.</p>
            <div>
                <font color="red">(Fields marked with an * are Required)</font>
            </div>
            <table width="100%">
                <tr>
                    <td align="center">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                            Width="120px" ClientIDMode="Static" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
