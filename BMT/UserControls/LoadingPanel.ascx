<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoadingPanel.ascx.cs"
    Inherits="BMT.UserControls.LoadingPanel" %>
<link rel="Stylesheet" type="text/css" href="../Themes/loading-panel.css" />
<asp:Panel ID="panelload" runat="server" HorizontalAlign="Center">
    <asp:HiddenField ID="lblMessage" runat="server" Value="" />
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div id="reportLightboxId" class="reportLightbox">
            </div>
            <div class="UpdateProgressContent">
                Please wait<br />
                <script type="text/javascript">
                    var message = document.getElementById('<%=lblMessage.ClientID %>').value;
                    document.write(message);                    
                </script>
                <br />
                <img id="ImageLoading" src="~/Themes/Images/loading.gif" runat="server" alt="" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Panel>
