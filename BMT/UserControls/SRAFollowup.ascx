<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SRAFollowup.ascx.cs" ViewStateMode="Enabled"
    Inherits="SRAFollowup" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<link rel="Stylesheet" type="text/css" href="../Themes/SRA.css" media="all" />
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
            <asp:CheckBox ID="chbFollowFinalize" runat="server" Text="Finalize & Lock" CssClass="followup-checkbox"/>
        </td>
        <td width="11%" align="right">
            Completed by:
        
        </td>
        <td width="25%" valign="top">
            <asp:Label ID="lblFollowUserInfo" runat="server" ClientIDMode="Static" CssClass="finding-userinfo"></asp:Label>
        </td>
    </tr>
    </tr>
    <tr>
        <td class="standard-title" width="32%">
            Followup & Remediation
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
            <asp:Button ID="btnPrintFollowup" runat="server" Text="Save & Print Remediation Now" OnClick="btnPrintFollowup_Click" 
            OnClientClick="javascript:BeginRequestHandler();" UseSubmitBehavior="false" />
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
        </td>
        <td align="right" style="padding-right: 5px;">
            <a href="../StDocs/General/Security Risk Assessment - Instructions.pdf#page=12" target="_blank" >Read Instructions for this Tab</a>
        </td>
    </tr>
</table>
<asp:Panel ID="pnlFollowup" runat="server" ClientIDMode="Static">
</asp:Panel>
<asp:HiddenField ID="hdnFollowLockAttr" ClientIDMode="Static" runat="server" />
<asp:HiddenField ID="hdnFollowEdit" ClientIDMode="Static" runat="server" />