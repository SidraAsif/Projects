<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SecurityRiskAssessment.ascx.cs"
    Inherits="SecurityRiskAssessment" %>
<%@ Register Src="~/UserControls/SRASummary.ascx" TagName="Summary" TagPrefix="srasum" %>
<%@ Register Src="~/UserControls/SRAInventory.ascx" TagName="Inventory" TagPrefix="srainv" %>
<%@ Register Src="~/UserControls/SRAScreening.ascx" TagName="Screening" TagPrefix="srascr" %>
<%@ Register Src="~/UserControls/SRAProcess.ascx" TagName="Process" TagPrefix="srapro" %>
<%@ Register Src="~/UserControls/SRATechnology.ascx" TagName="Technology" TagPrefix="sratec" %>
<%@ Register Src="~/UserControls/SRAFindings.ascx" TagName="Findings" TagPrefix="srafin" %>
<%@ Register Src="~/UserControls/SRAFollowup.ascx" TagName="Followup" TagPrefix="srafol" %>
<%@ Register Src="~/UserControls/LoadingPanel.ascx" TagName="LoadingPanel" TagPrefix="ucl" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<%@ Reference Control="~/UserControls/SRASummary.ascx" %>
<%@ Reference Control="~/UserControls/SRAInventory.ascx" %>
<link rel="Stylesheet" type="text/css" href="../Themes/SRA.css" />
<link rel="Stylesheet" type="text/css" href="../Themes/popup.css" />
<link rel="Stylesheet" type="text/css" href="../Themes/jquery-ui-1.8.12.custom.css" />

<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jquery-ui-1.8.12.custom.min.js") %>"></script>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/SRA.js") %>"></script>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/SRAInventory.js") %>"></script>
<div class="body-container-right" style="min-height: 300px">
    <asp:HiddenField ID="hdnCurrentTab" runat="server" ClientIDMode="Static" Value="0" />
    <asp:HiddenField ID="hdnNextTab" runat="server" ClientIDMode="Static" />
    <div id="reportLightboxId" class="reportLightbox">
    </div>
    <div class="UpdateProgressContent" style="display: none;">
        Please Wait
        <br />
        <br />
        <img id="ImageLoading" src="~/Themes/Images/loading.gif" runat="server" alt="" />
    </div>
    <asp:Panel ID="pnlTabs" runat="server">
        <ul class="tabs">
            <li id="tabList1" class="activeTab">
                <asp:LinkButton ID="lbSRASummary" runat="server" Text="Summary" ClientIDMode="Static"
                    OnClientClick="javascript:updateClickTab(1);"></asp:LinkButton>
            </li>
            <li id="tabList2">
                <asp:LinkButton ID="lbSRAInventory" runat="server" Text="Inventory" ClientIDMode="Static"
                    OnClientClick="javascript:updateClickTab(2);"></asp:LinkButton>
            </li>
            <li id="tabList3">
                <asp:LinkButton ID="lbSRAScreening" runat="server" Text="Screening" ClientIDMode="Static"
                    OnClientClick="javascript:updateClickTab(3);"></asp:LinkButton>
            </li>
            <li id="tabList4">
                <asp:LinkButton ID="lbSRAProcess" runat="server" Text="Process" ClientIDMode="Static"
                    OnClientClick="javascript:updateClickTab(4);"></asp:LinkButton>
            </li>
            <li id="tabList5">
                <asp:LinkButton ID="lbSRATechnology" runat="server" Text="Technology" ClientIDMode="Static"
                    OnClientClick="javascript:updateClickTab(5);"></asp:LinkButton>
            </li>
            <li id="tabList6">
                <asp:LinkButton ID="lbSRAFindings" runat="server" Text="Findings" ClientIDMode="Static"
                    OnClientClick="javascript:updateClickTab(6);"></asp:LinkButton>
            </li>
            <li id="tabList7">
                <asp:LinkButton ID="lbSRAFollowup" runat="server" Text="Remediation" ClientIDMode="Static"
                    OnClientClick="javascript:updateClickTab(7);"></asp:LinkButton>
            </li>
        </ul>
    </asp:Panel>
    <asp:Panel ID="pnlDisclaimer" runat="server" Visible="false">
        <asp:Label ID="lblDisclaimer" runat="server">
        </asp:Label>
        <br />
        <br />
        <br />
        <asp:Button ID="btnNext" runat="server" Text="I Accept" OnClick="btnNext_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="I Don't Accept" OnClick="btnCancel_Click" />
    </asp:Panel>
    <!-- tab "panels" -->
    <asp:Panel ID="pnlSRASummary" runat="server" Visible="false">
        <srasum:Summary ID="SRASummary" runat="server" />
    </asp:Panel>
    <asp:Panel ID="pnlSRAInventory" runat="server" Visible="false">
        <srainv:Inventory ID="SRAInventory" runat="server" />
    </asp:Panel>
    <asp:Panel ID="pnlSRAScreening" runat="server" Visible="false">
        <srascr:Screening ID="SRAScreening" runat="server" />
    </asp:Panel>
    <asp:Panel ID="pnlSRAProcess" runat="server" Visible="false">
        <srapro:Process ID="SRAProcess" runat="server" />
    </asp:Panel>
    <asp:Panel ID="pnlSRATechnology" runat="server" Visible="false">
        <sratec:Technology ID="SRATechnology" runat="server" />
    </asp:Panel>
    <asp:Panel ID="pnlSRAFindings" runat="server" Visible="false">
        <srafin:Findings ID="SRAFindings" runat="server" />
    </asp:Panel>
    <asp:Panel ID="pnlSRAFollowup" runat="server" Visible="false">
        <srafol:Followup ID="SRAFollowup" runat="server" />
    </asp:Panel>
    <asp:Panel ID="pnlSRA" runat="server">
    </asp:Panel>
    <asp:Button ID="btnSRASave" runat="server" ClientIDMode="Static" Style="display: none;"
        Text="Save" OnClick="btnSRASave_Click" />
    <asp:Button ID="btnRefreshSRA" runat="server" ClientIDMode="Static" Style="display: none;"
        Text="Save" OnClick="btnRefreshSRA_Click" />
</div>
