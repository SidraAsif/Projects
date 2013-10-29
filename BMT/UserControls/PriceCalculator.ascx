<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PriceCalculator.ascx.cs"
    Inherits="BMT.UserControls.PriceCalculator" ViewStateMode="Enabled" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<link rel="Stylesheet" type="text/css" href="../Themes/priceCalculator.css" />
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/apprise-1.5.min.js") %>"></script>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jquery.numeric.js") %>"></script>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/priceCalculator.js") %>"></script>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jquery.maxLength-min.js") %>"></script>
<div class="heading1">
    EHR/PM Total Cost of Ownership (TOC) - Price Calculator
</div>
<div class="heading2">
    You can calculate TOC and compare up to 4 EHR/PM systems with various purchasing
    models
</div>
<!-- List of System -->
<div id="divSystemSelection" runat="server">
    <asp:Literal ID="literalSystem" runat="server"></asp:Literal>
</div>
<asp:Panel ID="pnlmessage" runat="server" Style="clear: both; float: left; margin-left: 10px;">
    <ucdm:displaymessage id="message" runat="server" displaymessagewidth="700"></ucdm:displaymessage>
</asp:Panel>
<div id="systemInfo" class="systemSelection">
    <asp:Label ID="lblSystemInfo" runat="server" ClientIDMode="Static" CssClass="systemInfo"></asp:Label>
    <div id="divSystem" class="system" runat="server">
    </div>
</div>
<div class="selection">
    <p class="selection-title">
        Please select Purchase Model to begin</p>
    <table>
        <tr>
            <td>
                EHR/PM Purchase Model:<span class="asterik">*</span>
            </td>
            <td>
                <asp:RadioButtonList ID="rbQuestionnaire" runat="server" RepeatDirection="Horizontal"
                    OnSelectedIndexChanged="rbQuestionnaire_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Text="Subscription (ASP)" Value="3"> </asp:ListItem>
                    <asp:ListItem Text="Upfront License Purchase" Value="4"> </asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>
</div>
<asp:Panel ID="pnlFee" runat="server" Visible="false" Style="clear: both; float: left;">
    <table width="100%" id="tableFeeControl">
        <tr>
            <td id="Row1" style="min-width: 100%; background-color: #6699cc; color: White; padding-left: 5px;"
                align="left">
                Recurring Fees
            </td>
        </tr>
        <tr>
            <td id="Row2" style="min-width: 80%; padding-left: 444px; font-weight: bold;">
                <div class="headerAmount">
                    $ Amount
                </div>
                <div class="paymentMethod">
                    Payment Method</div>
            </td>
        </tr>
        <tr>
            <td style="min-width: 100%;" id="Row3">
                <div id="divSubsOngoingFees" class="OngoingFees" runat="server">
                </div>
            </td>
        </tr>
        <tr>
            <td id="Row4" style="min-width: 100%; background-color: #6699cc; color: White; padding-left: 5px;"
                align="left">
                One Time Fees
            </td>
        </tr>
        <tr>
            <td id="Row5" style="min-width: 80%; padding-left: 448px; font-weight: bold;">
                <div class="headerAmount">
                    $ Amount
                </div>
                <div class="paymentMethod">
                    Payment Method</div>
            </td>
        </tr>
        <tr>
            <td style="min-width: 100%;" id="Row6">
                <div id="divSubsOneTimeFees" class="OneTimeFees" runat="server">
                </div>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="pnlOperation" runat="server" Visible="false">
    <div id="divOperation" class="operation">
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnClearContents" runat="server" Text="Clear Contents" ClientIDMode="Static"
                        CausesValidation="false" OnClientClick="javascript:ClearContents().toggle();return false;" />
                </td>
                <td>
                    <asp:Button ID="btnDiscardChanges" runat="server" Text="Discard Changes" ClientIDMode="Static"
                        CausesValidation="false" OnClientClick="javascript:DiscardChanges().toggle();return false;" />
                </td>
                <td>
                    <asp:Button ID="btnSaveAndContinue" runat="server" Text="Save" OnClick="btnSaveAndContinue_Click"
                        ClientIDMode="Static" OnClientClick="javascript:FetchingValues().toggle();" CausesValidation="false" />
                </td>
                <td>
                    <asp:Button ID="btnDeleteSystem" runat="server" Text="Delete" OnClientClick="javascript:DeleteSystem().toggle(); return false;"
                        CausesValidation="false" />
                </td>
                <td width="100px">
                </td>
                <td>
                    <asp:Button ID="btnCalculatePrice" runat="server" CausesValidation="false" ClientIDMode="Static"
                        OnClientClick="javascript:CalculatePrice(); return false;" Text="Calculate Price" />
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>
<asp:HiddenField ID="hdnType" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnInterfaceType" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnOngoingPaymentMethod" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnOneTimePaymentMethod" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnTablesId" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnSystemId" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnSlectionInProcessFlag" runat="server" ClientIDMode="Static" />
<!-- Trigggers to click from client side -->
<asp:Button ID="btnTriggerSwitchSystem" runat="server" ClientIDMode="Static" Style="display: none;"
    OnClick="btnTriggerSwitchSystem_Click" CausesValidation="false" />
<asp:Button ID="btnTriggerClearContents" runat="server" ClientIDMode="Static" Style="display: none;"
    OnClick="btnTriggerClearContents_Click" CausesValidation="false" />
<asp:Button ID="btnTriggerDiscardChanges" runat="server" ClientIDMode="Static" Style="display: none;"
    OnClick="btnDiscardChanges_Click" CausesValidation="false" />
<asp:Button ID="btnTriggerAddEHR" runat="server" ClientIDMode="Static" Style="display: none;"
    OnClick="btnTriggerAddEHR_Click" CausesValidation="false" OnClientClick="javascript:FetchingValues();" />
<asp:Button ID="btnTriggerDeleteSystem" runat="server" ClientIDMode="Static" Style="display: none;"
    OnClick="btnTriggerDeleteSystem_Click" CausesValidation="false" />
<asp:Button ID="btnTriggerCalculatePrice" runat="server" ClientIDMode="Static" Style="display: none;"
    OnClick="btnTriggerCalculatePrice_Click" />
