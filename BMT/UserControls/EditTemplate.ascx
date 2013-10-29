<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditTemplate.ascx.cs"
    Inherits="EditTemplate" ViewStateMode="Enabled" %>
<link rel="Stylesheet" type="text/css" href="../Themes/jquery-ui-1.8.12.custom.css" />
<link rel="Stylesheet" type="text/css" href="../Themes/popup.css" />
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<%@ Register Src="~/UserControls/LoadingPanel.ascx" TagName="LoadingPanel" TagPrefix="ucl" %>
<%-- ############################ KNOWLEDGEBASE-POPUP START HERE #####################################################--%>
<div id="lightbox-popup" class="knowledgebase-popup" style="border: 1px solid #5880B3;
    min-width: 873px; position: absolute; left: auto; height: auto;">
    <div id="popupHeaderText">
        <table width="100%">
            <tr>
                <td width="80%" align="left" valign="middle">
                    <span class="rspan" id="popupHeader"></span>
                </td>
                <td width="20%" align="right" valign="middle">
                    <a id="close-knowledgebase-popup">close[x] </a>
                </td>
            </tr>
        </table>
    </div>
    <table width="100%">
        <tr>
            <td>
                <asp:Panel ID="pnlDocViewer" runat="server">
                    <iframe id="IframeDocViewer" style="min-height: 650px; width: 100%; overflow: scroll"
                        frameborder="0"></iframe>
                    <%--                    <a id="close-knowledgebase-popup">
                        <asp:Button ID="cancelPopup" runat="server" ClientIDMode="Static" CssClass="cancelButton"
                            Text="Cancel"/></a>--%>
                </asp:Panel>
            </td>
        </tr>
    </table>
</div>
<%-- ############################ KNOWLEDGEBASE-POPUP END HERE #####################################################--%>
<asp:UpdatePanel ID="upnlEditTemp" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div style="width: 730">
            <h1>
                Edit Templates
            </h1>
            <h2 style="color: #C10000">
                <asp:Label ID="tempName" runat="server" ClientIDMode="Static"></asp:Label>
            </h2>
            <ucdm:DisplayMessage ID="messageEditTemplate" runat="server" ClientIDMode="Static" />
            <table class="fixed">
                <tr>
                    <td width="230px" valign="top">
                        <asp:GridView ID="gvHeader" runat="server" AllowPaging="True" AllowSorting="True"
                            ShowFooter="true" PageSize="15" AutoGenerateColumns="False" HeaderStyle-BackColor="#5880B3"
                            HeaderStyle-ForeColor="#FFFFFF" BorderColor="#CCCCCC" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-CssClass="grid-border" CssClass="submission-grid" DataKeyNames="KnowledgeBaseId"
                            OnRowCommand="gvHeader_RowCommand" OnRowDeleting="gvHeader_RowDeleting" OnPageIndexChanging="gvHeader_PageIndexChanging">
                            <EmptyDataRowStyle BackColor="LightBlue" />
                            <EmptyDataTemplate>
                                <table width="100%" style="background-color: #5880B3; color: #FFFFFF;">
                                    <tr>
                                        <td align="center">
                                            No Record Found.
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <AlternatingRowStyle BackColor="#F2F2F2" />
                            <HeaderStyle BackColor="#5880B3" CssClass="grid-header" ForeColor="White" HorizontalAlign="Center" />
                            <PagerSettings FirstPageText="First Page" LastPageText="Last Page" Mode="NumericFirstLast"
                                Position="Bottom" />
                            <PagerStyle BackColor="#5880B3" ForeColor="White" BorderColor="Transparent" BorderWidth="0px"
                                BorderStyle="None" Font-Bold="true" />
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="grid-border"
                                    HeaderStyle-Width="80%">
                                    <HeaderTemplate>
                                        <asp:CheckBox ClientIDMode="Static" runat="server" OnClick="OnCheckAll(this)" ID="chkHeader" />
                                        <asp:Label ID="lblheaders" runat="server" Text='Headers' Font-Bold="True" Font-Size="10pt"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ClientIDMode="Static" runat="server" OnClick="OnCheckChange(this)"
                                            ID="chkHeaderName" />
                                        <asp:LinkButton ID="lnkHeader" runat="server" CommandName="Select" Text='<%# Eval("Name") %>'
                                            CommandArgument='<%# Eval("KnowledgeBaseId") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:LinkButton ID="addSubHeader" CommandName="Add" CommandArgument='<%# Eval("KnowledgeBaseTypeId") %>'
                                            runat="server" Text="Add Header" OnClientClick="DisplayEnterKnowledgebasePopup('Header');"></asp:LinkButton>
                                    </FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Left" BorderColor="#5880B3" CssClass="grid-border" />
                                    <ItemStyle BorderColor="#CCCCCC" HorizontalAlign="Left" CssClass="grid-border" />
                                    <FooterStyle BorderColor="#CCCCCC" HorizontalAlign="Left" CssClass="grid-border" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-border"
                                    HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("KnowledgeBaseId") %>'
                                            ImageUrl="~/Themes/Images/Edit-16.png" ToolTip="Edit" Text="Edit" OnClientClick='<%# "return DisplayCustomizeKnowledgebasePopup(\"" + Eval("KnowledgeBaseId").ToString() + "\", \"" + Eval("KnowledgeBaseTypeId").ToString() + "\");"%>' />
                                        <%--         <asp:LinkButton ID="lnkbtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("KnowledgeBaseId") %>'
                                            OnClientClick="return confirm('Are you sure want to delete?');">
                                            <img id="imgDelete" src="~/Themes/Images/Delete.png" runat="server" alt="Sub-Header"
                                                title="Delete" />
                                        </asp:LinkButton>--%>
                                        <asp:LinkButton ID="lnkingHeader" runat="server" CommandName="Select" CommandArgument='<%# Eval("KnowledgeBaseId") %>'>
                                          <img src="~/Themes/Images/arrownext.png" runat="server" alt="Sub-Header" title="Sub-Headers"/>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="30px" BorderColor="#5880B3" CssClass="grid-border"/>
                                    <ItemStyle BorderColor="#CCCCCC" HorizontalAlign="Center" CssClass="grid-border" />
                                    <FooterStyle BorderColor="#5880B3" BackColor="#F2F2F2" CssClass="grid-border" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                    <td width="10px">
                    </td>
                    <td width="230px" valign="top">
                        <asp:GridView ID="gvSubHeader" runat="server" AllowPaging="True" AllowSorting="True"
                            ShowFooter="true" PageSize="15" AutoGenerateColumns="False" HeaderStyle-BackColor="#5880B3"
                            HeaderStyle-ForeColor="#FFFFFF" BorderColor="#CCCCCC" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-CssClass="grid-header" CssClass="submission-grid" DataKeyNames="KnowledgeBaseId"
                            OnRowCommand="gvSubHeader_RowCommand" OnRowDeleting="gvSubHeader_RowDeleting"
                            OnPageIndexChanging="gvSubHeader_PageIndexChanging">
                            <EmptyDataRowStyle BackColor="LightBlue" />
                            <EmptyDataTemplate>
                                <table width="100%" style="background-color: #5880B3; color: #FFFFFF;">
                                    <tr>
                                        <td align="center">
                                            No Record Found.
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <AlternatingRowStyle BackColor="#F2F2F2" />
                            <HeaderStyle BackColor="#5880B3" CssClass="grid-header" ForeColor="White" HorizontalAlign="Center" />
                            <PagerSettings FirstPageText="First Page" LastPageText="Last Page" Mode="NumericFirstLast"
                                Position="Bottom" />
                            <PagerStyle BackColor="#5880B3" ForeColor="White" BorderColor="Transparent" BorderWidth="0px"
                                BorderStyle="None" Font-Bold="true" />
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="grid-border"
                                    HeaderStyle-Width="80%">
                                    <HeaderTemplate>
                                        <asp:CheckBox ClientIDMode="Static" runat="server" ID="chkSubHeader" OnClick="OnCheckAll(this)" />
                                        <asp:Label ID="lblheaders" runat="server" Text='Sub-Headers' Font-Bold="True" Font-Size="10pt"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ClientIDMode="Static" runat="server" OnClick="OnCheckChange(this)"
                                            ID="chkSubHeaderName" />
                                        <asp:LinkButton ID="lnkSubHeader" runat="server" CommandName="Select" Text='<%# Eval("Name") %>'
                                            CommandArgument='<%# Eval("KnowledgeBaseId") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:LinkButton ID="addHeader" runat="server" Text="Add Sub Header" CommandName="Add"
                                            CommandArgument='<%# Eval("KnowledgeBaseTypeId") %>' OnClientClick="DisplayEnterKnowledgebasePopup('SubHeader');"></asp:LinkButton>
                                    </FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Left" BorderColor="#5880B3" CssClass="grid-border"/>
                                    <ItemStyle BorderColor="#CCCCCC" HorizontalAlign="Left" CssClass="grid-border" />
                                    <FooterStyle BorderColor="#CCCCCC" HorizontalAlign="Left" CssClass="grid-border" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-border"
                                    HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Themes/Images/Edit-16.png"
                                            ToolTip="Edit" Text="Edit" OnClientClick='<%# "return DisplayCustomizeKnowledgebasePopup(\"" + Eval("KnowledgeBaseId").ToString() + "\", \"" + Eval("KnowledgeBaseTypeId").ToString() + "\");"%>' />
                                        <%--     <asp:LinkButton ID="lnkbtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("KnowledgeBaseId") %>'
                                            OnClientClick="return confirm('Are you sure want to delete?');">
                                            <img id="imgDelete" src="~/Themes/Images/Delete.png" runat="server" alt="Sub-Header"
                                                title="Delete" />
                                        </asp:LinkButton>--%>
                                        <asp:LinkButton ID="lnkingHeader" runat="server" CommandName="Select" CommandArgument='<%# Eval("KnowledgeBaseId") %>'>
                                            <img id="imgNext" src="~/Themes/Images/arrownext.png" runat="server" alt="Sub-Header"
                                                title="Questions" />
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="30px" BorderColor="#5880B3" CssClass="grid-border" />
                                    <ItemStyle BorderColor="#CCCCCC" HorizontalAlign="Center" CssClass="grid-border" />
                                    <FooterStyle BorderColor="#5880B3" BackColor="#F2F2F2" CssClass="grid-border" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                    <td width="10px">
                    </td>
                    <td width="230px" valign="top">
                        <asp:GridView ID="gvQuestion" runat="server" AllowPaging="True" AllowSorting="True"
                            PageSize="15" AutoGenerateColumns="False" HeaderStyle-BackColor="#5880B3" HeaderStyle-ForeColor="#FFFFFF"
                            BorderColor="#CCCCCC" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="grid-header"
                            CssClass="submission-grid" DataKeyNames="KnowledgeBaseId" ShowFooter="true" OnRowDeleting="gvQuestion_RowDeleting"
                            OnRowCommand="gvQuestion_RowCommand" OnPageIndexChanging="gvQuestion_PageIndexChanging">
                            <EmptyDataRowStyle BackColor="LightBlue" />
                            <EmptyDataTemplate>
                                <table width="100%" style="background-color: #5880B3; color: #FFFFFF;">
                                    <tr>
                                        <td align="center">
                                            No Record Found.
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <AlternatingRowStyle BackColor="#F2F2F2" />
                            <HeaderStyle BackColor="#5880B3" CssClass="grid-header" ForeColor="White" HorizontalAlign="Center" />
                            <PagerSettings FirstPageText="First Page" LastPageText="Last Page" Mode="NumericFirstLast"
                                Position="Bottom" />
                            <PagerStyle BackColor="#5880B3" ForeColor="White" BorderColor="Transparent" BorderWidth="0px"
                                BorderStyle="None" Font-Bold="true" />
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="grid-border"
                                    HeaderStyle-Width="80%">
                                    <HeaderTemplate>
                                        <asp:CheckBox ClientIDMode="Static" runat="server" ID="chkQuestion" OnClick="OnCheckAll(this)" />
                                        <asp:Label ID="lblheaders" runat="server" Text='Questions' Font-Bold="True" Font-Size="10pt"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ClientIDMode="Static" runat="server" OnClick="OnCheckChange(this)"
                                            ID="chkQuestionName" />
                                        <asp:LinkButton ID="lnkQuestion" runat="server" CommandName="Select" Text='<%# Eval("Name") %>'
                                            CommandArgument='<%# Eval("KnowledgeBaseId") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:LinkButton ID="addQuestion" runat="server" CommandName="Add" Text="Add Question"
                                            CommandArgument='<%# Eval("KnowledgeBaseTypeId") %>' OnClientClick="DisplayEnterKnowledgebasePopup('Question');"></asp:LinkButton>
                                    </FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Left" BorderColor="#5880B3" CssClass="grid-border"  />
                                    <ItemStyle BorderColor="#CCCCCC" HorizontalAlign="Left" CssClass="grid-border" />
                                    <FooterStyle BorderColor="#CCCCCC" HorizontalAlign="Left" CssClass="grid-border" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-border"
                                    HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Themes/Images/Edit-16.png"
                                            ToolTip="Edit" Text="Edit" OnClientClick='<%# "return DisplayCustomizeKnowledgebasePopup(\"" + Eval("KnowledgeBaseId").ToString() + "\", \"" + Eval("KnowledgeBaseTypeId").ToString() + "\");"%>' />
                                        <%--    <asp:LinkButton ID="lnkbtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("KnowledgeBaseId") %>'
                                            OnClientClick="return confirm('Are you sure want to delete?');">
                                            <img id="imgDelete" src="~/Themes/Images/Delete.png" runat="server" alt="Sub-Header"
                                                title="Delete" />
                                        </asp:LinkButton>--%>
                                    </ItemTemplate>
                                    <HeaderStyle Width="30px" BorderColor="#5880B3" CssClass="grid-border" />
                                    <ItemStyle BorderColor="#CCCCCC" HorizontalAlign="Center" CssClass="grid-border"/>
                                    <FooterStyle BorderColor="#5880B3" BackColor="#F2F2F2" CssClass="grid-border"/>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <table align="center">
                            <tr>
                                <td>
                                    <asp:Button ID="btnSaveAll" runat="server" OnClick="btnSaveAll_OnClick" CssClass="hideDisplay" />
                                    <asp:Button ID="btnSave" runat="server" OnClick="btnSaveHeader_OnClick" CssClass="hideDisplay" />
                                    <asp:Button ID="btnSaveTemplateChanges" runat="server" Text="Save and Return to My Templates"
                                        OnClientClick="SettingsFormSection('#createMORe');returnBackTemplate()" />
                                </td>
                                <%--                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnDiscardEditChanges" runat="server" Text="Discard Changes" OnClientClick="return confirm('Are you sure want to discard the changes?');" />
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnCancelChnages" CausesValidation="false" runat="server" Text="Cancel"
                                        OnClientClick="SettingsFormSection('#createMORe')" />
                                </td>--%>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <ucl:LoadingPanel ID="ldpnlEditTemp" runat="server" />
             <asp:HiddenField ID="hdnTempId" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hiddenParentId" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hiddenSubHeaderParent" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hiddenQuestionParent" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnNotEditTempLoads" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnHeaderId" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnSubHeaderId" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnKBType" runat="server" ClientIDMode="Static" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
