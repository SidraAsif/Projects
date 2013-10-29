<%@ Page Language="C#" MasterPageFile="~/BMTMaster.Master" AutoEventWireup="true"
    CodeBehind="Editor.aspx.cs" Inherits="BMT.Webforms.Editor" ValidateRequest="false" %>

<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="Stylesheet" type="text/css" href="../Themes/jquery-ui-1.8.12.custom.css" />
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jquery-ui-1.8.12.custom.min.js") %>"></script>
    <script type="text/javascript" src="../Scripts/jquery.markitup.js"></script>
    <script type="text/javascript" src="../Scripts/markitup.set.js"></script>
    <link rel="stylesheet" type="text/css" href="../Themes/markitupstyle.css" />
    <script type="text/javascript">
        $(function () {
        
            var curr_date = new Date();
            $(".datePicker").datepicker({
                showOn: 'button',
                buttonImage: '../Themes/Images/calendar.gif',
                buttonImageOnly: true,
                dateFormat: 'mm/dd/yy',
               // minDate: curr_date

            });
        });   
           
    </script>
    <script type="text/javascript">


        function getHdnMarkupId() {
            return "<%=hdnMarkup.ClientID %>";
        }

        function getPrvId() {
            return "<%=prv.ClientID %>";
        }

        function Load_Data() {
            var markUp = document.getElementById('markItUp').value;
            document.getElementById('markItUp').value = "";
            __doPostBack(markUp, 'load');
        }

        function show_preview() {
            var postedDate = $('#txtPostdate').val();
            if (postedDate.length == 0) {
                $('#MessageBox').removeClass();
                $('#MessageBox').addClass('error');
                $('#MessageBox p').html('Post date is required.');
            }
            else {
                var markUp = document.getElementById('markItUp').value;
                document.getElementById('markItUp').value = "";
                __doPostBack(markUp, 'save');
            }
        }

        $(document).ready(function () {
            $('#markItUp').markItUp(mySettings);

            $('#ddlEnterprise').change(function () {
                if (document.getElementById('markItUp')) {
                    document.getElementById('markItUp').value = "";
                }
                __doPostBack('', 'onchange');
            });

            $('#cmbPage').change(function () {
                $('#hiddenSelectedId').val($('#cmbPage option:selected').val());
            });

            $('.add').click(function () {
                $.markItUp({ openWith: '<opening tag>',
                    closeWith: '<\/closing tag>',
                    placeHolder: "New content"
                }
				);
                return false;
            });

            $('#btnLogOut').click(function () {
                if (document.getElementById('markItUp')) {
                    document.getElementById('markItUp').value = "";
                }
                __doPostBack('', 'onLogOff');
            });

            $("#lbChangePassword").click(function () {
                if (document.getElementById('markItUp')) {
                    document.getElementById('markItUp').value = "";
                }
                __doPostBack('', 'onChangePassword');

            });

            $('.toggle').click(function () {
                if ($("#markItUp.markItUpEditor").length === 1) {
                    $("#markItUp").markItUpRemove();
                    $("span", this).text("get markItUp! back");
                } else {
                    $('#markItUp').markItUp(mySettings);
                    $("span", this).text("remove markItUp!");
                }
                return false;
            });
        });
    </script>
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
                        z-index: 10000" Visible="false" ClientIDMode="Static">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bodyContainer" runat="server">
    <asp:Panel ID="pnlEditorMessage" runat="server">
        <ucdm:DisplayMessage ID="EditorMessage" runat="server" DisplayMessageWidth="700"
            ShowCloseButton="false"></ucdm:DisplayMessage>
    </asp:Panel>
    <asp:Panel ID="CMSPanel" runat="server">
        <div>
            <asp:HiddenField ID="hiddenSelectedId" runat="server" ClientIDMode="Static" />
            &nbsp;
            <asp:Label ID="LabelPage" runat="server" Text="Page: " Font-Bold="true"></asp:Label>
            <asp:DropDownList ID="cmbPage" runat="server" ClientIDMode="Static">
            </asp:DropDownList>
            <div class="validator">
                <asp:RequiredFieldValidator ID="rfvPage" runat="server" Text="*" Display="Dynamic"
                    ControlToValidate="cmbPage" InitialValue="0" ValidationGroup="upnlPage">
                </asp:RequiredFieldValidator></div>
            &nbsp;
            <asp:Label ID="LabelDate" runat="server" Text="PostDate: "></asp:Label>
            <asp:TextBox ID="txtPostdate" AutoPostBack="false" runat="server" CssClass="datePicker"
                Enabled="true" ClientIDMode="Static">
            </asp:TextBox>
            <input type="button" onclick="Load_Data();" value="Load" />
            <input type="button" onclick="show_preview();" value="Save" />
        </div>
        <asp:TextBox ID="markItUp" TextMode="MultiLine" ClientIDMode="Static" Columns="80"
            Rows="20" runat="server">
        </asp:TextBox>
        <p>
            &nbsp; <b>PREVIEW</b>
            <input type="hidden" id="hdnMarkup" runat="server" /></p>
        <div id="prv" class="markItUpPreviewFrame2" runat="server">
        </div>
        <br />
    </asp:Panel>
</asp:Content>
