<%@ Page Language="C#" MasterPageFile="~/BMTMaster.Master" AutoEventWireup="true"
    CodeBehind="Home.aspx.cs" Inherits="BMT.Webforms.Home" %>

<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<asp:Content ID="contentHead" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="contentInnerMenuContainer" ContentPlaceHolderID="innerMenuConatiner"
    runat="server">
</asp:Content>
<asp:Content ID="contentBodyContainer" ContentPlaceHolderID="bodyContainer" runat="server">
    <div>
        <asp:Panel ID="pnlMessage" runat="server">
            <div id="userMessage" style="margin-left: 120px;">
                <ucdm:DisplayMessage ID="Message" runat="server" validationGroup="upnlPractice" DisplayMessageWidth="690"
                    ShowCloseButton="false"></ucdm:DisplayMessage>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlHtml" runat="server">
            <br />
            <br />
            <div id="Content1" runat="server">
                <br />
            </div>
        </asp:Panel>
    </div>
</asp:Content>
