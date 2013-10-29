<%@ Page Language="C#" MasterPageFile="~/BMTMaster.master" AutoEventWireup="true"
    CodeBehind="Templates.aspx.cs" Inherits="BMT.Webforms.Templates" %>

<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>

<asp:Content ID="contentHead" ContentPlaceHolderID="head" runat="server">
    <link rel="Stylesheet" type="text/css" href="../Themes/style.css" />
    <link href="../Themes/jquery.contextMenu.css" rel="Stylesheet" type="text/css" />
    <link href="../Themes/popup.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="contentInnerMenuContainer" ContentPlaceHolderID="innerMenuConatiner"
    runat="server">
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jquery-ui-1.8.12.custom.min.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/Templates.js") %>"></script>
    <script src="../Scripts/jquery.contextMenu.js" type="text/javascript"></script>
    
    <div class="inner-menu-hover-container-left-combo" style="width: 600px">
        <table> 
            <tr>
                <td style="font-size: 14px; font-weight: bold; float: left; color: #FFFFFF; margin-right: 5px;">
                    <asp:Label ID="lblEnterprise" runat="server" Text="Enterprise:"></asp:Label>
                </td>
                <td style="font-size: 14px; font-weight: bold; float: left">
                    <asp:DropDownList ID="ddlEnterprise" runat="server" CssClass="body-hover-combo" Style="width: 180px;
                        z-index: 10000" AutoPostBack="true" OnTextChanged="ddlEnterprise_OnTextChange">
                    </asp:DropDownList>
                </td>
                <td style="font-size: 14px; font-weight: bold; float: left; color: #FFFFFF; margin-right: 5px;
                    margin-left: 20px;">
                    <asp:Label ID="lblTemplates" runat="server" Text="Template:"></asp:Label>
                </td>
                <td style="font-size: 14px; font-weight: bold; float: left">
                    <asp:DropDownList ID="ddlTemplates" runat="server" CssClass="body-hover-combo" Style="width: 180px;
                        z-index: 10000" AutoPostBack="true" OnSelectedIndexChanged="ddlTemplates_OnTextChange">
                        <asp:ListItem Value="1" Text="saf"></asp:ListItem>
                        <asp:ListItem Value="2" Text="ww4476ww"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
    <div class="inner-menu-hover-container-right-combo">
        <!--  Add User, Search text boxes, upload documents -->
        <div style="float: right; margin-left: 10px; width: 340px">
            <asp:Button ID="btnTemplate" runat="server" Text="New Template" CssClass="top-button-yollaw"
                ClientIDMode="Static" Style="width: 130px; margin-right: 10px" OnClientClick="return false;" />
            <asp:Button ID="btnUploadSheet" runat="server" Text="Import Sheet" CssClass="top-button-yollaw-disable"
                ClientIDMode="Static" Enabled="false" Style="width: 130px" OnClientClick="return false;" />
                <%--top-button-yollaw--%>
        </div>
    </div>
</asp:Content>
<asp:Content ID="contentBodyContainer" ContentPlaceHolderID="bodyContainer" runat="server">
   <asp:Panel ID="pnlTemplates" runat="server">
    
        <div style="width: 99%; margin: 10px auto 0px auto; padding-bottom: 20px;">   
        <ucdm:DisplayMessage ID="Message" runat="server" DisplayMessageWidth="985" ShowCloseButton="false"></ucdm:DisplayMessage>          
            <h1>
                Edit Templates
            </h1>
            <h2 style="color: #C10000" runat="server" id="templateName">                
            </h2>
            <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>

            <table width="100%">
                <tr>
                    <td width="33%" valign="top">
                        <asp:GridView ID="grdHeader" runat="server" AllowPaging="True" AllowSorting="True"
                            OnPageIndexChanging="grdHeader_PageIndexChanging"
                            ShowFooter="false" PageSize="15" AutoGenerateColumns="False" HeaderStyle-BackColor="#5880B3"
                            HeaderStyle-ForeColor="#FFFFFF" BorderColor="White" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-CssClass="grid-header" CssClass="submission-grid" OnRowEditing="grdHeader_RowEditing"
                            OnRowCancelingEdit="grdHeader_RowCancelingEdit" OnRowUpdating="grdHeader_RowUpdating"
                            OnRowDeleting="grdHeader_RowDeleting" OnRowCommand="grdHeader_RowSelecting" DataKeyNames="KnowledgeBaseId"
                            SelectedRowStyle-BackColor="LightYellow">
                            <EmptyDataRowStyle BackColor="LightBlue" />
                            <EmptyDataTemplate>
                                <table width="100%" style="background-color: #5880B3; color: #FFFFFF;">
                                    <tr>
                                        <td align="center">
                                            No Record Found.
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <AlternatingRowStyle BackColor="#F2F2F2" />
                            <HeaderStyle BackColor="#5880B3" CssClass="grid-header" ForeColor="White" HorizontalAlign="Center" />
                            <PagerSettings FirstPageText="First Page" LastPageText="Last Page" Mode="NumericFirstLast"
                                Position="Bottom" />
                            <PagerStyle BackColor="#5880B3" ForeColor="White" BorderColor="Transparent" BorderWidth="0px"
                                BorderStyle="None" Font-Bold="true" />
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="grid-border"
                                    HeaderStyle-Width="80%">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblheaders" runat="server" Text='Headers' Font-Bold="True" Font-Size="10pt"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkHeader" runat="server" CommandArgument='<%# Eval("KnowledgeBaseId") %>'
                                            CommandName="Select" Text='<%# Eval("Name") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEditHeader" runat="server" MaxLength="250" Width="93%" Text='<%# Bind("Name") %>' onkeypress="if(event.keyCode==13) return false;"/>
                                        <asp:RequiredFieldValidator ID="EditHeaderRFV" runat="server" Text="*" ControlToValidate="txtEditHeader"
                                            ValidationGroup="btnUpdate" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtAddHeader" runat="Server" MaxLength="250" Width="93%" onkeypress="if(event.keyCode==13) return false;"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="AddHeaderRFV" runat="server" Text="*" ControlToValidate="txtAddHeader"
                                            ValidationGroup="btnInsert" />
                                    </FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Left" BorderColor="#5880B3" CssClass="grid-border" />
                                    <ItemStyle BorderColor="#CCCCCC" HorizontalAlign="Left" CssClass="grid-border" />
                                    <FooterStyle BorderColor="#CCCCCC" HorizontalAlign="Left" CssClass="grid-border" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-border"
                                    HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Themes/Images/Edit-16.png"
                                            ToolTip="Edit" CommandName="Edit" Text="Edit" OnClientClick="javascript:return onEditCofirmation();"/>
                                        <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/Themes/Images/Delete.png"
                                            ToolTip="Delete" CommandName="Delete" Text="Delete" OnClientClick="javascript:return onDeleteCofirmation();"/>
                                        <asp:ImageButton ID="btnNext" runat="server" ImageUrl="~/Themes/Images/arrownext.png"
                                            ToolTip="Sub-Headers" CommandArgument='<%# Eval("KnowledgeBaseId") %>' CommandName="Select"
                                            Text="Nexts" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="btnUpdate" runat="server" Text="Update" CommandName="Update" ToolTip="Update" 
                                            ImageUrl="~/Themes/Images/save-16.png" ValidationGroup="btnUpdate" />
                                        <asp:ImageButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" ToolTip="Cancel"
                                            ImageUrl="~/Themes/Images/cancel-cross.png" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="btnInsert" runat="server" Text="Save" CommandName="Insert" ToolTip="Save" ImageUrl="~/Themes/Images/save-16.png"
                                            ValidationGroup="btnInsert" />
                                        <asp:ImageButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" ToolTip="Cancel"
                                            ImageUrl="~/Themes/Images/cancel-cross.png" />
                                    </FooterTemplate>
                                    <HeaderStyle Width="30px" BorderColor="#5880B3" CssClass="grid-border" />
                                    <ItemStyle BorderColor="#CCCCCC" HorizontalAlign="Center" CssClass="grid-border" />
                                    <FooterStyle BorderColor="#CCCCCC" HorizontalAlign="Center" CssClass="grid-border" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                    <td width="10px">
                    </td>
                    <td width="33%" valign="top">
                    <%----%>
                        <asp:GridView ID="grdSubHeader" runat="server" AllowPaging="True" AllowSorting="True"
                            OnPageIndexChanging="grdSubHeader_PageIndexChanging"
                            ShowFooter="false" PageSize="15" AutoGenerateColumns="False" HeaderStyle-BackColor="#5880B3"
                            HeaderStyle-ForeColor="#FFFFFF" BorderColor="White" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-CssClass="grid-header" CssClass="submission-grid" OnRowEditing="grdSubHeader_RowEditing"
                            OnRowCancelingEdit="grdSubHeader_RowCancelingEdit" OnRowUpdating="grdSubHeader_RowUpdating"
                            OnRowDeleting="grdSubHeader_RowDeleting" OnRowCommand="grdSubHeader_RowSelecting"
                            DataKeyNames="KnowledgeBaseId" SelectedRowStyle-BackColor="LightYellow">
                            <EmptyDataRowStyle BackColor="LightBlue" />
                            <EmptyDataTemplate>
                                <table width="100%" style="background-color: #5880B3; color: #FFFFFF;">
                                    <tr>
                                        <td align="center">
                                            No Record Found.
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <AlternatingRowStyle BackColor="#F2F2F2" />
                            <HeaderStyle BackColor="#5880B3" CssClass="grid-header" ForeColor="White" HorizontalAlign="Center" />
                            <PagerSettings FirstPageText="First Page" LastPageText="Last Page" Mode="NumericFirstLast"
                                Position="Bottom" />
                            <PagerStyle BackColor="#5880B3" ForeColor="White" BorderColor="Transparent" BorderWidth="0px"
                                BorderStyle="None" Font-Bold="true" />
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="grid-border"
                                    HeaderStyle-Width="80%">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblheaders" runat="server" Text='Sub-Headers' Font-Bold="True" Font-Size="10pt"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSubHeader" runat="server" CommandArgument='<%# Eval("KnowledgeBaseId") %>'
                                            CommandName="Select" Text='<%# Eval("Name") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEditSubHeader" runat="server" MaxLength="250" Width="93%" Text='<%# Bind("Name") %>' onkeypress="if(event.keyCode==13) return false;" />
                                        <asp:RequiredFieldValidator ID="AddSubHeaderRFV" runat="server" Text="*" ControlToValidate="txtEditSubHeader"
                                            ValidationGroup="btnUpdate" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtAddSubHeader" runat="Server" MaxLength="250" Width="93%" onkeypress="if(event.keyCode==13) return false;"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="AddSubHeaderRFV" runat="server" Text="*" ControlToValidate="txtAddSubHeader"
                                            ValidationGroup="btnInsert" />
                                    </FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Left" BorderColor="#5880B3" CssClass="grid-border"/>
                                    <ItemStyle BorderColor="#CCCCCC" HorizontalAlign="Left" CssClass="grid-border" />
                                    <FooterStyle BorderColor="#CCCCCC" HorizontalAlign="Left" CssClass="grid-border" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-border"
                                    HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Themes/Images/Edit-16.png"
                                            ToolTip="Edit" CommandName="Edit" Text="Edit"  OnClientClick="javascript:return onEditCofirmation();"/>
                                        <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/Themes/Images/Delete.png"
                                            ToolTip="Delete" CommandName="Delete" Text="Delete" OnClientClick="javascript:return onDeleteCofirmation();"/>
                                        <asp:ImageButton ID="btnNext" runat="server" ImageUrl="~/Themes/Images/arrownext.png"
                                            ToolTip="Questions" CommandArgument='<%# Eval("KnowledgeBaseId") %>' CommandName="Select"
                                            Text="Nexts" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="btnUpdate" runat="server" Text="Update" CommandName="Update" ToolTip="Update"
                                            ImageUrl="~/Themes/Images/save-16.png" ValidationGroup="btnUpdate" />
                                        <asp:ImageButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" ToolTip="Cancel"
                                            ImageUrl="~/Themes/Images/cancel-cross.png" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="btnInsert" runat="server" Text="Save" CommandName="Insert" ToolTip="Save" ImageUrl="~/Themes/Images/save-16.png"
                                            ValidationGroup="btnInsert" />
                                        <asp:ImageButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" ToolTip="Cancel"
                                            ImageUrl="~/Themes/Images/cancel-cross.png" />
                                    </FooterTemplate>
                                    <HeaderStyle Width="30px" BorderColor="#5880B3" CssClass="grid-border" />
                                    <ItemStyle BorderColor="#CCCCCC" HorizontalAlign="Center" CssClass="grid-border" />
                                    <FooterStyle BorderColor="#CCCCCC" HorizontalAlign="Center" CssClass="grid-border" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                    <td width="10px">
                    </td>
                    <td width="33%" valign="top">
                        <asp:GridView ID="grdQuestion" runat="server" AllowPaging="True" AllowSorting="True"
                            OnPageIndexChanging="grdQuestion_PageIndexChanging"
                            PageSize="15" AutoGenerateColumns="False" HeaderStyle-BackColor="#5880B3" HeaderStyle-ForeColor="#FFFFFF"
                            BorderColor="White" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="grid-header"
                            CssClass="submission-grid" OnRowEditing="grdQuestion_RowEditing" OnRowCancelingEdit="grdQuestion_RowCancelingEdit"
                            OnRowUpdating="grdQuestion_RowUpdating" OnRowDeleting="grdQuestion_RowDeleting"
                            OnRowCommand="grdQuestion_RowSelecting" DataKeyNames="KnowledgeBaseId"
                            SelectedRowStyle-BackColor="LightYellow">
                            <EmptyDataRowStyle BackColor="LightBlue" />
                            <EmptyDataTemplate>
                                <table width="100%" style="background-color: #5880B3; color: #FFFFFF;">
                                    <tr>
                                        <td align="center">
                                            No Record Found.
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <AlternatingRowStyle BackColor="#F2F2F2" />
                            <HeaderStyle BackColor="#5880B3" CssClass="grid-header" ForeColor="White" HorizontalAlign="Center" />
                            <PagerSettings FirstPageText="First Page" LastPageText="Last Page" Mode="NumericFirstLast"
                                Position="Bottom" />
                            <PagerStyle BackColor="#5880B3" ForeColor="White" BorderColor="Transparent" BorderWidth="0px"
                                BorderStyle="None" Font-Bold="true" />
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="grid-border"
                                    HeaderStyle-Width="85%">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblheaders" runat="server" Text='Questions' Font-Bold="True" Font-Size="10pt"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lblTestName" runat="server" Text='<%# Eval("Questions") %>'></asp:Label>--%>
                                        <asp:LinkButton ID="lnkQuestion" runat="server" Text='<%# Eval("Name") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEditQuestion" runat="server" MaxLength="250" Width="93%" Text='<%# Bind("Name") %>' onkeypress="if(event.keyCode==13) return false;"/>
                                        <asp:RequiredFieldValidator ID="EditQuestionRFV" runat="server" Text="*" ControlToValidate="txtEditQuestion"
                                            ValidationGroup="btnUpdate" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtAddQuestion" runat="Server" MaxLength="250" Width="93%" onkeypress="if(event.keyCode==13) return false;"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="AddQuestionRFV" runat="server" Text="*" ControlToValidate="txtAddQuestion"
                                            ValidationGroup="btnInsert" />
                                    </FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Left" BorderColor="#5880B3" CssClass="grid-border" />
                                    <ItemStyle BorderColor="#CCCCCC" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-border"
                                    HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Themes/Images/Edit-16.png"
                                            ToolTip="Edit" CommandName="Edit" Text="Edit"  OnClientClick="javascript:return onEditCofirmation();"/>
                                        <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/Themes/Images/Delete.png"
                                            ToolTip="Delete" CommandName="Delete" Text="Delete" OnClientClick="javascript:return onDeleteCofirmation();"/>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="btnUpdate" runat="server" Text="Update" CommandName="Update" ToolTip="Update"
                                            ValidationGroup="btnUpdate" ImageUrl="~/Themes/Images/save-16.png" />
                                        <asp:ImageButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" ToolTip="Cancel"
                                            ImageUrl="~/Themes/Images/cancel-cross.png" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="btnInsert" runat="server" Text="Save" CommandName="Insert" ToolTip="Save" ImageUrl="~/Themes/Images/save-16.png"
                                            ValidationGroup="btnInsert" />
                                        <asp:ImageButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" ToolTip="Cancel"
                                            ImageUrl="~/Themes/Images/cancel-cross.png" />
                                    </FooterTemplate>
                                    <HeaderStyle Width="30px" BorderColor="#5880B3" CssClass="grid-border" />
                                    <ItemStyle BorderColor="#CCCCCC" HorizontalAlign="Center" CssClass="grid-border" />
                                    <FooterStyle BorderColor="#CCCCCC" HorizontalAlign="Center" CssClass="grid-border" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnAddHeader" runat="server" OnClick="grdHeader_NewRow" Text="Add New Header"
                            Visible="false" />
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnAddSubHeader" runat="server" OnClick="grdSubHeader_NewRow" Text="Add New Sub-Header"
                            Visible="false" />
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnAddQuestion" runat="server" OnClick="grdQuestion_NewRow" Text="Add New Question"
                            Visible="false" />
                    </td>
                </tr>
            </table>

            <%--</ContentTemplate>
            </asp:UpdatePanel>--%>
        </div>
    </asp:Panel>
    <div id="lightbox-popupTemplate" class="edit-popup" style="border: 1px solid #5880B3;">
        <div id="popupHeaderText">
            <table width="100%">
                <tr>
                    <td width="60%" align="right" valign="middle">
                        File Import
                    </td>
                    <td width="45%" align="right" valign="middle">
                        <a id="close-editpopup" href="#" style="text-decoration: none; font-size: 16px; color: White;">
                            close[x]</a>
                    </td>
                </tr>
            </table>
        </div>      
       <%-- <div id="MessagePopup" visible="false" class="error">
        <table width="100%">
        <tr>
            <td id="MessagePopupTD"  colspan="4" visible="false">                                    
                                    
            </td>
        </tr>        
        </table>
        </div>--%>
        <table width="100%">    
        <tr>
        <td>
        <ucdm:DisplayMessage ID="DisplayMessage1" runat="server" DisplayMessageWidth="330" ShowCloseButton="false"></ucdm:DisplayMessage>
        </td>
        </tr>                
            <tr>
                <td align="center">
                    File Name:
                    <asp:FileUpload runat="server" ID="uploadFile" Width="240" ClientIDMode="Static" />                    
                </td>
            </tr>
            <tr>
                <td class="normal" align="center">
                    <asp:ImageButton ID="imgUploadDoc" runat="server" ImageUrl="~/Themes/Images/import.png"
                        ToolTip="Import" OnClick="btnUpload_Sheet" OnClientClick = "return ValidateFile();" />
                </td> 
            </tr>
        </table>
    </div>
    <!-- /lightbox-panel -->
    <div id="lightbox">
    </div>
    <div id="lightbox-popupTemplate" class="uploadbox-popup" style="border: 1px solid #5880B3;">
        <div id="popupHeaderText">
            <table width="100%">
                <tr>
                    <td width="65%" align="right" valign="middle">
                        New Template
                    </td>
                    <td width="35%" align="right" valign="middle">
                        <a id="close-popup" href="#" style="text-decoration: none; font-size: 16px; color: White;">
                            close[x]</a>
                    </td>
                </tr>
            </table>
        </div>
        <table width="100%">
            <tr>
                <td>
                    <%--<asp:Panel ID="pnlIFrameImporter" runat="server">--%>
                        <table width="400">
                            <tr>
                         <td colspan="2">
                          <ucdm:DisplayMessage ID="DisplayMessage2" runat="server" DisplayMessageWidth="330" ShowCloseButton="false"></ucdm:DisplayMessage>
                         </td>
                          </tr> 
                            <tr>
                                <td class="normalTemplatesPagePopup" style="width: 40px; padding-left:25px;">
                                    <p>
                                        Template Name: *</p>
                                </td>
                                <td class="normalTemplatesPagePopup">
                                    <asp:TextBox ID="txtTemplateName" runat="server" MaxLength="50" CssClass="text-field02" ClientIDMode="Static"/>
                                    <%--<asp:RequiredFieldValidator ID="txtTemplateNameRFV" runat="server" Text="*" ControlToValidate="txtTemplateName"
                                        ValidationGroup="btnSaveTemplate" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="normalTemplatesPagePopup" style="width: 40px; padding-left:25px;">
                                    <p>
                                        Short Name: *</p>
                                </td>
                                <td class="normalTemplatesPagePopup">
                                    <asp:TextBox ID="txtShortName" runat="server" MaxLength="30" CssClass="text-field02" ClientIDMode="Static" />
                                    <%--<asp:RequiredFieldValidator ID="txtShortNameRFV" runat="server" Text="*" ControlToValidate="txtShortName"
                                        ValidationGroup="btnSaveTemplate" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="normalTemplatesPagePopup" style="width: 40px; padding-left:25px;">
                                    <p>
                                        Description:</p>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDescription" runat="server" MaxLength="250" CssClass="text-field02" ClientIDMode="Static">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td id="templateAccessTd" class="normalTemplatesPagePopup" style="width: 40px; padding-left:25px;" visible="false">
                                    <p>
                                        Template Access:</p>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddTemplateAccess" runat="server" ClientIDMode="Static" Width="145px" Enabled="false">
                                        <asp:ListItem Value="1" Text="Public" ></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Enterprise" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                               <td align="right">
                                <asp:Button ID="saveTemplate" runat="server" Text="Save" AutoPostBack="True" OnClick="btnSave_Template"
                                        OnClientClick = "return ValidateTemplate();" Width="70px" />
                               </td>
                               
                                <td class="normalTemplatesPagePopup" align="left">
                                   
                                        <asp:Button ID="closepopup" runat="server" Text="Cancel" Width="70px" OnClientClick="javascript:cancelpopup();" />
                                </td>                               
                                
                            </tr>
                        </table>
                   <%-- </asp:Panel>--%>
                </td>
            </tr>
        </table>
    </div>
    <!-- /lightbox-panel -->
    <div id="lightbox">
    </div>
        <div class="body-container-right">
      
        <input id="hdnSectionID" type="hidden" runat="server" clientidmode="Static" />
        <input id="hdnContentType" type="hidden" runat="server" clientidmode="Static" />
    </div>
</asp:Content>
