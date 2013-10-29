<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DisplayMessage.ascx.cs"
    Inherits="BMT.UserControls.DisplayMessage" %>
<link rel="Stylesheet" type="text/css" href="../Themes/display-message.css" />
<div class="container">
    <asp:Panel ID="MessageBox" runat="server" ClientIDMode="Static">
        <asp:HyperLink runat="server" ID="CloseButton">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Themes/Images/close.png" AlternateText="Click here to close this message" />
        </asp:HyperLink>
        <p>
            <asp:Literal ID="litMessage" runat="server" ClientIDMode="Static"></asp:Literal></p>
    </asp:Panel>
</div>
<asp:Panel ID="pnlValidationSummary" runat="server">
    <div id="ValidationSummary">
        <asp:ValidationSummary ID="vSummary" runat="server" CssClass="error-Summary" DisplayMode="BulletList"
            ShowSummary="true" Visible="true" />
    </div>
</asp:Panel>
