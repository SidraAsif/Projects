<%@ Page Language="C#" MasterPageFile="~/BMTMaster.Master" AutoEventWireup="true"
    ViewStateMode="Enabled" CodeBehind="Projects.aspx.cs" Inherits="BMT.Webforms.Projects" %>

<%@ Register Src="~/UserControls/NCQARequirements.ascx" TagName="NCQARequirements"
    TagPrefix="NCQA" %>
<%@ Register Src="~/UserControls/MOReRequirements.ascx" TagName="MOreRequirements"
    TagPrefix="MORe" %>
<%@ Register Src="~/UserControls/NCQASubmission.ascx" TagName="NCQASubmission" TagPrefix="NCQASub" %>
<%@ Register Src="~/UserControls/DocumentVista.ascx" TagName="ProjectDoc" TagPrefix="PD" %>
<%@ Register Src="~/UserControls/ExpressAssessment.ascx" TagName="ExpressAssessment"
    TagPrefix="EA" %>
<%@ Register Src="~/UserControls/LoadingPanel.ascx" TagName="LoadingPanel" TagPrefix="ucl" %>
<%@ Register Src="~/UserControls/TreeView.ascx" TagName="TreeView" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PriceCalculator.ascx" TagName="PriceCalculator"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/SecurityRiskAssessment.ascx" TagName="SecurityRiskAssessment"
    TagPrefix="SRA" %>
<%@ Register Src="~/UserControls/ITConsultant.ascx" TagName="ITConsultant" TagPrefix="IT" %>
<asp:Content ID="contentHead" ContentPlaceHolderID="head" runat="server">
    <link rel="Stylesheet" type="text/css" href="../Themes/popup.css" />
    <link rel="Stylesheet" type="text/css" href="../Themes/loading-panel.css" />
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/querystring-0.9.0.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/project.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jquery-ui-1.8.16.custom.min.js") %>"></script>
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
                <td style="font-size: 14px; font-weight: bold; float: left; color: #FFFFFF; margin-right: 5px;
                    margin-left: 20px;">
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
                Style="width: 130px" OnClientClick="return false; " Visible="false" />
            <asp:Button ID="btnAddEHR" runat="server" Text="Add Another EHR/PM" CssClass="top-button-yollaw"
                Style="width: 150px" OnClientClick="javascript:return false;" Visible="false"
                ClientIDMode="Static" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="contentBodyContainer" ContentPlaceHolderID="bodyContainer" runat="server">
    <uc1:TreeView ID="TreeControl" runat="server" TableName="ProjectSection" />
    <%-- #############################################  POPUP START HERE #####################################################--%>
    <div id="lightbox-popup" class="uploadbox-popup" style="border: 1px solid #5880B3;">
        <div id="popupHeaderText">
            <table width="100%">
                <tr>
                    <td class="trow">
                    </td>
                    <td width="80%" align="center" valign="middle">
                        <span class="rspan"><b>Upload File</b></span>
                    </td>
                    <td width="20%" align="right" valign="middle">
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
                            width: 100%; overflow: hidden;" src="../ProjectFileUpload.aspx" frameborder="0">
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
        <asp:UpdatePanel ID="UpdatePanelControl" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="lblContentTypeName" runat="server" Font-Bold="True" Font-Names="Arial"
                    Font-Size="15pt"></asp:Label>
                <PD:ProjectDoc ID="ProjectDocument" runat="server" DBTableName="ProjectDocument"
                    Visible="false" />
                <EA:ExpressAssessment ID="ExpressAssessment" runat="server" Visible="false" />
                <NCQA:NCQARequirements ID="NCQARequirements" runat="server" Visible="false" />
                <MORe:MOreRequirements ID="MOReRequirments" runat="server" Visible ="false" />
                <NCQASub:NCQASubmission ID="NCQASubmission" runat="server" Visible="false" />
                <uc1:PriceCalculator ID="PriceCalculator" runat="server" Visible="false" />
                <SRA:SecurityRiskAssessment ID="SecurityRiskAssessment" runat="server" Visible="false" />
                <IT:ITConsultant ID="ITConsultant" runat="server" Visible="false" />
                <asp:Panel ID="pnlDynamicControl" runat="server">
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <input id="hdnProjectUsageID" type="hidden" runat="server" clientidmode="Static" />
        <input id="hdnTreeNodeID" type="hidden" runat="server" clientidmode="Static" />
    </div>
</asp:Content>
