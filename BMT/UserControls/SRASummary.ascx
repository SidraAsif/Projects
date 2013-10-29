<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SRASummary.ascx.cs"
    Inherits="SRASummary" ViewStateMode="Enabled"%>
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
    <br />
    <br />
    <tr class="standard-title">
        <td>
            Site Information
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblsiteDescription" runat="server" Text="" CssClass="element-title"
                Style="margin-left: 27px;"></asp:Label>
            <p style="margin-left: 30px; font-style: italic;">
                (To edit Site Information, please go to the Settings tab and select Site Administration
                from the menu on the left)</p>
        </td>
    </tr>
</table>
<asp:Panel ID="pnlTable" runat="server"  ClientIDMode="Static">
    <table width="100%">
        <tr class="standard-title">
            <td colspan="2">
                Contributors
            </td>
        </tr>
        <tr class="contributors-text">
            <td>
                Practice Contact
            </td>
            <td>
                Name:
            </td>
            <td>
                <asp:TextBox ID="txtPCName" runat="server" ClientIDMode="Static">
                </asp:TextBox>
            </td>
            <td>
                Phone:
            </td>
            <td>
                <asp:TextBox ID="txtPCPhone" runat="server" ClientIDMode="Static">
                </asp:TextBox>
            </td>
            <td>
                Email:
            </td>
            <td>
                <asp:TextBox ID="txtPCEmail" runat="server" ClientIDMode="Static">
                </asp:TextBox>
            </td>
        </tr>
        <tr class="contributors-text">
            <td>
                IT Consultant Contact:
            </td>
            <td>
                Name:
            </td>
            <td>
                <asp:TextBox ID="txtITName" runat="server" ClientIDMode="Static">
                </asp:TextBox>
            </td>
            <td>
                Phone:
            </td>
            <td>
                <asp:TextBox ID="txtITPhone" runat="server" ClientIDMode="Static">
                </asp:TextBox>
            </td>
            <td>
                Email:
            </td>
            <td>
                <asp:TextBox ID="txtITEmail" runat="server" ClientIDMode="Static">
                </asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Panel>
<table width="100%">
    <br />
    <tr>
        <td class="standard-title">
            Summary Status Report
        </td>
    </tr>
    <tr>
        <td align="right" style="padding-right: 5px;">
            <a href="../StDocs/General/Security Risk Assessment - Instructions.pdf" target="_blank" >Read Instructions & Documentation</a>            
        </td>
    </tr>
</table>
<asp:Panel ID="pnlSummary" runat="server">
</asp:Panel>
<asp:HiddenField ID="hdnIsEdited" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnContributor" runat="server" ClientIDMode="Static" />
<table width="100%">
    <br />
    <tr>
        <td align="right">
            <asp:Button ID="btnPrintSummary" runat="server" Text="Print Summary" OnClick="btnPrintSummary_Click" OnClientClick="javascript:BeginRequestHandler();" UseSubmitBehavior="false" />
        </td>
        <td>
            <input type="button" id="btnNewAssessment" value="Start New Assessment" class="newAssessment-btn" disabled="disabled" runat="server"/>
        </td>
    </tr>
    <tr>
        <td width="50%">
        </td>
        <td class="NewAssessmentNote">
            Note: To start a new Assessment, you must first Finalize the Findings and Followup
            tabs. This is designed to protect you from accidental data loss.
        </td>
    </tr>
    <br />
</table>
<div id="lightbox-popup" class="newAssessment-popup" style="border: 1px solid #5880B3;">
    <div id="popupHeaderText">
        <table width="100%">
            <tr>
                <td class="trow">
                </td>
                <td width="80%" align="center" valign="middle">
                    <span class="rspan">Start New Security Risk Assessment</span>
                </td>
                <td width="20%" align="right" valign="middle">
                    <a id="close-newAssessment">close[x]</a>
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
                <img src="../Themes/Images/caution.png" alt="caution"/>
            </td>
            <td>
                You are about to delete all user entered data in ALL the Security Risk Assessment
                Tabs.
                <br />
                <div class="assessment-warning">
                This action CANNOT be undone!!
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
            <td>
                Do you wish to continue?
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
                <input type="button" id="btnDeleteAssessment" value="Delete" />
                <input type="button" id="btnCancelAssessment" value="Cancel" />
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
            <td style="font-size: 11px;" class="assessment-warning">
                If you choose to Delete, a complete copy of your current Security Risk Assessment
                will be automatically saved in PDF format in your SRA Copies folder for future reference.
            </td>
        </tr>
    </table>
</div>
<div class="lightbox">
</div>
