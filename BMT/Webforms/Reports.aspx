<%@ Page Title="" Language="C#" MasterPageFile="~/BMTMaster.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="Reports.aspx.cs" Inherits="BMT.Webforms.Reports" %>

<%@ Register Src="~/UserControls/LoadingPanel.ascx" TagName="LoadingPanel" TagPrefix="ucl" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<%@ Register Src="~/UserControls/TreeView.ascx" TagName="TreeView" TagPrefix="uc1" %>
<asp:Content ID="contentHead" ContentPlaceHolderID="head" runat="server">
    <link rel="Stylesheet" type="text/css" href="../Themes/loading-panel.css" />
    <link rel="Stylesheet" type="text/css" href="../Themes/style.css" />
    <link rel="Stylesheet" type="text/css" href="../Themes/PCMH.css" />
    <style type="text/css">
        .style8
        {
            width: 84px;
        }
        .style9
        {
            width: 138px;
        }
        .style10
        {
            width: 158px;
        }
        .style11
        {
            width: 160px;
        }
        .style14
        {
            width: 164px;
        }
        .style15
        {
            width: 200px;
        }
        .style16
        {
            width: 179px;
        }
    </style>
</asp:Content>
<asp:Content ID="contentInnerMenuContainer" ContentPlaceHolderID="innerMenuConatiner"
    runat="server">
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jquery-ui-1.8.12.custom.min.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/pcmhReports.js") %>"></script>
    <div class="inner-menu-hover-container-left-combo">
        <table>
            <tr>
                <td style="font-size: 14px; font-weight: bold; float: left; color: #FFFFFF; margin-right: 5px;">
                    <asp:Label ID="lblEnterprise" runat="server" Text="Enterprise:" Visible="false"></asp:Label>
                </td>
                <td style="font-size: 14px; font-weight: bold; float: left">
                    <asp:DropDownList ID="ddlEnterprise" runat="server" CssClass="body-hover-combo" Style="width: 180px;
                        z-index: 10000" Visible="false">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="bodyContainer" runat="server">
    <asp:HiddenField ID="hiddenElementId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenFactorId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenPCMHId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenPCMHTitle" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenElementTitle" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HiddenFactorTitle" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenConsultantId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenConsultantName" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenCompleteId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenCompleteText" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenPracticeSizeId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenPracticeSizeTitle" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenContentType" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenIsNewRport" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenSectionId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenFactorArray" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenFactorTitleArray" runat="server" ClientIDMode="Static" />
    <div style="width: 550px">
        <uc1:TreeView ID="TreeControl" runat="server" TableName="ReportSection" />
    </div>
    <div class="body-container-right">
        <asp:Panel ID="pnlReports" runat="server" Visible="false">
            <div id="reports">
                <table width="100%">
                    <tr>
                        <td>
                            <h1>
                                PCMH General Status Report</h1>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblTitle" runat="server" CssClass="reports-label" Text="Edit Report Title"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblTitleLine1" runat="server" CssClass="reports-elements" Text="Title Line 1:"></asp:Label>
                            <asp:TextBox ID="txtLine1" runat="server" CssClass="reports-elements" Style="margin-left: 30px;
                                width: 500px" ValidationGroup="runReport" CausesValidation="true">Patient Centered Medical Home – Status Report</asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvFtxtLine1" runat="server" ErrorMessage="Enter Report Title"
                                Text="Please Enter Title 1" ControlToValidate="txtLine1" ValidationGroup="runReport"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblTitleLine2" runat="server" CssClass="reports-elements" Text="Title Line 2:"></asp:Label>
                            <asp:TextBox ID="txtLine2" runat="server" CssClass="reports-elements" Style="margin-left: 30px;
                                width: 500px">All Practices</asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblGoals" runat="server" CssClass="reports-label" Text="Select Project Goals"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td valign="top" class="style15">
                                        <asp:Label ID="lblPCMHStandard" runat="server" CssClass="reports-elements" Style="margin-top: 0px;
                                            margin-left: 0px" Text="PCMH Standards:"></asp:Label>
                                    </td>
                                    <td valign="top" class="style14">
                                        <asp:Label ID="lblPCMHElements" runat="server" CssClass="reports-elements" Style="margin-top: 0px;
                                            margin-left: 0px" Text="PCMH Elements:"></asp:Label>
                                    </td>
                                    <td valign="top" class="style16">
                                        <asp:Label ID="lblPCMHFactors" runat="server" CssClass="reports-elements" Style="margin-top: 0px;
                                            margin-left: 0px" Text="PCMH Factors:"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style15">
                                        <asp:ListBox ID="lstPCMHStandard" runat="server" CssClass="reports-elements" Style="height: 100px;
                                            width: 145px; margin-left: 50px" onChange="javascript:GetElements();" ClientIDMode="Static">
                                            <asp:ListItem></asp:ListItem>
                                        </asp:ListBox>
                                    </td>
                                    <td class="style14">
                                        <asp:ListBox ID="lstPCMHElements" runat="server" CssClass="reports-elements" Style="margin-left: 33px;
                                            height: 100px; width: 120px" onChange="javascript:GetFactors();" ClientIDMode="Static">
                                        </asp:ListBox>
                                    </td>
                                    <td class="style16">
                                        <asp:ListBox ID="lstPCMHFacotrs" runat="server" CssClass="reports-elements" Style="margin-left: 32px;
                                            height: 100px; width: 110px" ClientIDMode="Static" onChange="javascript:GetSelectedFactors();"
                                            SelectionMode="Multiple"></asp:ListBox>
                                    </td>
                                    <td valign="top">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkFactor" runat="server" CssClass="reports-elements" Checked="true"
                                                        Text="Display selected Factors"></asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkElement" runat="server" Visible="false" CssClass="reports-elements"
                                                        Text="Display selected Elements"></asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkPCMHStandard" runat="server" Visible="false" CssClass="reports-elements"
                                                        Text="Display selected Standards"></asp:CheckBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblPractices" runat="server" CssClass="reports-label" Text="Select Practices"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td valign="top" class="style15">
                                        <asp:Label ID="lblConsultant" runat="server" CssClass="reports-elements" Style="margin-top: 0px;
                                            margin-left: 0px" Text="Consultant:"></asp:Label>
                                    </td>
                                    <td valign="top" class="style14">
                                        <asp:Label ID="lblComplete" runat="server" CssClass="reports-elements" Style="margin-top: 0px;
                                            margin-left: 0px" Text="% Complete:"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblPracticeSize" runat="server" CssClass="reports-elements" Style="margin-top: 0px;
                                            margin-left: 0px" Text="Practice Size:"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style15">
                                        <asp:ListBox ID="lstConsultant" runat="server" CssClass="reports-elements" Style="margin-left: 50px;
                                            height: 100px; width: 145px" ClientIDMode="Static" onChange="javascript:GetSelectedConsultant();"
                                            SelectionMode="Multiple"></asp:ListBox>
                                    </td>
                                    <td width="156px">
                                        <asp:ListBox ID="lstComplete" runat="server" CssClass="reports-elements" Style="margin-left: 33px;
                                            height: 100px; width: 120px" ClientIDMode="Static" onChange="javascript:GetSelectedComplete();"
                                            SelectionMode="Single">
                                            <asp:ListItem Value="0">All Percentages</asp:ListItem>
                                            <asp:ListItem Value="1">< 25%</asp:ListItem>
                                            <asp:ListItem Value="2">25% =< and < 50%</asp:ListItem>
                                            <asp:ListItem Value="3">50% =< and < 75%</asp:ListItem>
                                            <asp:ListItem Value="4">> = 75%</asp:ListItem>
                                            <asp:ListItem Value="5">Reviewed</asp:ListItem>
                                            <asp:ListItem Value="6">Submitted</asp:ListItem>
                                            <asp:ListItem Value="7">Recognized</asp:ListItem>
                                        </asp:ListBox>
                                    </td>
                                    <td>
                                        <asp:ListBox ID="lstPracticeSize" runat="server" CssClass="reports-elements" Style="margin-left: 32px;
                                            height: 100px; width: 120px" ClientIDMode="Static" onChange="javascript:GetSelectedPractice();"
                                            SelectionMode="Multiple"></asp:ListBox>
                                    </td>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkConsultant" runat="server" CssClass="reports-elements" Checked="true"
                                                        Text="Group by Consultant"></asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkComplete" runat="server" CssClass="reports-elements" Checked="true"
                                                        Text="Group by % Complete"></asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkPracticeSize" runat="server" CssClass="reports-elements" Checked="true"
                                                        Text="Group by practice size"></asp:CheckBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblOptions" runat="server" CssClass="reports-label" Text="Display Options"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td valign="top" class="style8">
                                        <asp:Label ID="lblData" runat="server" CssClass="reports-elements" Style="margin-top: 0px;
                                            margin-left: 0px" Text="Report data:"></asp:Label>
                                    </td>
                                    <td class="style10">
                                        <asp:CheckBox ID="chkPracticeSiteName" runat="server" CssClass="reports-elements"
                                            Checked="true" Style="margin-left: 0px" Text="Show practice & site name"></asp:CheckBox>
                                    </td>
                                    <td class="style9">
                                        <asp:CheckBox ID="chkPointsEarned" runat="server" CssClass="reports-elements" Style="margin-left: 5px"
                                            Checked="true" Text="Show points earned"></asp:CheckBox>
                                    </td>
                                    <td class="style11">
                                        <asp:CheckBox ID="chkDocumentsUploaded" runat="server" CssClass="reports-elements"
                                            Checked="true" Style="margin-left: 5px" Text="Show documents uploaded"></asp:CheckBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkMustPassStatus" runat="server" CssClass="reports-elements" Style="margin-left: 4px"
                                            Checked="true" Text="Show MUST PASS status"></asp:CheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style8">
                                    </td>
                                    <td class="style10">
                                        <asp:CheckBox ID="chkLastActivityDate" runat="server" Style="margin-left: 0px" CssClass="reports-elements"
                                            Checked="true" Text="Show last activity date"></asp:CheckBox>
                                    </td>
                                    <td class="style9">
                                    </td>
                                    <td class="style11">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style8">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style8">
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="style8">
                                        <asp:Label ID="lblGraphicDisplay" runat="server" CssClass="reports-elements" Style="margin-top: 0px;
                                            margin-left: 0px" Text="Graphic display:"></asp:Label>
                                    </td>
                                    <td class="style10">
                                        <asp:CheckBox ID="chkConvertElements" runat="server" CssClass="reports-elements"
                                            Checked="true" Style="margin-left: 0px" Text="Convert Elements to %"></asp:CheckBox>
                                    </td>
                                    <td class="style9">
                                        <asp:CheckBox ID="chkOverallbarGraph" runat="server" CssClass="reports-elements"
                                            ClientIDMode="Static" Style="margin-left: 5px" Text="Show overall bar graph">
                                        </asp:CheckBox>
                                    </td>
                                    <td class="style11">
                                        <asp:CheckBox ID="chkOverallGraph" runat="server" CssClass="reports-elements" Style="margin-left: 5px"
                                            Checked="true" Text="Show overall graph"></asp:CheckBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkGroupGraphs" runat="server" CssClass="reports-elements" Style="margin-left: 4px"
                                            ClientIDMode="Static" Text="Show group graphs"></asp:CheckBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnCustomReport" runat="server" Text="Save as Custom Report" Width="170px"
                                            OnClick="Save_CustomReport" ValidationGroup="runReport" CausesValidation="true" />
                                    </td>
                                    <td style="width: 20px">
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="btnRunReport" runat="server" Text="Run Report" OnClick="btnPrintReport_Click"
                                            UseSubmitBehavior="true" Width="170px" OnClientClick="javascript:BeginRequestHandler();"
                                            ClientIDMode="Static" ValidationGroup="runReport" CausesValidation="true" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="reportLightbox">
            </div>
             <div class="UpdateProgressContent" style="display: none;" id="LoadingDiv">
                Please Wait
                <br />
                <br />
                <img id="ImageLoading" src="~/Themes/Images/loading.gif" runat="server" alt="" />
            </div>
                        
        </asp:Panel>
        <asp:Label ID="lblContentTypeName" runat="server" Font-Bold="True" Font-Names="Arial"
            Font-Size="15pt"></asp:Label>
        <asp:Panel ID="pnlDynamicControl" runat="server">
        </asp:Panel>
        <input id="hdnSectionID" type="hidden" runat="server" clientidmode="Static" />
        <input id="hdnContentType" type="hidden" runat="server" clientidmode="Static" />
        <input id="hdnTreeNodeID" type="hidden" runat="server" clientidmode="Static" />
    </div>
</asp:Content>
