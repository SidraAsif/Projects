<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MOReCorporateSubmission.aspx.cs"
    Inherits="BMT.Webforms.MOReCorporateSubmission" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>CorporateSubmission</title>
    <link type="text/css" rel="Stylesheet" href="../Themes/style.css" />
    <link rel="Stylesheet" type="text/css" href="../Themes/popup.css" />
    <%@ register src="~/UserControls/DisplayMessage.ascx" tagname="DisplayMessage" tagprefix="ucdm" %>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jQuery-1.7.1.min.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/MORecorporateSubmission.js") %>"></script>
</head>
<body style="height: 540px">
    <form id="formCorporateSubmission" runat="server">
    <div>
        <table width="100%" id="corporatePopUp" class="corporatePopUp" runat="server" clientidmode="Static">
            <tr>
                <td>
                    <ucdm:DisplayMessage ID="messagePopup" runat="server" DisplayMessageWidth="580" ClientIDMode="Static" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    This list includes the 16 elements that are eligible for the Corporate Survey Tool.
                    All other element require responses in the site specific survey tools. You must
                    respond to atleast 11 elements in the Corparate Survey Tool. Please designate which
                    elements you will be responding to at the Corporate level.
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    NOTE: Do not respond to the designated elements in the site survey as this will
                    cause loss of data.
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="elementListDiv" runat="server" ClientIDMode="Static">
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ClientIDMode="Static" runat="server" ID="saveElement" Text="Save/Update"
                        OnClientClick="CorpElement(); return false;" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnPracticeId" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnTemplateId" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnRecievedQuestionnaire" runat="server" ClientIDMode="Static" />
    </div>
    <%-- ############################ CONFIRMATION POPUP START HERE #####################################################--%>
    <div id="lightbox-popup" class="confirmation-popup" style="border: 1px solid #5880B3;
        width: 400px;">
        <div id="popupHeaderText">
            <table width="100%">
                <tr>
                    <td width="80%" align="left" valign="middle">
                        <span class="rspan">Remove Document</span>
                    </td>
                    <td width="20%" align="right" valign="middle">
                        <a id="close-confirmation">close[x]</a>
                    </td>
                </tr>
            </table>
        </div>
        <table width="100%" id="WarningPopUp" class="corporatePopUp" runat="server" clientidmode="Static">
            <tr>
                <td width="20%">
                    <img src="../Themes/Images/caution.png" alt="caution" />
                </td>
                <td width="80%">
                    <asp:Label runat="server" ClientIDMode="Static" ID="alertNotification"></asp:Label>
                    <br />
                    <br />
                    <asp:Label runat="server" ClientIDMode="Static" ID="selectedElementName" Font-Bold="true"></asp:Label>
                    <br />
                    <br />
                    <asp:Label runat="server" ClientIDMode="Static" ID="alertNotificationRem"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr align="center">
                <td colspan="2">
                    <asp:Label runat="server" ClientIDMode="Static" Font-Size="10px" ForeColor="Red"
                        ID="warning"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr align="center">
                <td colspan="2">
                    <a id="close-confirmation" href="#" style="text-decoration: none; font-size: 16px;
                        color: White;">
                        <asp:Button ID="btnDeleteCorpElement" runat="server" Text="OK" CausesValidation="false"
                            ClientIDMode="Static" OnClientClick="deleteFromXMLNode(); return false;" /></a>
                    <a id="close-confirmation" href="#" style="text-decoration: none; font-size: 16px;
                        color: White;">
                        <asp:Button ID="btnCancelNotificationPopup" runat="server" Text="Cancel" CausesValidation="false"
                            OnClientClick="return false;" ClientIDMode="Static" /></a>
                </td>
            </tr>
        </table>
    </div>
    <!-- /lightbox-panel -->
    <div class="lightbox">
    </div>
    <!-- /lightbox -->
    <%-- ############################ CONFIRMATION POPUP END HERE #####################################################--%>
    </form>
</body>
</html>
