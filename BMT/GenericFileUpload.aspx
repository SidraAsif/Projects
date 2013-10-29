<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenericFileUpload.aspx.cs"
    Inherits="BMT.GenericFileUpload" %>

<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Upload File</title>
    <link rel="Stylesheet" type="text/css" href="Themes/style.css" />
    <style type="text/css">
        a
        {
            color: white;
        }
        #text
        {
            margin: 3px; /*Default is 25px*/
        }
        ul
        {
            list-style: none; /*Default is none;*/
            float: left;
            margin: 0 auto;
        }
        
        .uploadcontainer
        {
            padding: 0;
            margin: 0 auto;
            width: auto;
            height: auto;
        }
    </style>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jQuery-1.7.1.min.js") %>"></script>
    <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/ajaxfileupload.js") %>"></script>
    <script type="text/javascript">

        function callUploader() {

            var regExp = /^[a-zA-Z0-9\s\~\!\@\#\$\%\^\&\*\(\)\-\_\=\+\[\{\]\}\;\:\'\'\<\>\.\,\?]{1,50}$/;
            //var regExp = /^["\"]{1,50}$/;
            var docName = document.getElementById('txtName').value;
            var expressVarify = regExp.test(docName);
            if (!expressVarify) {
                $('#MessageBox').addClass('error');
                $('#MessageBox p').html('File name is invalid.');
                return false;
            }

            if (document.getElementById('txtName').value == "") {
                $('#MessageBox').addClass('error');
                $('#MessageBox p').html('Name is a required field.');
                return false;
            }
            else if (document.getElementById('txtName').value != "")
                document.getElementById('lblmessage').innerHTML = "";

            $('#uploadcontainer .files').empty();
            var file = $('#FUDoc').val();
            var fileName = file.split('\\').pop();

            if (fileName.length == 0) {
                $('#MessageBox').addClass('error');
                $('#MessageBox p').html('Please select a file.');
                return false;
            }
            else if (fileName.length != 0)
                document.getElementById('lblmessage').innerHTML = "";


            $('#imgUploadDoc').attr('src', 'Themes/Images/uploading.gif');

            $.ajaxFileUpload({
                url: 'GenericFileHandler.ashx',
                secureuri: false,
                fileElementId: 'FUDoc',
                data: { TableName: $('#hiddenTableName').val(),
                    Name: document.getElementById('txtName').value,
                    Description: document.getElementById('txtDesc').value,
                    PracticeId: $('#hiddenPracticeId').val(),
                    SectionId: $('#hiddenSectionId').val()
                },
                complete: function () {
                    $('#imgUploadDoc').attr('src', 'Themes/Images/uploadimg.png');
                },
                success: function (response) {

                    var receivedResponse = $(response).find("pre").html();
                    
                    if (receivedResponse == 'success') {
                        $('#MessageBox').removeClass();
                        $('#MessageBox').addClass('success');
                        $('#MessageBox p').html('File "' + fileName + '" uploaded successfully.');
                        document.getElementById('txtName').value = "";
                        document.getElementById('txtDesc').value = "";
                        document.getElementById('FUDoc').value = "";
                    }
                    else if (receivedResponse == 'errorFileExt') {
                        $('#MessageBox').addClass('error');
                        $('#MessageBox p').html('only images & documents are allowed.');
                    }
                    else {
                        $('#MessageBox').addClass('error');
                        $('#MessageBox p').html('An error occured while uploading the file.');
                    }
                },
                error: function (response) {
                    $('#MessageBox').addClass('error');
                    $('#MessageBox p').html('An error occured while uploading the file.');
                }
            }
		)
            return false;

        }


        // **********************************************************
        // TODO: Disable tab key on last control focus
        // **********************************************************

        $(document).ready(function () {
            $('input:text:first').focus();

            $('#FUDoc').live('focus', function (e) {
                $(this).keypress(function (e) {
                    if (e.keyCode == '9') { e.preventDefault(); }
                });
            });
        });   
        
    </script>
</head>
<body>
    <form id="frmFileUploader" runat="server" action="" method="post" enctype="multipart/form-data">
    <asp:HiddenField ID="hiddenTableName" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenPracticeId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hiddenSectionId" runat="server" ClientIDMode="Static" />
    <asp:Label ID="lblmessage" Width="100%" ForeColor="Red" runat="server" Text=""></asp:Label>
    <table width="100%">
        <tr>
            <td colspan="3">
                <ucdm:DisplayMessage ID="message" runat="server" DisplayMessageWidth="380" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <ul>
                    <li id="uploadcontainer" class="uploadcontainer">
                        <ol class="files">
                        </ol>
                    </li>
                </ul>
            </td>
        </tr>
        <tr>
            <td>
                <p>
                    Name:*</p>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtName" runat="server" CssClass="text-field02" ClientIDMode="Static"
                    MaxLength="50" Width="80%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <p>
                    Description:</p>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtDesc" runat="server" CssClass="text-field02" ClientIDMode="Static"
                    Height="31px" MaxLength="5000" Style="resize: none;" TextMode="MultiLine" Width="80%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <p>
                    Document:</p>
            </td>
            <td colspan="2">
                <asp:FileUpload ID="FUDoc" runat="server" ClientIDMode="Static" Width="60%" />
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <asp:Image ID="imgUploadDoc" runat="server" ClientIDMode="Static" onClick="javascript:callUploader();"
                    ImageUrl="~/Themes/Images/uploadimg.png" />
            </td>
        </tr>
    </table>
    <table style="height: 20px;">
        <tr>
            <td>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
