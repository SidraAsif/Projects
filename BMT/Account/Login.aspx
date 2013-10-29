<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BMT.Account.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
<%@ Register Src="~/UserControls/LoadingPanel.ascx" TagName="LoadingPanel" TagPrefix="ucl" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="MainHead" runat="server">
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
                                            <div class="menu-bg" style="width: 240px;">
                                                <ul>
                                                    <li>
                                                        <asp:LinkButton ID="lbTopLogin" runat="server" Text="Login" OnClick="lbTopLogin_Click"></asp:LinkButton></li>
                                                    <li>
                                                        <asp:LinkButton ID="lbTopSignUp" runat="server" Text="SignUp" OnClick="lbTopSignUp_Click"></asp:LinkButton></li>
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
                                                <asp:UpdatePanel ID="upnlogin" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="pnlLoginForm" runat="server">
                                                            <td height="340" style="background-image: url('../Themes/Images/from-cnt.png');"
                                                                valign="top">
                                                                <asp:Panel ID="pnlLoginMessage" runat="server" Style="margin-left: 5px;">
                                                                    <ucdm:DisplayMessage ID="LoginMesssage" runat="server" validationGroup="Login" DisplayMessageWidth="440"
                                                                        ShowCloseButton="false"></ucdm:DisplayMessage>
                                                                </asp:Panel>
                                                                <table width="400" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td class="main-form-heading">
                                                                            <h1>
                                                                                Log in</h1>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="10">
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            User Name:
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="340">
                                                                            <asp:TextBox ID="txtLoginUserName" runat="server" CssClass="text-field" Width="340px"
                                                                                ValidationGroup="Login" MaxLength="50"></asp:TextBox>
                                                                        </td>
                                                                        <td width="10" align="center">
                                                                            <asp:RequiredFieldValidator ID="rfvtxtLoginUserName" runat="server" ControlToValidate="txtLoginUserName"
                                                                                ErrorMessage="Username is required!" Display="None" ValidationGroup="Login"></asp:RequiredFieldValidator>
                                                                            <asp:RegularExpressionValidator runat="server" ID="revtxtLoginUserName" ControlToValidate="txtLoginUserName"
                                                                                Display="None" ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,50}$"
                                                                                ErrorMessage="Username is invalid!" ValidationGroup="Login"></asp:RegularExpressionValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="12">
                                                                            <img src="../Themes/Images/sp.png" width="1" height="1" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            Password:
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="340">
                                                                            <asp:TextBox ID="txtLoginPassword" runat="server" TextMode="Password" CssClass="text-field"
                                                                                Width="340px" ValidationGroup="Login" MaxLength="100"></asp:TextBox>
                                                                        </td>
                                                                        <td width="10" align="center">
                                                                            <asp:RequiredFieldValidator ID="rfvtxtLoginPassword" runat="server" ControlToValidate="txtLoginPassword"
                                                                                ErrorMessage="Password is required!" Display="None" ValidationGroup="Login"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkBoxRememberMe" runat="server" Text="Remember User Name" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Button ID="btnLogin" runat="server" CssClass="login-but" Text="Login" OnClick="btnLogin_Click"
                                                                                ValidationGroup="Login" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <div style="margin: 17px 0px 0px 20px">
                                                                                <asp:LinkButton ID="lbForgotPassword" runat="server" Text="Forgot your password?"
                                                                                    PostBackUrl="~/Account/ForgotPassword.aspx"></asp:LinkButton></div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="15">
                                                                            <img src="../Themes/Images/sp.png" width="1" height="1" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr bgcolor="#CCCCCC">
                                                                        <td height="4">
                                                                            <img src="../Themes/Images/sp.png" width="1" height="1" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="400" border="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        Don't have an account?
                                                                                    </td>
                                                                                    <td align="right">
                                                                                        <asp:LinkButton ID="lblsignUp" runat="server" Text="Sign up for a Free account" OnClick="lbTopSignUp_Click"></asp:LinkButton>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        Need help? Contact us at
                                                                                    </td>
                                                                                    <td align="right">
                                                                                        <asp:HyperLink ID="mailToSupport" runat="server">
                                                                                        </asp:HyperLink>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="10">
                                                                            <img src="../Themes/Images/sp.png" width="1" height="1" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <div class="footer-version-text">
                                                                                <%--Version 1.0 Beta--%>
                                                                                <asp:Label ID="lblver" runat="server"></asp:Label>
                                                                            </div>
                                                                            <div class="footer-version-text">
                                                                                <asp:Label ID="lblsysteminfo" runat="server"></asp:Label></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <asp:UpdatePanel ID="upnlSignUp" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="pnlSignUpForm" runat="server">
                                                            <td height="350" style="background-image: url('../Themes/Images/from-cnt.png');"
                                                                valign="top">
                                                                <asp:Panel ID="pnlSignUpMessage" runat="server" Style="margin-left: 10px;">
                                                                    <ucdm:DisplayMessage ID="SignUpMessage" runat="server" validationGroup="SignUp" DisplayMessageWidth="430"
                                                                        ShowCloseButton="false"></ucdm:DisplayMessage>
                                                                </asp:Panel>
                                                                <table width="400" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td class="main-form-heading">
                                                                            <h1>
                                                                                Sign Up</h1>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="400" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        First Name
                                                                                    </td>
                                                                                    <td width="32">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        Last Name
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="text-field" Width="180px"
                                                                                            MaxLength="25"></asp:TextBox>
                                                                                    </td>
                                                                                    <td width="32" align="center">
                                                                                        <asp:RequiredFieldValidator ID="rfvtxtFirstName" runat="server" ControlToValidate="txtFirstName"
                                                                                            Text="*" Display="Static" ValidationGroup="SignUp"></asp:RequiredFieldValidator>
                                                                                        <asp:RegularExpressionValidator runat="server" ID="revtxtFirstName" ControlToValidate="txtFirstName"
                                                                                            Display="None" ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,25}$"
                                                                                            ErrorMessage="First Name is invalid!" ValidationGroup="SignUp"></asp:RegularExpressionValidator>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtLastName" runat="server" CssClass="text-field" Width="180px"
                                                                                            MaxLength="25"></asp:TextBox>
                                                                                    </td>
                                                                                    <td width="32" align="center">
                                                                                        <asp:RequiredFieldValidator ID="rfvtxtLastName" runat="server" ControlToValidate="txtLastName"
                                                                                            Text="*" Display="Static" ValidationGroup="SignUp"></asp:RequiredFieldValidator>
                                                                                        <asp:RegularExpressionValidator runat="server" ID="revtxtLastName" ControlToValidate="txtLastName"
                                                                                            Display="None" ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,25}$"
                                                                                            ErrorMessage="Last Name is invalid!" ValidationGroup="SignUp"></asp:RegularExpressionValidator>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="10">
                                                                                        <img src="../Themes/Images/sp.png" width="1" height="1" />
                                                                                    </td>
                                                                                    <td width="32">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td height="10">
                                                                                        <img src="../Themes/Images/sp.png" width="1" height="1" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        Email
                                                                                    </td>
                                                                                    <td width="32">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        Confirm Email
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="text-field" Width="180px" MaxLength="255"></asp:TextBox>
                                                                                    </td>
                                                                                    <td width="32" align="center">
                                                                                        <asp:RequiredFieldValidator ID="rfvtxtEmail" runat="server" ControlToValidate="txtEmail"
                                                                                            Text="*" Display="Static" ValidationGroup="SignUp"></asp:RequiredFieldValidator>
                                                                                        <asp:RegularExpressionValidator runat="server" ID="revtxtEmail" ControlToValidate="txtEmail"
                                                                                            Display="None" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                                                            ErrorMessage="Email Address is invalid!" ValidationGroup="SignUp"></asp:RegularExpressionValidator>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtConfirmEmail" runat="server" CssClass="text-field" Width="180px"
                                                                                            MaxLength="255"></asp:TextBox>
                                                                                    </td>
                                                                                    <td width="32" align="center">
                                                                                        <asp:RequiredFieldValidator ID="rfvtxtConfirmEmail" runat="server" ControlToValidate="txtConfirmEmail"
                                                                                            Text="*" Display="Static" ValidationGroup="SignUp"></asp:RequiredFieldValidator>
                                                                                        <asp:RegularExpressionValidator runat="server" ID="revtxtConfirmEmail" ControlToValidate="txtConfirmEmail"
                                                                                            Display="None" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                                                            ErrorMessage="Email Address is invalid!" ValidationGroup="SignUp"></asp:RegularExpressionValidator>
                                                                                        <asp:CompareValidator ControlToValidate="txtEmail" ControlToCompare="txtConfirmEmail"
                                                                                            runat="server" ErrorMessage="Emails do not match" Display="None" ValidationGroup="SignUp"></asp:CompareValidator>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="10">
                                                                                        <img src="../Themes/Images/sp.png" width="1" height="1" />
                                                                                    </td>
                                                                                    <td width="32">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td height="10">
                                                                                        <img src="../Themes/Images/sp.png" width="1" height="1" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        Practice Size
                                                                                    </td>
                                                                                    <td width="32">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        Phone
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="ddlPracticeSize" runat="server" Width="180px">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td width="32" align="center">
                                                                                        <asp:RequiredFieldValidator ID="rfvddlPracticeSize" runat="server" ControlToValidate="ddlPracticeSize"
                                                                                            Text="*" Display="Static" ValidationGroup="SignUp" InitialValue="--Select--"></asp:RequiredFieldValidator>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtPhone" runat="server" CssClass="text-field" Width="180px" MaxLength="30"></asp:TextBox>
                                                                                    </td>
                                                                                    <td width="32" align="center">
                                                                                        <asp:RequiredFieldValidator ID="rfvtxtPhone" runat="server" ControlToValidate="txtPhone"
                                                                                            Text="*" Display="Static" ValidationGroup="SignUp"></asp:RequiredFieldValidator>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="10">
                                                                                        <img src="../Themes/Images/sp.png" width="1" height="1" />
                                                                                    </td>
                                                                                    <td width="32">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td height="10">
                                                                                        <img src="../Themes/Images/sp.png" width="1" height="1" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        Speciality
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="ddlSpeciality" runat="server" Width="180px">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td width="32" align="center">
                                                                                        <asp:RequiredFieldValidator ID="rfvddlSpeciality" runat="server" ControlToValidate="ddlSpeciality"
                                                                                            Text="*" Display="Static" ValidationGroup="SignUp" InitialValue="--Select--"></asp:RequiredFieldValidator>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="8">
                                                                            <img src="../Themes/Images/sp.png" width="1" height="1" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkBoxAgreement" runat="server" CausesValidation="false" AutoPostBack="true"
                                                                                OnCheckedChanged="chkBoxAgreement_CheckedChanged" />
                                                                            I acknowledge that I have read and agree to the terms of the <a href="../StDocs/General/License_Agreement.pdf"
                                                                                target="_blank">License Agreement</a> and <a href="../StDocs/General/Privacy_Policy.pdf"
                                                                                    target="_blank">Privacy Policy</a>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Button ID="btnSubmit" runat="server" Text="Sign Up" CssClass="login-but" ValidationGroup="SignUp"
                                                                                OnClick="btnSubmit_Click" Enabled="false" ClientIDMode="Static" UseSubmitBehavior="false" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="2">
                                                                            <img src="../Themes/Images/sp.png" width="1" height="1" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr bgcolor="#CCCCCC">
                                                                        <td height="4">
                                                                            <img src="../Themes/Images/sp.png" width="1" height="1" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0">
                                                                                <tr>
                                                                                    <td width="200">
                                                                                        Already have an account?
                                                                                    </td>
                                                                                    <td width="200" align="right">
                                                                                        <asp:LinkButton ID="lbLogin" runat="server" Text="Log in Now" OnClick="lbTopLogin_Click"></asp:LinkButton>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td width="200">
                                                                                        Need help? Contact us at
                                                                                    </td>
                                                                                    <td width="200" align="right">
                                                                                        <asp:HyperLink ID="mailTo" runat="server">
                                                                                        </asp:HyperLink>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="8">
                                                                            <img src="../Themes/Images/sp.png" width="1" height="1" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <div class="footer-version-text">
                                                                                <asp:Label ID="lblVerSignup" runat="server"></asp:Label>
                                                                            </div>
                                                                            <div class="footer-version-text">
                                                                                <asp:Label ID="lblsystemsignup" runat="server"></asp:Label></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
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
