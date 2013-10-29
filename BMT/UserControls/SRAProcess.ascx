<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SRAProcess.ascx.cs" ViewStateMode="Enabled"
    Inherits="SRAProcess" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<link rel="Stylesheet" type="text/css" href="../Themes/SRA.css" media="all" />
<br />
<asp:Panel ID="pnlmessage" runat="server" Style="margin-left: 10px;">
    <ucdm:DisplayMessage ID="message" runat="server" DisplayMessageWidth="700" ValidationSummaryHeaderText="<div class='error-SummaryHeader'>Required information is missing.</div>">
    </ucdm:DisplayMessage>
</asp:Panel>
<div class="siteInfo">
    <asp:Label ID="lblSiteName" runat="server" CssClass="site-title" Text=""></asp:Label>
</div>
<table width="100%">
    <tr>
    <td>
    </td>
    <td align="right"><asp:Button ID="btnPrintProcess" runat="server" Text="Save & Print Process Now" OnClick="btnPrintProcess_Click" 
    OnClientClick="javascript:BeginRequestHandler();" UseSubmitBehavior="false" />
    </td>
    </tr>
    <tr>
        <td class="standard-title" width="26%">
            People & Processes Risks
        </td>
        <td>
            <p class="titleDesc" style="color: #C10000;">
                In order to correctly assess your risks ALL fields in this tab must be completed.
            </p>
        </td>
    </tr>
</table>
<table width="100%">
    <br />
    <tr>
        <td align="right" style="padding-right: 5px;">
            <a href="../StDocs/General/Security Risk Assessment - Instructions.pdf#page=7" target="_blank" >Read Instructions for this Tab</a>
        </td>
    </tr>
</table>
<asp:Panel ID="pnlProcess" runat="server" ClientIDMode="Static">
</asp:Panel>

<asp:HiddenField ID="hdnUsrName" ClientIDMode="Static" runat="server" />
