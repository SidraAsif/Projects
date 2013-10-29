<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MOReUploadedDocs.aspx.cs"
    Inherits="BMT.Webforms.MOReUploadedDocs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Document Viewer</title>
    <link type="text/css" rel="Stylesheet" href="../Themes/style.css" />
    <link rel="Stylesheet" type="text/css" href="../Themes/popup.css" />
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jQuery-1.7.1.min.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/MOReUploadedDocs.js") %>"></script>
</head>
<body>
    <form id="frmNCQADoc" runat="server">
    <asp:DataGrid ID="datagridDocViewer" runat="server" AllowPaging="true" AllowSorting="true"
        PageSize="8" AutoGenerateColumns="false" HeaderStyle-BackColor="#5880B3" HeaderStyle-ForeColor="#FFFFFF"
        BorderColor="#CCCCCC" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="grid-header"
        CssClass="grid" ClientIDMode="Static" OnSortCommand="datagridDocViewer_SortCommand">
        <AlternatingItemStyle CssClass="gridAlternateRow" />
        <PagerStyle BackColor="#5880B3" ForeColor="White" BorderColor="#CCCCCC" BorderWidth="0px"
            BorderStyle="None" Font-Bold="true" Mode="NumericPages" HorizontalAlign="Left" />
        <Columns>
            <asp:BoundColumn DataField="FactorSequence" HeaderText="Factor" ItemStyle-HorizontalAlign="Left"
                HeaderStyle-Width="20" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" ReadOnly="true" SortExpression="FactorSequence" />
            <asp:BoundColumn DataField="Type" HeaderText="Type" ItemStyle-HorizontalAlign="Left"
                HeaderStyle-Width="150" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" ReadOnly="true" SortExpression="Type" />
            <asp:BoundColumn DataField="Title" HeaderText="Title" ItemStyle-HorizontalAlign="Left"
                HeaderStyle-Width="150" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" ReadOnly="true" SortExpression="Title" />
            <asp:BoundColumn DataField="DocLinkedTo" HeaderText="Linked to..." ItemStyle-HorizontalAlign="Left"
                HeaderStyle-Width="50" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" ReadOnly="true" SortExpression="DocLinkedTo" />
                 <asp:BoundColumn DataField="LastUpdatedDate" HeaderText="Last Uploaded" ItemStyle-HorizontalAlign="Left"
                HeaderStyle-Width="50" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" ReadOnly="true" DataFormatString="{0:MM/dd/yyyy}" SortExpression="LastUpdatedDate" />
            <asp:TemplateColumn HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30"
                ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC">
                <ItemTemplate>
                    <a onclick="javascript:FileMove('<%# DataBinder.Eval(Container.DataItem,"PCMHSequence") %>', '<%# DataBinder.Eval(Container.DataItem,"ElementSequence") %>', '<%# DataBinder.Eval(Container.DataItem,"FactorSequence") %>', '<%# DataBinder.Eval(Container.DataItem,"File") %>', '<%# DataBinder.Eval(Container.DataItem,"DocName") %>', '<%# DataBinder.Eval(Container.DataItem,"ReferencePage") %>', '<%# DataBinder.Eval(Container.DataItem,"RelevancyLevel") %>', '<%# DataBinder.Eval(Container.DataItem,"DocType") %>','<%# DataBinder.Eval(Container.DataItem,"FactorTitle") %>','<%# DataBinder.Eval(Container.DataItem,"ProjectUsageId") %>');">
                        <asp:Image ID="imgEditProperties" runat="server" ImageUrl="~/Themes/Images/edit-16.png"
                            AlternateText="Edit" ToolTip="Edit" />
                    </a>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Replace" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30"
                ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC">
                <ItemTemplate>
                    <a onclick="javascript:ProcessReplaceFile('<%# DataBinder.Eval(Container.DataItem,"DocName") %>', '<%# DataBinder.Eval(Container.DataItem,"ElementSequence") %>', '<%# DataBinder.Eval(Container.DataItem,"FactorSequence") %>', '<%# DataBinder.Eval(Container.DataItem,"PCMHSequence") %>', '<%# DataBinder.Eval(Container.DataItem,"ProjectUsageId") %>', 'NCQA Submission', '<%# DataBinder.Eval(Container.DataItem,"PracticeId") %>', '<%# DataBinder.Eval(Container.DataItem,"SiteId") %>', '<%# DataBinder.Eval(Container.DataItem,"DocName") %>', '<%# DataBinder.Eval(Container.DataItem,"ReferencePage") %>', '<%# DataBinder.Eval(Container.DataItem,"RelevancyLevel") %>', '<%# DataBinder.Eval(Container.DataItem,"File") %>', '<%# DataBinder.Eval(Container.DataItem,"DocLinkedTo") %>','<%# DataBinder.Eval(Container.DataItem,"DocType") %>');">
                        
                        <asp:Image ID="imgReplaceDoc" runat="server" ImageUrl="~/Themes/Images/replace.png"
                            AlternateText="Replace" ToolTip="Replace" />
                    </a>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Remove" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30"
                ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC">
                <ItemTemplate>
                    <a onclick="javascript:ProcessDeleteFile('MOReDeleteFiles.aspx?pcmh=<%# DataBinder.Eval(Container.DataItem,"PCMHSequence") %>&element=<%# DataBinder.Eval(Container.DataItem,"ElementSequence") %>&factor=<%# DataBinder.Eval(Container.DataItem,"FactorSequence") %>&file=<%#  DataBinder.Eval(Container.DataItem,"File") + "|" + DataBinder.Eval(Container.DataItem,"DocName") + "|" %>&project=<%# DataBinder.Eval(Container.DataItem,"ProjectUsageId")%>&practiceId=<%# DataBinder.Eval(Container.DataItem,"PracticeId") %>&siteId=<%# DataBinder.Eval(Container.DataItem,"SiteId") %>&pageNo=2','<%# DataBinder.Eval(Container.DataItem,"PCMHSequence") %>','<%# DataBinder.Eval(Container.DataItem,"ElementSequence") %>','<%# DataBinder.Eval(Container.DataItem,"FactorSequence") %>','<%# DataBinder.Eval(Container.DataItem,"DocType") %>','<%# DataBinder.Eval(Container.DataItem,"FactorTitle") %>','<%# DataBinder.Eval(Container.DataItem,"DocLinkedTo") %>');">
                        <asp:Image ID="imgDeleteDoc" runat="server" ImageUrl="~/Themes/Images/remove-16.png"
                            AlternateText="Remove" ToolTip="Remove" />
                    </a>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    <asp:Panel ID="pnlRecordWarning" runat="server" Visible="false">
        <div style="background-color: #5880B3; color: #FFFFFF; text-align: center; width: 100%;">
            No Record found</div>
    </asp:Panel>
    <!-- Open File Delete page in iframe when needed -->
    <div id="lightbox-popup-delepro" class="lightbox-popup" style="border: 0px none;
        background-color: transparent;">
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
    <div id="lightbox-popup" style="border: 1px solid #5880B3;">
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
                    <asp:TextBox ID="txtReferencePage" runat="server" CssClass="text-field02" ClientIDMode="Static" MaxLength="50">
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
    <!-- Delete File Properties page -->
    <div id="lightbox-popup-delete" class="lightbox-popup" style="border: 1px solid #5880B3;
        width: 450px">
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
    <div id="lightbox-popup" class="replace-popup" style="border: 1px solid #5880B3; top:10%;">
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
    <asp:HiddenField ID="hdnProjectUsageId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnSiteId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnCurrentDocType" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnPageUrl" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnDocLinkedTo" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnPracticeName" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnSiteName" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnTemplateId" runat="server" ClientIDMode="Static" />
    </form>
</body>
</html>
