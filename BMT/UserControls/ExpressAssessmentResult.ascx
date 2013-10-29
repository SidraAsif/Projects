<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpressAssessmentResult.ascx.cs"
    Inherits="ExpressAssessmentResult" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<link rel="Stylesheet" type="text/css" href="../Themes/jquery-ui-1.8.16.custom.css" />
<link rel="Stylesheet" type="text/css" href="../Themes/ExpressAssessment.css" />
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jquery-ui-1.8.16.custom.min.js") %>"></script>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/expressAssessmentResult.js") %>"></script>
<div class="body-container-right">
    <asp:Panel ID="pnlmessage" runat="server" Style="margin-left: 10px;">
        <ucdm:DisplayMessage ID="message" runat="server" DisplayMessageWidth="700" />
    </asp:Panel>
    <asp:Panel ID="pnlresultAlert" runat="server">
        <div style="font-family: Segoe UI; font-size: 20px; font-weight: bold; margin-bottom: 10px;
            color: #c10000">
            <asp:Label ID="lblSiteName" runat="server"></asp:Label>
        </div>
        <div id="result-Top-Header">
            <asp:Label ID="lblTopHeaderText" runat="server" CssClass="TopHeaderText"></asp:Label>
        </div>
        <div id="score-info">
            <div id="result-progress-bar">
                <div id="progressbar">
                </div>
                <asp:Label ID="lblScoringPoint" runat="server" CssClass="progress-bar"></asp:Label>
            </div>
            <div id="result-progress-message">
                <asp:Label ID="lblResultMessage" runat="server" CssClass="result-message" Text=""></asp:Label>
            </div>
        </div>
        <div class="Warning-message">
            <asp:Panel ID="pnlWarningMessage" runat="server">
            </asp:Panel>
        </div>
    </asp:Panel>
    <br />
    <br />
    <div class="seperator">
    </div>
    <p style="font-weight: bold; font-size: 16px;">
        Your Answers</p>
    <p style="font-weight: bold; font-size: 11px; color: Red; font-style: italic;">
        Critical deficiencies are marked in Red</p>
    <div id="result-progress">
        <asp:Panel ID="pnlResultProgress" runat="server">
        </asp:Panel>
    </div>
    <div id="result-action">
        <asp:Panel ID="pnlResultAction" runat="server" CssClass="questionSubmit">
        </asp:Panel>
    </div>
</div>
