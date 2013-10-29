<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScoringRules.ascx.cs" Inherits="ScoringRules" ViewStateMode="Enabled" %>

<link rel="Stylesheet" type="text/css" href="../Themes/jquery-ui-1.8.12.custom.css" />
<link rel="Stylesheet" type="text/css" href="../Themes/NCQA.css" />
<link rel="Stylesheet" type="text/css" href="../Themes/popup.css" />
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<%@ Register Src="~/UserControls/LoadingPanel.ascx" TagName="LoadingPanel" TagPrefix="ucl" %>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/settings.js") %>"></script>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/ScoringRules.js") %>"></script>

<asp:UpdatePanel ID="upnlScoringRules" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
            
            <asp:Label ID="lblEditScore" Text="Edit Scoring Rules" runat="server" CssClass="edit-score-title"></asp:Label> 
            <br />   
            <br />
            <asp:Label ID="templateTitle" runat="server" CssClass="template-title"></asp:Label>  
            <br />
            <br />        
            <%--<h2 style="color: #C10000; font-weight: bold; vertical-align:top" id="templateTitles" runat="server">                
            </h2>--%>
           
    <div style="width: 730">               
            
            <ucdm:DisplayMessage ID="scoringRulesMessage" runat="server" DisplayMessageWidth="719" ShowCloseButton="false"></ucdm:DisplayMessage> 
            <asp:Panel ID="pnlNCQASummary" runat="server">
            </asp:Panel>  
            <br />
            <table width="100%">
            <tr>
            <td align="center">
            
            <asp:Button ID="btnsaveScoringRules" runat="server" 
                    Text="Save and Return to My Templates" OnClick="btnsaveScoringRules_Click" ValidationGroup="btnsaveScoringRules" />
            <asp:Button ID="btnDiscardChanges" runat="server" Text="Discard Changes" OnClientClick="return confirm('Are you sure want to discard the changes?');" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClientClick="SettingsFormSection('#createMORe');returnBackTemplate()" />
            
            </td>
            </tr>
            </table>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>




