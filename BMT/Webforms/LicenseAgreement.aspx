
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LicenseAgreement.aspx.cs" Inherits="BMT.Webforms.LicenseAgreement" %>




<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
<%@ Register Src="~/UserControls/LoadingPanel.ascx" TagName="LoadingPanel" TagPrefix="ucl" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>


<html xmlns="http://www.w3.org/1999/xhtml">


<head id="MainHead" runat="server">

<link href="../Themes/style.css" rel="stylesheet" type="text/css" />

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
                    </table>
                       
                    </ContentTemplate>
                    </asp:UpdatePanel>
                      
                       </div>
                       

                      
<p>LicenseAgrament</p>

            <div class="footer-container">
              <div class="footer-text">

              <span>
              <div style="vertical-align:bottom"; ></div>
                                    </span>This site developed by <a href="http://www.datadynamics-inc.com" target="_blank">
              
              <img src="../Themes/Images/dd.jpg" alt="DataDynamics" height="22px" width="88px"/>
                                </a>
              </div>
                    </div>

                    
</form>
</body>

</html>

