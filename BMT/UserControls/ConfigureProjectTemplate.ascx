<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfigureProjectTemplate.ascx.cs"
    Inherits="ConfigureProjectTemplate" ViewStateMode="Enabled" %>
<link rel="Stylesheet" type="text/css" href="../Themes/popup.css" />
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/configureProjects.js") %>"></script>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<%@ Register Src="~/UserControls/LoadingPanel.ascx" TagName="LoadingPanel" TagPrefix="ucl" %>
<script language="javascript" type="text/javascript">
    function onCheckChanged() {
        $("#btnCheckClick")[0].click();
    }
</script>
<asp:UpdatePanel ID="upnlConfigureProjects" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div style="font-family: Segoe UI; font-size: 16px; font-weight: bold; margin-bottom: 5px">
            My Projects</div>
        <table width="730" border="0">
            <tr>
                <td>
                    <ucdm:DisplayMessage ID="msgConfigureProject" runat="server" DisplayMessageWidth="715"
                        ClientIDMode="Static" />
                </td>
            </tr>
            <tr>
                <td class="child-title">
                    Available Projects
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gvPracTemp" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#5880B3"
                        HeaderStyle-ForeColor="#FFFFFF" BorderColor="#CCCCCC" HeaderStyle-HorizontalAlign="Center"
                        PageSize="8" DataKeyNames="ProjectId" AllowPaging="true" OnPageIndexChanging="gvPracTemp_PageIndexChanging"
                        HeaderStyle-CssClass="header-border">
                        <EmptyDataRowStyle BackColor="LightBlue" BorderColor="#CCCCCC" ForeColor="Red" Width="600" />
                        <EmptyDataTemplate>
                            <table width="715" style="background-color: #5880B3; color: #FFFFFF;">
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
                            <asp:TemplateField ShowHeader="true" HeaderText="S.No." ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="70" ItemStyle-CssClass="grid-border">
                                <ItemStyle HorizontalAlign="Center" BorderColor="#CCCCCC" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkProjects" runat="server" OnClick="onCheckChanged()" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="Project Name" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="160" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" />
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-border"
                                HeaderStyle-Width="215" HeaderText="Description">
                                <ItemStyle HorizontalAlign="Center" BorderColor="#CCCCCC" />
                                <ItemTemplate>
                                    <%# Eval("Description").ToString().Length > 25 ? (Eval("Description") as string).Substring(0, 25) + "<a href='#' class='ttm'>(More)<span class='tooltip'><span class='top'></span><span class='middle'> &nbsp;&nbsp;" + Eval("Description") + "</span><span class='bottom'></span></span></a>" : Eval("Description")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="LastUpdatedDate" HeaderText="Last Updated" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="142" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" />
                            <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="115" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnSelectProject" Text="Save to My Selected Projects" OnClick="btnSelectProject_Click"
                        runat="server" ClientIDMode="Static" />
                </td>
            </tr>
            <tr>
                <td class="child-title">
                    My Selected Projects
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gvConfigureProjectTemplate" runat="server" AutoGenerateColumns="false"
                        HeaderStyle-BackColor="#5880B3" HeaderStyle-CssClass="header-border" HeaderStyle-ForeColor="#FFFFFF"
                        BorderColor="#CCCCCC" HeaderStyle-HorizontalAlign="Center" DataKeyNames="ProjectId"
                        OnRowCommand="gvConfigureProjectTemplate_RowCommand">
                        <EmptyDataRowStyle BorderColor="#CCCCCC" BackColor="LightBlue" ForeColor="Red" Width="600" />
                        <EmptyDataTemplate>
                            <table width="715" style="background-color: #5880B3; color: #FFFFFF;">
                                <tr>
                                    <td align="center">
                                        No Record Found.
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <AlternatingRowStyle BackColor="#F2F2F2" />
                        <Columns>
                            <asp:BoundField DataField="ProjectId" HeaderText="Project Id" HeaderStyle-CssClass="practiceSave"
                                ItemStyle-CssClass="practiceSave" ItemStyle-BorderColor="#CCCCCC" />
                            <asp:BoundField DataField="Name" HeaderText="Project Name" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="245" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" />
                            <asp:BoundField DataField="LastUpdatedDate" HeaderText="Last Updated" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="175" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" />
                            <asp:TemplateField ShowHeader="true" HeaderText="Show in My Projects Tab" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="205" ItemStyle-CssClass="grid-border">
                                <ItemStyle HorizontalAlign="Center" BorderColor="#CCCCCC" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkMyProjects" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="true" HeaderText="Corporate" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="205" HeaderStyle-CssClass="practiceSave" ItemStyle-CssClass="practiceSave">
                                <ItemStyle HorizontalAlign="Center" BorderColor="#CCCCCC" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkCorporate" runat="server" OnClick='<%# "OnCheckCorporate(\"" + Eval("ProjectId").ToString() + "\");"%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="true" HeaderText="Move" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="80" ItemStyle-CssClass="grid-border">
                                <ItemStyle HorizontalAlign="Center" BorderColor="#CCCCCC" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnUp" runat="server" CommandName="Up" ImageUrl="~/Themes/Images/arrow-up.png"
                                        ToolTip="Move Up" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                    <asp:ImageButton ID="btnDown" runat="server" CommandName="Down" ImageUrl="~/Themes/Images/arrow-down.png"
                                        ToolTip="Move Down" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
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
        <table align="center">
            <tr>
                <td>
                    <asp:Button ID="btnSaveProject" runat="server" ClientIDMode="Static" Text="Save"
                        OnClick="btnSaveProject_Click" />
                </td>
                <td>
                    <asp:Button ID="btnCancelProject" runat="server" ClientIDMode="Static" Text="Cancel"
                        OnClick="btnCancelProject_Click" OnClientClick="return confirm('Are you sure want to cancel the changes?');" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnRefreshPage" runat="server" ClientIDMode="Static" Text="RefreshPage"
                        OnClick="btnRefreshPage_Click" CssClass="hideDisplay" />
                    <asp:Button ID="btnCheckClick" runat="server" ClientIDMode="Static" Text="RefreshPage"
                        OnClick="chkMyProjects_OnCheckedChanged" CssClass="hideDisplay" />
                    <asp:Button ID="btnGetSiteInfo" runat="server" ClientIDMode="Static" Text="RefreshPage"
                        OnClick="getPracticeSite_Click" CssClass="hideDisplay" />
                    <asp:HiddenField ID="hiddenTemplateId" runat="server" ClientIDMode="Static" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<%-- ############################ CORPORATE SUBMISSION POPUP START HERE #####################################################--%>
<div id="lightbox-popup" class="Corporate-template-popup" style="border: 1px solid #5880B3;
    width: 300px;">
    <div id="popupHeaderText">
        <table width="100%">
            <tr>
                <td width="80%" align="left" valign="middle">
                    <span class="rspan">Corporate Submission</span>
                </td>
                <td width="20%" align="right" valign="middle">
                    <a id="close-Corporate-popup">close[x]</a>
                </td>
            </tr>
        </table>
    </div>
    <asp:UpdatePanel ID="upnlCorporate" runat="server" ClientIDMode="Static" UpdateMode="Conditional">
        <ContentTemplate>
            <table id="Table1" width="100%" runat="server" clientidmode="Static">
                <tr>
                    <td>
                        <ucdm:DisplayMessage ID="msgCorporateProject" runat="server" DisplayMessageWidth="295"
                            ClientIDMode="Static" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="corporateType" runat="server" style="margin-bottom: 10px; margin-top: 10px;
                            height: 20px; width: 270px;">
                            <div class="bodytxt-fieldlabel06">
                                Corporate Type:</div>
                            <div style="float: left">
                                <asp:RadioButton ID="corporateTypeYes" runat="server" Text="Yes" GroupName="corporate"
                                    onclick="OnCorporateTypeChanged(this);" Checked="true" />&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:RadioButton ID="corporateTypeNo" runat="server" Text="No" GroupName="corporate"
                                    onclick="OnCorporateTypeChanged(this);" />
                            </div>
                        </div>
                        <div id="siteSelector" runat="server" style="margin-bottom: 10px; height: 20px; width: 270px;">
                            <div class="bodytxt-fieldlabel06">
                                Select Site:*</div>
                            <div style="float: left">
                                <asp:DropDownList ID="ddlPracSiteName" runat="server" Width="150px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvDdlSiteName" runat="server" Text="*" Display="Static"
                                    ControlToValidate="ddlPracSiteName" ValidationGroup="upnlPractice" InitialValue="0"
                                    Enabled="false"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div id="CorpMessage" runat="server" class="CorpMessage">
                            <asp:Label Width="275px" ID="lbCorpSelectionMessage" runat="server" Text="(Go to General tab and select Corporate Elements.)"></asp:Label>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="padding-left: 92px;">
                        <asp:Button ID="btnSaveCorpSiteMORe" runat="server" Text="Save" CausesValidation="false"
                            ClientIDMode="Static" OnClientClick="CheckCorporateTemplate(); return false;" />
                        <a id="close-Corporate-popup" href="#" style="text-decoration: none; font-size: 16px;
                            color: White;">
                            <asp:Button ID="btnCancelPopup" runat="server" Text="Cancel" CausesValidation="false"
                                OnClientClick="return false;" ClientIDMode="Static" /></a>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<!-- /lightbox-panel -->
<div class="lightbox">
</div>
<!-- /lightbox -->
<%-- ############################ CORPORATE SUBMISSION POPUP END HERE #####################################################--%>
<%-- ############################ CONFIRMATION POPUP START HERE #####################################################--%>
<div id="lightbox-popup" class="confirmationTemplate-popup" style="border: 1px solid #5880B3;
    width: 400px;">
    <div id="popupHeaderText">
        <table width="100%">
            <tr>
                <td width="80%" align="left" valign="middle">
                    <span class="rspan">Remove Document</span>
                </td>
                <td width="20%" align="right" valign="middle">
                    <a id="close-confirmationTemplate">close[x]</a>
                </td>
            </tr>
        </table>
    </div>
    <table width="100%" id="WarningPopUpMORe" runat="server" clientidmode="Static">
        <tr>
            <td width="20%">
                <img src="../Themes/Images/caution.png" alt="caution" />
            </td>
            <td width="80%">
                <asp:Label runat="server" ClientIDMode="Static" ID="alertNotificationMORe"></asp:Label>
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
                    ID="warningMORe"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr align="center">
            <td colspan="2">
                <a id="close-confirmationTemplate">
                    <asp:Button ID="btnChangeCorpSiteMORe" runat="server" Text="OK" CausesValidation="false"
                        ClientIDMode="Static" OnClientClick="changeCorporateSiteMORe(); return false;"
                        CssClass="hideDisplay" /></a> <a id="close-confirmationTemplate">
                            <asp:Button ID="btnCopyCorpElementMORe" runat="server" Text="OK" CausesValidation="false"
                                ClientIDMode="Static" OnClientClick="TemplateCopyToNonCorporateSite(); return false;"
                                CssClass="hideDisplay" /></a> <a id="close-confirmationTemplate">
                                    <asp:Button ID="btnCancelNotificationPopupMORe" runat="server" Text="Cancel" CausesValidation="false"
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