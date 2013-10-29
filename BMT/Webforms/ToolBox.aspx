<%@ Page Title="" Language="C#" MasterPageFile="~/BMTMaster.Master" AutoEventWireup="true"
    CodeBehind="ToolBox.aspx.cs" Inherits="BMT.Webforms.ToolBox" %>

<%@ Register Src="~/UserControls/TreeView.ascx" TagName="TreeView" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ExpressAssessment.ascx" TagName="ExpressAssessment"
    TagPrefix="EA" %>
<%@ Register Src="~/UserControls/DocumentVista.ascx" TagName="ToolDoc" TagPrefix="TL" %>

<asp:Content ID="contentHead" ContentPlaceHolderID="head" runat="server">
    <link rel="Stylesheet" type="text/css" href="../Themes/popup.css" />
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/toolBox.js") %>"></script>
</asp:Content>
<asp:Content ID="contentInnerMenuContainer" ContentPlaceHolderID="innerMenuConatiner"
    runat="server">
    <div class="inner-menu-hover-container-left-combo">
        <table>
            <tr>
                <td style="font-size: 14px; font-weight: bold; float: left; color: #FFFFFF; margin-right: 5px;">
                    <asp:Label ID="lblEnterprise" runat="server" Text="Enterprise:" Visible="false"></asp:Label>
                </td>
                <td style="font-size: 14px; font-weight: bold; float: left">
                    <asp:DropDownList ID="ddlEnterprise" runat="server" CssClass="body-hover-combo" Style="width: 180px;
                        z-index: 10000" Visible="false" AutoPostBack="true" OnTextChanged="ddlEnterprise_OnTextChange">
                    </asp:DropDownList>
                </td>
                <td style="font-size: 14px; font-weight: bold; float: left; color: #FFFFFF; margin-right: 5px; margin-left: 20px;">
                    <asp:Label ID="lblPractices" runat="server" Text="Practice:" Visible="false"></asp:Label>
                </td>
                <td style="font-size: 14px; font-weight: bold; float: left">
                    <asp:DropDownList ID="ddlPractices" runat="server" CssClass="body-hover-combo" Style="width: 180px;
                        z-index: 10000" Visible="false" AutoPostBack="true" OnTextChanged="ddlPractices_OnTextChange">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
    <div class="inner-menu-hover-container-right-combo">
        <!--  Add User, Search text boxes, upload documents -->
        <div style="float: right; margin-left: 10px; width: 150px">
            <asp:Button ID="btnUploadDocuments" runat="server" Text="Upload" CssClass="top-button-yollaw"
                Style="width: 130px" OnClientClick="return false;" Visible="false" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="contentBodyContainer" ContentPlaceHolderID="bodyContainer" runat="server">
    <div style="width: 550px">
        <uc1:TreeView ID="TreeControl" runat="server" TableName="ToolSection" />
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
                        <iframe id="GenericPopupIFrame" scrolling="no" style="height: auto; min-height: 200px;
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
                    
                <TL:ToolDoc ID="ToolList" runat="server" Visible="true" DBTableName="ToolDocument"  />
                
            </ContentTemplate>
        </asp:UpdatePanel>
        <input id="hdnSectionID" type="hidden" runat="server" clientidmode="Static" />
        <input id="hdnContentType" type="hidden" runat="server" clientidmode="Static" />
    </div>
</asp:Content>
