<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateEditTemplate.ascx.cs"
    Inherits="CreateEditTemplate" ViewStateMode="Enabled" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<%@ Register Src="~/UserControls/LoadingPanel.ascx" TagName="LoadingPanel" TagPrefix="ucl" %>
<link rel="Stylesheet" type="text/css" href="../Themes/jquery-ui-1.8.12.custom.css" />
<link rel="Stylesheet" type="text/css" href="../Themes/popup.css" />
<script language="javascript" type="text/javascript">
    function TemplateTypeChanged(rdo) {
        if (rdo.id == '<%= blankTemplate.ClientID %>') {
            document.getElementById('<%= txtCopyFromExisting.ClientID %>').disabled = true;
            document.getElementById('<%= TemplateSearch.ClientID %>').disabled = true;
        }
        else if (rdo.id == '<%= copyFromExisting.ClientID %>') {
            document.getElementById('<%= txtCopyFromExisting.ClientID %>').disabled = false;
            document.getElementById('<%= TemplateSearch.ClientID %>').disabled = false;
        }
    }

    function ClearSearch(elementid) {
        document.getElementById(elementid).value = '';
        document.getElementById(elementid).style.color = "black";
    }
    function ShowDocumentStore() {

        if ($('#<%= rbDocStoreYes.ClientID %>').is(':checked')) {
            $(".folderDocument").show();
        } else if ($('#<%= rbDocStoreNo.ClientID %>').is(':checked')) {
            $(".folderDocument").hide();
        }


        var chkboxYes = document.getElementById('<%= rbDocStoreYes.ClientID %>');
        if (chkboxYes.checked) {
            var validator = document.getElementById('<%= rfvtxtStoreName.ClientID %>');
            ValidatorEnable(validator, true);
        }
        else {
            var validator = document.getElementById('<%= rfvtxtStoreName.ClientID %>');
            ValidatorEnable(validator, false);
        }

    }
    function ShowTextBoxes() {

        if ($('#<%= rbYes.ClientID %>').is(':checked')) {
            $(".folderSection").show();
        }
        else if ($('#<%= rbNo.ClientID %>').is(':checked')) {
            $(".folderSection").hide();
        }
        var chkboxYes = document.getElementById('<%= rbYes.ClientID %>');
        if (chkboxYes.checked) {
            var validator = document.getElementById('<%= rfvStandardFolder.ClientID %>');
            ValidatorEnable(validator, true);
        }
        else {
            var validator = document.getElementById('<%= rfvStandardFolder.ClientID %>');
            ValidatorEnable(validator, false);

        }
    }

    function ShowDropDown() {

        if ($('#<%= rbEditYes.ClientID %>').is(':checked')) {
            $(".editFolderSection").show();
        } else if ($('#<%= rbEditNo.ClientID %>').is(':checked')) {
            $(".editFolderSection").hide();
        }

        var chkboxYes = document.getElementById('<%= rbEditYes.ClientID %>');
        if (chkboxYes.checked) {
            var validator = document.getElementById('<%= rfvStanFolder.ClientID %>');
            ValidatorEnable(validator, true);
        }
        else {
            var validator = document.getElementById('<%= rfvStanFolder.ClientID %>');
            ValidatorEnable(validator, false);
        }


    }

    function ShowDocumentStoreTextBox() {
        if ($('#<%= rbDocyes.ClientID %>').is(':checked')) {
            $(".editStoreName").show();
        } else if ($('#<%= rbDocNo.ClientID %>').is(':checked')) {
            $(".editStoreName").hide();
        }

        var chkboxYes = document.getElementById('<%= rbDocyes.ClientID %>');
        if (chkboxYes.checked) {
            var validator = document.getElementById('<%= rfvStoName.ClientID %>');
            ValidatorEnable(validator, true);
        }
        else {
            var validator = document.getElementById('<%= rfvStoName.ClientID %>');
            ValidatorEnable(validator, false);
        }


    }
    //    ValidatorEnable(document.getElementById('<%=rfvStandardFolder.ClientID%>'), true);
    //    ValidatorEnable(document.getElementById('<%=rfvtxtStoreName.ClientID%>'), true);
    //    ValidatorEnable(document.getElementById('<%=rfvStanFolder.ClientID%>'), true);
    //    ValidatorEnable(document.getElementById('<%=rfvStoName.ClientID%>'), true);
</script>
<%-- ############################ TEMPLATE POPUP START HERE #####################################################--%>
<div id="lightbox-popup" class="template-popup" style="border: 1px solid #5880B3;
    width: 480px;">
    <div id="popupHeaderText">
        <table width="100%">
            <tr>
                <td width="80%" align="left" valign="middle">
                    <span class="rspan">Select Template to Copy</span>
                </td>
                <td width="20%" align="right" valign="middle">
                    <a id="close-template">close[x]</a>
                </td>
            </tr>
        </table>
    </div>
    <table width="100%" id="templatePopUp" class="corporatePopUp" runat="server" clientidmode="Static">
        <tr>
            <td>
                <ucdm:DisplayMessage ID="messagePopup" runat="server" DisplayMessageWidth="450" ClientIDMode="Static" />
            </td>
        </tr>
        <tr>
            <th colspan="2" align="left" style="font-weight: bolder; font-size: 16px">
                Template Gallery
            </th>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr id="imgPlusRow">
            <td>
                <div style="max-height: 350px; overflow-y: scroll; width: 480px">
                    <asp:UpdatePanel ID="upnlProjectTempalte" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="ProjectTemplatePopUp" runat="server" ClientIDMode="Static">
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td align="center">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnCopyTemplatePopup" runat="server" Text="Copy Selected" CausesValidation="false"
                                ClientIDMode="Static" OnClientClick="CopyTemplate(); return false;" />
                        </td>
                        <td width="15">
                        </td>
                        <td>
                            <a id="close-template" href="#" style="text-decoration: none; font-size: 16px; color: White;">
                                <asp:Button ID="btnCancelTemplatePopup" runat="server" Text="Cancel" CausesValidation="false"
                                    CssClass="cancel-popUp" OnClientClick="return false;" ClientIDMode="Static" /></a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<!-- /lightbox-panel -->
<div class="lightbox">
</div>
<!-- /lightbox -->
<%-- ############################ TEMPLATE-POPUP END HERE #####################################################--%><%-- ############################ TEMPLATE DOCUMENT POPUP START HERE #####################################################--%>
<div id="lightbox-popup" class="templateDocument-popup" style="border: 1px solid #5880B3;
    width: 480px;">
    <div id="popupHeaderText">
        <table width="100%">
            <tr>
                <td width="80%" align="left" valign="middle">
                    <span class="rspan">Template Document</span>
                </td>
                <td width="20%" align="right" valign="middle">
                    <a id="close-templateDocument">close[x]</a>
                </td>
            </tr>
        </table>
    </div>
    <table width="100%" id="tempDocsPopUp" runat="server" clientidmode="Static">
        <tr>
            <td>
                <ucdm:DisplayMessage ID="msgTempDocs" runat="server" DisplayMessageWidth="450" ClientIDMode="Static" />
            </td>
        </tr>
        <tr>
            <th colspan="2" align="left" style="font-weight: bolder; font-size: 16px">
                Avaliable Template Documents
            </th>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr id="Tr1">
            <td>
                <div style="max-height: 300px; width: 480px; overflow-y: scroll;">
                    <asp:UpdatePanel ID="upnlTempDocs" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlTempDocs" runat="server" ClientIDMode="Static">
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td align="center">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnSaveTempDoc" runat="server" Text="Save" CausesValidation="false"
                                ClientIDMode="Static" OnClientClick="SelectTemplateDocument(); return false;" />
                        </td>
                        <td width="15">
                        </td>
                        <td>
                            <a id="close-templateDocument" href="#" style="text-decoration: none; font-size: 16px;
                                color: White;">
                                <asp:Button ID="btnCancelTempDocsPopup" runat="server" Text="Cancel" CausesValidation="false"
                                    CssClass="cancel-popUp" OnClientClick="return false;" ClientIDMode="Static" /></a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<!-- /lightbox-panel -->
<div class="lightbox">
</div>
<!-- /lightbox -->
<%-- ############################ TEMPLATE DOCUMENT-POPUP END HERE #####################################################--%>
<asp:UpdatePanel ID="upnlCreateMORe" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div style="font-family: Segoe UI; font-size: 16px; font-weight: bold; margin-bottom: 5px">
            My Project Templates</div>
        <table width="730" border="0">
            <tr>
                <td>
                    <asp:GridView ID="gvCreateMORe" runat="server" AllowPaging="true" AllowSorting="true"
                        PageSize="8" AutoGenerateColumns="false" HeaderStyle-BackColor="#5880B3" OnRowCommand="gvCreateMORe_RowCommand"
                        DataKeyNames="TemplateId" HeaderStyle-ForeColor="#FFFFFF" OnPageIndexChanging="gvCreateMORe_PageIndexChanging"
                        HeaderStyle-HorizontalAlign="Center" BorderColor="#CCCCCC" HeaderStyle-CssClass="header-border">
                        <EmptyDataRowStyle BackColor="LightBlue" ForeColor="Red" Width="600" />
                        <EmptyDataTemplate>
                            <table width="700" style="background-color: #5880B3; color: #FFFFFF;">
                                <tr>
                                    <td align="center">
                                        No Record Found.
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <AlternatingRowStyle BackColor="#F2F2F2" />
                        <PagerSettings FirstPageText="First Page" LastPageText="Last Page" Mode="NumericFirstLast"
                            Position="Bottom" />
                        <PagerStyle BackColor="#5880B3" ForeColor="White" BorderColor="Transparent" BorderWidth="0px"
                            BorderStyle="None" Font-Bold="true" />
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="Template Name" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="150" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" />
                            <asp:BoundField DataField="TemplateCategory" HeaderText="Category" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="80" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" />
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-border"
                                HeaderStyle-Width="200" HeaderText="Description">
                                <ItemStyle HorizontalAlign="Center" BorderColor="#CCCCCC" />
                                <ItemTemplate>
                                    <%# Eval("Description").ToString().Length > 25 ? (Eval("Description") as string).Substring(0, 25) + "<a href='#' class='ttm'>(More)<span class='tooltip'><span class='top'></span><span class='middle'> &nbsp;&nbsp;" + Eval("Description") + "</span><span class='bottom'></span></span></a>" : Eval("Description")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TemplateAccess" HeaderText="Access Type" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="80" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" />
                            <asp:BoundField DataField="CreatedDate" HeaderText="Created on" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="80" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" />
                            <asp:BoundField DataField="IsActive" HeaderText="Status" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="70" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" />
                            <asp:TemplateField ShowHeader="true" HeaderText="Edit" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="60" ItemStyle-CssClass="grid-border">
                                <ItemStyle HorizontalAlign="Center" BorderColor="#CCCCCC" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnView" runat="server" CommandName="Select" ImageUrl="~/Themes/Images/edit-16.png"
                                        ToolTip="Edit" CommandArgument='<%# Eval("TemplateId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <div style="border-top: #999999 1px solid; height: 10px; margin: 10px 0px 0px 0px;
            width: 725px">
        </div>
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlCreateTemplate" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="pnlMOReMassage" runat="server">
                                <ucdm:DisplayMessage ID="MOReMessage" runat="server" DisplayMessageWidth="700" ShowCloseButton="false"
                                    validationGroup="upnlCreateTemplate"></ucdm:DisplayMessage>
                            </asp:Panel>
                            <asp:Panel ID="pnlCreateTempForm" runat="server">
                                <div style="font-family: Segoe UI; font-size: 16px; font-weight: bold; margin-bottom: 5px">
                                    Create New Template</div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Template Name:*
                                    </div>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtTemplateName" runat="server" CssClass="bodytxt-field" MaxLength="50"
                                            Width="345px" ValidationGroup="upnlCreateTemplate"></asp:TextBox>
                                    </div>
                                    <div class="validator">
                                        <asp:RequiredFieldValidator ID="rfvTemplateName" runat="server" Text="*" Display="Static"
                                            ControlToValidate="txtTemplateName" ValidationGroup="upnlCreateTemplate"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="reTemplateName" runat="server" Display="None"
                                            ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,50}$" ControlToValidate="txtTemplateName"
                                            ErrorMessage="Template Name is invalid!" ValidationGroup="upnlCreateTemplate"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Short Name:*
                                    </div>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtTemplateShortName" runat="server" CssClass="bodytxt-field" MaxLength="30"
                                            ValidationGroup="upnlCreateTemplate">
                                        </asp:TextBox>
                                    </div>
                                    <div class="validator">
                                        <asp:RequiredFieldValidator ID="rfvTemplateShrtName" runat="server" Text="*" Display="Static"
                                            ControlToValidate="txtTemplateShortName" ValidationGroup="upnlCreateTemplate"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="reTemplateShrtName" runat="server" Display="None"
                                            ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,50}$" ControlToValidate="txtTemplateShortName"
                                            ErrorMessage="Template Short Name is invalid!" ValidationGroup="upnlCreateTemplate"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div style="margin-bottom: 10px; height: 75px">
                                    <div class="bodytxt-fieldlabel">
                                        Template Description:
                                    </div>
                                    <div style="float: left;">
                                        <textarea id="txtTemplateDescription" runat="server" class="bodytxt-field" rows="4"
                                            cols="51" onkeydown="textCounter(this,250);" onkeyup="textCounter(this,250);"></textarea>
                                    </div>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Template Type:*
                                    </div>
                                    <div style="float: left;">
                                        <asp:DropDownList ID="ddlTemplateType" runat="server" Width="150px" ValidationGroup="upnlCreateTemplate">
                                        </asp:DropDownList>
                                        <img src="../Themes/Images/icon-help.png" alt="help" />
                                    </div>
                                    <div class="validator">
                                        <asp:RequiredFieldValidator ID="tfvTemplateType" runat="server" Text="*" Display="Static"
                                            ControlToValidate="ddlTemplateType" InitialValue="0" ValidationGroup="upnlCreateTemplate"></asp:RequiredFieldValidator></div>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Template Category:*
                                    </div>
                                    <div style="float: left;">
                                        <asp:DropDownList ID="ddlTemplateCategory" runat="server" Width="150px" ValidationGroup="upnlCreateTemplate">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="validator">
                                        <asp:RequiredFieldValidator ID="rfvTemplateCCategory" runat="server" Text="*" Display="Static"
                                            ControlToValidate="ddlTemplateCategory" InitialValue="0" ValidationGroup="upnlCreateTemplate"></asp:RequiredFieldValidator></div>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Allow Access To:*
                                    </div>
                                    <div style="float: left;">
                                        <asp:DropDownList ID="ddlAllowAccess" runat="server" Width="150px" ValidationGroup="upnlCreateTemplate">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="validator">
                                        <asp:RequiredFieldValidator ID="rfvAllowAccess" runat="server" Text="*" Display="Static"
                                            ControlToValidate="ddlAllowAccess" InitialValue="0" ValidationGroup="upnlCreateTemplate"></asp:RequiredFieldValidator></div>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Submitted To:*
                                    </div>
                                    <div style="float: left;">
                                        <asp:DropDownList ID="ddlTemplateSubmittedTo" runat="server" Width="150px" ValidationGroup="upnlCreateTemplate">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="validator">
                                        <asp:RequiredFieldValidator ID="rfvSubmittedTo" runat="server" Text="*" Display="Static"
                                            ControlToValidate="ddlTemplateSubmittedTo" InitialValue="0" ValidationGroup="upnlCreateTemplate"></asp:RequiredFieldValidator></div>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Tool Level:*
                                    </div>
                                    <div style="float: left;">
                                        <asp:DropDownList ID="ddlToolLevel" runat="server" Width="150px" ValidationGroup="upnlCreateTemplate">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="validator">
                                        <asp:RequiredFieldValidator ID="rfvToolLevel" runat="server" Text="*" Display="Static"
                                            ControlToValidate="ddlToolLevel" InitialValue="0" ValidationGroup="upnlCreateTemplate"></asp:RequiredFieldValidator></div>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Template Document:
                                    </div>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtTemplateDocument" runat="server" CssClass="bodytxt-field" onKeyPress="javascript:return false;"
                                            onKeyDown="javascript:return false;" TabIndex="1">
                                        </asp:TextBox>
                                    </div>
                                    <asp:ImageButton ID="imgTemplateDocs" OnClientClick="DisplayTemplateDocumentPopup(); return false;"
                                        runat="server" ToolTip="Search Template" ImageUrl="../Themes/Images/icon-search.png"
                                        CssClass="icon-search" />
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Standard Document:
                                    </div>
                                    <div style="position: relative; float: left; margin: 0 10px 0 0;">
                                        Yes
                                        <div style="position: relative; float: right; margin-right: 15px;">
                                            <asp:RadioButton ID="rbYes" Checked="true" runat="server" GroupName="StandardDocument"
                                                TextAlign="Left" onclick="ShowTextBoxes();" />
                                        </div>
                                    </div>
                                    <div style="position: relative; float: left; margin: 0 10px 0 0;">
                                        No
                                        <div style="position: relative; float: right; margin-right: 15px;">
                                            <asp:RadioButton ID="rbNo" runat="server" GroupName="StandardDocument" TextAlign="Left"
                                                onclick="ShowTextBoxes();" /></div>
                                    </div>
                                </div>
                                <div class="folderSection">
                                    <div style="margin-bottom: 10px; height: 25px">
                                        <div class="bodytxt-fieldlabel">
                                            Standard Folder:*
                                        </div>
                                        <div style="float: left;">
                                            <asp:DropDownList ID="ddlStandardFolder" runat="server" Width="150px" ValidationGroup="upnlCreateTemplate">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="validator">
                                            <asp:RequiredFieldValidator ID="rfvStandardFolder" runat="server" Text="*" Display="Static"
                                                ControlToValidate="ddlStandardFolder" InitialValue="0" ValidationGroup="upnlCreateTemplate"></asp:RequiredFieldValidator></div>
                                    </div>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Document Store:
                                    </div>
                                    <div style="position: relative; float: left; margin: 0 10px 0 0;">
                                        Yes
                                        <div style="position: relative; float: right; margin-right: 15px;">
                                            <asp:RadioButton ID="rbDocStoreYes" Checked="true" runat="server" GroupName="DocumentStore"
                                                TextAlign="Left" onclick="ShowDocumentStore();" />
                                        </div>
                                    </div>
                                    <div style="position: relative; float: left; margin: 0 10px 0 0;">
                                        No
                                        <div style="position: relative; float: right; margin-right: 15px;">
                                            <asp:RadioButton ID="rbDocStoreNo" runat="server" GroupName="DocumentStore" TextAlign="Left"
                                                onclick="ShowDocumentStore();" /></div>
                                    </div>
                                </div>
                                <div class="folderDocument">
                                    <div style="margin-bottom: 10px; height: 25px">
                                        <div class="bodytxt-fieldlabel">
                                            Store Name:*
                                        </div>
                                        <div style="float: left;">
                                            <asp:TextBox ID="txtStoreName" runat="server" CssClass="bodytxt-field" MaxLength="50"
                                                ValidationGroup="upnlCreateTemplate">
                                            </asp:TextBox>
                                        </div>
                                        <div class="validator">
                                            <asp:RequiredFieldValidator ID="rfvtxtStoreName" runat="server" Text="*" Display="Static"
                                                ControlToValidate="txtStoreName" ValidationGroup="upnlCreateTemplate"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="reStoreName" runat="server" Display="None" ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,50}$"
                                                ControlToValidate="txtStoreName" ErrorMessage="Template Store Name is invalid!"
                                                ValidationGroup="upnlCreateTemplate"></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Create Blank:
                                        <div style="position: relative; float: right; margin-right: 15px;">
                                            <asp:RadioButton ID="blankTemplate" runat="server" GroupName="templateType" TextAlign="Left"
                                                onclick="TemplateTypeChanged(this);" /></div>
                                    </div>
                                    <div class="bodytxt-fieldlabel07">
                                        Copy From Existing:
                                        <div style="position: relative; float: right; margin-right: 15px;">
                                            <asp:RadioButton ID="copyFromExisting" runat="server" GroupName="templateType" TextAlign="Left"
                                                onclick="TemplateTypeChanged(this);" /></div>
                                    </div>
                                    <div style="float: left;">
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="txtCopyFromExisting" runat="server" CssClass="bodytxt-field" onKeyPress="javascript:return false;"
                                            onKeyDown="javascript:return false;" TabIndex="1">
                                        </asp:TextBox>
                                    </div>
                                    <asp:ImageButton ID="TemplateSearch" OnClientClick="DisplayTemplatePopup(); return false;"
                                        runat="server" ToolTip="Search Template" ImageUrl="../Themes/Images/icon-search.png"
                                        Height="18px" Width="20px" />
                                </div>
                                <div style="margin-bottom: 10px">
                                    <div style="font-size: 11px; height: 25px; font-style: italic; color: #FF0000; margin: 0px 0px 0px 10px">
                                        (Fields marked with on * are Required)</div>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <table>
                                        <tr>
                                            <td align="right" width="300">
                                                <asp:Button ID="btnSaveTemplate" runat="server" Text="Create New Template" OnClick="btnSaveTemplate_Click"
                                                    ValidationGroup="upnlCreateTemplate" />
                                            </td>
                                            <td width="15">
                                                &nbsp;
                                            </td>
                                            <td width="200">
                                                <asp:Button ID="btnCancelTemplate" runat="server" Text="Cancel" OnClick="btnCancelTemplate_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <ucl:LoadingPanel ID="ldpnCreateTemp" runat="server" Message="Saving" />
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="upnlUpdateTemplate" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="pnlEditTempMessage" runat="server">
                                <ucdm:DisplayMessage ID="EditMessage" runat="server" DisplayMessageWidth="700" ShowCloseButton="false"
                                    validationGroup="upnlUpdateTemplate"></ucdm:DisplayMessage>
                            </asp:Panel>
                            <asp:Panel ID="pnlEditTemplate" runat="server">
                                <div style="font-family: Segoe UI; font-size: 16px; font-weight: bold; margin-bottom: 5px">
                                    Edit Template</div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Template Name:*
                                    </div>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtTempName" runat="server" CssClass="bodytxt-field" MaxLength="50"
                                            Width="345px" ValidationGroup="upnlUpdateTemplate"></asp:TextBox>
                                    </div>
                                    <div class="validator">
                                        <asp:RequiredFieldValidator ID="rfvTempName" runat="server" Text="*" Display="Static"
                                            ControlToValidate="txtTempName" ValidationGroup="upnlUpdateTemplate"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="reTempName" runat="server" Display="None" ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,50}$"
                                            ControlToValidate="txtTempName" ErrorMessage="Template Name is invalid!" ValidationGroup="upnlUpdateTemplate"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Short Name:*
                                    </div>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtTempShortName" CssClass="bodytxt-field" runat="server" MaxLength="30"
                                            ValidationGroup="upnlUpdateTemplate">
                                        </asp:TextBox>
                                    </div>
                                    <div class="validator">
                                        <asp:RequiredFieldValidator ID="rfvTempShrtName" runat="server" Text="*" Display="Static"
                                            ControlToValidate="txtTempShortName" ValidationGroup="upnlUpdateTemplate"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="reTempShrtNane" runat="server" Display="None"
                                            ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,50}$" ControlToValidate="txtTempShortName"
                                            ErrorMessage="Template short Name is invalid!" ValidationGroup="upnlUpdateTemplate"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div style="margin-bottom: 10px; height: 75px">
                                    <div class="bodytxt-fieldlabel">
                                        Template Description:
                                    </div>
                                    <div style="float: left;">
                                        <textarea id="txtTempDescription" runat="server" class="bodytxt-field" rows="4" cols="51"
                                            maxlength="50" onkeydown="textCounter(this,250);" onkeyup="textCounter(this,250);"></textarea>
                                    </div>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px; width: 690px">
                                    <div class="bodytxt-fieldlabel">
                                        Template Type:*
                                    </div>
                                    <div style="float: left;">
                                        <asp:DropDownList ID="ddlTempType" runat="server" Width="150px" ValidationGroup="upnlUpdateTemplate">
                                        </asp:DropDownList>
                                        <img src="../Themes/Images/icon-help.png" alt="help" />
                                    </div>
                                    <div class="validator">
                                        <asp:RequiredFieldValidator ID="rfvddlTempType" runat="server" Text="*" Display="Static"
                                            ControlToValidate="ddlTempType" InitialValue="0" ValidationGroup="upnlUpdateTemplate"></asp:RequiredFieldValidator></div>
                                    <div class="bodytxt-fieldlabel05">
                                        Activate/Deactivate Template:
                                    </div>
                                    <div style="float: left">
                                        <asp:RadioButton ID="inactive" runat="server" Text="Inactive" GroupName="status"
                                            Checked="true" />
                                        <asp:RadioButton ID="active" runat="server" Text="Active" GroupName="status" />
                                    </div>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Template Category:*
                                    </div>
                                    <div style="float: left;">
                                        <asp:DropDownList ID="ddlTempCategory" runat="server" Width="150px" ValidationGroup="upnlUpdateTemplate">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="validator">
                                        <asp:RequiredFieldValidator ID="rfvddlTempCategory" runat="server" Text="*" Display="Static"
                                            ControlToValidate="ddlTempCategory" InitialValue="0" ValidationGroup="upnlUpdateTemplate"></asp:RequiredFieldValidator></div>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Allow Access To:*
                                    </div>
                                    <div style="float: left;">
                                        <asp:DropDownList ID="ddlAllowTempAccess" runat="server" Width="150px" ValidationGroup="upnlUpdateTemplate">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="validator">
                                        <asp:RequiredFieldValidator ID="rfvddlAllowTempAccess" runat="server" Text="*" Display="Static"
                                            ControlToValidate="ddlAllowTempAccess" InitialValue="0" ValidationGroup="upnlUpdateTemplate"></asp:RequiredFieldValidator></div>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Submitted To:*
                                    </div>
                                    <div style="float: left; height: 22px;">
                                        <asp:DropDownList ID="ddlSubmittedTo" runat="server" Width="150px" ValidationGroup="upnlUpdateTemplate">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="validator">
                                        <asp:RequiredFieldValidator ID="rfvddlSubmittedTo" runat="server" Text="*" Display="Static"
                                            ControlToValidate="ddlSubmittedTo" InitialValue="0" ValidationGroup="upnlUpdateTemplate"></asp:RequiredFieldValidator></div>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Tool Level:*
                                    </div>
                                    <div style="float: left; height: 22px;">
                                        <asp:DropDownList ID="ddlTools" runat="server" Width="150px" ValidationGroup="upnlUpdateTemplate">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="validator">
                                        <asp:RequiredFieldValidator ID="rfvTools" runat="server" Text="*" Display="Static"
                                            ControlToValidate="ddlTools" InitialValue="0" ValidationGroup="upnlUpdateTemplate"></asp:RequiredFieldValidator></div>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Template Document:
                                    </div>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtTempDocs" runat="server" CssClass="bodytxt-field" onKeyPress="javascript:return false;"
                                            onKeyDown="javascript:return false;" TabIndex="1">
                                        </asp:TextBox>
                                    </div>
                                    <asp:ImageButton ID="imgTempDocs" OnClientClick="DisplayTemplateDocumentPopup(); return false;"
                                        runat="server" ToolTip="Search Template" ImageUrl="../Themes/Images/icon-search.png"
                                        CssClass="icon-search" />
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Standard Document:
                                    </div>
                                    <div style="position: relative; float: left; margin: 0 10px 0 0;">
                                        Yes
                                        <div style="position: relative; float: right; margin-right: 15px;">
                                            <asp:RadioButton ID="rbEditYes" Checked="true" runat="server" GroupName="EditStandardDocument"
                                                TextAlign="Left" onclick="ShowDropDown();" />
                                        </div>
                                    </div>
                                    <div style="position: relative; float: left; margin: 0 10px 0 0;">
                                        No
                                        <div style="position: relative; float: right; margin-right: 15px;">
                                            <asp:RadioButton ID="rbEditNo" runat="server" GroupName="EditStandardDocument" TextAlign="Left"
                                                onclick="ShowDropDown();" /></div>
                                    </div>
                                </div>
                                <div class="editFolderSection">
                                    <div style="margin-bottom: 10px; height: 25px">
                                        <div class="bodytxt-fieldlabel">
                                            Standard Folder:*
                                        </div>
                                        <div style="float: left;">
                                            <asp:DropDownList ID="ddlStanFolder" runat="server" Width="150px" ValidationGroup="upnlUpdateTemplate">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="validator">
                                            <asp:RequiredFieldValidator ID="rfvStanFolder" runat="server" Text="*" Display="Static"
                                                ControlToValidate="ddlStanFolder" InitialValue="0" ValidationGroup="upnlUpdateTemplate"></asp:RequiredFieldValidator></div>
                                    </div>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Document Store:
                                    </div>
                                    <div style="position: relative; float: left; margin: 0 10px 0 0;">
                                        Yes
                                        <div style="position: relative; float: right; margin-right: 15px;">
                                            <asp:RadioButton ID="rbDocyes" Checked="true" runat="server" GroupName="EditDocumentStore"
                                                TextAlign="Left" onclick="ShowDocumentStoreTextBox();" />
                                        </div>
                                    </div>
                                    <div style="position: relative; float: left; margin: 0 10px 0 0;">
                                        No
                                        <div style="position: relative; float: right; margin-right: 15px;">
                                            <asp:RadioButton ID="rbDocNo" runat="server" GroupName="EditDocumentStore" TextAlign="Left"
                                                onclick="ShowDocumentStoreTextBox();" /></div>
                                    </div>
                                </div>
                                <div class="editStoreName">
                                    <div style="margin-bottom: 10px; height: 25px">
                                        <div class="bodytxt-fieldlabel">
                                            Store Name:*
                                        </div>
                                        <div style="float: left;">
                                            <asp:TextBox ID="txtStoName" runat="server" CssClass="bodytxt-field" MaxLength="50"
                                                ValidationGroup="upnlUpdateTemplate">
                                            </asp:TextBox>
                                        </div>
                                        <div class="validator">
                                            <asp:RequiredFieldValidator ID="rfvStoName" runat="server" Text="*" Display="Static"
                                                ControlToValidate="txtStoName" ValidationGroup="upnlUpdateTemplate"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="None"
                                                ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,50}$" ControlToValidate="txtStoName"
                                                ErrorMessage="Template Store Name is invalid!" ValidationGroup="upnlUpdateTemplate"></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Merge Template:
                                    </div>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtAnotherTemp" CssClass="bodytxt-field" runat="server" onKeyPress="javascript:return false;"
                                            onKeyDown="javascript:return false;" TabIndex="1">
                                        </asp:TextBox>
                                    </div>
                                    <asp:ImageButton ID="TempSearch" OnClientClick="DisplayTemplatePopup(); return false;"
                                        runat="server" ToolTip="Search Template" ImageUrl="../Themes/Images/icon-search.png" />
                                </div>
                                <div style="margin-bottom: 10px">
                                    <div style="font-size: 11px; height: 25px; font-style: italic; color: #FF0000; margin: 0px 0px 0px 10px">
                                        (Fields marked with on * are Required)</div>
                                </div>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnEditTemplate" runat="server" Text="Edit Template Contents" OnClientClick="SettingsFormSection('#editTemplate')" />
                                        </td>
                                        <td width="15">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Button ID="btnEditRules" runat="server" Text="Edit Scoring Rules" OnClientClick="SettingsFormSection('#scoringRules')" />
                                        </td>
                                        <td width="15">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSaveChanges" runat="server" Text="Save Changes" ValidationGroup="upnlUpdateTemplate"
                                                OnClick="btnUpdateTemplate_Click" />
                                        </td>
                                        <td width="15">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Button ID="btnDiscardChanges" runat="server" Text="Discard Changes" OnClick="btnDiscardChanges_Click"
                                                OnClientClick="return confirm('Are you sure want to discard the changes?');" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:HiddenField ID="hdnTemplateId" runat="server" ClientIDMode="Static" />
                                <asp:HiddenField ID="hdnUserType" runat="server" ClientIDMode="Static" />
                                <asp:HiddenField ID="hdnIsEdit" runat="server" ClientIDMode="Static" />
                                <asp:HiddenField ID="hdnIsCreate" runat="server" ClientIDMode="Static" />
                                <asp:HiddenField ID="hdnTemplateName" runat="server" ClientIDMode="Static" />
                                <ucl:LoadingPanel ID="ldpnlEditTemp" runat="server" Message="Saving" />
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
