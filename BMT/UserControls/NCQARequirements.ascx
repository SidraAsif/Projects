<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NCQARequirements.ascx.cs"
    Inherits="NCQARequirements" %>
<%@ Register Src="~/UserControls/PCMH.ascx" TagName="PCMH" TagPrefix="ucncqa" %>
<%@ Register Src="~/UserControls/General.ascx" TagName="General" TagPrefix="gn" %>
<%@ Register Src="~/UserControls/NCQASummary.ascx" TagName="Summary" TagPrefix="sm" %>
<%@ Register Src="~/UserControls/LoadingPanel.ascx" TagName="LoadingPanel" TagPrefix="ucl" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<%@ Reference Control="~/UserControls/PCMH.ascx" %>
<link rel="Stylesheet" type="text/css" href="../Themes/popup.css" />
<link rel="Stylesheet" type="text/css" href="../Themes/PCMH.css" />
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jquery.numeric.js") %>"></script>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/ncqaRequirements.js") %>"></script>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/pcmh.js") %>"></script>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jquery-ui-1.8.16.custom.min.js") %>"></script>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/ncqaSummary.js") %>"></script>
<div class="body-container-right" style="min-height: 300px">
    <asp:UpdatePanel ID="upnlNCQARequirements" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlmessage" runat="server" Style="margin-left: 10px;">
                <ucdm:DisplayMessage ID="message" runat="server" DisplayMessageWidth="700" ValidationSummaryHeaderText="<div class='error-SummaryHeader'>Type warning message here</div>">
                </ucdm:DisplayMessage>
            </asp:Panel>
            <!-- the tabs -->
            <asp:Panel ID="pnlTabs" runat="server">
                <ul class="tabs">
                    <li id="tabList8" class="activeTab">
                        <asp:LinkButton ID="lbSummary" runat="server" Text="Summary" ClientIDMode="Static"
                            OnClientClick="javascript:updateClickTab(8);"></asp:LinkButton>
                    </li>
                    <li id="tabList7">
                        <asp:LinkButton ID="lbGeneral" runat="server" Text="General" ClientIDMode="Static"
                            OnClientClick="javascript:updateClickTab(7);"></asp:LinkButton>
                    </li>
                    <li id="tabList1">
                        <asp:LinkButton ID="lbPCMH1" runat="server" Text="PCMH 1" ClientIDMode="Static" OnClientClick="javascript:updateClickTab(1);"></asp:LinkButton>
                    </li>
                    <li id="tabList2">
                        <asp:LinkButton ID="lbPCMH2" runat="server" Text="PCMH 2" ClientIDMode="Static" OnClientClick="javascript:updateClickTab(2);"></asp:LinkButton>
                    </li>
                    <li id="tabList3">
                        <asp:LinkButton ID="lbPCMH3" runat="server" Text="PCMH 3" ClientIDMode="Static" OnClientClick="javascript:updateClickTab(3);"></asp:LinkButton>
                    </li>
                    <li id="tabList4">
                        <asp:LinkButton ID="lbPCMH4" runat="server" Text="PCMH 4" ClientIDMode="Static" OnClientClick="javascript:updateClickTab(4);"></asp:LinkButton>
                    </li>
                    <li id="tabList5">
                        <asp:LinkButton ID="lbPCMH5" runat="server" Text="PCMH 5" ClientIDMode="Static" OnClientClick="javascript:updateClickTab(5);"></asp:LinkButton>
                    </li>
                    <li id="tabList6">
                        <asp:LinkButton ID="lbPCMH6" runat="server" Text="PCMH 6" ClientIDMode="Static" OnClientClick="javascript:updateClickTab(6);"></asp:LinkButton>
                    </li>
                </ul>
            </asp:Panel>
            <!-- Disclaimer Panel" -->
            <asp:Panel ID="pnlDisclaimer" runat="server" Visible="false">
                <asp:Label ID="lblDisclaimer" runat="server">
                </asp:Label>
                <br />
                <br />
                <br />
                <asp:Button ID="btnNext" runat="server" Text="I agree" OnClick="btnNext_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="I do not agree" OnClick="btnCancel_Click" />
            </asp:Panel>
            <!-- tab "panels" -->
            <asp:Panel ID="Panel1" runat="server">
                <sm:Summary ID="summary" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlGeneral" runat="server">
                <gn:General ID="general" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlNCQAPCMH1" runat="server">
                <ucncqa:PCMH ID="pcmh1" runat="server" PCMHType="PCMH1" />
            </asp:Panel>
            <asp:Panel ID="pnlNCQAPCMH2" runat="server">
                <ucncqa:PCMH ID="pcmh2" runat="server" PCMHType="PCMH2" />
            </asp:Panel>
            <asp:Panel ID="pnlNCQAPCMH3" runat="server">
                <ucncqa:PCMH ID="pcmh3" runat="server" PCMHType="PCMH3" />
            </asp:Panel>
            <asp:Panel ID="pnlNCQAPCMH4" runat="server">
                <ucncqa:PCMH ID="pcmh4" runat="server" PCMHType="PCMH4" />
            </asp:Panel>
            <asp:Panel ID="pnlNCQAPCMH5" runat="server">
                <ucncqa:PCMH ID="pcmh5" runat="server" PCMHType="PCMH5" />
            </asp:Panel>
            <asp:Panel ID="pnlNCQAPCMH6" runat="server">
                <ucncqa:PCMH ID="pcmh6" runat="server" PCMHType="PCMH6" />
            </asp:Panel>
            <asp:HiddenField ID="hiddenClickTab" runat="server" ClientIDMode="Static" Value="0" />
            <asp:HiddenField ID="hdnIsConsultant" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnRequiredDocsEnabled" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnMarkAsCompleteEnabled" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnSummaryElementId" runat="server" ClientIDMode="Static" />
            <ucl:LoadingPanel ID="loading" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="hiddenClickTab" />
        </Triggers>
    </asp:UpdatePanel>
</div>
