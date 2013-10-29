<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs"
    Inherits="BMT.Account.ForgotPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
<%@ Register Src="~/UserControls/LoadingPanel.ascx" TagName="LoadingPanel" TagPrefix="ucl" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="MainHead" runat="server">
    <title>BizMed Toolbox | Forgot Password</title>
    <!-- Main css start here -->
    <link href="../Themes/style.css" rel="stylesheet" type="text/css" />
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
                                        <div class="menu">
                                            <div class="menu-bg" style="width: 120px;">
                                                <ul>
                                                    <li>
                                                        <asp:LinkButton ID="lbTopLogin" runat="server" Text="Login" PostBackUrl="~/Account/Login.aspx"></asp:LinkButton></li>
                                                    <%-- <li>
                                                        <asp:LinkButton ID="lbTopSignUp" runat="server" Text="SignUp" OnClick="lbTopSignUp_Click"></asp:LinkButton></li>--%>
                                                </ul>
                                            </div>
                                        </div>
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
                                                        <ucdm:DisplayMessage ID="Message" runat="server" validationGroup="ForgotPassword"
                                                            DisplayMessageWidth="440" ShowCloseButton="false"></ucdm:DisplayMessage>
                                                    </asp:Panel>
                                                    <table width="400" border="0" align="center" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td class="main-form-heading">
                                                                <h1>
                                                                    Forgot your password?</h1>
                                                                <br />
                                                                Please enter your email address to receive credentials for your account.
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td height="10">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Email address:
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="340">
                                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="text-field" Width="340px" ValidationGroup="ForgotPassword"
                                                                    MaxLength="50"></asp:TextBox>
                                                            </td>
                                                            <td width="10" align="center">
                                                                <asp:RequiredFieldValidator ID="rfvtxtEmail" runat="server" ControlToValidate="txtEmail"
                                                                    ErrorMessage="Email address is required!" Text="*" Display="None" ValidationGroup="ForgotPassword"></asp:RequiredFieldValidator>
                                                                <asp:RegularExpressionValidator runat="server" ID="revtxtEmail" ControlToValidate="txtEmail"
                                                                    Display="None" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                                    ErrorMessage="Email address is invalid!" ValidationGroup="ForgotPassword"></asp:RegularExpressionValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Button ID="btnsubmit" runat="server" CssClass="login-but" Text="Submit" ValidationGroup="ForgotPassword"
                                                                    OnClick="btnSubmit_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div class="footer-version-text">
                                                                    <asp:Label ID="lblVer" runat="server"></asp:Label>
                                                                    </div>
                                                                <div class="footer-version-text">
                                                                   <asp:Label ID="lblsysteminfo" runat="server"></asp:Label>
                                                                    </div>
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
