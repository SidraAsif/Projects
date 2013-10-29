<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpressAssessment.ascx.cs"
    Inherits="ExpressAssessment" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/expressAssessment.js") %>"></script>
<div class="body-container-right">
    <asp:Panel ID="pnlmessage" runat="server" Style="margin-left: 10px;">
        <ucdm:DisplayMessage ID="message" runat="server" DisplayMessageWidth="700" ValidationSummaryHeaderText="<div class='error-SummaryHeader'>Required information is missing.</div>">
        </ucdm:DisplayMessage>
    </asp:Panel>
    <div style="font-family: Segoe UI; font-size: 20px; font-weight: bold; margin-bottom: 10px;
        color: #c10000">
        <asp:Label ID="lblSiteName" runat="server"></asp:Label>
    </div>
    <div id="questionTitle">
        <asp:Panel ID="pnlQuestionTitle" runat="server">
        </asp:Panel>
    </div>
    <div id="questionForm">
        <asp:Panel ID="pnlQuestion" runat="server">
        </asp:Panel>
    </div>
    <div id="questionSubmit">
        <asp:Panel ID="pnlQuestionSubmit" runat="server" CssClass="questionSubmit">
        </asp:Panel>
    </div>
</div>
