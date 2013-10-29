<%@ Page Title="" Language="C#" MasterPageFile="~/BMTMaster.master" AutoEventWireup="true"
    CodeBehind="Dashboard.aspx.cs" Inherits="BMT.Webforms.Dashboard" %>

<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/dashboard.js") %>"></script>
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
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="bodyContainer" runat="server">
    <div style="color: #333333;font-family:Trebuchet MS;font-size:12px;width:98%;margin-top:10px;margin-bottom:-35px;">
        <table width="100%" id="TblOptions" runat="server">
        <tr>
        <td align="right" colspan="2" style="width:100%;">
        Choose Dashboard:
        </td>
        <td>
        <asp:DropDownList runat="server" ID="DDLSelectDashboard" Width="185px" OnSelectedIndexChanged="DDLSelectDashboard_SelectedIndexChanged" AutoPostBack="true">
        <asp:ListItem Text="All Projects" Value = "0"></asp:ListItem>
        <asp:ListItem Text="Security Risk Assessment" Value = "1"></asp:ListItem>
        </asp:DropDownList>
        </td>
        </tr>
        <tr>
        <td align="right" colspan="3" style="width:100%;">
        <asp:CheckBox ID="CBHomeDashboard" Text="Make this my home dashboard" runat="server" AutoPostBack="true" OnCheckedChanged="CBHomeDashboard_CheckedChanged"
         Checked="false" />
        </td>
        </tr>
        </table>
        </div>

    <asp:Panel ID="pnlDashBoard" runat="server">
        <div id="dashboardwrapper">
            <h1>
                My Practices</h1>
            <asp:DataGrid ID="datagridPractice" runat="server" AllowPaging="true" AllowCustomPaging="true"
                AllowSorting="true" PageSize="15" AutoGenerateColumns="false" HeaderStyle-BackColor="#5880B3"
                HeaderStyle-ForeColor="#FFFFFF" BorderColor="White" HeaderStyle-HorizontalAlign="Center"
                HeaderStyle-CssClass="grid-header" OnPageIndexChanged="datagridPractice_OnPageIndexChanged"
                OnSortCommand="datagridPractice_SortCommand" CssClass="grid">
                <AlternatingItemStyle CssClass="gridAlternateRow" />
                <PagerStyle BackColor="#5880B3" ForeColor="White" BorderColor="Transparent" BorderWidth="0px"
                    BorderStyle="None" Font-Bold="true" Mode="NumericPages" HorizontalAlign="Left" />
                <Columns>
                    <asp:BoundColumn DataField="PracticeName" HeaderText="Practice Name" ItemStyle-HorizontalAlign="Left"
                        HeaderStyle-Width="200" ItemStyle-BorderColor="White" ReadOnly="true" SortExpression="PracticeName" />
                    <asp:TemplateColumn HeaderText="Site Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200"
                        ItemStyle-BorderColor="White" SortExpression="SiteName">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlSiteName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SiteName")%>'
                                
                                Target="_self" />
                                <%--NavigateUrl='<%# "Projects.aspx?PTemp="+ DataBinder.Eval(Container.DataItem,"SecurePracticeId")  + "&ETemp="+ DataBinder.Eval(Container.DataItem,"SecureEnterpriseId")  + "&NodeContentType=NCQARequirements" + "&NodeProjectID=" + DataBinder.Eval(Container.DataItem,"ProjectId") + "&Path=" + "PCMH Recognition/" + DataBinder.Eval(Container.DataItem,"SiteName") + "/" +"3" %>'--%>
                        </ItemTemplate>
                    </asp:TemplateColumn>

                    <asp:BoundColumn DataField="Points" HeaderText="%Complete" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="80" ItemStyle-BorderColor="White" ReadOnly="true" SortExpression="Points"
                        DataFormatString="{0} %" />
                    <asp:BoundColumn DataField="Documents" HeaderText="%Documentation" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="125" ItemStyle-BorderColor="White" ReadOnly="true" SortExpression="Documents"
                        DataFormatString="{0} %" />
                    <asp:TemplateColumn HeaderText="Last Activity" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100"
                        ItemStyle-BorderColor="White" SortExpression="LastActivity">
                        <ItemTemplate>
                            <asp:Label ID="lblPracticeLastActDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"LastActivity","{0:MM/dd/yyyy}")%>'
                                ForeColor='<%# Convert.ToString(Eval("DateForeColor"))=="Gray"?System.Drawing.Color.Gray:System.Drawing.Color.Black %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn DataField="ContactName" HeaderText="Contact Name" ItemStyle-HorizontalAlign="Left"
                        HeaderStyle-Width="110" ItemStyle-BorderColor="White" ReadOnly="true" SortExpression="ContactName" />
                    <asp:BoundColumn DataField="ContactPhone" HeaderText="Contact Phone" ItemStyle-HorizontalAlign="Left"
                        HeaderStyle-Width="100" ItemStyle-BorderColor="White" ReadOnly="true" />
                    <asp:BoundColumn DataField="EmailText" DataFormatString="<a href='mailto:{0}' target='_blank'><img src='../Themes/Images/email-icon.png' title='Send Email' alt='Send Email' /></a>"
                        HeaderStyle-Width="35" ReadOnly="true" ItemStyle-BorderColor="White" />
                </Columns>
            </asp:DataGrid>
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlSRA" runat="server">
    <div id="dashboardwrapper">
            <h1>
                My Practices</h1>
            <h2>
                Security Risk Assessment Project
            </h2>

            <asp:DataGrid ID="dataGridSRA" runat="server" AllowPaging="true" AllowCustomPaging="true"
                AllowSorting="true" PageSize="15" AutoGenerateColumns="false" HeaderStyle-BackColor="#5880B3"
                HeaderStyle-ForeColor="#FFFFFF" BorderColor="White" HeaderStyle-HorizontalAlign="Center"
                HeaderStyle-CssClass="grid-header"  OnSortCommand="dataGridSRA_SortCommand" OnPageIndexChanged="dataGridSRA_OnPageIndexChanged" CssClass="grid">
                <AlternatingItemStyle CssClass="gridAlternateRow" />
                <PagerStyle BackColor="#5880B3" ForeColor="White" BorderColor="Transparent" BorderWidth="0px"
                    BorderStyle="None" Font-Bold="true" Mode="NumericPages" HorizontalAlign="Left" />
                <Columns>
                    <asp:BoundColumn DataField="PracticeName" HeaderText="Practice Name" ItemStyle-HorizontalAlign="Left"
                        HeaderStyle-Width="200" ItemStyle-BorderColor="White" ReadOnly="true" SortExpression="PracticeName" />
                    <asp:TemplateColumn HeaderText="Site Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200"
                        ItemStyle-BorderColor="White" SortExpression="SiteName">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlSiteNameSRA" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SiteName")%>'
                            Target="_self" /><%--NavigateUrl='<%# "Projects.aspx?PTemp="+ DataBinder.Eval(Container.DataItem,"SecurePracticeId")  + "&ETemp="+ DataBinder.Eval(Container.DataItem,"SecureEnterpriseId")
                             + "&NodeContentType=Security Risk Assessment" + "&NodeProjectID=" + DataBinder.Eval(Container.DataItem,"ProjectId") + "&Active=ctl00_bodyContainer_TreeControl_treeViewt27&Path=SRA/"
                              + DataBinder.Eval(Container.DataItem,"SiteName") + "/" + "29" %>'--%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn DataField="FindingsFinalized" HeaderText="Findings Finalized" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="140" ItemStyle-BorderColor="White" ReadOnly="true" SortExpression="FindingsFinalized" />
                    <asp:BoundColumn DataField="FollowupFinalized" HeaderText="Followup Finalized" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="140" ItemStyle-BorderColor="White" ReadOnly="true" SortExpression="FollowupFinalized" />
                    <asp:TemplateColumn HeaderText="Last Activity" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100"
                        ItemStyle-BorderColor="White" SortExpression="LastActivity">
                        <ItemTemplate>
                            <asp:Label ID="lblLastActDateSRA" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"LastActivity","{0:MM/dd/yyyy}")%>'
                                ForeColor='<%# Convert.ToString(Eval("DateForeColor"))=="Gray"?System.Drawing.Color.Gray:System.Drawing.Color.Black %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn DataField="ContactName" HeaderText="Contact Name" ItemStyle-HorizontalAlign="Left"
                        HeaderStyle-Width="115" ItemStyle-BorderColor="White" ReadOnly="true" SortExpression="ContactName" />
                    <asp:BoundColumn DataField="ContactPhone" HeaderText="Contact Phone" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="105" ItemStyle-BorderColor="White" ReadOnly="true" />
                    <asp:BoundColumn DataField="EmailText" DataFormatString="<a href='mailto:{0}' target='_blank'><img src='../Themes/Images/email-icon.png' title='Send Email' alt='Send Email' /></a>"
                        HeaderStyle-Width="45" ReadOnly="true" ItemStyle-BorderColor="White" ItemStyle-HorizontalAlign="Center" />
                </Columns>
            </asp:DataGrid>
            
    </div>
    
    </asp:Panel>
</asp:Content>
