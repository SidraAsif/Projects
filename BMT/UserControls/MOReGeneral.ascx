<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MOReGeneral.ascx.cs" Inherits="MOReGeneral" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<link rel="Stylesheet" type="text/css" href="../Themes/style.css" />
<link rel="Stylesheet" type="text/css" href="../Themes/PCMH.css" />
<link rel="Stylesheet" type="text/css" href="../Themes/popup.css" />
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jQuery-1.7.1.min.js") %>"></script>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/general.js") %>"></script>
<br />
<asp:Panel ID="pnlmessage" runat="server" Style="margin-left: 10px;">
    <ucdm:DisplayMessage ID="message" runat="server" DisplayMessageWidth="700" ValidationSummaryHeaderText="<div class='error-SummaryHeader'>Required information is missing.</div>">
    </ucdm:DisplayMessage>
</asp:Panel>
<asp:HiddenField ID="hiddenProjectId" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hiddenProjectUsageId" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hiddenPracticeId" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hiddenPracticeName" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hiddenSiteId" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hiddenSiteName" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hiddentemplateId" runat="server" ClientIDMode="Static" />
<div class="siteInfo">
    <asp:Label ID="lblSiteInfo" runat="server" CssClass="project-title" Text=""></asp:Label>
</div>
<div id="lightbox-popup" class="MOReCorporateElement-popup" style="border: 1px solid #5880B3;
    width: 600px; left: 500px; top: 50px;">
    <div id="popupHeaderText">
        <table width="100%">
            <tr>
                <td width="80%" align="left" valign="middle">
                    <span class="rspan">Corporate Element List</span>
                </td>
                <td width="20%" align="right" valign="middle">
                    <a id="close-MOReCorporateElement">close[x]</a>
                </td>
            </tr>
        </table>
    </div>
    <asp:Panel ID="pnlDocViewer" runat="server">
        <iframe id="IframeDocViewer" style="min-height: 540px; overflow: hidden; width: 100%;
            margin: auto;" frameborder="0" scrolling="auto"></iframe>
    </asp:Panel>
</div>
<table width="100%">
    <tr class="standard-title">
        <td>
            Site Information
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblsiteDescription" runat="server" Text="" CssClass="element-title"
                Style="margin-left: 27px;"></asp:Label>
            <br />
            <p style="margin-left: 30px; font-style: italic;">
                (To edit Site Information, please go to the Settings tab and select Site Administration
                from the menu on the left)</p>
        </td>
    </tr>
        <tr id="visibleOnlyForCorporateSite" runat="server">
        <td>
            <asp:HyperLink ID="hypFacilitator" Text="Corporate Submission" ClientIDMode="Static"
                runat="server" NavigateUrl="javascript:DisplayMOReCorporateElement();"></asp:HyperLink>
        </td>
    </tr>
    <tr class="standard-title">
        <td>
            Primary Care Providers at Site
        </td>
    </tr>
    <tr>
        <td>
            <asp:GridView ID="gvPrimaryCareProvider" runat="server" AllowPaging="true" AllowSorting="true"
                PageSize="8" AutoGenerateColumns="false" HeaderStyle-BackColor="#5880B3" HeaderStyle-ForeColor="#FFFFFF"
                BorderColor="White" HeaderStyle-HorizontalAlign="Center" OnPageIndexChanging="gvPrimaryCareProvider_OnPageIndexChanging">
                <EmptyDataRowStyle BackColor="LightBlue" ForeColor="Red" Width="600" />
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
                        HeaderStyle-Width="125" ItemStyle-BorderColor="White" />
                    <asp:BoundField DataField="FirstName" HeaderText="First Name" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="125" ItemStyle-BorderColor="White" />
                    <asp:BoundField DataField="Credentials" HeaderText="Credentials" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="125" ItemStyle-BorderColor="White" />
                    <asp:BoundField DataField="Speciality" HeaderText="Speciality" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="200" ItemStyle-BorderColor="White" />
                    <asp:BoundField DataField="BPRP" HeaderText="BPRP" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="200" ItemStyle-BorderColor="White" />
                    <asp:BoundField DataField="DRP" HeaderText="DRP" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="200" ItemStyle-BorderColor="White" />
                    <asp:BoundField DataField="HSRP" HeaderText="HSRP" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="200" ItemStyle-BorderColor="White" />
                </Columns>
            </asp:GridView>
        </td>
    </tr>
    <tr>
        <td>
            <p style="margin-left: 7px; font-style: italic; font-size: smaller;">
                (To edit Provider Information or to add/remove Providers, please go to the Settings
                tab and select User Administration from the menu on the left)</p>
        </td>
    </tr>
    <tr>
        <td>
            <p style="margin-left: 0px; font-weight: bold; width: 600px;">
                Optional: Attach Supporting Documentation for Site Information and/or Provider Information
                <asp:Image ID="imgUpload" runat="server" ImageUrl="~/Themes/Images/upload.png" CssClass="uploadPopUp"
                    ClientIDMode="Static" AlternateText="Upload File" ToolTip="Upload File" onClick="javascript:updateSrc(this);" /></p>
        </td>
    </tr>
</table>
<div id="lightbox-popup" class="uploadbox-popup" style="border: 1px solid #5880B3;
    height: auto;">
    <div id="popupHeaderText">
        <table width="100%">
            <tr>
                <td class="trow">
                </td>
                <td width="80%" align="center" valign="middle">
                    Upload File
                </td>
                <td width="20%" align="right" valign="middle">
                    <a id="close-UploadPopUp" href="#">close[x]</a>
                </td>
            </tr>
        </table>
    </div>
    <table width="100%">
        <tr>
            <td class="popup-info">
                <asp:Label ID="lblInfo" runat="server" ClientIDMode="Static" Text="Uploading file for General Practice Information"
                    Width="395px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlFrame" runat="server">
                    <iframe scrolling="no" id="fuPage" style="height: auto; min-height: 280px; width: 100%;
                        overflow: hidden;" src="../MOReFileUpload.aspx" frameborder="0"></iframe>
                </asp:Panel>
            </td>
        </tr>
    </table>
</div>
<!-- /lightbox-panel -->
<div id="lightbox" class="uploadbox">
</div>
