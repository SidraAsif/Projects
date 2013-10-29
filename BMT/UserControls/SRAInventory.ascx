<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SRAInventory.ascx.cs"
    Inherits="SRAInventory" ViewStateMode="Enabled"%>
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
    <td></td>
    <td align="right"><asp:Button ID="btnPrintInventory" runat="server" Text="Save & Print Inventory Now" OnClick="btnPrintInventory_Click" 
    OnClientClick="javascript:BeginRequestHandler();" UseSubmitBehavior="false"/></td>
    </tr>
    <tr>
        <td class="standard-title" width="18%">
            Assets Inventory
        </td>
        <td>
            <p class="titleDesc" style="color: #C10000;">
                This section is optional but highly recommended for a thorough Security Risk Assessment
            </p>
            <p class="titleDesc" style="color: Black;">
                Note: Only list assets that process, store, transmit or govern Electronic Personal
                Health Information (EPHI)
            </p>
        </td>
    </tr>
</table>
<table width="100%">
    <br />
    <tr>
        <td align="right" style="padding-right: 5px;">
            <a href="../StDocs/General/Security Risk Assessment - Instructions.pdf#page=5" target="_blank" >Read Instructions for this Tab</a>
        </td>
    </tr>
</table>
<asp:Panel ID="pnlInventory" runat="server" ClientIDMode="Static">
</asp:Panel>
