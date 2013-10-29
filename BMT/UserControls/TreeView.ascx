<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TreeView.ascx.cs" Inherits="BMT.UserControls.ParentChildTreeView" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/querystring-0.9.0.js") %>"></script>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/tree.js") %>"></script>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/apprise-1.5.min.js") %>"></script>
<script src="../Scripts/jquery.contextMenu.js" type="text/javascript"></script>
<link href="../Themes/jquery.contextMenu.css" rel="Stylesheet" type="text/css" />
<link href="../Themes/popup.css" rel="Stylesheet" type="text/css" />
<asp:HiddenField ID="hiddenActiveNode" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnActiveURl" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnActiveId" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnScreen" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnLastInsertedId" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnElementQuantity" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnActiveClass" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnTreeNodePath" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnEnterpriseName" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnEnterpriseId" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnRestrictedFolderList" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnJumpMaterialList" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnIsJumpMaterialFolder" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnDeletedNodeID" runat="server" ClientIDMode="Static" />
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="body-container-left-tree" id="treeContainer" style="vertical-align: top;">
            <div id="divRootFolder" style="float: left;" runat="server" visible="false">
                <asp:Image ID="imgAddRootFolder" runat="server" ImageUrl="~/Themes/Images/AddRootFolder.png"
                    ClientIDMode="Static" ToolTip="Add Top Level Folder" AlternateText="Add Top Level Folder" />
            </div>
            <asp:TreeView ID="treeView" runat="server" ExpandDepth="0" NodeStyle-HorizontalPadding="2"
                ExpandImageUrl="~/Themes/Images/Plus.png" CollapseImageUrl="~/Themes/Images/Minus.png"
                Font-Names="Calibri" ForeColor="Black" NodeIndent="15" CssClass="treeView">
                <LeafNodeStyle Font-Size="Small" HorizontalPadding="0px" VerticalPadding="0px" />
                <NodeStyle HorizontalPadding="2px" />
                <ParentNodeStyle Font-Bold="True" Font-Size="Small" HorizontalPadding="2px" VerticalPadding="2px" />
                <RootNodeStyle Font-Bold="True" Font-Size="Medium" />
            </asp:TreeView>
        </div>
        <div id="contextMenuContainer" runat="server">
        </div>
        <div id="lightbox" class="lightbox" style="height: 4000px;">
        </div>
        <div id="lightbox-add" class="lightbox-popup" style="border: 1px solid #5880B3;">
            <div class="popupHeaderText">
                <table width="100%">
                    <tr>
                        <td class="trow">
                        </td>
                        <td width="50%" align="center" valign="middle">
                            <span style="margin-right: -5.4em;">Add Folder </span>
                        </td>
                        <td width="20%" align="right" valign="middle">
                            <a class="close">close[x]</a>
                        </td>
                    </tr>
                </table>
            </div>
            <table width="100%">
                <tr>
                    <td>
                        <div id="addMessage" style="display: none; height: 2em">
                            <span id="Span2" style="margin: 0 0 0 20px;">Please enter a valid folder name.</span>
                        </div>
                    </td>
                </tr>
                <tr id="msgRow">
                    <td>
                        <ucdm:DisplayMessage ID="message" runat="server" DisplayMessageWidth="380" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td width="80%">
                        <span style="margin: 0 0 0 5em">Name*: </span>
                        <input style="width: 15em; background: none repeat scroll 0 0 #EAEFF5; border: 1px solid"
                            id='txtadd' maxlength="50" class="txtFlolderName" type="text" />
                    </td>
                </tr>
                <tr>
                    <td class="popup-info1">
                        <div style="margin: 0 0 0 11em">
                            <input type="button" name="add" value="Add" class="bttadd" />
                            <input type="button" value="Cancel" name="cancel" class="bttcancel" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="lightbox-rename" class="lightbox-popup" style="border: 1px solid #5880B3;">
            <div class="popupHeaderText">
                <table width="100%">
                    <tr>
                        <td class="trow">
                        </td>
                        <td width="50%" align="center" valign="middle">
                            <span style="margin-right: -5.4em;">Rename Folder </span>
                        </td>
                        <td width="20%" align="right" valign="middle">
                            <a class="close">close[x]</a>
                        </td>
                    </tr>
                </table>
            </div>
            <table width="100%">
                <tr>
                    <td>
                        <div id="renameMessage" style="display: none; height: 2em">
                            <span id="txtrename" style="margin: 0 0 0 3px">Please enter a valid folder name.</span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td width="80%">
                        <span style="margin: 0 0 0 5em">Name*: </span>
                        <input style="width: 15em; background: none repeat scroll 0 0 #EAEFF5; border: 1px solid"
                            id='txtsave' maxlength="50" class="txtFlolderName" type="text" />
                    </td>
                </tr>
                <tr>
                    <td class="popup-info1">
                        <div style="margin: 0 0 0 11em">
                            <input type="button" name="save" value="Save" id="rnbttsave" class="bttadd" />
                            <input type="button" value="Cancel" name="cancel" id="rnbttcancel" class="bttcancel" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
