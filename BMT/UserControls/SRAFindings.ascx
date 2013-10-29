<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SRAFindings.ascx.cs" Inherits="SRAFindings"  ViewStateMode="Enabled" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<link rel="Stylesheet" type="text/css" href="../Themes/SRA.css" media="all" />
<link rel="Stylesheet" type="text/css" href="../Themes/jquery-ui-1.8.12.custom.css" />
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jquery-ui-1.8.12.custom.min.js") %>"></script>
<br />
<asp:Panel ID="pnlmessage" runat="server" Style="margin-left: 10px;">
    <ucdm:DisplayMessage ID="message" runat="server" DisplayMessageWidth="700" ValidationSummaryHeaderText="<div class='error-SummaryHeader'>Required information is missing.</div>">
    </ucdm:DisplayMessage>
</asp:Panel>
<table width="100%">
    <tr>
        <td class="siteInfo" width="35%">
            <asp:Label ID="lblSiteName" runat="server" CssClass="site-title" Text=""></asp:Label>
        </td>
        <td width="12%">
            <asp:CheckBox ID="chbFinalize" runat="server" Text="Finalize & Lock" CssClass="finding-checkbox"/>
        </td>
        <td width="11%" align="right">
            Completed by:
        </td>
        <td width="25%" valign="top">
            <asp:Label ID="lblFindUserInfo" runat="server" ClientIDMode="Static" CssClass="finding-userinfo"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="standard-title" width="32%">
            Findings and Recommendations
        </td>
    </tr>
</table>
<table width="100%">
    <tr>
        <td width="32%">
        </td>
        <td>
            <p class="titleDesc" style="color: #C10000;">
                Important!: Please Save & Print a copy for your records
            </p>
        </td>
        <td>
            <asp:Button ID="btnPrintFindings" runat="server" Text="Save & Print Findings Now" OnClick="btnPrintFindings_Click" 
            OnClientClick="javascript:BeginRequestHandler();" UseSubmitBehavior="false"/>
        </td>
    </tr>
</table>
<table width="100%">
    <br />
    <tr>
        <td>
            <img id="imgHighRisk" src="../Themes/Images/highrisk.png" runat="server" />
            <asp:Label ID="lblHighRisks" runat="server" CssClass="riskfound"></asp:Label>
        </td>
        <td>
            <img id="imgMediumRisk" src="../Themes/Images/mediumrisk.png" runat="server" />
            <asp:Label ID="lblMediumRisks" runat="server" CssClass="riskfound"></asp:Label>
        </td>
        <td>
            <img id="imgLowRisk" src="../Themes/Images/lowrisk.png" runat="server" />
            <asp:Label ID="lblLowRisks" runat="server" CssClass="riskfound"></asp:Label>
        </td>
        <td align="right" style="padding-right: 5px;">
            <a href="../StDocs/General/Security Risk Assessment - Instructions.pdf#page=11" target="_blank" >Read Instructions for this Tab</a>
        </td>
    </tr>
</table>
<asp:Panel ID="pnlFindings" runat="server" ClientIDMode="Static">
</asp:Panel>
<asp:HiddenField ID="hdnFindLockAttr" ClientIDMode="Static" runat="server" />
<asp:HiddenField ID="hdnFindEdit" ClientIDMode="Static" runat="server" />