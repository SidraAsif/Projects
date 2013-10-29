<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs"
    Inherits="BMT.Account.ChangePassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
<%@ Register Src="~/UserControls/LoadingPanel.ascx" TagName="LoadingPanel" TagPrefix="ucl" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="MainHead" runat="server">
    <title>BizMed Toolbox | Change Password</title>
    <!-- Main css start here -->
    <link href="../Themes/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .changepassword td
        {
            height: 25px;
        }
        .changepassword .seperator
        {
            border-bottom: 3px solid #ccc;
        }
    </style>
    <!-- Main css close here -->
    <!-- Main JScript Start here -->
    <!-- Main JScript close here -->
</head>
<body>
    <form id="mainForm" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server" EnablePartialRendering="true"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <div>
        <!-- Login Top Header Wrapper start here -->
        <div class="login-top-header-wrapper">
            <div class="login-top-header-container">
                <div class="ehr-logo">
                    <img src="../Themes/Images/ehr-logo.png" alt="BIZMED Toolbox" />
                </div>
            </div>
        </div>
        <!-- Login Top Header Wrapper close here -->
        <!-- Login Form Wrapper start here -->
        <asp:UpdatePanel ID="upnlMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <!--  bizmed-logo -->
                            <div class="bizmed-logo-container">
                                <div class="bizmed-logo">
                                    <img src="../Themes/Images/logo-bizmed.png" /></div>
                            </div>
                            <!--  bizmed-logo -->
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <table width="450" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <!-- menu start here -->
                                        <!-- menu close here -->
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="450" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <img src="../Themes/Images/from-top.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="150" style="background-image: url('../Themes/Images/from-cnt.png');"
                                                    valign="top">
                                                    <asp:Panel ID="pnlMessage" runat="server" Style="margin-left: 5px;">
                                                        <ucdm:DisplayMessage ID="Message" runat="server" DisplayMessageWidth="440" ShowCloseButton="false">
                                                        </ucdm:DisplayMessage>
                                                    </asp:Panel>
                                                    <table width="440">
                                                        <tr>
                                                            <td width="430" align="right">
                                                                <asp:LinkButton ID="lbLogin" runat="server" PostBackUrl="~/Account/Login.aspx" Text="To login: Click here"
                                                                    CausesValidation="false"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table width="400" border="0" align="center" cellpadding="0" cellspacing="0" class="changepassword">
                                                        <tr>
                                                            <td class="main-form-heading">
                                                                <h1>
                                                                    Welcome,
                                                                    <asp:Label ID="lblName" runat="server"></asp:Label></h1>
                                                                <br />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="font-weight: bold;">
                                                                Your Username is:
                                                                <asp:Label ID="lblUsername" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td height="10">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Current password:
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="245">
                                                                <asp:TextBox ID="txtCurrentPassword" runat="server" CssClass="text-field" Width="240px"
                                                                    MaxLength="50" TextMode="Password"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvtxtCurrentPassword" runat="server" ControlToValidate="txtCurrentPassword"
                                                                    Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                New password:
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="245">
                                                                <asp:TextBox ID="txtNewPassword" runat="server" CssClass="text-field" Width="240px"
                                                                    MaxLength="50" TextMode="Password"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvtxtNewPassword" runat="server" ControlToValidate="txtNewPassword"
                                                                    Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Confirm new password:
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="245">
                                                                <asp:TextBox ID="txtConfirmNewPassword" runat="server" CssClass="text-field" Width="240px"
                                                                    MaxLength="50" TextMode="Password"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvtxtConfirmNewPassword" runat="server" ControlToValidate="txtConfirmNewPassword"
                                                                    Text="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:CompareValidator ID="cvPassword" runat="server" ValueToCompare="txtConfirmNewPassword"
                                                                    ControlToValidate="txtConfirmNewPassword" ControlToCompare="txtNewPassword" Display="None"
                                                                    ErrorMessage="The Password you typed do not match"></asp:CompareValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Button ID="btnNext" runat="server" Text="Change password" OnClick="btnNext_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="seperator">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <table>
                                                                    <tr>
                                                                        <td width="200" style="font-weight: bold;">
                                                                            Need help? Contact us at
                                                                        </td>
                                                                        <td width="200" align="right">
                                                                            <asp:HyperLink ID="mailToSupport" runat="server">
                                                                                    </asp:HyperLink>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div class="footer-version-text">
                                                                    <asp:Label ID="lblVer" runat="server"></asp:Label>
                                                                </div>
                                                                <div class="footer-version-text">
                                                                    <asp:Label ID="lblsysteminfo" runat="server"></asp:Label></div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="../Themes/Images/from-botm.png" />
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
                            &nbsp;
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <!-- Footer container start here -->
                            <!-- Footer container close here -->
                        </td>
                    </tr>
                </table>
                <ucl:LoadingPanel ID="MainLoadingPanel" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <p style="clear: both">
        </p>
        <!-- Login Form Wrapper close here -->
    </div>
    </form>
</body>
</html>
