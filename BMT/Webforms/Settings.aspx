<%@ Page Title="" Language="C#" MasterPageFile="~/BMTMaster.Master" AutoEventWireup="true"
    CodeBehind="Settings.aspx.cs" Inherits="BMT.Webforms.Settings" %>

<%@ Register Src="~/UserControls/LoadingPanel.ascx" TagName="LoadingPanel" TagPrefix="ucl" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<%@ Register Src="~/UserControls/CreateEditTemplate.ascx" TagName="CreateEditTemplate"
    TagPrefix="cet" %>

<%@ Register Src="~/UserControls/CreateProject.ascx" TagName="CreateEditProject"
    TagPrefix="cep" %>

<%@ Register Src="~/UserControls/EditTemplate.ascx" TagName="EditTemplate" TagPrefix="et" %>
<%@ Register Src="~/UserControls/ScoringRules.ascx" TagName="ScoringRules" TagPrefix="sr" %>
<%@ Register Src="~/UserControls/ConfigureProjectTemplate.ascx" TagName="ConfigureProject"
    TagPrefix="cp" %>
<asp:Content ID="contentHead" ContentPlaceHolderID="head" runat="server">
    <link rel="Stylesheet" type="text/css" href="../Themes/jquery-ui-1.8.12.custom.css" />
    <link rel="Stylesheet" type="text/css" href="../Themes/popup.css" />
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jquery-ui-1.8.12.custom.min.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/settings.js") %>"></script>
    <script language="javascript" type="text/javascript">
        function CorporateTypeChanged(rdo) {
            if (rdo.id == '<%= corporateTypeYes.ClientID %>') {
                document.getElementById('<%= ddlSiteName.ClientID %>').disabled = false;
                document.getElementById('ctl00_bodyContainer_CorpMessage').style.display = "block";
            }
            else if (rdo.id == '<%= corporateTypeNo.ClientID %>') {
                document.getElementById('<%= ddlSiteName.ClientID %>').disabled = true;
                document.getElementById('ctl00_bodyContainer_CorpMessage').style.display = "none";
            }
        }
        function CheckCorporate() {

            var chkboxYes = document.getElementById('<%= corporateTypeYes.ClientID %>');
            if (chkboxYes.checked) {
                document.getElementById('<%= ddlSiteName.ClientID %>').disabled = false;
                var validator = document.getElementById('<%= rfvDdlSiteName.ClientID %>');
                ValidatorEnable(validator, true);
                checkSiteForCorporateSite();
            }
            else {
                document.getElementById('<%= ddlSiteName.ClientID %>').disabled = true;
                var validator = document.getElementById('<%= rfvDdlSiteName.ClientID %>');
                ValidatorEnable(validator, false);
                removeCorporate();
            }
        }
    </script>
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
        <asp:UpdatePanel ID="uPanelNewRecord" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="float: right; margin-left: 10px; width: 100px;">
                    <asp:Button ID="btnAddNewSite" runat="server" CssClass="top-button-yollaw" Text="Add Site"
                        Style="width: 100px;" OnClick="btnAddNewSite_Click" CausesValidation="false"
                        OnClientClick="onNewSite();" />
                </div>
                <div style="float: right; margin-left: 10px; width: 100px;">
                    <asp:Button ID="btnAddUser" runat="server" CssClass="top-button-yollaw" Text="Add User"
                        Style="width: 100px;" OnClick="btnAddUser_Click" CausesValidation="false" OnClientClick="onNewUser();" />
                </div>
                <div style="float: right; margin-left: 10px; width: 200px;">
                    <asp:Button ID="btnCreateMORe" runat="server" CssClass="top-button-yollaw" Text="Create New Project Template"
                        Style="width: 200px;" OnClick="btnCreateMORe_Click" CausesValidation="false" />
                </div>

                <div style="float: right; margin-left: 10px; width: 200px;">
                    <asp:Button ID="btncreateProjectMORe" runat="server" CssClass="top-button-yollaw" Text="Create New Project"
                        Style="width: 200px;" OnClientClick="SettingsFormSection('#createProjectMORe'); return false;" OnClick="btncreateProjectMORe_Click" CausesValidation="false" />
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="contentBodyContainer" ContentPlaceHolderID="bodyContainer" runat="server">
    <asp:Panel ID="pnlSettings" runat="server" Style="display: none;">
        <div class="body-container-left" style="height: 500px;">
            <asp:UpdatePanel ID="upanelLeftMenu" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="hideForConsultant" runat="server">
                        <asp:HiddenField ID="hdnActiveLinkId" runat="server" ClientIDMode="Static" Value="ctl00_bodyContainer_lbPractice" />
                        <div id="practiceSetup" class="left-settings-menu">
                            <asp:LinkButton ID="lbPractice" runat="server" Text="Practice Setup" CssClass="active"
                                OnClientClick="SettingsFormSection('#practice');" OnClick="lbPractice_Click"
                                CausesValidation="false"></asp:LinkButton></div>
                        <div id="practiceSites" class="left-settings-menu">
                            <asp:LinkButton ID="lbSite" runat="server" Text="Practice Sites" OnClientClick="SettingsFormSection('#sites');"
                                OnClick="lbSite_Click" CausesValidation="false"></asp:LinkButton></div>
                        <div id="userAdministration" class="left-settings-menu">
                            <asp:LinkButton ID="lbUser" runat="server" Text="Users Administration" OnClientClick="SettingsFormSection('#users');"
                                OnClick="lbUser_Click" CausesValidation="false"></asp:LinkButton>
                        </div>
                    </div>
                    <div id="ManageProject" class="left-settings-menu" runat="server">
                        <a onclick="ManageProject();">
                            <img id="imgMoreManage" class='toggle-img' src='../Themes/Images/Plus.png' alt="Expand" />
                                <asp:LinkButton ID="manage" Text="Manage Projects" runat="server" ClientIDMode="Static"
                                Font-Bold="true" OnClientClick="ManageProject(); return false;" CssClass="manageTreeHover"></asp:LinkButton></a>
                        <table id="MOReTree" runat="server" clientidmode="Static" class="MOReTree">
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lbConfigureMORe" runat="server" Text="Configure My Projects"
                                        OnClientClick="SettingsFormSection('#configureProject'); return false;" OnClick="lbConfigureMORe_Click"
                                        CausesValidation="false"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr id="CreateEditProjectHide" runat="server">
                                <td>
                                    <asp:LinkButton ID="lbCreateMORe" runat="server" Text="Create/Edit Templates" OnClientClick="SettingsFormSection('#createMORe'); return false;"
                                        OnClick="lbCreateMORe_Click" CausesValidation="false"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr id="CreateEditProjectsHide" runat="server">
                                <td>
                                    <asp:LinkButton ID="lbProjectMORe" runat="server" Text="Create/Edit Project" OnClientClick="SettingsFormSection('#ProjectMORe'); return false;"
                                        OnClick="lbProjectMORe_Click" CausesValidation="false"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <ucl:LoadingPanel ID="LeftMenyLoadingPanel" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="body-container-right">
            <div id="practice" style="display: none;">
                <div id="practiceContainer" runat="server">
                    <asp:UpdatePanel ID="upnlPractice" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div style="font-family: Segoe UI; font-size: 16px; font-weight: bold; margin-bottom: 5px">
                                Practice Information</div>
                            <div style="font-family: Segoe UI; font-style: italic; margin-bottom: 8px">
                                This information will prepopulate your NCQA submission. Please make sure all fields
                                are accurate</div>
                            <div style="font-family: Segoe UI; font-style: italic; margin-bottom: 25px">
                                Please document all sites associated with your practice prior to creating new users</div>
                            <asp:Panel ID="pnlPracticeMessage" runat="server">
                                <ucdm:DisplayMessage ID="PracticeMessage" runat="server" validationGroup="upnlPractice"
                                    DisplayMessageWidth="700" ShowCloseButton="false"></ucdm:DisplayMessage>
                            </asp:Panel>
                            <table>
                                <tr>
                                    <td>
                                        <div style="margin-bottom: 10px; height: 25px">
                                            <div class="bodytxt-fieldlabel">
                                                Practice Legal Name:*</div>
                                            <div style="float: left;">
                                                <asp:TextBox ID="txtPracticeName" runat="server" CssClass="bodytxt-field" MaxLength="50"
                                                    ValidationGroup="upnlPractice"></asp:TextBox>
                                            </div>
                                            <div class="validator">
                                                <asp:RequiredFieldValidator ID="rfvtxtPracticeName" runat="server" Text="*" Display="Static"
                                                    ControlToValidate="txtPracticeName" ValidationGroup="upnlPractice"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator runat="server" ID="revtxtPracticeName" ControlToValidate="txtPracticeName"
                                                    Display="None" ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,50}$"
                                                    ErrorMessage="Practice Name is invalid!" ValidationGroup="upnlPractice"></asp:RegularExpressionValidator>
                                            </div>
                                            <div class="bodytxt-fieldlabel02">
                                                Practice Size:*</div>
                                            <div style="float: left">
                                                <asp:DropDownList ID="ddlPracticeSize" runat="server" ValidationGroup="upnlPractice"
                                                    Width="150px">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="validator">
                                                <asp:RequiredFieldValidator ID="rfvddlPracticeSize" runat="server" Text="*" Display="Dynamic"
                                                    ControlToValidate="ddlPracticeSize" InitialValue="0" ValidationGroup="upnlPractice"></asp:RequiredFieldValidator></div>
                                        </div>
                                        <div style="margin-bottom: 10px; height: 25px">
                                            <div class="bodytxt-fieldlabel">
                                                Address 1:</div>
                                            <div style="float: left;">
                                                <asp:TextBox ID="txtPracticeAddress1" runat="server" CssClass="bodytxt-field" MaxLength="50"
                                                    ValidationGroup="upnlPractice"></asp:TextBox>
                                            </div>
                                            <div class="validator">
                                                <asp:RegularExpressionValidator runat="server" ID="REValidatoradress1" ControlToValidate="txtPracticeAddress1"
                                                    Display="None" ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,50}$"
                                                    ErrorMessage="Address1 is invalid!" ValidationGroup="upnlPractice"></asp:RegularExpressionValidator>
                                                &nbsp;
                                            </div>
                                            <div class="bodytxt-fieldlabel02">
                                                Practice Primary Specialty:*</div>
                                            <div style="float: left">
                                                <asp:DropDownList ID="ddlPracticePrimarySpeciality" runat="server" ValidationGroup="upnlPractice"
                                                    Width="150px">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="validator">
                                                <asp:RequiredFieldValidator ID="rfvddlPracticePrimarySpeciality" runat="server" Text="*"
                                                    Display="Dynamic" ControlToValidate="ddlPracticePrimarySpeciality" InitialValue="0"
                                                    ValidationGroup="upnlPractice"></asp:RequiredFieldValidator></div>
                                        </div>
                                        <div style="margin-bottom: 10px; height: 25px">
                                            <div class="bodytxt-fieldlabel">
                                                Address 2:</div>
                                            <div style="float: left;">
                                                <asp:TextBox ID="txtPracticeAddress2" runat="server" CssClass="bodytxt-field" MaxLength="50"
                                                    ValidationGroup="upnlPractice"></asp:TextBox>
                                            </div>
                                            <div class="validator">
                                                <asp:RegularExpressionValidator runat="server" ID="REVAdress2" ControlToValidate="txtPracticeAddress2"
                                                    Display="None" ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,50}$"
                                                    ErrorMessage="Address2 is invalid!" ValidationGroup="upnlPractice"></asp:RegularExpressionValidator>
                                                &nbsp;
                                            </div>
                                            <div id="corporateType" runat="server" style="display:none;">
                                                <div class="bodytxt-fieldlabel02">
                                                    Corporate Type:</div>
                                                <div style="float: left">
                                                    <asp:RadioButton ID="corporateTypeYes" runat="server" Text="Yes" GroupName="corporate"
                                                        onclick="CorporateTypeChanged(this);" Checked="true" />&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:RadioButton ID="corporateTypeNo" runat="server" Text="No" GroupName="corporate"
                                                        onclick="CorporateTypeChanged(this);" />
                                                </div>
                                            </div>
                                        </div>
                                        <div style="margin-bottom: 10px; height: 25px">
                                            <div class="bodytxt-fieldlabel">
                                                City:*</div>
                                            <div style="float: left;">
                                                <asp:TextBox ID="txtPracticeCity" runat="server" CssClass="bodytxt-field" MaxLength="30"
                                                    ValidationGroup="upnlPractice"></asp:TextBox>
                                            </div>
                                            <div class="validator">
                                                <asp:RequiredFieldValidator ID="rfvtxtPracticeCity" runat="server" Text="*" Display="Static"
                                                    ControlToValidate="txtPracticeCity" ValidationGroup="upnlPractice"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator runat="server" ID="REVCityName" ControlToValidate="txtPracticeCity"
                                                    Display="None" ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,30}$"
                                                    ErrorMessage="City Name is invalid!" ValidationGroup="upnlPractice"></asp:RegularExpressionValidator>
                                            </div>
                                            <div id="siteSelector" runat="server" style="display:none;">
                                                <div class="bodytxt-fieldlabel02">
                                                    Select Site:*</div>
                                                <div style="float: left">
                                                    <asp:DropDownList ID="ddlSiteName" runat="server" Width="150px">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvDdlSiteName" runat="server" Text="*" Display="Static"
                                                        ControlToValidate="ddlSiteName" ValidationGroup="upnlPractice" InitialValue="0"
                                                        Enabled="false"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="margin-bottom: 10px; height: 25px">
                                            <div class="bodytxt-fieldlabel">
                                                State:*</div>
                                            <span style="float: left;">
                                                <asp:TextBox ID="txtPracticeState" runat="server" CssClass="bodytxt-field" MaxLength="30"
                                                    ValidationGroup="upnlPractice"></asp:TextBox>
                                            </span>
                                            <div class="validator">
                                                <asp:RequiredFieldValidator ID="rfvtxtPracticeState" runat="server" Text="*" Display="Static"
                                                    ControlToValidate="txtPracticeState" ValidationGroup="upnlPractice"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator runat="server" ID="REVState" ControlToValidate="txtPracticeState"
                                                    Display="None" ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,30}$"
                                                    ErrorMessage="State Name is invalid!" ValidationGroup="upnlPractice"></asp:RegularExpressionValidator>
                                            </div>
                                            <div id="CorpMessage" runat="server" class="CorpMessage" style="display:none;">
                                                <asp:Label ID="lbCorpSelectionMessage" runat="server" Text="(Go to General tab and select Corporate Elements.)"></asp:Label>
                                            </div>
                                        </div>
                                        <div style="margin-bottom: 10px; height: 25px">
                                            <div class="bodytxt-fieldlabel">
                                                ZIP:</div>
                                            <div style="float: left; margin-right: 35px;">
                                                <asp:TextBox ID="txtPracticeZipCode" runat="server" CssClass="bodytxt-field" MaxLength="30"
                                                    ValidationGroup="upnlPractice"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div style="margin-bottom: 10px; height: 25px">
                                            <div class="bodytxt-fieldlabel">
                                                Phone:*</div>
                                            <div style="float: left;">
                                                <asp:TextBox ID="txtPracticePhone" runat="server" CssClass="bodytxt-field" MaxLength="30"
                                                    ValidationGroup="upnlPractice"></asp:TextBox>
                                            </div>
                                            <div class="validator">
                                                <asp:RequiredFieldValidator ID="rfvtxtPracticePhone" runat="server" Text="*" Display="Static"
                                                    ControlToValidate="txtPracticePhone" ValidationGroup="upnlPractice"></asp:RequiredFieldValidator></div>
                                        </div>
                                        <div style="margin-bottom: 10px; height: 25px">
                                            <div class="bodytxt-fieldlabel">
                                                Fax:</div>
                                            <div style="float: left; margin-right: 35px;">
                                                <asp:TextBox ID="txtPracticeFax" runat="server" CssClass="bodytxt-field" MaxLength="30"
                                                    ValidationGroup="upnlPractice"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div style="margin-bottom: 10px; height: 25px">
                                            <div class="bodytxt-fieldlabel">
                                                Contact Name:*</div>
                                            <div style="float: left;">
                                                <asp:TextBox ID="txtPracticeContactName" runat="server" CssClass="bodytxt-field"
                                                    MaxLength="50" ValidationGroup="upnlPractice"></asp:TextBox>
                                            </div>
                                            <div class="validator">
                                                <asp:RequiredFieldValidator ID="rfvPracticeContactName" runat="server" Text="*" Display="Static"
                                                    ControlToValidate="txtPracticeContactName" ValidationGroup="upnlPractice"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="rePracticeContactName" runat="server" Display="None"
                                                    ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,50}$" ControlToValidate="txtPracticeContactName"
                                                    ErrorMessage="Contact Name is invalid!" ValidationGroup="upnlPractice"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                        <div style="margin-bottom: 10px; height: 25px">
                                            <div class="bodytxt-fieldlabel">
                                                Contact Email:*</div>
                                            <div style="float: left;">
                                                <asp:TextBox ID="txtPracticeContactEmail" runat="server" CssClass="bodytxt-field"
                                                    MaxLength="50" ValidationGroup="upnlPractice"></asp:TextBox>
                                            </div>
                                            <div class="validator">
                                                <asp:RequiredFieldValidator ID="rfvtxtPracticeContactEmail" runat="server" Text="*"
                                                    Display="Static" ControlToValidate="txtPracticeContactEmail" ValidationGroup="upnlPractice"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="revPracticeContactEmail" runat="server" Display="None"
                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtPracticeContactEmail"
                                                    ErrorMessage="Email Address is invalid!" ValidationGroup="upnlPractice"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                        <div style="margin-bottom: 10px; height: 25px">
                                            <div class="bodytxt-fieldlabel">
                                                Confirm Email:*</div>
                                            <div style="float: left;">
                                                <asp:TextBox ID="txtConfirmPracticeContactEmail" runat="server" CssClass="bodytxt-field"
                                                    MaxLength="50" ValidationGroup="upnlPractice"></asp:TextBox>
                                            </div>
                                            <div class="validator">
                                                <asp:RequiredFieldValidator ID="rfvtxtConfirmPracticeContactEmail" runat="server"
                                                    Text="*" Display="Static" ControlToValidate="txtConfirmPracticeContactEmail"
                                                    ValidationGroup="upnlPractice"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="revtxtConfirmPracticeContactEmail" runat="server"
                                                    Display="None" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                    ControlToValidate="txtConfirmPracticeContactEmail" ErrorMessage="Confirm Email Address is invalid!"
                                                    ValidationGroup="upnlPractice"></asp:RegularExpressionValidator>
                                                <asp:CompareValidator ID="cvtxtConfirmPracticeContactEmail" ControlToValidate="txtPracticeContactEmail"
                                                    ControlToCompare="txtConfirmPracticeContactEmail" runat="server" ErrorMessage="Emails do not match"
                                                    Display="None" ValidationGroup="upnlPractice"></asp:CompareValidator>
                                            </div>
                                        </div>
                                        <div style="margin-bottom: 10px">
                                            <div style="font-size: 11px; height: 25px; font-style: italic; color: #FF0000; margin: 0px 0px 0px 10px">
                                                (Fields marked with on * are Required)</div>
                                        </div>
                                        <div style="margin-bottom: 25px">
                                            <table width="735" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="right" width="300">
                                                        <asp:Button ID="btnCorporateSave" runat="server" Text="Save" ValidationGroup="upnlPractice"
                                                            OnClientClick="CheckCorporate();" />
                                                        <asp:Button ID="btnPracticeSave" runat="server" Text="Save" ValidationGroup="upnlPractice"
                                                            OnClick="btnSavePractice_Click" />
                                                    </td>
                                                    <td width="15">
                                                        &nbsp;
                                                    </td>
                                                    <td width="400">
                                                        <asp:Button ID="btnPracticeDiscardChanges" runat="server" Text="Discard Changes"
                                                            ValidationGroup="upnlPractice" OnClick="btnPracticeDiscardChanges_Click" OnClientClick="return confirm('Are you sure want to discard the changes?');
 " />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <ucl:LoadingPanel ID="LpnlPractice" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <asp:UpdatePanel ID="upnlSiteAnduser" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="sites" style="display: none;">
                        <asp:UpdatePanel ID="upnlSites" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div style="font-family: Segoe UI; font-size: 16px; font-weight: bold; margin-bottom: 10px">
                                    Practice Sites</div>
                                <table width="730" border="0">
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gvSites" runat="server" AllowPaging="true" AllowSorting="true"
                                                PageSize="8" AutoGenerateColumns="false" HeaderStyle-BackColor="#5880B3" HeaderStyle-ForeColor="#FFFFFF"
                                                DataKeyNames="PracticeSiteId" HeaderStyle-CssClass="header-border" OnRowCommand="gvSites_RowCommand" BorderColor="#CCCCCC"
                                                HeaderStyle-HorizontalAlign="Center" OnPageIndexChanging="gvSites_PageIndexChanging" CssClass="submission-grid">
                                                <EmptyDataRowStyle BorderColor="#CCCCCC" BackColor="LightBlue" ForeColor="Red" Width="600" />
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
                                                    <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="250" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC"/>
                                                    <asp:BoundField DataField="IsMainSite" HeaderText="Main Site" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="100" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC"/>
                                                    <asp:BoundField DataField="City" HeaderText="City" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="200" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC"/>
                                                    <asp:BoundField DataField="State" HeaderText="State" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="80" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC"/>
                                                    <asp:ButtonField ButtonType="Link" Text="Edit" CommandName="Select" HeaderStyle-Width="100"
                                                        ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC"/>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <div style="border-top: #999999 1px solid; height: 10px; margin: 10px 0px 0px 0px;
                                    width: 725px">
                                </div>
                                <asp:Panel ID="pnlSiteMessage" runat="server">
                                    <ucdm:DisplayMessage ID="SiteMessge" runat="server" validationGroup="upnlSites">
                                    </ucdm:DisplayMessage>
                                </asp:Panel>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlSiteForm" runat="server">
                                                <div style="font-family: Segoe UI; font-style: italic; margin-bottom: 10px">
                                                    <p>
                                                        This information will prepopulate your NCQA submission. Please make sure all fields
                                                        are accurate</p>
                                                </div>
                                                <div class="checkbox">
                                                    <asp:CheckBox ID="chkboxMainSite" runat="server" Text="Main Site" />
                                                </div>
                                                <br />
                                                <div style="margin-bottom: 10px; height: 25px">
                                                    <div class="bodytxt-fieldlabel">
                                                        Site Name:*</div>
                                                    <div style="float: left;">
                                                        <asp:TextBox ID="txtSiteName" runat="server" CssClass="bodytxt-field" MaxLength="50"
                                                            ValidationGroup="upnlSites"></asp:TextBox>
                                                    </div>
                                                    <div class="validator">
                                                        <asp:RequiredFieldValidator ID="rfvtxtSiteName" runat="server" Text="*" Display="Static"
                                                            ControlToValidate="txtSiteName" ValidationGroup="upnlSites"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator runat="server" ID="REVSiteName" ControlToValidate="txtSiteName"
                                                            Display="None" ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,50}$"
                                                            ErrorMessage="Site Name is invalid!" ValidationGroup="upnlSites"></asp:RegularExpressionValidator>
                                                    </div>
                                                    <div class="bodytxt-fieldlabel02">
                                                        Site Primary Specialty:*</div>
                                                    <div style="float: left">
                                                        <asp:DropDownList ID="ddlSitePrimarySpeciality" runat="server" ValidationGroup="upnlSites"
                                                            Width="150px">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="validator">
                                                        <asp:RequiredFieldValidator ID="rfvddlSitePrimarySpeciality" runat="server" Text="*"
                                                            Display="Dynamic" ControlToValidate="ddlSitePrimarySpeciality" InitialValue="0"
                                                            ValidationGroup="upnlSites"></asp:RequiredFieldValidator></div>
                                                </div>
                                                <div style="margin-bottom: 10px; height: 25px">
                                                    <div class="bodytxt-fieldlabel">
                                                        Address 1:</div>
                                                    <div style="float: left;">
                                                        <asp:TextBox ID="txtSiteAddress1" runat="server" CssClass="bodytxt-field" MaxLength="50"
                                                            ValidationGroup="upnlSites"></asp:TextBox>
                                                    </div>
                                                    <div class="validator">
                                                        <asp:RegularExpressionValidator runat="server" ID="REValidateAddress1" ControlToValidate="txtSiteAddress1"
                                                            Display="None" ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,50}$"
                                                            ErrorMessage="Address1 is invalid!" ValidationGroup="upnlSites"></asp:RegularExpressionValidator>
                                                        &nbsp;
                                                    </div>
                                                    <div class="bodytxt-fieldlabel02">
                                                        Number of Provider:*</div>
                                                    <div style="float: left">
                                                        <asp:TextBox ID="txtSiteNumberOfProvider" runat="server" CssClass="bodytxt-field"
                                                            MaxLength="9" ValidationGroup="upnlSites"></asp:TextBox>
                                                    </div>
                                                    <div class="validator">
                                                        <asp:RequiredFieldValidator ID="rfvtxtSiteNumberOfProvider" runat="server" Text="*"
                                                            Display="Dynamic" ControlToValidate="txtSiteNumberOfProvider" ValidationGroup="upnlSites"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="revtxtSiteNumberOfProvider" runat="server" Display="None"
                                                            ValidationExpression="\d+" ControlToValidate="txtSiteNumberOfProvider" ErrorMessage="Number of Provider field is invalid (only numeric values are allowed)."
                                                            ValidationGroup="upnlSites"></asp:RegularExpressionValidator></div>
                                                </div>
                                                <div style="margin-bottom: 10px; height: 25px">
                                                    <div class="bodytxt-fieldlabel">
                                                        Address 2:</div>
                                                    <div style="float: left;">
                                                        <asp:TextBox ID="txtSiteAddress2" runat="server" CssClass="bodytxt-field" MaxLength="50"
                                                            ValidationGroup="upnlSites"></asp:TextBox>
                                                    </div>
                                                    <div class="validator">
                                                        <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidAdress2"
                                                            ControlToValidate="txtSiteAddress2" Display="None" ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,50}$"
                                                            ErrorMessage="Address2 is invalid!" ValidationGroup="upnlSites"></asp:RegularExpressionValidator>
                                                        &nbsp;
                                                    </div>
                                                    <div class="bodytxt-fieldlabel02">
                                                        Site Group NPI:*</div>
                                                    <div style="float: left">
                                                        <asp:TextBox ID="txtSiteNPI" runat="server" CssClass="bodytxt-field" MaxLength="50"
                                                            ValidationGroup="upnlSites"></asp:TextBox>
                                                    </div>
                                                    <div class="validator">
                                                        <asp:RequiredFieldValidator ID="rfvtxtSiteNPI" runat="server" Text="*" Display="Static"
                                                            ControlToValidate="txtSiteNPI" ValidationGroup="upnlSites"></asp:RequiredFieldValidator></div>
                                                </div>
                                                <div style="margin-bottom: 10px; height: 25px">
                                                    <div class="bodytxt-fieldlabel">
                                                        City:*</div>
                                                    <div style="float: left;">
                                                        <asp:TextBox ID="txtSiteCity" runat="server" CssClass="bodytxt-field" MaxLength="30"
                                                            ValidationGroup="upnlSites"></asp:TextBox>
                                                    </div>
                                                    <div class="validator">
                                                        <asp:RequiredFieldValidator ID="rfvtxtSiteCity" runat="server" Text="*" Display="Static"
                                                            ControlToValidate="txtSiteCity" ValidationGroup="upnlSites"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator runat="server" ID="REValidCity" ControlToValidate="txtSiteCity"
                                                            Display="None" ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,30}$"
                                                            ErrorMessage="City Name is invalid!" ValidationGroup="upnlSites"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                                <div style="margin-bottom: 10px; height: 25px">
                                                    <div class="bodytxt-fieldlabel">
                                                        State:*</div>
                                                    <span style="float: left;">
                                                        <asp:TextBox ID="txtSiteState" runat="server" CssClass="bodytxt-field" MaxLength="30"
                                                            ValidationGroup="upnlSites"></asp:TextBox>
                                                    </span>
                                                    <div class="validator">
                                                        <asp:RequiredFieldValidator ID="rfvtxtSiteState" runat="server" Text="*" Display="Static"
                                                            ControlToValidate="txtSiteState" ValidationGroup="upnlSites"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator runat="server" ID="REValidState" ControlToValidate="txtSiteState"
                                                            Display="None" ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,30}$"
                                                            ErrorMessage="State Name is invalid!" ValidationGroup="upnlSites"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                                <div style="margin-bottom: 10px; height: 25px">
                                                    <div class="bodytxt-fieldlabel">
                                                        ZIP:</div>
                                                    <div style="float: left; margin-right: 35px;">
                                                        <asp:TextBox ID="txtSiteZipCode" runat="server" CssClass="bodytxt-field" MaxLength="30"
                                                            ValidationGroup="upnlSites"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div style="margin-bottom: 10px; height: 25px">
                                                    <div class="bodytxt-fieldlabel">
                                                        Phone:*</div>
                                                    <div style="float: left;">
                                                        <asp:TextBox ID="txtSitePhone" runat="server" CssClass="bodytxt-field" MaxLength="30"
                                                            ValidationGroup="upnlSites"></asp:TextBox>
                                                    </div>
                                                    <div class="validator">
                                                        <asp:RequiredFieldValidator ID="rfvtxtSitePhone" runat="server" Text="*" Display="Static"
                                                            ControlToValidate="txtSitePhone" ValidationGroup="upnlSites"></asp:RequiredFieldValidator></div>
                                                </div>
                                                <div style="margin-bottom: 10px; height: 25px">
                                                    <div class="bodytxt-fieldlabel">
                                                        Fax:</div>
                                                    <div style="float: left; margin-right: 35px;">
                                                        <asp:TextBox ID="txtSiteFax" runat="server" CssClass="bodytxt-field" MaxLength="30"
                                                            ValidationGroup="upnlSites"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div style="margin-bottom: 10px; height: 25px">
                                                    <div class="bodytxt-fieldlabel">
                                                        Contact Name:*</div>
                                                    <div style="float: left;">
                                                        <asp:TextBox ID="txtSiteContactName" runat="server" CssClass="bodytxt-field" MaxLength="50"
                                                            ValidationGroup="upnlSites"></asp:TextBox>
                                                    </div>
                                                    <div class="validator">
                                                        <asp:RequiredFieldValidator ID="rfvSiteContactName" runat="server" Text="*" Display="Static"
                                                            ControlToValidate="txtSiteContactName" ValidationGroup="upnlSites"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="revSiteContactName" runat="server" Display="None"
                                                            ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,50}$" ControlToValidate="txtSiteContactName"
                                                            ErrorMessage="Contact Name is invalid!" ValidationGroup="upnlSites"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                                <div style="margin-bottom: 10px; height: 25px">
                                                    <div class="bodytxt-fieldlabel">
                                                        Contact Email:*</div>
                                                    <div style="float: left;">
                                                        <asp:TextBox ID="txtSiteContactEmail" runat="server" CssClass="bodytxt-field" MaxLength="50"
                                                            ValidationGroup="upnlSites"></asp:TextBox>
                                                    </div>
                                                    <div class="validator">
                                                        <asp:RequiredFieldValidator ID="rfvtxtSiteContactEmail" runat="server" Text="*" Display="Static"
                                                            ControlToValidate="txtSiteContactEmail" ValidationGroup="upnlSites"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="revtxtSiteContactEmail" runat="server" Display="None"
                                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtSiteContactEmail"
                                                            ErrorMessage="Email Address is invalid!" ValidationGroup="upnlSites"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                                <div style="margin-bottom: 10px; height: 25px">
                                                    <div class="bodytxt-fieldlabel">
                                                        Confirm Email:*</div>
                                                    <div style="float: left;">
                                                        <asp:TextBox ID="txtConfrmSiteContactEmail" runat="server" CssClass="bodytxt-field"
                                                            MaxLength="50" ValidationGroup="upnlSites"></asp:TextBox>
                                                    </div>
                                                    <div class="validator">
                                                        <asp:RequiredFieldValidator ID="rfvtxtConfrmSiteContactEmail" runat="server" Text="*"
                                                            Display="Static" ControlToValidate="txtConfrmSiteContactEmail" ValidationGroup="upnlSites"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="revtxtConfrmSiteContactEmail" runat="server"
                                                            Display="None" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                            ControlToValidate="txtConfrmSiteContactEmail" ErrorMessage="Confirm Email Address is invalid!"
                                                            ValidationGroup="upnlSites"></asp:RegularExpressionValidator>
                                                        <asp:CompareValidator ID="cvtxtConfrmSiteContactEmail" ControlToValidate="txtSiteContactEmail"
                                                            ControlToCompare="txtConfrmSiteContactEmail" runat="server" ErrorMessage="Emails do not match"
                                                            Display="None" ValidationGroup="upnlSites"></asp:CompareValidator>
                                                    </div>
                                                </div>
                                                <div>
                                                    <asp:HiddenField ID="hdnSiteId" runat="server" ClientIDMode="Static" />
                                                    <asp:HiddenField ID="hdnsiteAddressId" runat="server" ClientIDMode="Static" />
                                                </div>
                                                <div style="margin-bottom: 10px">
                                                    <div style="font-size: 11px; height: 25px; font-style: italic; color: #FF0000; margin: 0px 0px 0px 10px">
                                                        (Fields marked with on * are Required)</div>
                                                </div>
                                                <div style="margin-bottom: 25px">
                                                    <table width="735" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td align="right" width="300">
                                                                <asp:Button ID="btnSiteSave" runat="server" Text="Save" ValidationGroup="upnlSites"
                                                                    OnClick="btnSiteSave_Click" />
                                                            </td>
                                                            <td width="15">
                                                                &nbsp;
                                                            </td>
                                                            <td width="100">
                                                                <a id="SiteDiscardTag">
                                                                    <asp:Button ID="btnSiteDiscardChanges" runat="server" Text="Discard Changes" CausesValidation="false"
                                                                        OnClick="btnSiteDiscardChanges_Click" OnClientClick="return confirm('Are you sure want to discard the changes?');
 " /></a>
                                                            </td>
                                                            <td width="18">
                                                                &nbsp;
                                                            </td>
                                                            <td width="306">
                                                                <asp:Button ID="btnSiteDelete" runat="server" Text="Delete Site" ValidationGroup="upnlSites"
                                                                    OnClick="btnSiteDelete_Click" OnClientClick="return confirm('Are you sure want to delete the selected site?');
 " />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <ucl:LoadingPanel ID="lpnlSite" runat="server" Message="Saving" />
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                                <asp:HiddenField ID="hiddenPracticeSitesList" runat="server" ClientIDMode="Static" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <script type="text/javascript">
                        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (evt, args) {
                            /* Apply jQuery datepicker control */
                            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
                            function EndRequestHandler(sender, args) {
                                var curr_date = new Date();
                                $(".datePicker").datepicker({
                                    showOn: 'button',
                                    buttonImage: '../Themes/Images/calendar.gif',
                                    buttonImageOnly: true,
                                    dateFormat: 'mm/dd/yy',
                                    minDate: curr_date
                                });

                                $('#' + _txtUserBPRP).attr('disabled', 'disabled');
                                $('#' + _txtUserDRP).attr('disabled', 'disabled');
                                $('#' + _txtUserHSRP).attr('disabled', 'disabled');

                                /* POPUP SCREEN*/
                                $('.changePassowrd-Popup :input:not(input[type=submit])').keypress(function (e) {
                                    if (e.which == 13) {
                                        $('#btnChangePassword')[0].click();
                                        e.preventDefault();
                                    }
                                });

                                $('.changePassowrd-Popup :input').focus(function (e) {
                                    e.preventDefault();
                                });
                                $("a#show-popup").click(function (e) {
                                    $('#btnChangePassword').focus();
                                    $('#lightbox, .changePassowrd-Popup').fadeIn(300);

                                });
                                $("a#close-popup").click(function () {
                                    $('#ctl00_bodyContainer_txtcurrentPassword').val('');
                                    $('#ctl00_bodyContainer_txtNewPassword').val('');
                                    $('#ctl00_bodyContainer_txtConfirmNewPassword').val('');
                                    $('#lightbox, .changePassowrd-Popup').fadeOut(300);
                                });

                                // keep highlight the selected node
                                KeepActiveNodeSelected();
                            }
                        });                                          
                    </script>
                    <div id="users" style="display: none;">
                        <asp:UpdatePanel ID="upnlUser" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div id="lightbox-popup" class="changePassowrd-Popup" style="border: 1px solid #5880B3;">
                                    <asp:UpdatePanel ID="upnlChangePassword" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div id="popupHeaderText">
                                                <table width="100%">
                                                    <tr>
                                                        <td width="65%" align="right" valign="middle">
                                                            Change Password
                                                        </td>
                                                        <td width="35%" align="right" valign="middle">
                                                            <a id="close-popup" href="#">close[x]</a>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:Panel ID="Panel1" runat="server">
                                                <ucdm:DisplayMessage ID="changePwdMessage" runat="server" validationGroup="changePassword"
                                                    DisplayMessageWidth="400" ShowCloseButton="false"></ucdm:DisplayMessage>
                                            </asp:Panel>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <p>
                                                            Current password:*</p>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtcurrentPassword" runat="server" CssClass="text-field02" ValidationGroup="changePassword"
                                                            TextMode="Password"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvtxtcurrentPassword" runat="server" ControlToValidate="txtcurrentPassword"
                                                            ValidationGroup="changePassword" Text="*"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <p>
                                                            New password:*</p>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNewPassword" runat="server" CssClass="text-field02" ValidationGroup="changePassword"
                                                            TextMode="Password"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" ControlToValidate="txtNewPassword"
                                                            ValidationGroup="changePassword" Text="*"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <p>
                                                            Confirm new password:*</p>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtConfirmNewPassword" runat="server" CssClass="text-field02" ValidationGroup="changePassword"
                                                            TextMode="Password"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvtxtConfirmNewPassword" runat="server" ControlToValidate="txtConfirmNewPassword"
                                                            ValidationGroup="changePassword" Text="*"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <asp:CompareValidator ID="cvPassword" runat="server" ValueToCompare="txtConfirmNewPassword"
                                                            ControlToValidate="txtConfirmNewPassword" ControlToCompare="txtNewPassword" Display="None"
                                                            ErrorMessage="The Password you typed do not match" ValidationGroup="changePassword"></asp:CompareValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="400px">
                                                <tr>
                                                    <td align="right" width="185px">
                                                        <asp:Button ID="btnChangePassword" runat="server" Text="OK" ValidationGroup="changePassword"
                                                            OnClick="btnChangePassword_Click" ClientIDMode="Static" />
                                                    </td>
                                                    <td align="left" width="200px">
                                                        <a id="close-popup" href="#" style="text-decoration: none; font-size: 16px; color: White;">
                                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                                                                OnClick="btnCancelChangePwd_Click" ClientIDMode="Static" /></a>
                                                    </td>
                                                </tr>
                                            </table>
                                            <ucl:LoadingPanel ID="LoadingPanel1" runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <!-- /lightbox-panel -->
                                <!-- /lightbox-panel -->
                                <div class="lightbox">
                                </div>
                                <!-- /lightbox -->
                                <div style="font-family: Segoe UI; font-size: 16px; font-weight: bold; margin-bottom: 10px">
                                    User Administration</div>
                                <table border="0" width="730">
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gvUsers" runat="server" AllowPaging="true" AllowSorting="true"
                                                PageSize="8" AutoGenerateColumns="false" HeaderStyle-BackColor="#5880B3" HeaderStyle-ForeColor="#FFFFFF"
                                                DataKeyNames="UserId" HeaderStyle-CssClass="header-border" OnRowCommand="gvUsers_RowCommand" BorderColor="#CCCCCC" HeaderStyle-HorizontalAlign="Center"
                                                OnPageIndexChanging="gvUsers_PageIndexChanging" CssClass="submission-grid">
                                                <EmptyDataRowStyle BackColor="LightBlue" BorderColor="#CCCCCC" ForeColor="Red" Width="600" />
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
                                                    <asp:BoundField DataField="LastName" HeaderText="Last Name" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="125" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC"/>
                                                    <asp:BoundField DataField="FirstName" HeaderText="First Name" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="125" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC"/>
                                                    <asp:BoundField DataField="Username" HeaderText="Username" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="125" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC"/>
                                                    <asp:BoundField DataField="Name" HeaderText="Site Name" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="200" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC"/>
                                                    <asp:BoundField DataField="IsActive" HeaderText="Active" ItemStyle-HorizontalAlign="Center"
                                                        HeaderStyle-Width="100" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC"/>
                                                    <asp:ButtonField ButtonType="Link" Text="Edit" CommandName="Select" HeaderStyle-Width="100"
                                                        ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC"/>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                <div style="border-top: #999999 1px solid; height: 10px; margin: 10px 0px 0px 0px;
                                    width: 725px">
                                </div>
                                <asp:Panel ID="pnlUserMessage" runat="server">
                                    <ucdm:DisplayMessage ID="UserMessage" runat="server" validationGroup="upnlUser">
                                    </ucdm:DisplayMessage>
                                </asp:Panel>
                                <asp:Panel ID="pnlUserForm" runat="server">
                                    <div style="font-family: Segoe UI; margin-bottom: 10px; color: #FF0000; font-size: 14px;
                                        font-style: italic">
                                        <p>
                                            Important! For NCQA PCMH projects, you MUST register ALL providers (MD, DO, NP,
                                            PA) in your practice.</p>
                                    </div>
                                    <table border="0" cellpadding="0" cellspacing="0" width="720">
                                        <tr>
                                            <td>
                                                <div style="margin-bottom: 10px; height: 25px">
                                                    <div class="bodytxt-fieldlabel03">
                                                        <p>
                                                            Last Name:*</p>
                                                    </div>
                                                    <div style="float: left;">
                                                        <asp:TextBox ID="txtUserLastName" runat="server" CssClass="bodytxt-field" MaxLength="25"
                                                            ValidationGroup="upnlUser"></asp:TextBox>
                                                    </div>
                                                    <div class="validator">
                                                        <asp:RequiredFieldValidator ID="rfvtxtUserLastName" runat="server" Display="Static"
                                                            Text="*" ErrorMessage="Please select the Last Name" ControlToValidate="txtUserLastName"
                                                            ValidationGroup="upnlUser"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator runat="server" ID="RegExpValidLastName" ControlToValidate="txtUserLastName"
                                                            Display="None" ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,25}$"
                                                            ErrorMessage="Last Name is invalid!" ValidationGroup="upnlUser"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div style="margin-bottom: 10px; height: 25px">
                                                    <div class="bodytxt-fieldlabel04">
                                                        User Name:
                                                    </div>
                                                    <div style="float: left">
                                                        <asp:TextBox ID="txtUserName" runat="server" CssClass="bodytxt-field" MaxLength="25"
                                                            ValidationGroup="upnlUser" Enabled="false" Text="Auto Generate"></asp:TextBox></div>
                                                    <div class="validator">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="margin-bottom: 10px; height: 25px">
                                                    <div class="bodytxt-fieldlabel03">
                                                        <p>
                                                            First Name:*</p>
                                                    </div>
                                                    <div style="float: left;">
                                                        <asp:TextBox ID="txtUserFirstName" runat="server" CssClass="bodytxt-field" MaxLength="25"
                                                            ValidationGroup="upnlUser"></asp:TextBox></div>
                                                    <div class="validator">
                                                        <asp:RequiredFieldValidator ID="rfvtxtUserFirstName" runat="server" Display="Static"
                                                            Text="*" ErrorMessage="Please select the First Name" ControlToValidate="txtUserFirstName"
                                                            ValidationGroup="upnlUser"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator runat="server" ID="RegularExpValidFirstName" ControlToValidate="txtUserFirstName"
                                                            Display="None" ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,25}$"
                                                            ErrorMessage="First Name is invalid!" ValidationGroup="upnlUser"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div style="margin-bottom: 10px; height: 25px">
                                                    <div class="bodytxt-fieldlabel04">
                                                        Password:</div>
                                                    <div style="float: left">
                                                        <asp:TextBox ID="txtUserPassword" runat="server" CssClass="bodytxt-field" MaxLength="25"
                                                            ValidationGroup="upnlUser" Enabled="false"></asp:TextBox></div>
                                                    <div class="validator">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="margin-bottom: 10px; height: 25px">
                                                    <div class="bodytxt-fieldlabel03">
                                                        <p>
                                                            User Primary Site:*</p>
                                                    </div>
                                                    <div style="float: left;">
                                                        <asp:DropDownList ID="ddlUserPrimarySite" runat="server" ValidationGroup="upnlUser"
                                                            Width="150px">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="validator">
                                                        <asp:RequiredFieldValidator ID="rfvddlUserPrimarySite" runat="server" Display="Static"
                                                            Text="*" ErrorMessage="Please select the User Primary Site" ControlToValidate="ddlUserPrimarySite"
                                                            InitialValue="0" ValidationGroup="upnlUser"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div style="margin-bottom: 10px; height: 25px">
                                                    <div class="bodytxt-fieldlabel04">
                                                        &nbsp;
                                                    </div>
                                                    <div style="float: left">
                                                        <asp:Panel ID="pnlChangepwdlink" runat="server" ClientIDMode="Static">
                                                            <a id='show-popup' href='#'>Change Password</a></asp:Panel>
                                                    </div>
                                                    <div class="validator">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="margin-bottom: 10px; height: 25px">
                                                    <div class="bodytxt-fieldlabel03">
                                                        <p>
                                                            Email:*</p>
                                                    </div>
                                                    <div style="float: left;">
                                                        <asp:TextBox ID="txtUserEmail" runat="server" CssClass="bodytxt-field" MaxLength="50"
                                                            ValidationGroup="upnlUser"></asp:TextBox>
                                                    </div>
                                                    <div class="validator">
                                                        <asp:RequiredFieldValidator ID="rfvtxtUserEmail" runat="server" Display="Static"
                                                            Text="*" ErrorMessage="Please select the Email" ControlToValidate="txtUserEmail"
                                                            ValidationGroup="upnlUser"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator runat="server" ID="revtxtUserEmail" ControlToValidate="txtUserEmail"
                                                            Display="None" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                            ErrorMessage="Email Address is invalid!" ValidationGroup="upnlUser"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div style="margin-bottom: 10px; height: 25px">
                                                    <div class="bodytxt-fieldlabel04">
                                                        Deactivate User:</div>
                                                    <div style="float: left">
                                                        <asp:RadioButtonList ID="rblUserDeactivate" runat="server" RepeatColumns="2">
                                                            <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Blocked" Value="0"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </div>
                                                    <div class="validator">
                                                        <asp:RequiredFieldValidator ID="rfvrblUserDeactivate" runat="server" ErrorMessage="Please select the User Acitvation type."
                                                            Display="None" ControlToValidate="rblUserDeactivate" ValidationGroup="upnlUser"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="margin-bottom: 10px; height: 25px">
                                                    <div class="bodytxt-fieldlabel03">
                                                        <p>
                                                            Confirm Email:*</p>
                                                    </div>
                                                    <div style="float: left;">
                                                        <asp:TextBox ID="txtConfrmUserEmail" runat="server" CssClass="bodytxt-field" MaxLength="50"
                                                            ValidationGroup="upnlUser"></asp:TextBox>
                                                    </div>
                                                    <div class="validator">
                                                        <asp:RequiredFieldValidator ID="rfvtxtConfrmUserEmail" runat="server" Display="Static"
                                                            Text="*" ErrorMessage="Please select the Confirm Email" ControlToValidate="txtConfrmUserEmail"
                                                            ValidationGroup="upnlUser"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator runat="server" ID="revtxtConfrmUserEmail" ControlToValidate="txtConfrmUserEmail"
                                                            Display="None" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                            ErrorMessage="Confirm Email Address is invalid!" ValidationGroup="upnlUser"></asp:RegularExpressionValidator>
                                                        <asp:CompareValidator ID="cvtxtConfrmUserEmail" ControlToValidate="txtUserEmail"
                                                            ControlToCompare="txtConfrmUserEmail" runat="server" ErrorMessage="Emails do not match"
                                                            Display="None" ValidationGroup="upnlUser"></asp:CompareValidator>
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div style="margin-bottom: 10px; height: 25px">
                                                    <div class="bodytxt-fieldlabel04">
                                                        <p>
                                                            Provider Type:*</p>
                                                    </div>
                                                    <div style="float: left; margin-left: 0px;">
                                                        <asp:RadioButtonList ID="rblProviderType" runat="server" RepeatColumns="3" onclick="providerListDisable();">
                                                        </asp:RadioButtonList>
                                                    </div>
                                                    <div class="validator">
                                                        <asp:RequiredFieldValidator ID="rfvrblProviderType" runat="server" ErrorMessage="Please select the Provider Type."
                                                            Display="None" ControlToValidate="rblProviderType" ValidationGroup="upnlUser"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="margin-bottom: 10px; height: 25px">
                                                    <div class="bodytxt-fieldlabel03">
                                                        <p>
                                                            Credentials:*</p>
                                                    </div>
                                                    <div style="float: left;">
                                                        <asp:DropDownList ID="ddlUserCredential" runat="server" ValidationGroup="upnlUser"
                                                            CssClass="credpec" Width="150px">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="validator1">
                                                        <asp:CustomValidator ID="credentialcustom" runat="server" ControlToValidate="ddlUserCredential"
                                                            ClientValidationFunction="validation" Display="Dynamic" Text="*" ErrorMessage="Please select the Credential"
                                                            ValidationGroup="upnlUser"></asp:CustomValidator>
                                                    </div>
                                                </div>
                                            </td>
                                            <td rowspan="5" valign="top">
                                                <div style="margin-bottom: 5px; height: 20px">
                                                    <div style="float: left; font-weight: bold; color: #000000">
                                                        Existing/Prior NCQA Program Recognitions*</div>
                                                </div>
                                                <div style="margin-bottom: 5px; height: 20px">
                                                    <div style="float: right;">
                                                        Expiration(mm/dd/yyyy)</div>
                                                </div>
                                                <table border="0" width="100%">
                                                    <tr>
                                                        <td width="69%">
                                                            <div style="margin-bottom: 5px; height: 25px; width: 250px;">
                                                                <asp:CheckBox ID="chkBoxUserBPRP" runat="server" CausesValidation="false" />Back
                                                                Pain Recognition (BPRP)
                                                            </div>
                                                            <div style="margin-bottom: 5px; height: 25px; width: 250px;">
                                                                <asp:CheckBox ID="chkBoxUserDRP" runat="server" CausesValidation="false" />Diabetes
                                                                Recognition Program (DRP)
                                                            </div>
                                                            <div style="margin-bottom: 5px; height: 25px; width: 260px;">
                                                                <asp:CheckBox ID="chkBoxUserHSRP" runat="server" CausesValidation="false" />Heart/Stroke
                                                                Recognition Program (HSRP)
                                                            </div>
                                                        </td>
                                                        <td valign="top" width="31%">
                                                            <div style="margin-bottom: 5px; height: 25px; width: 100px;">
                                                                <asp:TextBox ID="txtUserBPRP" runat="server" CssClass="datePicker"></asp:TextBox>
                                                            </div>
                                                            <div style="margin-bottom: 5px; height: 25px; width: 100px;">
                                                                <asp:TextBox ID="txtUserDRP" runat="server" CssClass="datePicker"></asp:TextBox>
                                                            </div>
                                                            <div style="margin-bottom: 5px; height: 25px; width: 100px;">
                                                                <asp:TextBox ID="txtUserHSRP" runat="server" CssClass="datePicker"></asp:TextBox></div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="margin-bottom: 10px; height: 25px">
                                                    <div class="bodytxt-fieldlabel03">
                                                        <p>
                                                            Specialty:*</p>
                                                    </div>
                                                    <div style="float: left;">
                                                        <asp:DropDownList ID="ddlUserSpeciality" runat="server" ValidationGroup="upnlUser"
                                                            CssClass="credpec" Width="150px">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="validator2">
                                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="ddlUserSpeciality"
                                                            ClientValidationFunction="validation" Display="Dynamic" Text="*" ErrorMessage="Please select the Speciality"
                                                            ValidationGroup="upnlUser"></asp:CustomValidator>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="margin-bottom: 10px; height: 25px">
                                                    <div class="bodytxt-fieldlabel03">
                                                        <p>
                                                            Role:</p>
                                                    </div>
                                                    <div style="float: left;">
                                                    </div>
                                                    <asp:DropDownList ID="ddlUserRole" runat="server" ValidationGroup="upnlUser" Width="150px">
                                                    </asp:DropDownList>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                    <div>
                                        <asp:HiddenField ID="hdnUserId" runat="server" ClientIDMode="Static" />
                                    </div>
                                    <div style="margin-bottom: 10px">
                                        <div style="font-size: 11px; height: 25px; font-style: italic; color: #FF0000; margin: 0px 0px 0px 10px">
                                            (Fields marked with on * are Required)</div>
                                    </div>
                                    <div id="savedischarge" style="margin-bottom: 25px">
                                        <table border="0" cellpadding="0" cellspacing="0" width="735">
                                            <tr>
                                                <td align="right" width="282">
                                                    <asp:Button ID="btnUserSave" runat="server" Text="Save" ValidationGroup="upnlUser"
                                                        OnClick="btnUserSave_Click" />
                                                </td>
                                                <td width="18">
                                                    &nbsp;
                                                </td>
                                                <td width="111">
                                                    <asp:Button ID="btnUserDiscardChanges" runat="server" Text="Discard Changes" CausesValidation="false"
                                                        OnClick="btnUserDiscardChanges_Click" OnClientClick="return confirm('Are you sure want to discard the changes?');
 " />
                                                </td>
                                                <td width="18">
                                                    &nbsp;
                                                </td>
                                                <td width="306">
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <ucl:LoadingPanel ID="lpnlUser" runat="server" />
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="configureProject" style="display: none;">
                        <cp:ConfigureProject ID="ConfigProj" runat="server" />
                    </div>
                    <div id="createMORe" style="display: none;">
                        <cet:CreateEditTemplate ID="CreateEditTemplate" runat="server" />
                    </div>

                    <div id="editTemplate" style="display: none;">
                        <et:EditTemplate ID="EditTemp" runat="server"></et:EditTemplate>
                        <asp:HiddenField ID="IsQueryString" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnPracticeSiteId" runat="server" ClientIDMode="Static" />
                    </div>
                    <div id="scoringRules" style="display: none;">
                        <sr:ScoringRules ID="sr" runat="server"></sr:ScoringRules>
                        <%--<asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />--%>
                    </div>
                    
                    <div id="createProjectMORe" style="display: none;">
                        <cep:CreateEditProject ID="CreateEditProject" runat="server" />
                    </div>
                </ContentTemplate>
                <%--  <Triggers>
                    <asp:PostBackTrigger ControlID="btnSiteSave" />
                </Triggers>--%>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
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
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr align="center">
                <td colspan="2">
                    <asp:Label runat="server" ClientIDMode="Static" ForeColor="Red" Font-Size="10px"
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
                        <asp:Button ID="btnChangeCorpSite" runat="server" Text="OK" CausesValidation="false"
                            ClientIDMode="Static" OnClientClick="changeCorporateSite(); return false;" CssClass="hideDisplay" /></a>
                    <a id="close-confirmation" href="#" style="text-decoration: none; font-size: 16px;
                        color: White;">
                        <asp:Button ID="btnCopyCorpElement" runat="server" Text="OK" CausesValidation="false"
                            ClientIDMode="Static" OnClientClick="copyToXML(); return false;" CssClass="hideDisplay" /></a>
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
</asp:Content>
