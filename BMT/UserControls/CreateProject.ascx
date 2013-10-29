<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateProject.ascx.cs"
    Inherits="CreateProject" ViewStateMode="Enabled" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<%@ Register Src="~/UserControls/LoadingPanel.ascx" TagName="LoadingPanel" TagPrefix="ucl" %>
<link rel="Stylesheet" type="text/css" href="../Themes/jquery-ui-1.8.12.custom.css" />
<link rel="Stylesheet" type="text/css" href="../Themes/popup.css" />
<script language="javascript" type="text/javascript">
    function LoadCreateEvents() {
        var folderList = $('#hdnFolderList').val();
        $("document").ready(function () {
            if (folderList != "") {
                GenerateMainFolderList();
            }
        });
    }

    function LoadEditEvents() {
        var folderList = $('#hdnFolderList').val();
        $("document").ready(function () {
            if (folderList != "") {
                GenerateMainFolderList();
            }
        });
    }
                            </script>
<div id="lightbox-popup" class="templateDocument-popup" style="border: 1px solid #5880B3;
    width: 480px;">
    <div id="popupHeaderText">
        <table width="100%">
            <tr>
                <td width="80%" align="left" valign="middle">
                    <span class="rspan">Templates</span>
                </td>
                <td width="20%" align="right" valign="middle">
                    <a id="close-templateDocument">close[x]</a>
                </td>
            </tr>
        </table>
    </div>
    <table width="100%" id="tempPopUp" runat="server" clientidmode="Static">
        <tr>
            <td>
                <ucdm:DisplayMessage ID="msgTempDocs" runat="server" DisplayMessageWidth="450" ClientIDMode="Static" />
            </td>
        </tr>
        <tr>
            <th colspan="2" align="left" style="font-weight: bolder; font-size: 16px">
                Available Template List
            </th>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr id="Tr1">
            <td>
                <div style="max-height: 343px; width: 480px; overflow-y: scroll;">
                    <asp:UpdatePanel ID="upnlTemp" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlTemp" runat="server" ClientIDMode="Static">
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td align="center">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnSaveTemp" runat="server" Text="Save" CausesValidation="false"
                                ClientIDMode="Static" OnClientClick="SelectTemplateProject(); return false;" />
                        </td>
                        <td width="15">
                        </td>
                        <td>
                            <a id="close-templateDocument" href="#" style="text-decoration: none; font-size: 16px;
                                color: White;">
                                <asp:Button ID="btnCancelTempPopup" runat="server" Text="Cancel" CausesValidation="false"
                                    CssClass="cancel-popUp" OnClientClick="return false;" ClientIDMode="Static" /></a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<!-- /lightbox-panel -->
<div class="lightbox">
</div>
<!-- /lightbox -->
<div id="lightbox-popup" class="Form-popup" style="border: 1px solid #5880B3; width: 480px;">
    <div id="popupHeaderText">
        <table width="100%">
            <tr>
                <td width="80%" align="left" valign="middle">
                    <span class="rspan">Forms</span>
                </td>
                <td width="20%" align="right" valign="middle">
                    <a id="close-Form">close[x]</a>
                </td>
            </tr>
        </table>
    </div>
    <table width="100%" id="tblForm" runat="server" clientidmode="Static">
        <tr>
            <td>
                <ucdm:DisplayMessage ID="msgFormPopup" runat="server" DisplayMessageWidth="450" ClientIDMode="Static" />
            </td>
        </tr>
        <tr>
            <th colspan="2" align="left" style="font-weight: bolder; font-size: 16px">
                Available Form List
            </th>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr id="Tr2">
            <td>
                <div style="max-height: 343px; width: 480px; overflow-y: scroll;">
                    <asp:UpdatePanel ID="upnlForm" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlForm" runat="server" ClientIDMode="Static">
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td align="center">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnSaveForm" runat="server" Text="Save" CausesValidation="false"
                                ClientIDMode="Static" OnClientClick="SelectProjectForm(); return false;" />
                        </td>
                        <td width="15">
                        </td>
                        <td>
                            <a id="close-Form" href="#" style="text-decoration: none; font-size: 16px; color: White;">
                                <asp:Button ID="btnCancelForm" runat="server" Text="Cancel" CausesValidation="false"
                                    CssClass="cancel-popUp" OnClientClick="return false;" ClientIDMode="Static" /></a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<!-- Folder Popup-->
<div id="lightbox-popup" class="OtherFolders-popup" style="border: 1px solid #5880B3;
    width: 480px;">
    <div id="popupHeaderText">
        <table width="100%">
            <tr>
                <td width="80%" align="left" valign="middle">
                    <span class="rspan">Other Folders</span>
                </td>
                <td width="20%" align="right" valign="middle">
                    <a id="close-OtherFolders">close[x]</a>
                </td>
            </tr>
        </table>
    </div>
    <table width="100%" id="FolderPopUp" runat="server" clientidmode="Static">
        <tr>
            <td>
                <ucdm:DisplayMessage ID="msgFolderPopup" runat="server" DisplayMessageWidth="450" ClientIDMode="Static" />
            </td>
        </tr>
        <tr>
            <th colspan="2" align="left" style="font-weight: bolder; font-size: 16px">
                Folder List
            </th>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr id="Tr3">
            <td>
                <div style="margin-bottom: 10px; margin-left:8px; height: auto" id="tableDiv" runat="server">
                <table border="0" id="tableElement">
					<tbody><tr>
						<td style="width:125px;"><span>Add Folder:</span></td>
                        <td><input type="text" class="bodytxt-field" id="Folder1" /></td>
                        <td><a href="javascript:GenerateSubFolderRows(1);" id="hypSubAddMore1">+ Add Child Folder</a></td>
					</tr>
                        <tr>
						    <td colspan="3">
                                <table border="0" id="SubFolderTable1">
                                    <tbody>
                                        <tr>
					                    </tr>
				                    </tbody>
                                </table>
                            </td>
					    </tr>
                    <tr>
						<td colspan="2"><a href="javascript:GenerateFolderRows(1);" id="hypLinkAddMore1">+ Add Another Folder</a></td>
					</tr>
				</tbody></table>
                                </div>
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td align="center">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnSaveFolders" runat="server" Text="Save" CausesValidation="false"
                                ClientIDMode="Static" OnClientClick="SaveFolderList(); return false;" />
                        </td>
                        <td width="15">
                        </td>
                        <td>
                            <a id="close-OtherFolders" href="#" style="text-decoration: none; font-size: 16px;
                                color: White;">
                                <asp:Button ID="btnCancelFoldersPopup" runat="server" Text="Cancel" CausesValidation="false"
                                    CssClass="cancel-popUp" OnClientClick="return false;" ClientIDMode="Static" /></a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>

<!--Folder Popup-->
<!-- /lightbox-panel -->
<div class="lightbox">
</div>
<!-- /lightbox -->
<asp:UpdatePanel ID="upnlCreateProjects" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div style="font-family: Segoe UI; font-size: 16px; font-weight: bold; margin-bottom: 5px">
            My Projects</div>
        <table width="730" border="0">
            <tr>
                <td>
                    <asp:GridView ID="gvCreateProjects" runat="server" AllowPaging="true" AllowSorting="true"
                        PageSize="8" AutoGenerateColumns="false" HeaderStyle-BackColor="#5880B3" OnRowCommand="gvCreateProjects_RowCommand"
                        DataKeyNames="ProjectId" HeaderStyle-ForeColor="#FFFFFF" OnPageIndexChanging="gvCreateProjects_PageIndexChanging"
                        HeaderStyle-HorizontalAlign="Center" BorderColor="#CCCCCC" HeaderStyle-CssClass="header-border">
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
                            <asp:BoundField HeaderText="Project Name" DataField="Name" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="180" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" />
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="grid-border"
                                HeaderStyle-Width="280" HeaderText="Description">
                                <ItemStyle HorizontalAlign="Center" BorderColor="#CCCCCC" />
                                <ItemTemplate>
                                    <%# Eval("Description").ToString().Length > 25 ? (Eval("Description") as string).Substring(0, 25) + "<a href='#' class='ttm'>(More)<span class='tooltip'><span class='top'></span><span class='middle'> &nbsp;&nbsp;" + Eval("Description") + "</span><span class='bottom'></span></span></a>" : Eval("Description")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Created By" DataField="CreatedBy" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="100" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" />
                            <asp:BoundField HeaderText="Created on" DataField="CreatedDate" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="100" ItemStyle-CssClass="grid-border" ItemStyle-BorderColor="#CCCCCC" />
                            <asp:TemplateField ShowHeader="true" HeaderText="Edit" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="80" ItemStyle-CssClass="grid-border">
                                <ItemStyle HorizontalAlign="Center" BorderColor="#CCCCCC" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnView" runat="server" CommandName="Select" ImageUrl="~/Themes/Images/edit-16.png"
                                        ToolTip="Edit" CommandArgument='<%# Eval("ProjectId") %>' />
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
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlCreateProject" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <script type="text/javascript">
                                Sys.Application.add_load(LoadCreateEvents);
                            </script>
                            <asp:Panel ID="pnlProjectMassage" runat="server">
                                <ucdm:DisplayMessage ClientIDMode="Static" ID="ProjectMassage" runat="server" DisplayMessageWidth="700"
                                    ShowCloseButton="false" validationGroup="upnlCreateProject"></ucdm:DisplayMessage>
                            </asp:Panel>
                            <asp:Panel ID="pnlCreateProjectForm" runat="server">
                                <div style="font-family: Segoe UI; font-size: 16px; font-weight: bold; margin-bottom: 5px">
                                    Create New Project</div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Project Name:*
                                    </div>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtProjectName" runat="server" CssClass="bodytxt-field" MaxLength="50"
                                            Width="345px" ValidationGroup="upnlCreateProject"></asp:TextBox>
                                    </div>
                                    <div class="validator">
                                        <asp:RequiredFieldValidator ID="rfvProjectName" runat="server" Text="*" Display="Static"
                                            ControlToValidate="txtProjectName" ValidationGroup="upnlCreateProject"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="reProjectName" runat="server" Display="None"
                                            ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,50}$" ControlToValidate="txtProjectName"
                                            ErrorMessage="Project Name is invalid!" ValidationGroup="upnlCreateProject"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div style="margin-bottom: 10px; height: 75px">
                                    <div class="bodytxt-fieldlabel">
                                        Project Description:
                                    </div>
                                    <div style="float: left;">
                                        <textarea id="txtProjectDescription" runat="server" class="bodytxt-field" rows="4"
                                            cols="51" onkeydown="textCounter(this,250);" onkeyup="textCounter(this,250);"></textarea>
                                    </div>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Allow Access To:*
                                    </div>
                                    <div style="float: left;">
                                        <asp:DropDownList ID="ddlAllowAccessTo" runat="server" Width="150px" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlAllowAccessChanged_Click">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="validator">
                                        <asp:RequiredFieldValidator ID="rfvAllowAccessTo" runat="server" Text="*" Display="Static"
                                            ControlToValidate="ddlAllowAccessTo" InitialValue="0" ValidationGroup="upnlCreateProject"></asp:RequiredFieldValidator></div>
                                </div>
                                <div id="divLstMGR" runat="server" clientidmode="Static" style="margin-bottom: 10px;
                                    height: 150px; padding-left: 6px; display: none">
                                    <table>
                                        <tr>
                                            <td>
                                                Available Medical Groups:
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                All Medical Groups
                                                <br />
                                                <asp:ListBox ID="lstAvailableMGR" runat="server" Font-Names="Arial" Font-Size="9pt"
                                                    SelectionMode="Multiple" Width="220px" Height="100px"></asp:ListBox>
                                            </td>
                                            <td>
                                                <asp:Button runat="server" ID="btnAddMGR" Text=">>" Width="50px" OnClick="btnAddMGR_Click" />
                                                <br />
                                                <asp:Button runat="server" ID="btnRemoveMGR" Text="<<" Width="50px" OnClick="btnRemoveMGR_Click" />
                                            </td>
                                            <td>
                                                Selected Medical Groups
                                                <br />
                                                <asp:ListBox ID="lstSelectedMGR" runat="server" Font-Names="Arial" Font-Size="9pt"
                                                    SelectionMode="Multiple" Width="220px" Height="100px"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divLstENT" runat="server" clientidmode="Static" style="margin-bottom: 10px;
                                    height: 150px; padding-left: 6px; display: none">
                                    <table>
                                        <tr>
                                            <td>
                                                Available Enterprises:
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                All Enterprises
                                                <br />
                                                <asp:ListBox ID="lstAvailableEnterprises" runat="server" Font-Names="Arial" Font-Size="9pt"
                                                    SelectionMode="Multiple" Width="220px" Height="100px"></asp:ListBox>
                                            </td>
                                            <td>
                                                <asp:Button runat="server" ID="btnAddEnt" Text=">>" Width="50px" OnClick="btnAddEnt_Click" />
                                                <br />
                                                <asp:Button runat="server" ID="btnRemoveEnt" Text="<<" Width="50px" OnClick="btnRemoveEnt_Click" />
                                            </td>
                                            <td>
                                                Selected Enterprises
                                                <br />
                                                <asp:ListBox ID="lstSelectedEnterprises" runat="server" Font-Names="Arial" Font-Size="9pt"
                                                    SelectionMode="Multiple" Width="220px" Height="100px"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divLstPractice" runat="server" clientidmode="Static" style="margin-bottom: 10px;
                                    height: 150px; padding-left: 6px; display: none">
                                    <table>
                                        <tr>
                                            <td>
                                                Available Practices:
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                All Practices
                                                <br />
                                                <asp:ListBox ID="lstPrac" runat="server" Font-Names="Arial" Font-Size="9pt" SelectionMode="Multiple"
                                                    Width="220px" Height="100px"></asp:ListBox>
                                            </td>
                                            <td>
                                                <asp:Button runat="server" ID="btnAddPrac" Text=">>" Width="50px" OnClick="btnAddPrac_Click" />
                                                <br />
                                                <asp:Button runat="server" ID="btnRemovePrac" Text="<<" Width="50px" OnClick="btnRemovePrac_Click" />
                                            </td>
                                            <td>
                                                Selected Practices
                                                <br />
                                                <asp:ListBox ID="assignedPrac" runat="server" Font-Names="Arial" Font-Size="9pt"
                                                    SelectionMode="Multiple" Width="220px" Height="100px"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Add Project Folder:*
                                    </div>
                                    <div style="position: relative; float: left; margin: 0 10px 0 0;">
                                        Yes
                                        <div style="position: relative; float: right; margin-right: 15px;">
                                            <asp:RadioButton ID="rdoYes" Checked="true" runat="server" GroupName="projectFolder"
                                                TextAlign="Left" />
                                        </div>
                                    </div>
                                    <div style="position: relative; float: left; margin: 0 10px 0 0;">
                                        No
                                        <div style="position: relative; float: right; margin-right: 15px;">
                                            <asp:RadioButton ID="rdoNo" runat="server" TextAlign="Left" GroupName="projectFolder" /></div>
                                    </div>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Select Template:
                                    </div>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtSelectTemplate" runat="server" CssClass="bodytxt-field" onKeyPress="javascript:return false;"
                                            onKeyDown="javascript:return false;" TabIndex="1"> </asp:TextBox>
                                    </div>
                                    <asp:ImageButton ID="imgTemplate" OnClientClick="DisplayProjectTemplatePopup(event); return false;"
                                        runat="server" ToolTip="Search Template" ImageUrl="../Themes/Images/icon-search.png"
                                        CssClass="icon-search" />
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Select Form:
                                    </div>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtForm" runat="server" CssClass="bodytxt-field" onKeyPress="javascript:return false;"
                                            onKeyDown="javascript:return false;" TabIndex="1"> </asp:TextBox>
                                    </div>
                                    <asp:ImageButton ID="imgForm" runat="server" OnClientClick="DisplayProjectFormPopup(event); return false;"
                                        ToolTip="Search Form" ImageUrl="../Themes/Images/icon-search.png" CssClass="icon-search" />
                                </div>
                                <div style="margin-bottom: 10px; height:auto; float:left;">
                                    <div class="bodytxt-fieldlabel" >
                                        Add More Folders:
                                    </div>
                                    <asp:ImageButton ID="ImgFolder" runat="server" CssClass="icon-search" ImageUrl="../Themes/Images/icon-search.png" 
                                    OnClientClick="DisplayFolderPopup(event); return false;" ToolTip="Add Other Folders" />    
                                </div>    
                                <div style="height:auto; clear:both;" id="divFolderList" runat="server">

                                </div>

                                <div style="margin-bottom: 10px;">
                                    <div style="font-size: 11px; height: 25px; font-style: italic; color: #FF0000; margin: 0px 0px 0px 10px">
                                        (Fields marked with on * are Required)</div>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <table>
                                        <tr>
                                            <td align="right" width="300">
                                                <asp:Button ID="btnSaveProject" runat="server" Text="Create New Project" OnClick="btnSaveProject_Click"
                                                    ValidationGroup="upnlCreateProject" OnClientClick="return validation();" />
                                            </td>
                                            <td width="15">
                                                &nbsp;
                                            </td>
                                            <td width="200">
                                                <asp:Button ID="btnCancelProject" runat="server" Text="Cancel" OnClick="btnCancelProject_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="upnlUpdateProject" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <script type="text/javascript">
                                Sys.Application.add_load(LoadEditEvents);
                            </script>
                            <asp:Panel ID="pnlEditProjMessage" runat="server">
                                <ucdm:DisplayMessage ID="EditMessage" runat="server" DisplayMessageWidth="700" ShowCloseButton="false"
                                    validationGroup="upnlUpdateProject"></ucdm:DisplayMessage>
                            </asp:Panel>
                            <asp:Panel ID="pnlEditProject" runat="server">
                                <div style="font-family: Segoe UI; font-size: 16px; font-weight: bold; margin-bottom: 5px">
                                    Edit Project</div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Project Name:*
                                    </div>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtProjName" runat="server" CssClass="bodytxt-field" MaxLength="50"
                                            Width="345px" ValidationGroup="upnlUpdateProject"></asp:TextBox>
                                    </div>
                                    <div class="validator">
                                        <asp:RequiredFieldValidator ID="rfvProjName" runat="server" Text="*" Display="Static"
                                            ControlToValidate="txtProjName" ValidationGroup="upnlUpdateProject"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="reProjName" runat="server" Display="None" ValidationExpression="^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,50}$"
                                            ControlToValidate="txtProjName" ErrorMessage="Project Name is invalid!" ValidationGroup="upnlUpdateProject"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div style="margin-bottom: 10px; height: 75px">
                                    <div class="bodytxt-fieldlabel">
                                        Project Description:
                                    </div>
                                    <div style="float: left;">
                                        <textarea id="txtProjDescription" runat="server" class="bodytxt-field" rows="4" cols="51"
                                            onkeydown="textCounter(this,250);" onkeyup="textCounter(this,250);"></textarea>
                                    </div>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Allow Access To:*
                                    </div>
                                    <div style="float: left;">
                                        <asp:DropDownList ID="ddlAllowProjAccess" runat="server" Width="150px" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlAllowProjAccessChange_Click">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="validator">
                                        <asp:RequiredFieldValidator ID="rfvddlAllowProjAccess" runat="server" Text="*" Display="Static"
                                            ControlToValidate="ddlAllowProjAccess" InitialValue="0" ValidationGroup="upnlUpdateProject"></asp:RequiredFieldValidator></div>
                                </div>
                                <div id="divEditMGR" runat="server" clientidmode="Static" style="margin-bottom: 10px;
                                    height: 150px; padding-left: 6px; display: none;">
                                    <table>
                                        <tr>
                                            <td>
                                                Available Medical Groups:
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                All Medical Groups
                                                <br />
                                                <asp:ListBox ID="lstBoxMGR" runat="server" Font-Names="Arial" Font-Size="9pt" SelectionMode="Multiple"
                                                    Width="220px" Height="100px"></asp:ListBox>
                                            </td>
                                            <td>
                                                <asp:Button runat="server" ID="btnAdd" Text=">>" Width="50px" OnClick="btnAdd_Click" />
                                                <br />
                                                <asp:Button runat="server" ID="btnRemove" Text="<<" Width="50px" OnClick="btnRemove_Click" />
                                            </td>
                                            <td>
                                                Selected Medical Groups
                                                <br />
                                                <asp:ListBox ID="lstBoxAssignedMGR" runat="server" Font-Names="Arial" Font-Size="9pt"
                                                    SelectionMode="Multiple" Width="220px" Height="100px"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divEditEnterprise" runat="server" clientidmode="Static" style="margin-bottom: 10px;
                                    height: 150px; padding-left: 6px; display: none;">
                                    <table>
                                        <tr>
                                            <td>
                                                Available Enterprises:
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                All Enterprises
                                                <br />
                                                <asp:ListBox ID="lstEnt" runat="server" Font-Names="Arial" Font-Size="9pt" SelectionMode="Multiple"
                                                    Width="220px" Height="100px"></asp:ListBox>
                                            </td>
                                            <td>
                                                <asp:Button runat="server" ID="btnEditAddEnt" Text=">>" Width="50px" OnClick="btnEditAddEnt_Click" />
                                                <br />
                                                <asp:Button runat="server" ID="btnEditRemoveEnt" Text="<<" Width="50px" OnClick="btnEditRemoveEnt_Click" />
                                            </td>
                                            <td>
                                                Selected Enterprises
                                                <br />
                                                <asp:ListBox ID="lstAssignedEnt" runat="server" Font-Names="Arial" Font-Size="9pt"
                                                    SelectionMode="Multiple" Width="220px" Height="100px"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divEditPrac" runat="server" clientidmode="Static" style="margin-bottom: 10px;
                                    height: 150px; padding-left: 6px; display: none">
                                    <table>
                                        <tr>
                                            <td>
                                                Available Practices:
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                All Practices
                                                <br />
                                                <asp:ListBox ID="lstEditPrac" runat="server" Font-Names="Arial" Font-Size="9pt" SelectionMode="Multiple"
                                                    Width="220px" Height="100px"></asp:ListBox>
                                            </td>
                                            <td>
                                                <asp:Button runat="server" ID="btnEditAddPrac" Text=">>" Width="50px" OnClick="btnEditAddPrac_Click" />
                                                <br />
                                                <asp:Button runat="server" ID="btnEditRemovePrac" Text="<<" Width="50px" OnClick="btnEditRemovePrac_Click" />
                                            </td>
                                            <td>
                                                Selected Practices
                                                <br />
                                                <asp:ListBox ID="assignedEditPrac" runat="server" Font-Names="Arial" Font-Size="9pt"
                                                    SelectionMode="Multiple" Width="220px" Height="100px"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Add Project Folder*:
                                    </div>
                                    <div style="position: relative; float: left; margin: 0 10px 0 0;">
                                        Yes
                                        <div style="position: relative; float: right; margin-right: 15px;">
                                            <asp:RadioButton ID="rdoEditYes" runat="server" TextAlign="Left" GroupName="editProjectFolder" />
                                        </div>
                                    </div>
                                    <div style="position: relative; float: left; margin: 0 10px 0 0;">
                                        No
                                        <div style="position: relative; float: right; margin-right: 15px;">
                                            <asp:RadioButton ID="rdoEditNo" runat="server" TextAlign="Left" GroupName="editProjectFolder" /></div>
                                    </div>
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Select Template:
                                    </div>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtSelectTemp" runat="server" CssClass="bodytxt-field" onKeyPress="javascript:return false;"
                                            onKeyDown="javascript:return false;" TabIndex="1"></asp:TextBox>
                                    </div>
                                    <asp:ImageButton ID="imgTemp" OnClientClick="DisplayProjectTemplatePopup(event); return false;"
                                        runat="server" ToolTip="Search Template" ImageUrl="../Themes/Images/icon-search.png"
                                        CssClass="icon-search" />
                                </div>
                                <div style="margin-bottom: 10px; height: 25px">
                                    <div class="bodytxt-fieldlabel">
                                        Select Form:
                                    </div>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtSelectedForm" runat="server" CssClass="bodytxt-field" onKeyPress="javascript:return false;"
                                            onKeyDown="javascript:return false;" TabIndex="1"></asp:TextBox>
                                    </div>
                                    <asp:ImageButton ID="imgSelForm" runat="server" OnClientClick="DisplayProjectFormPopup(event); return false;"
                                        ToolTip="Search Form" ImageUrl="../Themes/Images/icon-search.png" CssClass="icon-search" />
                                </div>
                                <div style="margin-bottom: 10px; height:auto; float:left;">
                                    <div class="bodytxt-fieldlabel" >
                                        Add More Folders:
                                    </div>
                                    <asp:ImageButton ID="ImgEditFolder" runat="server" CssClass="icon-search" ImageUrl="../Themes/Images/icon-search.png" 
                                    OnClientClick="DisplayFolderPopup(event); return false;" ToolTip="Add Other Folders" />    
                                </div>    
                                <div style="height:auto; clear:both;" id="divEditFolderList" runat="server">

                                </div>

                                <div style="margin-bottom: 10px">
                                    <div style="font-size: 11px; height: 25px; font-style: italic; color: #FF0000; margin: 0px 0px 0px 10px">
                                        (Fields marked with on * are Required)</div>
                                </div>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="right" width="300">
                                            <asp:Button ID="btnSaveChanges" runat="server" Text="Save Changes" OnClick="btnUpdateProject_Click"
                                                ValidationGroup="upnlUpdateProject" OnClientClick="return editValidation();" />
                                        </td>
                                        <td width="15">
                                            &nbsp;
                                        </td>
                                        <td width="200">
                                            <asp:Button ID="btnDiscardChanges" runat="server" Text="Discard Changes" OnClick="btnDiscardChanges_Click"
                                                OnClientClick="return confirm('Are you sure want to discard the changes?');" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:HiddenField ID="hdnProjectId" runat="server" ClientIDMode="Static" />
                                <asp:HiddenField ID="hdnIsCreate" runat="server" ClientIDMode="Static" />
                                <asp:HiddenField ID="hdnIsEdit" runat="server" ClientIDMode="Static" />
                                <asp:HiddenField ID="hdnProjectName" runat="server" ClientIDMode="Static" />
                                <asp:HiddenField ID="hdnFolderList" runat="server" ClientIDMode="Static" />
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Button ID="btnRefreshLists" runat="server" ClientIDMode="Static" Text="RefreshPage"
                        OnClick="btnRefreshList_Click" CssClass="hideDisplay" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
