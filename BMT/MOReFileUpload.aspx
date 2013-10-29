<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MOReFileUpload.aspx.cs" Inherits="BMT.Webforms.MOReFileUpload"
    ValidateRequest="false" EnableEventValidation="false" %>

<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Upload File</title>
    <link rel="Stylesheet" type="text/css" href="Themes/style.css" />
    <link rel="Stylesheet" type="text/css" href="Themes/fileUpload.css" />
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jQuery-1.7.1.min.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/ajaxfileupload.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/MOReFileUpload.js") %>"></script>
</head>
<body>
    <form id="frmFileUploader" runat="server" action="" method="post" enctype="multipart/form-data">
    <asp:HiddenField ID="hiddenPracticeId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenElementId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenFactorId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenPCMHId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenPracticeName" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenSiteName" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenNode" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenProjectUsageId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenSiteId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenTemplateId" runat="server" ClientIDMode="Static" />
    <table width="400">
        <tr>
            <td colspan="3">
                <ucdm:DisplayMessage ID="message" runat="server" DisplayMessageWidth="390" />
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlUploadNewFile" ClientIDMode="Static" runat="server">
        <table width="400">
            <tr>
                <td class="normal" style="width:40px;">
                    <p>
                        Document Name:</p>
                </td>
                <td class="normal">
                    <asp:TextBox ID="txtDocName" runat="server" CssClass="text-field02" ClientIDMode="Static"
                        MaxLength="100"> </asp:TextBox>
                    <div class="user-notice" style="width:280px;">
                        (If left blank name on file system will be used)</div>
                </td>
            </tr>
            <tr>
                <td class="normal" style="width:40px;">
                    <p>
                        Reference Pages:</p>
                </td>
                <td class="normal">
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
                <td class="normal">
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
                <td class="normal">
                    <asp:DropDownList ID="ddldocType" runat="server" Width="140px" ClientIDMode="Static">
                        <asp:ListItem Text="PoliciesOrProcess" Value="1"></asp:ListItem>
                        <asp:ListItem Text="ReportsOrLogs" Value="2"></asp:ListItem>
                        <asp:ListItem Text="ScreenshotsOrExamples" Value="3"></asp:ListItem>
                        <asp:ListItem Text="RRWB" Value="4"></asp:ListItem>
                        <asp:ListItem Text="Extra" Value="5"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnpNCQALink" runat="server" ClientIDMode="Static" style="display:none;">
        <table width="400">
            <tr>                
                <td align="center">
                    <asp:RadioButtonList ID="rbDocSelecttion" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal"
                        CausesValidation="false">
                        <asp:ListItem Value="1" Text="New"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Existing"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlExistingDoc" runat="server" ClientIDMode="Static" style="display:none;">
        <table width="400">
            <tr>
                <td class="normal" style="width:40px;">
                    <p>
                       Standards:*</p>
                </td>
                <td class="normal" style="width:280px;">
                    <asp:DropDownList ID="ddlStandards" runat="server" Width="140px" ClientIDMode="Static">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="normal">
                    <p>
                        Elements:*</p>
                </td>
                <td class="normal">
                    <asp:DropDownList ID="ddlElements" runat="server" Width="140px" ClientIDMode="Static">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="normal">
                    <p>
                        Factors:*</p>
                </td>
                <td class="normal">
                    <asp:DropDownList ID="ddlFactors" runat="server" Width="140px" ClientIDMode="Static">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <table width="400">
        <tr>
            <td class="normal"  style="width:40px;">
                <%--<p id="parDocumentName" runat="server">--%>
                <asp:Label ID="lblDocumentName" runat="server" ClientIDMode="Static" Text="Document:"></asp:Label>
            </td>
            <td colspan="2" class="normal" style="width:280px;">
                <asp:FileUpload ID="fudoc" runat="server" ClientIDMode="Static"/>
                <asp:ListBox ID="lbDocs" runat="server" ClientIDMode="Static" Width="240px" Height="36px"
                    SelectionMode="Single" style="display:none;"></asp:ListBox>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <asp:Image ID="imgUploadDoc" runat="server" ClientIDMode="Static" onClick="callUploader();"
                    ImageUrl="~/Themes/Images/uploadimg.png" ToolTip="Upload"/>
                <asp:Image ID="imgLinkDoc" runat="server" ClientIDMode="Static" onClick="javascript:CreateDocLink();"
                    ImageUrl="~/Themes/Images/doc-link.png" ToolTip="Create link" style="display:none;" />
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center" style="display: none;">
                <asp:Image ID="imgOkCancel" runat="server" ClientIDMode="Static" ImageUrl="~/Themes/Images/ok.png"
                    ToolTip="Ok" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
