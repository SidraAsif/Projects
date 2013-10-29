<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NCQASubmission.aspx.cs"
    MasterPageFile="~/BMTMaster.master" Inherits="BMT.Webforms.NCQASubmission" %>

<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">
    <link rel="Stylesheet" type="text/css" href="../Themes/NCQA.css" media="all" />
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/ncqaSubmission.js") %>"></script>
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
    <asp:Panel ID="pnlNCQASubmission" runat="server">
        <div style="width: 99%; margin: 10px auto 0px auto; padding-bottom: 20px;">
            <h1>
                NCQA Submission Request
            </h1>
            <asp:GridView ID="grdNCQASubmission" runat="server" AllowPaging="true" AllowSorting="true" CellSpacing="2" GridLines="None"
                PageSize="15" AutoGenerateColumns="false" HeaderStyle-BackColor="#5880B3" HeaderStyle-ForeColor="#FFFFFF"
                BorderColor="White" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="grid-header"
                CssClass="submission-grid" DataKeyNames="ProjectUsageId,PracticeSiteId,RequestedOn" OnRowEditing="grdNCQASubmission_RowEditing"
                OnRowDataBound="grdNCQASubmission_RowDataBound" OnRowUpdating="grdNCQASubmission_RowUpdating"
                OnRowCancelingEdit="grdNCQASubmission_RowCancelingEdit" OnPageIndexChanging="grdNCQASubmission_PageIndexChanging" OnRowCommand="grdNCQASubmission_RowCommand">
                <EmptyDataRowStyle BackColor="LightBlue"/>
                <EmptyDataTemplate>
                    <table  width="968" style="background-color: #5880B3; color: #FFFFFF;">
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
                <PagerStyle BackColor="#5880B3" ForeColor="White" Font-Bold="true" />
                <Columns>
                    <asp:TemplateField HeaderText="Site Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="170">
                        <ItemTemplate>
                            <asp:Label ID="lblProjectId" runat="server" Text='<%#Eval("SiteName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ISS License Number" ItemStyle-HorizontalAlign="Left"
                        HeaderStyle-Width="130">
                        <ItemTemplate>
                            <asp:Label ID="lblISSLicenseNumber" runat="server" Text='<%#Eval("ISSLicenseNumber") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="UserName" ItemStyle-HorizontalAlign="Left"
                        HeaderStyle-Width="110">
                        <ItemTemplate>
                            <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("UserName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Password" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="65">
                        <ItemTemplate>
                            <asp:HyperLink ID="hypPassword" runat="server" ToolTip="Click to view password"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="80">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lblStatusText" runat="server" Text='<%#Eval("Status") %>' Visible="false"></asp:Label>
                            <asp:DropDownList ID="ddlNCQAStatus" runat="server">
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Requested On" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="80">
                        <ItemTemplate>
                            <asp:Label ID="lblRequestedOn" runat="server" Text='<%#Eval("RequestedOn","{0:MM/dd/yyyy}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Completed On" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="85">
                        <ItemTemplate>
                            <asp:Label ID="lblCompletedOn" runat="server" Text='<%#Eval("CompletedOn","{0:MM/dd/yyyy}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Last Updated" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="80">
                        <ItemTemplate>
                            <asp:Label ID="lblLastUpdated" runat="server" Text='<%#Eval("LastUpdated","{0:MM/dd/yyyy}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Updated by" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="75">
                        <ItemTemplate>
                            <asp:Label ID="lblUpdatedby" runat="server" Text='<%#Eval("updatedby") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SubmissionType" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblSubmissionType" runat="server" Text='<%#Eval("SubmissionType") %>'></asp:Label>
                            <asp:Label ID="lblTemplateId" runat="server" Text='<%#Eval("TemplateId") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lblHdnSubmissionType" runat="server" Text='<%#Eval("SubmissionType") %>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="50">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Themes/Images/Edit-16.png" ToolTip="Edit"
                                CommandName="Edit" Text="Edit"/>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:ImageButton ID="btnUpdate" runat="server" CommandName="Update" Text="Update"
                                ImageUrl="~/Themes/Images/save-16.png"/>
                            <asp:ImageButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel"
                                ImageUrl="~/Themes/Images/cancel-cross.png" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="35">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="~/Themes/Images/Submit-16.png" ToolTip="Submit"
                                CommandName="Submit" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' Text="Submit"  OnClientClick="return alert('Submission in progress. You will be notified by email when its complete.');"/>
                        </ItemTemplate>
                        </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>
</asp:Content>
