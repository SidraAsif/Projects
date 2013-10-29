<%@ Page Language="C#" MasterPageFile="~/BMTMaster.Master" AutoEventWireup="true"
    CodeBehind="ConsultingUser.aspx.cs" Inherits="BMT.Webforms.ConsultingUser" %>

<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<%@ Register Src="~/UserControls/LoadingPanel.ascx" TagName="LoadingPanel" TagPrefix="ucl" %>
<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">
    <link rel="Stylesheet" type="text/css" href="../Themes/jquery-ui-1.8.12.custom.css" />
    <link rel="Stylesheet" type="text/css" href="../Themes/popup.css" />
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jquery-ui-1.8.12.custom.min.js") %>"></script>
    <%--<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/settings.js") %>"></script>--%>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/consultant.js") %>"></script>
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
            </tr>
        </table>
    </div>
    <div class="inner-menu-hover-container-right-combo">
        <div style="float: right; margin-right: 45px; width: 150px">
            <asp:Button ID="btnAddConsultant" runat="server" Text="Add Consultant" CssClass="top-button-yollaw"
                Style="width: 130px" OnClick="btnAddConsultant_Click" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bodyContainer" runat="server">
    <asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="90%" style="margin-left: auto; margin-right: auto;">
                <tr>
                    <td>
                        <ucdm:DisplayMessage ID="Message" runat="server" DisplayMessageWidth="900" ShowCloseButton="false"
                            validationGroup="First"></ucdm:DisplayMessage>
                    </td>
                </tr>
                <tr>
                    <td style="height: 50px;">
                        <asp:Label ID="LblHeader" runat="server" Font-Bold="True" Font-Names="Calibri" Font-Size="18pt"
                            Text="Consultant Administration"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="GVData" runat="server" AutoGenerateColumns="False" DataKeyNames="UserID"
                            AllowPaging="True" CellSpacing="3" GridLines="None" PageSize="8" Width="100%"
                            OnRowCommand="GVData_RowCommand" OnPageIndexChanging="GVData_PageIndexChanging">
                            <AlternatingRowStyle BackColor="#F2F2F2" />
                            <Columns>
                                <asp:BoundField DataField="UserID" HeaderText="User ID" SortExpression="User ID"
                                    Visible="false" />
                                <asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="Last Name" />
                                <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="First Name" />
                                <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="User Name" />
                                <asp:BoundField DataField="Organization" HeaderText="Organization" SortExpression="Organization" />
                                <asp:BoundField DataField="ConsultantTypeName" HeaderText="Consultant Type" SortExpression="Consultant Type" />
                                <asp:BoundField DataField="IsActive" HeaderText="Status" SortExpression="Status" />
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LbEdit" runat="server" CommandName="Select" CommandArgument='<%# Bind("UserID") %>'>Edit</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" BackColor="#5880B3"
                                ForeColor="White" />
                            <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td height="30px">
                        <asp:Label ID="lblGrayLine" runat="server" Height="1px" BackColor="DarkGray" Width="100%">
                        </asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlConsultant" runat="server" ClientIDMode="Static" Style="display: none;">
                <table width="90%" style="margin-left: auto; margin-right: auto;">
                    <tr>
                        <td colspan="5">
                            <asp:Panel ID="pnlConsultantValidationSUmmary" runat="server">
                                <ucdm:DisplayMessage ID="ConsultantValidationSUmmary" runat="server" validationGroup="save">
                                </ucdm:DisplayMessage>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 108px;">
                            Last Name:
                            <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ErrorMessage="Enter Last Name"
                                Text="*" ControlToValidate="txtLastName" ValidationGroup="save"></asp:RequiredFieldValidator>
                        </td>
                        <td style="width: 290px;">
                            <asp:TextBox runat="server" ID="txtLastName" Width="150px"></asp:TextBox>
                        </td>
                        <td style="width: 130px;">
                            User Name:
                            <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ErrorMessage="Enter User Name"
                                Text="*" ControlToValidate="txtUserName" ValidationGroup="save"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtUserName" Width="150px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            First Name:
                            <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ErrorMessage="Enter First Name"
                                Text="*" ControlToValidate="txtFirstName" ValidationGroup="save"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtFirstName" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            Password:
                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Enter Password"
                                Text="*" ControlToValidate="txtPassword" ValidationGroup="save"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPassword" Width="150px" autocomplete="off" TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Email:
                            <asp:RequiredFieldValidator ID="RfvtxtEmail" runat="server" ErrorMessage="Enter Email"
                                Text="*" ControlToValidate="txtEmail" ValidationGroup="save"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtEmail" Width="150px"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                ValidationGroup="save" ErrorMessage="Invalid Email Address" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$">
                            </asp:RegularExpressionValidator>
                        </td>
                        <td>
                            &nbsp
                        </td>
                        <td>
                            <asp:HyperLink ID="hypChangePassword" runat="server" NavigateUrl="javascript:OnChangePassword();"
                                Style="display: none;" Text="Change Password"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Confirm Email:
                            <asp:RequiredFieldValidator ID="rfvtxtConfirmEmail" runat="server" ErrorMessage="Mismatch Email"
                                Text="*" ControlToValidate="txtConfirmEmail" ValidationGroup="save"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtConfirmEmail" Width="150px"></asp:TextBox>
                            <asp:CompareValidator ID="cvtxtConfirmEmail" runat="server" ControlToValidate="txtConfirmEmail"
                                ControlToCompare="txtEmail" Text="Emails do not match" ValidationGroup="save"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Phone:
                            <asp:RequiredFieldValidator ID="rfvtxtPhone" runat="server" ErrorMessage="Enter Phone No."
                                Text="*" ControlToValidate="txtPhone" ValidationGroup="save"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPhone" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            Deactivate Consultant:
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rbListStatus" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Active " Value="1"></asp:ListItem>
                                <asp:ListItem Text="Blocked" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Consultant Type:
                            <asp:RequiredFieldValidator ID="rfvConsultantType" runat="server" ErrorMessage="Select Consultant Type"
                                Text="*" ControlToValidate="ddlConsultantType" InitialValue="0" ValidationGroup="save"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlConsultantType" Width="155px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Service Area:
                            <asp:RequiredFieldValidator ID="rfvServiceArea" runat="server" ErrorMessage="Enter Service Area"
                                Text="*" ControlToValidate="txtServiceArea" ValidationGroup="save"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtServiceArea" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Featured:
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rbListFeatured" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="No " Value="0"></asp:ListItem>
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td valign="top">
                            <div style="position: absolute; vertical-align: top;">
                                <table>
                                    <tr>
                                        <td>
                                            Assign Practices:
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            All Practices
                                            <br />
                                            <asp:ListBox ID="lstBoxPractice" runat="server" Font-Names="Arial" Font-Size="9pt"
                                                SelectionMode="Multiple" Width="220px" Height="200px"></asp:ListBox>
                                        </td>
                                        <td>
                                            <asp:Button runat="server" ID="btnAdd" Text=">>" Width="50px" OnClick="btnAdd_Click" />
                                            <br />
                                            <asp:Button runat="server" ID="btnRemove" Text="<<" Width="50px" OnClick="btnRemove_Click" />
                                        </td>
                                        <td>
                                            Selected Practices
                                            <br />
                                            <asp:ListBox ID="lstBoxAssignedPractice" runat="server" Font-Names="Arial" Font-Size="9pt"
                                                SelectionMode="Multiple" Width="220px" Height="200px"></asp:ListBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Website:
                            <asp:RequiredFieldValidator ID="rfvWebsite" runat="server" ErrorMessage="Enter Website"
                                Text="*" ControlToValidate="txtWebsite" ValidationGroup="save"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtWebsite" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Upload Logo:
                        </td>
                        <td>
                            <input runat="server" id="btnUploadLogo" style="width: 155px;" value="Upload Logo"
                                disabled="disabled" onclick="javascript:uploadLogo();" clientidmode="Static"
                                type="button" />
                            <br />
                            <div style="color: #C00000; font-size: 9px; width: 230px;">
                                (logo image must be 70 by 25 px or smaller)
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Organization:
                            <asp:RequiredFieldValidator ID="rfvOrganization" runat="server" ErrorMessage="Enter Organization"
                                Text="*" ControlToValidate="txtOrganization" ValidationGroup="save"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtOrganization" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Address1:
                            <asp:RequiredFieldValidator ID="rfvAddress1" runat="server" ErrorMessage="Enter Address"
                                Text="*" ControlToValidate="txtAddress1" ValidationGroup="save"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtAddress1" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Address2:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtAddress2" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            City:
                            <asp:RequiredFieldValidator ID="rfvCity" runat="server" ErrorMessage="Enter City"
                                Text="*" ControlToValidate="txtCity" ValidationGroup="save"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCity" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            State:
                            <asp:RequiredFieldValidator ID="rfvState" runat="server" ErrorMessage="Enter State"
                                Text="*" ControlToValidate="txtState" ValidationGroup="save"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtState" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Zip:
                            <asp:RequiredFieldValidator ID="rfvZip" runat="server" ErrorMessage="Enter Zip" Text="*"
                                ControlToValidate="txtZip" ValidationGroup="save"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtZip" Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="margin-left: auto; margin-right: auto;" colspan="4" align="center">
                            <asp:Button runat="server" ID="btnSave" Text="Save" Width="80px" CausesValidation="true"
                                ValidationGroup="save" OnClick="btnSave_Click" />
                            &nbsp
                            <asp:Button runat="server" ID="btnCancel" Text="Discard Changes" Width="130px" OnClick="btnCancel_Click" />
                            <asp:HiddenField ID="hdnUserId" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <ucl:LoadingPanel ID="LoadingPanel1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--#######################################################  Pop Up Change Password #################################################################--%>
    <div id="lightbox-popup" class="password-popup" style="border: 1px solid #5880B3;">
        <div id="popupHeaderText">
            <table width="100%">
                <tr>
                    <td width="65%" align="right" valign="middle">
                        Change Password
                    </td>
                    <td width="35%" align="right" valign="middle">
                        <a class="close-popup">close[x]</a>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Panel ID="Panel1" runat="server">
            <ucdm:DisplayMessage ID="changePwdMessage" runat="server" DisplayMessageWidth="400"
                ShowCloseButton="true" validationGroup="changePassword"></ucdm:DisplayMessage>
        </asp:Panel>
        <table>
            <tr>
                <td>
                    <p>
                        New password:*
                    </p>
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
                        ErrorMessage="Passwords do not match" ValidationGroup="changePassword"></asp:CompareValidator>
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
                    <input type="button" id="btncance" value="Cancel" class="close-popup" />
                </td>
            </tr>
        </table>
    </div>
    <%-- #####################################################  Pop Up Upload Logo #####################################################--%>
    <div id="lightbox-popup" class="uploadbox-popup" style="border: 1px solid #5880B3;">
        <div id="popupHeaderText">
            <table width="100%">
                <tr>
                    <td width="65%" align="right" valign="middle">
                        Upload File
                    </td>
                    <td width="35%" align="right" valign="middle">
                        <a class="close-popup">close[x]</a>
                    </td>
                </tr>
            </table>
        </div>
        <table width="100%">
            <tr>
                <td>
                    <asp:Panel ID="pnlIFrame" runat="server">
                        <iframe scrolling="no" id="LogoUploaderPopup" style="height: auto; min-height: 150px;
                            width: 100%; overflow: hidden;" src="../LogoUploader.aspx" frameborder="0"></iframe>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    <!-- /lightbox-panel -->
    <div id="lightbox">
    </div>
</asp:Content>
