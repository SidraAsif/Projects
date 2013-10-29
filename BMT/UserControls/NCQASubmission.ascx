<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NCQASubmission.ascx.cs"
    Inherits="NCQASubmission" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<link rel="Stylesheet" type="text/css" href="../Themes/NCQA.css" />
<link rel="Stylesheet" type="text/css" href="../Themes/popup.css" />
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/apprise-1.5.min.js") %>"></script>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/ncqaSubmission.js") %>"></script>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jquery-ui-1.8.16.custom.min.js") %>"></script>
<!--[if lt IE 9]>
<script src="http://ie7-js.googlecode.com/svn/version/2.1(beta4)/IE9.js"></script>
<![endif]-->
<br />
<asp:Panel ID="pnlmessage" runat="server" Style="margin-left: 10px;">
    <ucdm:DisplayMessage ID="message" runat="server" DisplayMessageWidth="700"></ucdm:DisplayMessage>
</asp:Panel>
<div class="siteInfo">
    <asp:Label ID="lblSiteInfo" runat="server" CssClass="master-title" Text=""></asp:Label>
</div>
<table width="95%">
    <tr class="child-title01">
        <td colspan="2">
            NCQA Documentation
        </td>
    </tr>
</table>
<asp:Panel ID="pnlNCQASummary" runat="server">
</asp:Panel>
<div class="lightbox-popup" style="border: 1px solid #5880B3;">
    <table width="100%">
        <tr>
            <td>
                <asp:Panel ID="pnlDeleteFile" runat="server">
                    <iframe scrolling="no" id="iFrameFileDelete" style="overflow: hidden; width: 100%;
                        min-height: 300px; margin: 0px;" src="MOReDeleteFiles.aspx" frameborder="0"></iframe>
                </asp:Panel>
            </td>
        </tr>
    </table>
</div>
<!-- Edit File Properties page -->
<div id="lightbox-popup" style="border: 1px solid #5880B3;" class="edit-popup">
    <div class="popupHeaderText">
        <table width="100%">
            <tr>
                <td width="80%" align="left" valign="middle">
                    <span class="rspan">Edit Document Properties</span>
                </td>
                <td width="20%" align="right" valign="middle">
                    <a id="close-UploadPopUp">close[x]</a>
                </td>
            </tr>
        </table>
    </div>
    <table width="100%">
        <tr>
            <td colspan="3" class="popup-info">
                <asp:Label ID="lblFactorInfo" runat="server" ClientIDMode="Static" Text="" Width="395px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="normal">
                <p>
                    Document Name:</p>
            </td>
            <td colspan="2" class="normal">
                <asp:TextBox ID="txtDocName" runat="server" CssClass="text-field02" ClientIDMode="Static"
                    MaxLength="100"> </asp:TextBox>
                <div class="user-notice">
                    (If left blank name on file system will be used)</div>
            </td>
        </tr>
        <tr>
            <td class="normal">
                <p>
                    Reference Pages:</p>
            </td>
            <td colspan="2" class="normal">
                <asp:TextBox ID="txtReferencePage" runat="server" CssClass="text-field02" ClientIDMode="Static">
                </asp:TextBox><div class="user-notice">
                    (If left blank all pages will be considered)</div>
            </td>
        </tr>
        <tr>
            <td class="normal">
                <p>
                    Relevancy Level:*</p>
            </td>
            <td colspan="2" class="normal">
                <asp:DropDownList ID="ddlRelevancyLevel" runat="server" Width="140px" ClientIDMode="Static">
                    <asp:ListItem Text="Primary" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Secondary" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Supporting" Value="3"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="normal">
                <p>
                    Document Type:*</p>
            </td>
            <td colspan="2" class="normal">
                <asp:DropDownList ID="ddldocType" runat="server" Width="140px" ClientIDMode="Static">
                    <asp:ListItem Text="PoliciesOrProcess" Value="1"></asp:ListItem>
                    <asp:ListItem Text="ReportsOrLogs" Value="2"></asp:ListItem>
                    <asp:ListItem Text="ScreenshotsOrExamples" Value="3"></asp:ListItem>
                    <asp:ListItem Text="RRWB" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Extra" Value="5"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <asp:Button ID="btnSaveDoc" runat="server" Text="Save" ClientIDMode="Static" OnClientClick="javascript:SaveChanges(); return false;" /><asp:Button
                    ID="btnCancel" runat="server" Text="Cancel" ClientIDMode="Static" OnClientClick="javascript:return false;" />
            </td>
        </tr>
    </table>
</div>
<!-- Delete File -->
<div id="lightbox-popup" class="delete-popup" style="border: 1px solid #5880B3; width: 450px">
    <div class="popupHeaderText">
        <table width="100%">
            <tr>
                <td width="80%" align="left" valign="middle">
                    <span class="rspan">Remove Document</span>
                </td>
                <td width="20%" align="right" valign="middle">
                    <a id="close-lightbox-popup-delete">close[x]</a>
                </td>
            </tr>
        </table>
    </div>
    <table width="100%">
        <tr>
            <td colspan="3" class="popup-info">
                <asp:Label ID="lblDelFactorInfo" runat="server" ClientIDMode="Static" Text="" Width="395px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="left">
                This document is linked to the following factors:
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <div id="dynamicFactorsList">
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="font-size: smaller; font-weight: bold">
                To remove the link to a particular Factor uncheck the appropriate box and click
                save
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <asp:Button ID="btnDelSave" runat="server" Text="Save" ClientIDMode="Static" OnClientClick="javascript:DeleteFile('save');return false;" />
            </td>
        </tr>
        <tr>
            <td colspan="3" style="font-size: smaller; font-weight: bold">
                To completely remove the document from your project click the delete button.
            </td>
        </tr>
        <tr>
            <td colspan="3" style="font-size: smaller; font-weight: bold; color: Red;">
                warning: This action cannot be undone.
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <asp:Button ID="btnDelete" runat="server" Text="Delete" ClientIDMode="Static" OnClientClick="javascript:DeleteFile('delete');return false;" />
                <asp:Button ID="btnDelCancel" runat="server" Text="Cancel" ClientIDMode="Static"
                    OnClientClick="javascript:return false;" />
            </td>
        </tr>
    </table>
</div>
<!-- Replace File -->
<div id="lightbox-popup" class="replace-popup" style="border: 1px solid #5880B3;">
    <div id="popupHeaderText">
        <table width="100%">
            <tr>
                <td class="trow">
                </td>
                <td width="80%" align="center" valign="middle">
                    <span class="rspan">Replace File</span>
                </td>
                <td width="20%" align="right" valign="middle">
                    <a id="close-Replace-popup">close[x]</a>
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
                    <iframe scrolling="no" id="fuPage" style="min-height: 125px; height: auto; width: 100%;
                        margin-left: 10px; overflow: hidden;" src="../FileUpload.aspx" frameborder="0">
                    </iframe>
                </asp:Panel>
            </td>
        </tr>
    </table>
</div>

<!-- /lightbox-panel -->
<div class="lightbox">
</div>
<asp:HiddenField ID="hdnPCMHId" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnElementId" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnFactorId" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnFile" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnProjectId" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnProjectUsageId" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hiddenPracticeId" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnTemplateId" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnSiteId" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hiddenPracticeName" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hiddenSiteName" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnCurrentDocType" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hiddenNode" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnPageUrl" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnDocLinkedTo" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hiddenSiteId" runat="server" ClientIDMode="Static" />
