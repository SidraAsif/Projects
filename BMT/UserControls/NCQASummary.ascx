<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NCQASummary.ascx.cs"
    Inherits="NCQASummary" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<link rel="Stylesheet" type="text/css" href="../Themes/NCQA.css" media="all" />
<link rel="Stylesheet" type="text/css" href="../Themes/jquery-ui-1.8.16.custom.css" />
<br />
<asp:Panel ID="pnlmessage" runat="server" Style="margin-left: 10px;">
    <ucdm:DisplayMessage ID="message" runat="server" DisplayMessageWidth="700" ValidationSummaryHeaderText="<div class='error-SummaryHeader'>Required information is missing.</div>">
    </ucdm:DisplayMessage>
</asp:Panel>
<div class="siteInfo">
    <asp:Label ID="lblSiteInfo" runat="server" CssClass="master-title" Text=""></asp:Label>
</div>
<div style="float: right; padding-right: 50px;">
    <asp:HyperLink ID="hypFacilitator" Text="Facilitators" ClientIDMode="Static" runat="server"
        NavigateUrl="javascript:DisplayFacilitators()"></asp:HyperLink>
</div>
<div id="lightbox-popup" class="Facilitators-popup" style="border: 1px solid #5880B3;">
    <div id="popupHeaderText">
        <table width="100%">
            <tr>
                <td class="trow">
                </td>
                <td width="80%" align="center" valign="middle">
                    <span class="rspan">Facilitators</span>
                </td>
                <td width="20%" align="right" valign="middle">
                    <a id="close-Facilitators">close[x]</a>
                </td>
            </tr>
        </table>
    </div>
    <asp:Panel ID="pnlFacilitators" runat="server">
    </asp:Panel>
</div>
<div class="lightbox">
</div>
<table width="95%">
    <tr>
        <td colspan="1" class="child-title01">
            Summary Status Report
        </td>
        <td colspan="2">
            <table>
                <tr>
                    <td style="padding-right: 8px; font-size: 11px;">
                        <asp:CheckBox ID="chbReviewed" runat="server" Text="Reviewed" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnRequestExists" runat="server" ClientIDMode="Static" />
                    </td>
                    <td style="padding-right: 8px; font-size: 11px;">
                        <asp:CheckBox ID="chbSubmitted" runat="server" Text="Submitted" ClientIDMode="Static" />
                    </td>
                    <td style="padding-right: 0px; font-size: 11px;">
                        <asp:CheckBox ID="chbRecognized" runat="server" Text="Recognized" ClientIDMode="Static" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td style="font-size: 10px;">
                        <asp:Label ID="lblSubmitted" runat="server" ClientIDMode="Static"></asp:Label>
                    </td>
                    <td style="font-size: 10px;">
                        <asp:Label ID="lblRecognized" runat="server" ClientIDMode="Static"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td align="right">
            <div id="progress-points-title">
                Points:
            </div>
            <div id="result-progress-bar-points">
                <div id="pointsProgress" style="max-height: 2em; height: 20px;">
                </div>
            </div>
            <asp:Label ID="lblTotalPoints" runat="server" CssClass="progress-bar"></asp:Label>
        </td>
        <td align="right">
            <div id="progress-docs-title">
                Documents:
            </div>
            <div id="result-progress-bar-docs">
                <div id="docsProgress" style="max-height: 2em; height: 20px;">
                </div>
            </div>
            <asp:Label ID="lblTotalDocs" runat="server" CssClass="progress-bar"></asp:Label>
        </td>
    </tr>
</table>
<asp:Panel ID="pnlNCQASummary" runat="server">
</asp:Panel>
<table width="100%">
    <tr>
        <td align="center">
            <asp:Button ID="btnPrintSummary" runat="server" Text="Print Summary" OnClick="btnPrintSummary_Click" OnClientClick="javascript:BeginRequestHandler();" />
            <asp:Button ID="btnPrintSummaryDetails" runat="server" Text="Print With Details"
                OnClick="btnPrintSummaryDetails_Click" OnClientClick="javascript:BeginRequestHandler();"/>
            <asp:Button ID="btnPrintNotes" runat="server" Text="Print Notes" OnClick="btnPrintNotes_Click" OnClientClick="javascript:BeginRequestHandler();" />
            <input type="button" id="btnRequestSubmission" value="Request Submission to NCQA"
                clientidmode="Static" disabled="disabled" runat="server" />
        </td>
    </tr>
    <tr>
        <td style="font-size: 10px; font-style: italic;">
            Disclaimer: Reports and numeric results in the BizMed Toolbox are independently
            calculated by the BizMed software and are not guaranteed to match any NCQA generated
            reports and numeric results.
        </td>
    </tr>
</table>
<asp:HiddenField ID="hdnSummaryProjectId" ClientIDMode="Static" runat="server" />
<asp:HiddenField ID="hdnLevel" ClientIDMode="Static" runat="server" />
<asp:Button ID="btnSaveNCQACredentials" runat="server" ClientIDMode="Static" Style="display: none;"
    Text="Save" OnClick="btnSaveNCQACredentials_Click" />
<div id="lightbox-popup" class="NCQACredentials-popup" style="border: 1px solid #5880B3;
    width: 480px;">
    <div id="popupHeaderText">
        <table width="100%">
            <tr>
                <td class="trow">
                </td>
                <td width="80%" align="left" valign="middle">
                    <span class="rspan">Enter NCQA Credentials</span>
                </td>
                <td width="20%" align="right" valign="middle">
                    <a id="close-NCQACredentials">close[x]</a>
                </td>
            </tr>
        </table>
    </div>
    <table width="100%">
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <img src="../Themes/Images/caution.png" alt="caution" />
            </td>
            <td>
                To complete this action you must first complete your application and pay the required
                application fees to NCQA. You should have received a confirmation email to that
                effect from NCQA.
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                ISS License Number:
                <asp:TextBox ID="txtLicenseNumber" runat="server" Width="200px" ClientIDMode="Static"></asp:TextBox>
                <div style="display: none; color: Red;">
                    <asp:Label ID="rfvtxtLicenseNumber" runat="server" Text="ISS License Number is required!"
                        ClientIDMode="Static"></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style="font-size: 11px; font-style: italic;">
                If you haven’t done so already, please create a new User in the NCQA ISS Survey
                Tool with all permissions enabled (accept all defaults)
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style="font-size: 11px;">
                User Name:&nbsp;&nbsp;
                <asp:TextBox ID="txtUserName" runat="server" Width="150px" ClientIDMode="Static"></asp:TextBox>
                <div style="display: none; color: Red;">
                    <asp:Label ID="rfvtxtUserName" runat="server" Text="UserName is required!" ClientIDMode="Static"></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style="font-size: 11px; padding-left: 6px;">
                Password:&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="txtPassword" runat="server" autocomplete="off" TextMode="Password"
                    Width="150px" ClientIDMode="Static"></asp:TextBox>
                <div style="display: none; color: Red;">
                    <asp:Label ID="rfvtxtPassword" runat="server" Text="Password is required!" ClientIDMode="Static"></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style="font-size: 10px; font-style: italic;" class="assessment-warning">
                We will use the above credentials to upload all your documentation to NCQA
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <input type="button" id="btnSubmitNCQA" value="Submit" />
                &nbsp;&nbsp;&nbsp;
                <input type="button" id="btnCancelNCQA" value="Cancel" />
            </td>
        </tr>
    </table>
</div>
<div class="lightbox">
</div>
