<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KnowledgeBaseViewer.aspx.cs"
    Inherits="BMT.Webforms.KnowledgeBaseViewer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Knowledgebase Viewer</title>
    <link type="text/css" rel="Stylesheet" href="../Themes/style.css" />
    <link rel="Stylesheet" type="text/css" href="../Themes/popup.css" />
    <%@ register src="~/UserControls/DisplayMessage.ascx" tagname="DisplayMessage" tagprefix="ucdm" %>
    <%@ register src="~/UserControls/LoadingPanel.ascx" tagname="LoadingPanel" tagprefix="ucl" %>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jQuery-1.7.1.min.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/knowledgeBase.js") %>"></script>
</head>
<body style="height: 650px">
    <form id="formKBDocs" runat="server">
    <div>
        <table width="100%" id="kbPopup">
            <tr>
                <td>
                    <asp:Panel ID="pnlEditTemplateMassage" runat="server">
                        <ucdm:DisplayMessage ID="EditTemplateMessage" runat="server" DisplayMessageWidth="840"
                            ShowCloseButton="false" validationGroup="upnlEditTemp"></ucdm:DisplayMessage>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="font-family: Segoe UI; font-size: 16px; font-weight: bold; margin-left: 4px;
                        margin-bottom: 5px" id="headingText" runat="server" clientidmode="Static">
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:GridView ID="gvkbInfo" runat="server" AllowPaging="true" AllowSorting="true"
                                    PageSize="1" AutoGenerateColumns="false" HeaderStyle-BackColor="#5880B3" HeaderStyle-ForeColor="#FFFFFF"
                                    BorderColor="#CCCCCC" HeaderStyle-HorizontalAlign="Center" OnRowCommand="gvkbInfo_RowCommand"
                                    OnPageIndexChanging="gvkbInfo_PageIndexChanging" DataKeyNames="KnowledgebaseId"
                                    HeaderStyle-CssClass="grid-header">
                                    <EmptyDataRowStyle BackColor="LightBlue" BorderColor="#CCCCCC" ForeColor="Red" Width="600" />
                                    <EmptyDataTemplate>
                                        <table width="774" style="background-color: #5880B3; color: #FFFFFF;">
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
                                        <asp:BoundField DataField="ParentId" HeaderText="Parent ID" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="90" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" />
                                        <asp:BoundField DataField="KnowledgebaseId" HeaderText="ID" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="90" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" />
                                        <asp:BoundField DataField="KnowledgebaseType" HeaderText="Type" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="130" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" />
                                        <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="250" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" />
                                        <asp:BoundField DataField="AccessBy" HeaderText="Access" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="110" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" />
                                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="150" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:GridView ID="gvKbEnter" runat="server" AllowPaging="true" AllowSorting="true"
                                    PageSize="1" AutoGenerateColumns="false" HeaderStyle-BackColor="#5880B3" HeaderStyle-ForeColor="#FFFFFF"
                                    BorderColor="#CCCCCC" HeaderStyle-HorizontalAlign="Center" OnRowCommand="gvkbInfo_RowCommand"
                                    OnPageIndexChanging="gvkbInfo_PageIndexChanging" DataKeyNames="KnowledgebaseId"
                                    RowStyle-VerticalAlign="Top" HeaderStyle-CssClass="grid-header">
                                    <EmptyDataRowStyle BackColor="LightBlue" BorderColor="#CCCCCC" ForeColor="Red" Width="600" />
                                    <EmptyDataTemplate>
                                        <table width="774" style="background-color: #5880B3; color: #FFFFFF;">
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
                                        <asp:BoundField DataField="ParentId" HeaderText="Parent ID" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="75" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" />
                                        <asp:BoundField DataField="KnowledgebaseId" HeaderText="ID" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="75" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" />
                                        <asp:BoundField DataField="KnowledgebaseType" HeaderText="Type" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="120" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" />
                                        <asp:TemplateField ShowHeader="true" HeaderText="Name" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="330" ItemStyle-BorderColor="#CCCCCC" ItemStyle-CssClass="grid-border">
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rdoAddExistingKB" runat="server" Text="Existing" GroupName="status"
                                                    Width="70px" ClientIDMode="Static" onclick="OnSelectExistingKB(this);" CssClass="subHeader" />
                                                <asp:TextBox ID="txtExistingKBName" runat="server" CssClass="bodytxt-field subHeader"
                                                    MaxLength="50" Width="208px" ClientIDMode="Static" onKeyPress="javascript:return false;"
                                                    onKeyDown="javascript:return false;"></asp:TextBox>
                                                <asp:ImageButton ID="KBSearch" runat="server" ImageUrl="../Themes/Images/icon-search-small.png"
                                                    OnClientClick="DisplayHeaderPopUp(); return false;" ClientIDMode="Static" CssClass="subHeader" />
                                                <br />
                                                <asp:RadioButton ID="rdoAddNewKB" runat="server" Checked="true" Text="Add New" GroupName="status"
                                                    Width="70px" ClientIDMode="Static" onclick="OnSelectExistingKB(this);" />
                                                <asp:TextBox ID="txtAddNewKB" runat="server" CssClass="bodytxt-field txt-fields"
                                                    MaxLength="250" Width="208px" ClientIDMode="Static"></asp:TextBox>
                                                <br />
                                                <div style="color: Red; font-size: 9px; float: right; margin-right: 40px;">
                                                    Up to 250 characters
                                                </div>
                                                <br />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="Select" ClientIDMode="Static" runat="server" Text="Select" Width="100px"
                                                    OnClick="btnSelect_Click" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="AccessBy" HeaderText="Access" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="100" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" />
                                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="125" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:GridView ID="gvAddHeader" runat="server" AllowPaging="true" AllowSorting="true"
                                    PageSize="1" AutoGenerateColumns="false" HeaderStyle-BackColor="#5880B3" HeaderStyle-ForeColor="#FFFFFF"
                                    BorderColor="#CCCCCC" HeaderStyle-HorizontalAlign="Center" DataKeyNames="KnowledgebaseId"
                                    RowStyle-VerticalAlign="Top" HeaderStyle-CssClass="grid-header">
                                    <EmptyDataRowStyle BackColor="LightBlue" BorderColor="#CCCCCC" ForeColor="Red" Width="600" />
                                    <EmptyDataTemplate>
                                        <table width="774" style="background-color: #5880B3; color: #FFFFFF;">
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
                                        <asp:BoundField DataField="ParentId" HeaderText="Parent ID" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="75" ItemStyle-BorderColor="#CCCCCC" ItemStyle-CssClass="grid-border" />
                                        <asp:BoundField DataField="KnowledgebaseId" HeaderText="ID" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="75" ItemStyle-BorderColor="#CCCCCC" ItemStyle-CssClass="grid-border" />
                                        <asp:BoundField DataField="KnowledgebaseType" HeaderText="Type" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="120" ItemStyle-BorderColor="#CCCCCC" ItemStyle-CssClass="grid-border" />
                                        <asp:TemplateField ShowHeader="true" HeaderText="Name" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="330" ItemStyle-BorderColor="#CCCCCC" ItemStyle-CssClass="grid-border">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAddHeader" runat="server" CssClass="bodytxt-field" MaxLength="250"
                                                    Width="275px" ClientIDMode="Static" onkeyup="javascript:Copy();"></asp:TextBox>
                                                <br />
                                                <div style="color: Red; font-size: 9px; float: right; margin-right: 20px;">
                                                    Up to 250 characters
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="AccessBy" HeaderText="Access" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="100" ItemStyle-BorderColor="#CCCCCC" ItemStyle-CssClass="grid-border" />
                                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="125" ItemStyle-BorderColor="#CCCCCC" ItemStyle-CssClass="grid-border" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%--            <tr>
                <td>
                    <br />
                </td>
            </tr>--%>
            <tr>
                <td>
                    <div style="font-family: Segoe UI; font-size: 16px; font-weight: bold; margin-left: 4px;
                        margin-bottom: 5px">
                        Template Usage Information</div>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="Header" runat="server">
                        <tr>
                            <td>
                                <asp:GridView ID="HeaderInfo" runat="server" AllowPaging="true" AllowSorting="true"
                                    PageSize="2" AutoGenerateColumns="false" HeaderStyle-BackColor="#5880B3" HeaderStyle-ForeColor="#FFFFFF"
                                    BorderColor="#CCCCCC" HeaderStyle-HorizontalAlign="Center" OnRowCommand="HeaderInfo_RowCommand"
                                    OnPageIndexChanging="HeaderInfo_PageIndexChanging" DataKeyNames="TemplateId"
                                    HeaderStyle-CssClass="grid-header">
                                    <EmptyDataRowStyle BackColor="LightBlue" BorderColor="#CCCCCC" ForeColor="Red" Width="820" />
                                    <EmptyDataTemplate>
                                        <table width="835" style="background-color: #5880B3; color: #FFFFFF;">
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
                                        <asp:BoundField DataField="TemplateId" HeaderText="Template ID" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="105" ItemStyle-BorderColor="#CCCCCC" ItemStyle-CssClass="grid-border" />
                                        <asp:BoundField DataField="TemplateName" HeaderText="Template Name" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="150" ItemStyle-BorderColor="#CCCCCC" ItemStyle-CssClass="grid-border" />
                                        <asp:BoundField DataField="DisplayName" HeaderText="Display Name" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="260" ItemStyle-BorderColor="#CCCCCC" ItemStyle-CssClass="grid-border" />
                                        <asp:BoundField DataField="TabName" HeaderText="Tab Name" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="120" ItemStyle-BorderColor="#CCCCCC" ItemStyle-CssClass="grid-border" />
                                        <asp:BoundField DataField="AccessBy" HeaderText="Access" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="95" ItemStyle-BorderColor="#CCCCCC" ItemStyle-CssClass="grid-border" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-BorderColor="#CCCCCC"
                                            HeaderStyle-Width="90" HeaderText="Instructions" ItemStyle-CssClass="grid-border">
                                            <ItemTemplate>
                                                <p>
                                                    <a href='#' class='ttt'>View<span class='tooltip'><span class='top'></span><span
                                                        class='middle'> &nbsp;&nbsp;
                                                        <%# Eval("Instruction")%>
                                                    </span><span class='bottom'></span></span></a>
                                                </p>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--                                        <asp:TemplateField ShowHeader="true" HeaderText="Copy" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="122" ItemStyle-BorderColor="White">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <input type="checkbox" id="Copy" runat="server" clientidmode="Static" value='<%# Eval("TemplateId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <table id="SubHeader" runat="server">
                        <tr>
                            <td>
                                <asp:GridView ID="SubHeaderInfo" runat="server" AllowPaging="true" AllowSorting="true"
                                    PageSize="2" AutoGenerateColumns="false" HeaderStyle-BackColor="#5880B3" HeaderStyle-ForeColor="#FFFFFF"
                                    BorderColor="#CCCCCC" HeaderStyle-HorizontalAlign="Center" OnRowCommand="SubHeaderInfo_RowCommand"
                                    OnPageIndexChanging="SubHeaderInfo_PageIndexChanging" DataKeyNames="TemplateId"
                                    HeaderStyle-CssClass="grid-header">
                                    <EmptyDataRowStyle BackColor="LightBlue" BorderColor="#CCCCCC" ForeColor="Red" Width="820" />
                                    <EmptyDataTemplate>
                                        <table width="835" style="background-color: #5880B3; color: #FFFFFF;">
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
                                        <asp:BoundField DataField="TemplateId" HeaderText="Template ID" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="130" ItemStyle-BorderColor="#CCCCCC" ItemStyle-CssClass="grid-border" />
                                        <asp:BoundField DataField="TemplateName" HeaderText="Template Name" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="160" ItemStyle-BorderColor="#CCCCCC" ItemStyle-CssClass="grid-border" />
                                        <asp:BoundField DataField="DisplayName" HeaderText="Display Name" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="250" ItemStyle-BorderColor="#CCCCCC" ItemStyle-CssClass="grid-border" />
                                        <asp:BoundField DataField="MustPass" HeaderText="Must Pass" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="100" ItemStyle-BorderColor="#CCCCCC" ItemStyle-CssClass="grid-border"/>
                                        <asp:BoundField DataField="AccessBy" HeaderText="Access" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="85" ItemStyle-BorderColor="#CCCCCC" ItemStyle-CssClass="grid-border"/>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-border"
                                            ItemStyle-BorderColor="#CCCCCC" HeaderStyle-Width="95" HeaderText="Instructions">
                                            <ItemTemplate>
                                                <p>
                                                    <a href='#' class='ttt'>View<span class='tooltip'><span class='top'></span><span
                                                        class='middle'>
                                                        <%# Eval("Instruction")%>
                                                    </span><span class='bottom'></span></span></a>
                                                </p>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--                                  <asp:TemplateField ShowHeader="true" HeaderText="Copy" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="122" ItemStyle-BorderColor="White">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <input type="checkbox" id="Copy" runat="server" clientidmode="Static" value='<%# Eval("TemplateId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <table id="Question" runat="server">
                        <tr>
                            <td>
                                <asp:GridView ID="QuestionInfo" runat="server" AllowPaging="true" AllowSorting="true"
                                    PageSize="2" AutoGenerateColumns="false" HeaderStyle-BackColor="#5880B3" HeaderStyle-ForeColor="#FFFFFF"
                                    BorderColor="#CCCCCC" HeaderStyle-HorizontalAlign="Center" OnRowCommand="QuestionInfo_RowCommand"
                                    OnPageIndexChanging="QuestionInfo_PageIndexChanging" DataKeyNames="TemplateId"
                                    HeaderStyle-CssClass="grid-header">
                                    <EmptyDataRowStyle BackColor="LightBlue" BorderColor="#CCCCCC" ForeColor="Red" Width="820" />
                                    <EmptyDataTemplate>
                                        <table width="835" style="background-color: #5880B3; color: #FFFFFF;">
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
                                        <asp:BoundField DataField="TemplateId" HeaderText="Template ID" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="100" ItemStyle-BorderColor="#CCCCCC" ItemStyle-CssClass="grid-border" />
                                        <asp:BoundField DataField="TemplateName" HeaderText="Template Name" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="80" ItemStyle-BorderColor="#CCCCCC" ItemStyle-CssClass="grid-border" />
                                        <asp:BoundField DataField="DisplayName" HeaderText="Display Name" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="220" ItemStyle-BorderColor="#CCCCCC" ItemStyle-CssClass="grid-border" />
                                        <asp:BoundField DataField="Critical" HeaderText="Critical" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="85" ItemStyle-BorderColor="#CCCCCC" ItemStyle-CssClass="grid-border" />
                                        <asp:BoundField DataField="AccessBy" HeaderText="Access" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="85" ItemStyle-BorderColor="#CCCCCC" ItemStyle-CssClass="grid-border" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-BorderColor="#CCCCCC"
                                            HeaderStyle-Width="80" HeaderText="Instructions">
                                            <ItemTemplate>
                                                <p>
                                                    <a href='#' class='ttt'>View<span class='tooltip'><span class='top'></span><span
                                                        class='middle'>
                                                        <%# Eval("Instruction")%>
                                                    </span><span class='bottom'></span></span></a>
                                                </p>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Answer" HeaderText="Answers" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="80" ItemStyle-BorderColor="#CCCCCC" ItemStyle-CssClass="grid-border" />
                                        <asp:BoundField DataField="AddOns" HeaderText="Add-ons" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="85" ItemStyle-BorderColor="#CCCCCC" ItemStyle-CssClass="grid-border" />
                                        <%--                                        <asp:TemplateField ShowHeader="true" HeaderText="Copy" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="90" ItemStyle-BorderColor="White">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <input type="checkbox" id="Copy" runat="server" clientidmode="Static" value='<%# Eval("TemplateId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table id="CurentTemplateForm" runat="server" width="100%">
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr>
                <td>
                    <div id="knowledgebaseForm" runat="server" style="font-family: Segoe UI; font-size: 16px;
                        font-weight: bold; margin-bottom: 5px; margin-left: 4px;">
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr id="ErrorMsg1" style="display: none;">
                            <td>
                                <img src="../Themes/Images/close.png" alt="Error" />
                            </td>
                            <td>
                                <asp:Label runat="server" ID="ErrorMessageOfKB" ForeColor="Red" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                        <tr id="ErrorMsg2" style="display: none;">
                            <td>
                                <img src="../Themes/Images/close.png" alt="Error" />
                            </td>
                            <td>
                                <asp:Label runat="server" ID="ErrorMsgOfKb" ForeColor="Red" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                        <tr id="ErrorMsg3" style="display: none;">
                            <td>
                                <img src="../Themes/Images/close.png" alt="Error" />
                            </td>
                            <td>
                                <asp:Label runat="server" ID="ErrorMsgOfDataBox" ForeColor="Red" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td style="width: 450px">
                                <div style="height: 25px; margin-bottom: 10px;">
                                    <div class="bodytxt-fieldlabel06">
                                        Display Name*:
                                    </div>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtDisplayName" runat="server" CssClass="bodytxt-field" MaxLength="250"
                                            Width="275px" ValidationGroup="UpdatePanelForm" CausesValidation="true" onkeyup="javascript:CopyBack();"></asp:TextBox>
                                    </div>
                                    <div class="validator">
                                        <div id="rfvTxtDisplayName" style="display: none;">
                                            *</div>
                                    </div>
                                    <div style="color: Red; font-size: 9px; float: right; margin-right: 53px; line-height: 12px;">
                                        (Up to 250 Characters)
                                    </div>
                                </div>
                                <div runat="server" id="MustPass" style="margin-bottom: 10px; height: 25px;">
                                    <div class="bodytxt-fieldlabel06">
                                        Must Pass*:
                                    </div>
                                    <div style="float: left">
                                        <asp:RadioButton ID="rdoMustPassNo" runat="server" Text="No" GroupName="mustPass" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rdoMustPassYes" runat="server" Text="Yes" GroupName="mustPass" />
                                    </div>
                                    <div class="validator">
                                        <div id="rfvMustPass" style="display: none;">
                                            *</div>
                                    </div>
                                </div>
                                <div runat="server" id="infoDocument" style="margin-bottom: 10px; height: 25px;">
                                    <div class="bodytxt-fieldlabel06">
                                        Info Document*:
                                    </div>
                                    <div style="float: left; vertical-align: top">
                                        <asp:RadioButton ID="rbInfoDocNo" Checked="true" runat="server" onclick="OnInfoDocChanged(this)"
                                            Text="No" GroupName="infoDocs" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rbInfoDocYes" runat="server" Text="Yes" onclick="OnInfoDocChanged(this)"
                                            GroupName="infoDocs" />
                                    </div>
                                    <div class="validator">
                                        <div id="rfvInfoDocs" style="display: none;">
                                            *</div>
                                    </div>
                                    <div runat="server" id="pageReference" style="padding-top: 2px; float: right; margin-right: 14px;">
                                        <div class="bodytxt-fieldlabel06">
                                            Page Reference:
                                        </div>
                                        <div style="float: left">
                                            <asp:TextBox ID="txtPageReference" runat="server" CssClass="bodytxt-field" MaxLength="200"
                                                Width="20px"> </asp:TextBox>
                                        </div>
                                        <div class="validator">
                                            <div id="rfvPageReference" style="display: none;">
                                                *</div>
                                        </div>
                                    </div>
                                </div>
                                <div runat="server" id="Critical" style="margin-bottom: 10px; height: 25px;">
                                    <div class="bodytxt-fieldlabel06">
                                        Critical*:
                                    </div>
                                    <div style="float: left">
                                        <asp:RadioButton ID="rdoCriticalNo" Checked="true" runat="server" Text="No" onclick="OnCriricalChanged(this)"
                                            GroupName="status" />&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rdoCriticalYes" runat="server" Text="Yes" onclick="OnCriricalChanged(this)"
                                            GroupName="status" />
                                    </div>
                                    <div class="validator">
                                        <div id="rfvRdoCritical" style="display: none;">
                                            *</div>
                                    </div>
                                </div>
                                <div runat="server" id="CriticalToolTip" style="margin-bottom: 10px; height: 25px;">
                                    <div class="bodytxt-fieldlabel06">
                                        ToolTip Text:
                                    </div>
                                    <div style="float: left">
                                        <asp:TextBox ID="txtToolTip" runat="server" CssClass="bodytxt-field" MaxLength="200"
                                            Width="275px"> </asp:TextBox>
                                    </div>
                                    <div class="validator">
                                        <div id="rfvRdoCriticalTool" style="display: none;">
                                            *</div>
                                    </div>
                                </div>
                                <div runat="server" id="Tab" style="margin-bottom: 10px; height: 75px;">
                                    <div class="bodytxt-fieldlabel06">
                                        Tab Name*:
                                    </div>
                                    <div style="float: left">
                                        Line 1:
                                        <asp:TextBox ID="txtTabLine1" runat="server" CssClass="bodytxt-field" MaxLength="12"
                                            Width="100"> </asp:TextBox>
                                        <br />
                                        <br />
                                        Line 2:
                                        <asp:TextBox ID="txtTabLine2" runat="server" CssClass="bodytxt-field" MaxLength="12"
                                            Width="100"> </asp:TextBox>
                                    </div>
                                    <div class="validator2">
                                        <div id="rfvTextTabLine1" style="display: none;">
                                            *</div>
                                    </div>
                                    <div style="color: Red; font-size: 9px; width: 95px; float: left; margin-left: 11px">
                                        (Up to 12 Characters)
                                        <br />
                                        <br />
                                        (Up to 12 Characters)
                                    </div>
                                </div>
                                <div style="margin-bottom: 10px; height: 100px">
                                    <div class="bodytxt-fieldlabel06">
                                        Instruction:
                                    </div>
                                    <div style="float: left;">
                                        <textarea id="txtInstruction" runat="server" class="bodytxt-field" rows="4" cols="40"
                                            onkeydown="textCounter(this,275);" onkeyup="textCounter(this,275);"></textarea>
                                        <br />
                                        <asp:Label ID="descriptionLength" runat="server" ClientIDMode="Static" Text="275 characters left"
                                            CssClass="descriptionValidator"></asp:Label>
                                    </div>
                                </div>
                                <div style="margin-bottom: 10px">
                                    <div style="font-size: 11px; height: 25px; font-style: italic; color: #FF0000; margin: 0px 0px 0px 10px">
                                        (Fields marked with on * are Required)</div>
                                </div>
                            </td>
                            <td style="width: 400px; vertical-align: top;">
                                <div id="right" runat="server">
                                    <div style="margin-bottom: 10px; height: 25px;">
                                        <div class="bodytxt-fieldlabel0">
                                            Answers:
                                        </div>
                                        <div style="float: left">
                                            <asp:RadioButton ID="AnsYesNo" runat="server" GroupName="Answer" Text="Yes/No" />&nbsp;&nbsp;&nbsp;
                                            <asp:RadioButton ID="AnsYesNoNA" runat="server" GroupName="Answer" Checked="true"
                                                Text="Yes/No/NA" />&nbsp;&nbsp;&nbsp;
                                            <%--<asp:RadioButton ID="AnsNone" runat="server" GroupName="Answer" Text="None" />--%>
                                        </div>
                                    </div>
                                    <div style="margin-bottom: 10px; height: 25px;">
                                        <div class="bodytxt-fieldlabel0">
                                            Add-ons:
                                        </div>
                                        <div style="float: left; height: 25px">
                                            <input type="checkbox" id="chkDataBox" runat="server" clientidmode="Static" onclick="OnDataBoxCheckChanged()" />&nbsp;Data
                                            Box(D)
                                            <br />
                                            <div id="databoxHeader" runat="server" clientidmode="Static" style="float: left;
                                                height: 25px; margin-top: 10px;">
                                                Data Box Header:&nbsp;&nbsp;<asp:TextBox ID="txtDataBoxHeader" runat="server" CssClass="bodytxt-field"
                                                    MaxLength="150"> </asp:TextBox>
                                                <div id="rfvTxtDataBoxHeader" style="display: none;">
                                                    *
                                                </div>
                                            </div>
                                            <%--                                        <br />
                                            <br />
                                            Data Box Type:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:DropDownList ID="DropDownList2" runat="server" Width="150px">
                                            </asp:DropDownList>
                                            <br />
                                            <input type="checkbox" id="Checkbox10" />Plan Tools(P)
                                            <br />
                                            <input type="checkbox" id="Checkbox11" />Mobile Patient Tools(M)--%>
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-left: 355px;">
                                <table>
                                    <tr align="center">
                                        <td>
                                            <asp:Button ID="btnSaveEditTemplate" runat="server" Text="Save" CausesValidation="true"
                                                Width="100px" ClientIDMode="Static" ValidationGroup="SaveButton" OnClientClick="savingTemplate(hdnUserId.value,hdnkbTypeId.value,hdnTemplateId.value,hdnkbId.value,hdnParentId.value,hdnGrandParentId.value,hdnIsEditOrAdd.value);return false;" />
                                            <%--                             <asp:Button ID="btnSaveEditTemplate" runat="server" Text="Save" CausesValidation="true"
                                                ClientIDMode="Static" ValidationGroup="SaveButton" OnClientClick="btnSave();" />--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hdnkbTypeId" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnTemplateId" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnkbId" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnParentId" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnGrandParentId" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnUserId" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnIsEditOrAdd" runat="server" ClientIDMode="Static" />
                </td>
            </tr>
        </table>
    </div>
    <%-- ############################ HEADER POPUP START HERE #####################################################--%>
    <div id="lightbox-popup" class="header-popup" style="border: 1px solid #5880B3; width: 480px;
        height: 450px; position: absolute; left: 328px;">
        <div id="popupHeaderText">
            <table style="width: 505px">
                <tr>
                    <td width="80%" align="left" valign="middle">
                        <span class="rspan" id="searchPopup" runat="server" clientidmode="Static"></span>
                    </td>
                    <td width="20%" align="right" valign="middle">
                        <a id="close-Header">close[x]</a>
                    </td>
                </tr>
            </table>
        </div>
        <table style="width: 480px" id="kbExistingPopup">
            <tr>
                <td colspan="2">
                    <ucdm:DisplayMessage ID="MessageExistingKb" runat="server" DisplayMessageWidth="470"
                        ShowCloseButton="false" validationGroup="upnlEditTemp"></ucdm:DisplayMessage>
                </td>
            </tr>
            <tr>
                <td>
                    <img src="../Themes/Images/caution.png" alt="caution" width="75%" />
                </td>
                <td id="text1" runat="server">
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                </td>
            </tr>
            <tr>
                <td style="font-size: 11px; font-style: italic; padding-left: 10px;" id="text2" runat="server"
                    colspan="2">
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" valign="top">
                    <table width="450px">
                        <tr>
                            <td align="left" valign="top">
                                <asp:Panel ID="pnlHeaderList" runat="server" ClientIDMode="Static" CssClass="pnlHeader">
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnAddKb" runat="server" Text="Add" CausesValidation="false" ClientIDMode="Static"
                        OnClientClick="CopyKnowledgebase(); return false;" />
                    <a id="close-Header" style="text-decoration: none; font-size: 16px; color: White;">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                            CssClass="cancel-popUp" OnClientClick="return false;" />
                    </a>
                </td>
            </tr>
        </table>
    </div>
    <%-- ############################ HEADER POPUP END HERE #####################################################--%>
    <div id="lightbox-popup" class="Instrction-popup" style="border: 1px solid #5880B3;
        width: auto; height: auto;">
        <div id="popupHeaderText">
            <table>
                <tr>
                    <td width="80%" align="left" valign="middle">
                        <span class="rspan">Instruction</span>
                    </td>
                    <td width="20%" align="right" valign="middle">
                        <a id="close-Instrction-popup">close[x]</a>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Instruction" runat="server" clientidmode="Static">
        </div>
    </div>
    </form>
</body>
</html>
