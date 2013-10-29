<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MORe.ascx.cs" Inherits="MORe"
    EnableViewState="true" ViewStateMode="Enabled" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<%@ Register Src="~/UserControls/LoadingPanel.ascx" TagName="LoadingPanel" TagPrefix="ucl" %>
<link rel="Stylesheet" type="text/css" href="../Themes/style.css" />
<link rel="Stylesheet" type="text/css" href="../Themes/PCMH.css" />
<br />
<asp:Panel ID="pnlmessage" runat="server" Style="margin-left: 10px;">
    <ucdm:DisplayMessage ID="message" runat="server" DisplayMessageWidth="700" ValidationSummaryHeaderText="<div class='error-SummaryHeader'>Required information is missing.</div>">
    </ucdm:DisplayMessage>
</asp:Panel>
<div class="projectInfo">
    <asp:Label ID="lblSiteInfo" runat="server"></asp:Label>
</div>
<div id="NCQARequirements">
    <asp:Panel ID="pnlMOReRequirements" runat="server">
    </asp:Panel>
    <asp:Button ID="btnsave" runat="server" ClientIDMode="Static" Style="display: none;"
        Text="Save" OnClick="btnSave_Click" />
</div>
<%-- ############################ POPUP ################################################################--%>
<%-- ############################ UPLOAD START HERE #####################################################--%>
<asp:HiddenField ID="hiddenActiveElementId" runat="server" ClientIDMode="Static" />
<div id="lightbox-popup" class="uploadbox-popup" style="border: 1px solid #5880B3;">
    <div id="popupHeaderText">
        <table width="100%">
            <tr>
                <td class="trow">
                </td>
                <td width="80%" align="center" valign="middle">
                    <span class="rspan">Upload File</span>
                </td>
                <td width="20%" align="right" valign="middle">
                    <a id="close-UploadPopUp">close[x]</a>
                </td>
            </tr>
        </table>
    </div>
    <table width="100%">
        <tr>
            <td>
                <asp:HiddenField ID="hiddenFUDElementId" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hiddenFUDFactorId" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hiddenFUDPCMH" runat="server" ClientIDMode="Static" />
            </td>
        </tr>
        <tr>
            <td class="popup-info">
                <asp:Label ID="lblFUDInfo" runat="server" ClientIDMode="Static" Text="" Width="395px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlFrame" runat="server">
                    <iframe scrolling="no" id="fuPage" style="min-height: 425px; height: auto; width: 100%;
                        margin-left: 10px; overflow: hidden;" src="../FileUpload.aspx" frameborder="0">
                    </iframe>
                </asp:Panel>
            </td>
        </tr>
    </table>
</div>
<!-- /lightbox-panel -->
<div id="lightbox" style="height: 4000px;" class="uploadbox">
</div>
<%-- ############################ UPLOAD END HERE #######################################################--%>
<%-- ############################ NOTES START HERE #######################################################--%>
<div id="lightbox-popup" class="factornotebox-popup" style="border: 1px solid #5880B3;">
    <div id="popupHeaderText">
        <table width="100%">
            <tr>
                <td class="trow">
                </td>
                <td width="80%" align="center" valign="middle">
                    <span style="margin-right: -5.4em;">Add/Edit Private Notes </span>
                </td>
                <td width="20%" align="right" valign="middle">
                    <a id="close-FactorNotePopUp">close[x]</a>
                </td>
            </tr>
        </table>
    </div>
    <table width="100%">
        <tr>
            <td colspan="2">
                <asp:HiddenField ID="hiddenFNElementId" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hiddenFNFactorId" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hiddenQuestionId" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hiddenSubheaderId" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hiddenFNPCMH" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="IsEdit" runat="server" ClientIDMode="Static" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="popup-info">
                <asp:Label ID="lblFNInfo" runat="server" ClientIDMode="Static" Text="" Width="395px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:TextBox ID="txtFNComments" runat="server" Width="395px" TextMode="MultiLine"
                    Height="100px" Style="resize: none;" ClientIDMode="Static">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div id="note-history">
                </div>
            </td>
        </tr>
        <tr>
            <td align="right" width="45%">
                <span>
                    <asp:Button ID="btnFNsave" runat="server" Text="OK" OnClick="btnFNSave_Click" CssClass="cancel-popUp"
                        ClientIDMode="Static" />
                </span>
            </td>
            <td align="left" width="50%">
                <a id="close-FactorNotePopUp" href="#" style="text-decoration: none; font-size: 16px;
                    color: White;">
                    <asp:Button ID="btnFNCancel" runat="server" Text="Cancel" CausesValidation="false"
                        CssClass="cancel-popUp" OnClientClick="return false;" ClientIDMode="Static" /></a>
            </td>
        </tr>
        <tr>
            <td colspan="2">
            </td>
        </tr>
    </table>
</div>
<!-- /lightbox-panel -->
<%-- ############################ NOTES END HERE #######################################################--%>
<%-- ############################ COMMENT START HERE #######################################################--%>
<div id="lightbox-popup" class="factorCriticalnotebox-popup" style="border: 1px solid #5880B3;">
    <div id="popupHeaderText">
        <table width="100%">
            <tr>
                <td class="trow">
                </td>
                <td width="80%" align="center" valign="middle">
                    <span class="rspan">Add Comment</span>
                </td>
                <td width="20%" align="right" valign="middle">
                    <a id="close-FactorCriticalNotePopUp">close[x]</a>
                </td>
            </tr>
        </table>
    </div>
    <table width="100%">
        <tr>
            <td colspan="2">
                <asp:HiddenField ID="hiddenFCNElementId" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hiddenFCNFactorId" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hiddenFCNPCMH" runat="server" ClientIDMode="Static" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="popup-info">
                <asp:Label ID="lblFCNInfo" runat="server" ClientIDMode="Static" Text="" Width="395px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblFCNNote" runat="server" ClientIDMode="Static" Text="" Width="395px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:TextBox ID="txtFCNComments" runat="server" Width="395px" TextMode="MultiLine"
                    Height="100px" Style="resize: none;" ClientIDMode="Static">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right" width="45%">
                <span>
                    <asp:Button ID="btnFCNSave" runat="server" Text="OK" OnClick="btnFCNSave_Click" CssClass="cancel-popUp" />
                </span>
            </td>
            <td align="left" width="50%">
                <a id="close-FactorCriticalNotePopUp" href="#" style="text-decoration: none; font-size: 16px;
                    color: White;">
                    <asp:Button ID="btnFCNCancel" runat="server" Text="Cancel" CausesValidation="false"
                        CssClass="cancel-popUp" OnClientClick="return false;" ClientIDMode="Static" /></a>
            </td>
        </tr>
    </table>
</div>
<!-- /lightbox-panel -->
<%-- ############################ COMMENT END HERE #######################################################--%>
<%-- ############################ EVALUATION (ELEMENT) NOTE START HERE #######################################################--%>
<div id="lightbox-popup" class="elementnotebox-popup" style="border: 1px solid #5880B3;">
    <div id="popupHeaderText">
        <table width="100%" hight="500px">
            <tr>
                <td class="trow">
                </td>
                <td width="80%" align="center" valign="middle">
                    <span class="rspan">Add Evaluation Note</span>
                </td>
                <td width="20%" align="right" valign="middle">
                    <a id="close-elementnotebox-popup">close[x]</a>
                </td>
            </tr>
        </table>
    </div>
    <table table="100%">
        <tr>
            <td colspan="2">
                <asp:HiddenField ID="hiddenElementId" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hiddenElementPCMH" runat="server" ClientIDMode="Static" />
            </td>
        </tr>
        <tr>
            <td colspan="2" class="popup-info">
                <asp:Label ID="lblElementInfo" runat="server" ClientIDMode="Static" Text="" Width="395px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:TextBox ID="txtElementComments" runat="server" Width="395px" TextMode="MultiLine"
                    Height="100px" Style="resize: none;" ClientIDMode="Static">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right" width="45%">
                <span>
                    <asp:Button ID="btnElementSave" runat="server" Text="OK" OnClick="btnElementSave_Click"
                        CssClass="cancel-popUp" OnClientClick="javascript:ChangeElementIconImage();" />
                </span>
            </td>
            <td align="left" width="50%">
                <a id="close-elementnotebox-popup" href="#" style="text-decoration: none; font-size: 16px;
                    color: White;">
                    <asp:Button ID="btnCancelElementNote" runat="server" Text="Cancel" CausesValidation="false"
                        CssClass="cancel-popUp" OnClientClick="return false;" ClientIDMode="Static" /></a>
            </td>
        </tr>
    </table>
</div>
<!-- /lightbox-panel -->
<%-- ############################ EVALUATION NOTE START HERE END HERE #######################################################--%>
<%-- ############################       NCQA Uploaded Docs            ####################################################### --%>
<div class="lightbox-popup" id="uploadedDocViewer" style="border: 1px solid #5880B3;
    min-height: 410px; min-width: 1000px; position: absolute; left: auto;">
    <div class="popupHeaderText">
        <table width="100%">
            <tr>
                <td width="75%" align="left" valign="middle">
                    <asp:Label ID="lblDocViewerTitle" runat="server" ClientIDMode="Static"></asp:Label>
                </td>
                <td width="25%" align="right" valign="middle">
                    <a id="closeUploadedDocViewer">close[x]</a>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td>
                    <asp:Panel ID="pnlDocViewer" runat="server">
                        <iframe id="IframeDocViewer" style="min-height: 400px; height: 400px; overflow: hidden;
                            width: 100%; margin: auto;" src="../Webforms/MOReUploadedDocs.aspx" frameborder="0"
                            scrolling="auto"></iframe>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
</div>
<!-- /lightbox-panel -->
<div class="lightbox">
</div>
<asp:HiddenField ID="IsSave" ClientIDMode="Static" runat="server" />
<!-- /lightbox -->
