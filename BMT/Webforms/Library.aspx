<%@ Page Title="" Language="C#" MasterPageFile="~/BMTMaster.Master" AutoEventWireup="true"
    CodeBehind="Library.aspx.cs" Inherits="BMT.Webforms.Library" %>

<%@ Register Src="~/UserControls/TreeView.ascx" TagName="TreeView" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/DocumentVista.ascx" TagName="LibraryDoc" TagPrefix="TL" %>
<asp:Content ID="contentHead" ContentPlaceHolderID="head" runat="server">
    <link rel="Stylesheet" type="text/css" href="../Themes/popup.css" />
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/library.js") %>"></script>
</asp:Content>
<asp:Content ID="contentInnerMenuContainer" ContentPlaceHolderID="innerMenuConatiner"
    runat="server">
    <div class="inner-menu-hover-container-left">
    </div>
    <div class="inner-menu-hover-container-right">
        <!--  Add User, Search text boxes, upload documents -->
        <div style="float: right; margin-left: 10px; width: 150px">
            <asp:Button ID="btnUploadDocuments" runat="server" Text="Upload" CssClass="top-button-yollaw"
                Style="width: 130px" OnClientClick="return false;" Visible="false" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="contentBodyContainer" ContentPlaceHolderID="bodyContainer" runat="server">
    <div style="width: 550px">
        <uc1:TreeView ID="TreeControl" runat="server" TableName="LibrarySection" />
        <input id="hdnTreeNodeID" type="hidden" runat="server" clientidmode="Static" />
    </div>
    <%-- #############################################  POPUP START HERE #####################################################--%>
    <div id="lightbox-popup" class="uploadbox-popup" style="border: 1px solid #5880B3;">
        <div id="popupHeaderText">
            <table width="100%">
                <tr>
                    <td class="trow">
                    </td>
                    <td width="62%" align="center" valign="middle">
                        <b>Upload File</b>
                    </td>
                    <td width="10%" align="right" valign="middle">
                        <a id="close-popup" href="#" style="text-decoration: none; font-size: 16px; color: White;">
                            close[x]</a>
                    </td>
                </tr>
            </table>
        </div>
        <table width="100%">
            <tr>
                <td>
                    <asp:Panel ID="pnlIFrame" runat="server">
                        <iframe scrolling="no" id="GenericPopupIFrame" style="height: auto; min-height: 200px;
                            width: 100%; overflow: hidden;" src="../GenericFileUpload.aspx" frameborder="0">
                        </iframe>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    <!-- /lightbox-panel -->
    <div id="lightbox" class="uploadbox">
    </div>
    <%-- ############################################# POPUP END HERE #####################################################--%>
    <div class="body-container-right">
        <asp:UpdatePanel ID="UpdatePanelControl" runat="server" OnLoad="UpdatePanelControl_Load">
            <ContentTemplate>
                <asp:Label ID="lblContentTypeName" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="15pt"></asp:Label>
                <TL:LibraryDoc ID="LibraryList" runat="server" DBTableName="LibraryDocument" Visible="true" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <input id="hdnLibrarySectionID" type="hidden" runat="server" clientidmode="Static" />
        <input id="hdnContentType" type="hidden" runat="server" clientidmode="Static" />
    </div>
</asp:Content>
