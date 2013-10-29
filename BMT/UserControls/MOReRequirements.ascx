<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MOReRequirements.ascx.cs"
    Inherits="MOReRequirements" %>
<%@ Register Src="~/UserControls/LoadingPanel.ascx" TagName="LoadingPanel" TagPrefix="ucl" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<%@ Reference Control="~/UserControls/PCMH.ascx" %>
<link rel="Stylesheet" type="text/css" href="../Themes/popup.css" />
<link rel="Stylesheet" type="text/css" href="../Themes/PCMH.css" />
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jquery.numeric.js") %>"></script>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/MOReRequirements.js") %>"></script>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/MORe.js") %>"></script>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jquery-ui-1.8.16.custom.min.js") %>"></script>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/MOReSummary.js") %>"></script>
<div class="body-container-right" style="min-height: 300px">
    <asp:UpdatePanel ID="upnlMOReRequirements" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <asp:Panel ID="pnlmessage" runat="server" Style="margin-left: 10px;">
                <ucdm:DisplayMessage ID="message" runat="server" DisplayMessageWidth="700" ValidationSummaryHeaderText="<div class='error-SummaryHeader'>Type warning message here</div>">
                </ucdm:DisplayMessage>
            </asp:Panel>

            <!-- the tabs -->
            
            <asp:Panel ID="pnlTabs" runat="server" CssClass="panelTabs">
            <div id="navigator"> 
               <a href="#" id="prev" title="Move Backward" ><div id="prevButton" class="prevDim"></div></a>
               <a href="#" id="next" title="Move Forward"><div id="nextButton" class="next"></div></a> 
            </div> 
            </asp:Panel>
            
            <asp:PlaceHolder runat="server" ID="PlaceHolderControls" ClientIDMode="Static" >
            </asp:PlaceHolder>

            <asp:HiddenField ID="hiddenClickTab" runat="server" ClientIDMode="Static" Value="0" />
            <asp:HiddenField ID="hiddenOldClickTab" runat="server" ClientIDMode="Static" Value="0" />
            <asp:HiddenField ID="hdnIsConsultant" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnRequiredDocsEnabled" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnMarkAsCompleteEnabled" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnSummaryElementId" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnTabDiv" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnTotalControls" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hiddenTemplateId" runat="server" ClientIDMode="Static" />
            <ucl:LoadingPanel ID="loading" runat="server" />
        </ContentTemplate>
        <%--<Triggers>
            <asp:AsyncPostBackTrigger ControlID="hiddenClickTab" />
        </Triggers>--%>
    </asp:UpdatePanel>
</div>
