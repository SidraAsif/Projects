
Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (evt, args) {

    var sectionId;
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    function EndRequestHandler(sender, args) {
        var Active = $.QueryString("Active");
        Active = (!Active) ? "null" : Active
        $('#' + Active).css("color", "blue");
    }

});

$(document).ready(function () {

    var Active = $.QueryString("Active");
    Active = (!Active) ? "null" : Active
    $('#' + Active).css("color", "blue");

    // on each node click
    $('#ctl00_bodyContainer_TreeControl_treeView a').live('click', function () {
        $('#hiddenActiveNode').val(this.id);
    });

    BindTopLevelFolderEvent();
    StyleDeletedNode();

});

$('.treeView a').live('click', (function () {
    if ($(this).html().toLowerCase().indexOf("<img") == -1) {
        if ($(this).attr('href').indexOf("javascript:return false") != -1) {
            var thisId = $(this).attr('id');
            var originalId = thisId.replace('ctl00_bodyContainer_TreeControl_treeViewt', '');

            var nId = 'ctl00_bodyContainer_TreeControl_treeViewn' + originalId;
            var nodeId = nId + 'Nodes';
            $(this).css("color", "black");
            window.location = 'javascript:TreeView_ToggleNode(ctl00_bodyContainer_TreeControl_treeView_Data,' + originalId + ',' + nId + '," ",' + nodeId + ')';
            //comment below line and add above line for fixing tree collapse issue.
            //TreeView_ToggleNode(ctl00_bodyContainer_TreeControl_treeView_Data, originalId, document.getElementById(nId), ' ', document.getElementById(nodeId));
            StyleDeletedNode();
        }
    }
}));

function next(elem) {
    do {
        elem = elem.nextSibling;
    } while (elem && elem.nodeType != 1);
    return elem;
}


function onnodeclick(SectionType,SectionId, ProjectUsageIdID,SiteId, path) {
    $('#hdnProjectUsageID').val(ProjectUsageIdID);
   
    var arg = SectionType + '/' + ProjectUsageIdID;
    var ActiveNodeId = $('#hiddenActiveNode').val();

    if (ActiveNodeId.length == 0) {
        ActiveNodeId = $.QueryString("Active");
        ActiveNodeId = (!ActiveNodeId) ? "null" : ActiveNodeId
    }
    sectionId = ProjectUsageIdID;
    var functionCall = "Projects.aspx?NodeContentType=" + SectionType + "&NodeSectionID=" + SectionId + "&NodeProjectUsageID=" + ProjectUsageIdID + '&Active=' + ActiveNodeId + '&SiteId=' + SiteId + '&Path=' + path;

    setTimeout("window.location = \"" + functionCall + "\"", 2000);
}

function ontemplatenodeclick(ContentType, ProjectID, TemplateId, path) {

    $('#hdnProjectID').val(ProjectID);
    var arg = ContentType + '/' + ProjectID;
    var ActiveNodeId = $('#hiddenActiveNode').val();

    if (ActiveNodeId.length == 0) {
        ActiveNodeId = $.QueryString("Active");
        ActiveNodeId = (!ActiveNodeId) ? "null" : ActiveNodeId
    }
    sectionId = ProjectID;
    var functionCall = "Projects.aspx?NodeContentType=" + ContentType + "&NodeProjectID=" + ProjectID + "&TemplateID=" + TemplateId + '&Active=' + ActiveNodeId + '&Path=' + path;

    setTimeout("window.location = \"" + functionCall + "\"", 2000);
}

//generate context menu and Manuipulate the folder structure by adding and deleting the folder

$(document).ready(function () {

    //    var treeType = $('#hdnScreen').val();
    //    if (treeType != 'ProjectSection')
    BindContextMenu();

    $(document).click(function (e) {
        var targ;
        if (!e) var e = window.event;
        if (e.target) targ = e.target;
        else if (e.srcElement) targ = e.srcElement;

        if ($(targ).is('a')) {
            if (e.target.id) {
                var nodeId = $('#' + e.target.id);
                var nodeLink = nodeId.attr('href');
                if (nodeLink) {
                    if (nodeLink.indexOf('false') != -1) {
                        nodeId.css('color', 'black');
                    }
                }
            }
        }
    });

});

//This method try to open the context menu on folder hierarchy

function BindContextMenu() {

    $(".treeView a").contextMenu({
        menu: 'myMenu'
    }, function (action, el, pos) {
        if (action == "delete") {
            //This condition used to delete the folder from the tree hierarchy 
            //but if in the tree hierarchy My Tools is present then this option will be disable

            if ($(el).text() != 'My Tools') {
                apprise('Are you sure you want to permanently delete this file?', { 'verify': true }, function (response) {
                    if (response) {

                        var treeType = $('#hdnScreen').val();
                        var data = $('#hdnActiveURl').val().split(',');

                        var sectionId = "";

                        for (var index =0; index < data.length; index++) {
                            sectionId = data[index+1];
                            break;
                        }

                        //getting section id of the element by refining them through regression expression

                        //sectionId = sectionId.replace(/(^\d+)(.+$)/i, '$1');
                        if (sectionId.length > 3) {
                            sectionId = sectionId.replace("'", '');
                            sectionId = sectionId.replace(/(^\d+)(.+$)/i, '$1');
                        }
                        DeleteFolder(sectionId, treeType);
                    }
                    else {
                    }
                }
            );
            }
        }

        if (action == 'rename') {
            //This condition used to rename the folder from the tree hierarchy 
            //but if in the tree hierarchy My Tools is present then this option will be disable
            //alert(action);

            if ($(el).text() != 'My Tools') {
                $('.lightbox, #lightbox-rename').fadeIn(300);
                $('.bttadd').live('click', function () {


                    var nodeId = $("#hdnActiveId").val();
                    var nodeelement = $('#' + nodeId);
                    var treeType = $('#hdnScreen').val();

                    var data = $('#hdnActiveURl').val().split(',');

                    for (var index = 0; index < data.length; index++) {
                        sectionId = data[index + 1];
                        break;
                    }
                    sectionId = sectionId.replace(/(^\d+)(.+$)/i, '$1');
                    if (sectionId.length > 3) {
                        sectionId = sectionId.replace("'", '');
                        sectionId = sectionId.replace(/(^\d+)(.+$)/i, '$1');
                    }

                    //This function check in the folder tree hierarchy but if the new name of the folder is present in the given hierarchy 
                    //then this function will block the user to change the name

                    RenameValidation(sectionId, treeType, el);

                    //'background-color': '#FFBABA'
                });

                $('.bttcancel, a.close').live('click', function () {
                    $('.lightbox-popup, .lightbox').fadeOut(300);
                    $('#txtsave').val('');
                    $('#renameMessage').css('display', 'none');
                    window.location.href = window.location.href;
                });
            }
        }

        if (action == 'add') {

            //This condition used to add the folder in the tree hierarchy 
            //but if in the tree hierarchy My Tools is present then this option will be disable

            if ($(el).text() != 'My Tools') {
                $('.lightbox, #lightbox-add').fadeIn(300);
                $('.bttadd').live('click', function () {
                    var nodeId = $("#hdnActiveId").val();
                    var nodeelement = $('#' + nodeId);
                    var treeType = $('#hdnScreen').val();

                    var data = $('#hdnActiveURl').val().split(',');

                    for (var index = 0; index < data.length; index++) {
                        sectionId = data[index + 1];
                        break;
                    }

                    sectionId = sectionId.replace(/(^\d+)(.+$)/i, '$1');

                    if (sectionId.length > 3) {
                        sectionId = sectionId.replace("'", '');
                        sectionId = sectionId.replace(/(^\d+)(.+$)/i, '$1');
                    }

                    var restrictedFoldersList = $('#hdnRestrictedFolderList').val();
                    var restrictedFolders = restrictedFoldersList.split(",");
                    var restrictedFoldersCount = restrictedFolders.length - 1;
                    var isRestrictedFolder = "false";

                    for (index = 0; index < restrictedFoldersCount; index++) {
                        if (restrictedFolders[index] == $(el).text()) {
                            isRestrictedFolder = "true";
                        }
                    }

                    if (isRestrictedFolder == "false") {
                        $('#hdnEnterpriseId').val(0);
                    }

                    $('#hdnIsJumpMaterialFolder').val(0);
                    var jumpMaterialList = $('#hdnJumpMaterialList').val();
                    var jumpMaterialFolders = jumpMaterialList.split(",");
                    var jumpMaterialFoldersCount = jumpMaterialFolders.length - 1;                    

                    for (index = 0; index < jumpMaterialFoldersCount; index++) {
                        if (jumpMaterialFolders[index] == $(el).text()) {
                            $('#hdnIsJumpMaterialFolder').val(1);
                        }
                    }

                    //This method used to check in the hierachy if the same name of folder is present having same id then it will block the user
                    //from adding
                    GetIds(sectionId, treeType, el, pos);


                });

                $('.bttcancel, a.close').live('click', function () {
                    if ($('#hdnLastInsertedId').val() != '') {
                        var url = window.location.href;
                        if (url.indexOf("?") != -1)
                            url = url.substring(0, url.indexOf("?"));
                        url = url + "?NodeType=" + $('#hdnLastInsertedId').val();
                        window.location.href = url;
                    }
                    else {
                    }
                    $('.lightbox-popup, .lightbox').fadeOut(300);
                    $('#txtadd').val('');
                    $('#addMessage').css('display', 'none');
                    window.location.href = window.location.href;
                });
            }
        }
    });

}

function RenameFolder(treeType, sectionId, newfolderName) {

    $.ajax({
        type: "POST",
        url: "../WebServices/FolderManipulation.asmx/RenameFolder",
        data: "{'treeType':'" + treeType + "' ,'sectionId':'" + sectionId + "', 'newFolderName':'" + newfolderName + "'}",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
        },
        complete: function () {
        },
        success: function (response) {
        },
        failure: function (msg) {

        }
    });
}


//Delete the folder from the folder tree hierarchy

function DeleteFolder(sectionId, treeType) {
    $.ajax({
        type: "POST",
        url: "../WebServices/FolderManipulation.asmx/DeleteFolder",
        data: "{'sectionId':'" + sectionId + "' ,'treeType':'" + treeType + "'}",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
        },
        complete: function () {
        },
        success: function (response) {
            if (response.d) {
                var nodeId = $("#hdnActiveId").val();
                var nodeelement = $('#' + nodeId);
                var Ids = response.d.toString().split(',');
                var sectionId = "";

                for (var v = 0; v <= Ids.length + 1; v++) {
                    $('#treeContainer a').each(function (x) {

                        var check = $(this).attr('href');
                        var myreg = /javascript/;

                        if (myreg.test(check)) {
                            var data = $(this).attr('href').split(',');
                            for (var index = 0; index < data.length; index++) {
                                sectionId = data[index + 1];
                                break;
                            }
                            if (sectionId != undefined) {
                                sectionId = sectionId.replace(/(^\d+)(.+$)/i, '$1');

                                if (sectionId.length > 3) {
                                    sectionId = sectionId.replace("'", '');
                                    sectionId = sectionId.replace(/(^\d+)(.+$)/i, '$1');
                                }

                                if (Ids[v] == sectionId) {
                                    $(this).css({ 'text-decoration': 'line-through', 'cursor': 'auto'}).live('click', function () {
                                        return false;
                                    });
                                }
                            }
                        }

                    });
                }
            }
               BindTopLevelFolderEvent();
               BindTreeRootEvent(nodeId);
            
        },
        failure: function (msg) {
        }
    });
}

//Add the folder in the folder tree hierarchy

function AddNewFolder(sectionId, treeType, folderName, el, position, partNode, enterpriseId) {
    $.ajax({
        type: 'POST',
        url: "../WebServices/FolderManipulation.asmx/AddFolder",
        data: "{'sectionId':'" + sectionId + "' ,'treeType':'" + treeType + "', 'folderName':'" + folderName + "', 'enterpriseId':'" + enterpriseId + "', 'isJumpMaterialFolder':'" + $('#hdnIsJumpMaterialFolder').val() + "'}",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
        },
        complete: function () { },
        success: function (response) {
            $('#hdnLastInsertedId').val(response.d);
            var nodeId = $("#hdnActiveId").val();
            var nodeParent = $('#' + nodeId).parent();
            var nodeelement = $('#' + nodeId).parent().parent();
            numberOfAddedIds = new Array();
            numberOfAddedIds[count] = sectionId;
            count++;

            $('#' + nodeId).prepend($('<img></img>').attr({ 'src': '../Themes/Images/Minus.png', 'class': function () { return folderName.replace(/[^a-z]/gi, '') } }).css({ 'position': 'relative', 'float': 'left', 'clear': 'both', 'margin': '2px 0 0 -2px', 'padding': '1.5px' }));
            $('.' + folderName.replace(/[^a-z]/gi, '')).css('display', 'none');
            if (partNode == 'false') {
                $('#' + nodeId).css('margin', '0 0 0 0px');
                $('.' + folderName.replace(/[^a-z]/gi, '')).css('display', 'none');
                nodeelement.find('img').css({ 'position': 'relative', 'margin': '4px 0 0 2px' });
                nodeelement.find('td').css({ 'vertical-align': 'top' });
            }

            var newNodeDiv = $('<div></div>').attr('id', folderName).css({ 'margin': '0 0 0 18px', 'width': '30px' }).append($('<a></a>').click(function () { $('#' + folderName).show() }).attr({ 'id': function (i) { return (folderName + i); }, 'href': "javascript:onclicknode('" + folderName + "','" + $('#hdnLastInsertedId').val() + "');", 'margin': '0 0 0 2px' }).text(folderName));
            $('#hdnElementQuantity').val(response.d);

            nodeParent.append(newNodeDiv);
            if ($(el)) {
                $('#minusimg').css({ 'display': 'block', top: (position.y + 6) + 'px', left: (position.x - 3) + 'px' });
            }

            BindContextMenu();
        },
        failure: function () { }
    });
}
var count = 0;
var numberOfAddedIds = "";

//Before adding folder validate the user on the basis of ids

function GetIds(sectionId, treeType, el, pos) {
    $.ajax({
        type: 'POST',
        url: "../WebServices/FolderManipulation.asmx/GetName",
        data: "{'sectionId':'" + sectionId + "','treeType':'" + treeType + "'}",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
        },
        complete: function () { },
        success: function (response1) {
            var name = "";
            if (response1.d != null)
                name = response1.d.split(',');

            var flag = true;
            if ($('#txtadd').val() != '') {
                for (var index = 0; index < name.length; index++) {
                    var input = $.trim($('#txtadd').val().toLowerCase());
                    if (name[index].toLowerCase() == input || input == $(el).text().toLowerCase()) {
                        $('#Span2').text('Folder name is already Exist.').css({ 'display': 'block', 'margin': '0 0 0 2em' });
                        $('#addMessage').css({ 'color': '#D8000C', 'display': 'block', 'background-image': 'url(../Themes/Images/error.png)', 'background-repeat': 'no-repeat', 'background-color': '#FFBABA', 'border': '1px solid #D8000C' });
                        flag = false;
                    }
                }
            }
            else {
                $('#Span2').text('Information is required.').css({ 'display': 'block', 'margin': '0 0 0 2em' });
                $('#addMessage').css({ 'color': '#D8000C', 'display': 'block', 'background-image': 'url(../Themes/Images/error.png)', 'background-repeat': 'no-repeat', 'background-color': '#FFBABA', 'border': '1px solid #D8000C' });
                flag = false;
            }

            if (flag == true) {
                //var regExp = /^[A-Za-z ]+$/;
                var regExp = /^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)_^%$#!~@,-]{1,50}$/;
                var name = $('#txtadd').val();
                var result = regExp.test(name)

                if (name != '' && name != null) {

                    if (result) {
                        var response = $('#txtadd').val();
                        response = $.trim(response);
                        var sectionId = "";
                        var treeType = $('#hdnScreen').val();
                        var enterpriseId = $('#hdnEnterpriseId').val();
                        var data = $('#hdnActiveURl').val().split(',');
                        var dat = $('#hdnActiveURl').val().split(',');
                        var parentNode = dat[0].split(':')[1].substr(7, 11).toString();

                        for (var index = 0; index < data.length; index++) {
                            sectionId = data[index + 1];
                            break;
                        }
                        sectionId = sectionId.replace(/(^\d+)(.+$)/i, '$1');
                        if (sectionId.length > 3) {
                            sectionId = sectionId.replace("'", '');
                            sectionId = sectionId.replace(/(^\d+)(.+$)/i, '$1');
                        }

                        //After Passing Verification this will add the folder in the databsae

                        AddNewFolder(sectionId, treeType, response, el, pos, parentNode, enterpriseId);

                        $('#Span2').text('Your folder has been Added successfully.').css({ 'display': 'block', 'margin': '0 0 0 2em' });
                        $('#addMessage').css({ 'display': 'block', 'color': '#4F8A10', 'background-image': 'url(../Themes/Images/success.png)', 'background-repeat': 'no-repeat', 'background-color': '#DFF2BF', 'border': '1px solid' });


                        $('#txtadd').val('');
                        $('#treeContainer a').css({ 'text-decoration': 'none', 'color': 'black' });
                    }

                    else {

                        $('#Span2').text('Please enter a valid folder name.').css({ 'display': 'block', 'margin': '0 0 0 2em' });
                        $('#addMessage').css({ 'display': 'block', 'color': '#D8000C', 'background-image': 'url(../Themes/Images/error.png)', 'background-repeat': 'no-repeat', 'background-color': '#FFBABA', 'border': '1px solid' });
                    }
                }
                else {
                    $('#Span2').text('Information is required.').css({ 'display': 'block', 'margin': '0 0 0 2em' });
                    $('#addMessage').css({ 'color': '#D8000C', 'display': 'block', 'background-image': 'url(../Themes/Images/error.png)', 'background-repeat': 'no-repeat', 'background-color': '#FFBABA', 'border': '1px solid #D8000C' });

                }
            }
        },
        failure: function () { }
    });
}

//Before renaming the folder validate the user that whether this name is already exist or not
function RenameValidation(sectionId, treeType, el) {

    $.ajax({
        type: 'POST',
        url: "../WebServices/FolderManipulation.asmx/GetName",
        data: "{'sectionId':'" + sectionId + "','treeType':'" + treeType + "'}",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () { },
        complete: function () { },
        success: function (response1) {
            var name = "";
            var flag = true;

            if (response1.d != null)
                var name = response1.d.split(',');

            if ($('#txtsave').val() != '') {

                for (var index = 0; index < name.length; index++) {
                    var input = $.trim($('#txtsave').val().toLowerCase());
                    if (name[index].toLowerCase() == input || input == $(el).text().toLowerCase()) {
                        $('#txtrename').text('Folder name is already Exist.').css({ 'display': 'block', 'margin': '0 0 0 2em' });
                        $('#renameMessage').css({ 'color': '#D8000C', 'display': 'block', 'background-image': 'url(../Themes/Images/error.png)', 'background-repeat': 'no-repeat', 'background-color': '#FFBABA', 'border': '1px solid #D8000C' });
                        flag = false;
                    }
                }
            }
            else {
                $('#txtrename').text('Information is required.').css({ 'display': 'block', 'margin': '0 0 0 2em' });
                $('#renameMessage').css({ 'color': '#D8000C', 'display': 'block', 'background-image': 'url(../Themes/Images/error.png)', 'background-repeat': 'no-repeat', 'background-color': '#FFBABA', 'border': '1px solid #D8000C' });
            }

            if (flag == true) {
                $('#txtrename').css('display', 'none');
                //var regExp = /^[a-zA-Z ]*$/;
                var regExp = /^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)_^%$#!~@,-]{1,50}$/;
                var name = $('#txtsave').val();
                var result = regExp.test(name)

                if (name != '' && name != null) {
                    if (result) {

                        $('#txtrename').text('Your folder has been successfully renamed.').css({ 'display': 'block', 'margin': '0 0 0 2em' });
                        $('#renameMessage').css({ 'display': 'block', 'color': '#4F8A10', 'background-image': 'url(../Themes/Images/success.png)', 'background-repeat': 'no-repeat', 'background-color': '#DFF2BF', 'border': '1px solid' });

                        var folderName = $('#txtsave').val();
                        var treeType = $('#hdnScreen').val();
                        var sectionId = "";
                        var treeType = $('#hdnScreen').val();
                        var data = $('#hdnActiveURl').val().split(',');
                        var dat = $('#hdnActiveURl').val().split(',');
                        var parentNode = dat[0].split(':')[1].substr(7, 11).toString();

                        for (var index = 0; index < data.length; index++) {
                            sectionId = data[index + 1];
                            break;
                        }
                        sectionId = sectionId.replace(/(^\d+)(.+$)/i, '$1');
                        if (sectionId.length > 3) {
                            sectionId = sectionId.replace("'", '');
                            sectionId = sectionId.replace(/(^\d+)(.+$)/i, '$1');
                        }


                        //After passing the Validation this method will rename the folder in the database
                        RenameFolder(treeType, sectionId, folderName);

                        $('#txtsave').val('');
                        var nodeId = $("#hdnActiveId").val();
                        $("#" + nodeId).text(folderName);
                    }
                    else {
                        $('#txtrename').css({ 'display': 'block', 'margin': '0 0 0 2em' });
                        $('#renameMessage').css({ 'color': '#D8000C', 'display': 'block', 'background-image': 'url(../Themes/Images/error.png)', 'background-repeat': 'no-repeat', 'background-color': '#FFBABA', 'border': '1px solid #D8000C' });
                    }
                }
                else {
                    $('#txtrename').text('Information is required.').css({ 'display': 'block', 'margin': '0 0 0 2em' });
                    $('#renameMessage').css({ 'color': '#D8000C', 'display': 'block', 'background-image': 'url(../Themes/Images/error.png)', 'background-repeat': 'no-repeat', 'background-color': '#FFBABA', 'border': '1px solid #D8000C' });

                }
            }

        },
        async: true,
        failure: function () { }
    });

}

// Top Leve Folder event bind()
function BindTopLevelFolderEvent() {


    // To add top level folder
    $("#imgAddRootFolder").live('click', function () {

        $('.lightbox, #lightbox-add').fadeIn(300);
        $(".bttadd").attr("onClick", "javascript:AddTopLevelFolder();");

        // referesh view to delete top level folder
        $('.bttcancel, a.close').live('click', function () {
            $(".bttadd").removeAttr("onClick");
            window.location.href = window.location.href;
        });
    });
}

function AddTopLevelFolder() {
    var regExp = /^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,50}$/;

    var folderName = $("#txtadd").val();
    var expressVarify = regExp.test(folderName);

    if (folderName.length == 0)
        ShowMessage("Folder name is required.", "error");
    else if (!expressVarify)
        ShowMessage("Folder name is invalid.", "error");
    else {
        var treeType = "";
        var url = window.location.href;
        if (url.indexOf("ToolBox") != -1)
            treeType = "ToolSection";
        else if (url.indexOf("Library") != -1)
            treeType = "LibrarySection";

        $.ajax({
            type: 'POST',
            url: "../WebServices/FolderManipulation.asmx/AddTopLevelFolder",
            data: "{'folderName':'" + folderName + "','treeType':'" + treeType + "'}",
            contentType: "application/json; charset=utf-8",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.d)
                    ShowMessage("Top level folder added successfully", "success");
                else
                    ShowMessage("Folder name is already exists.", "error");
                $('#txtadd').val('');
            },
            failure: function () { }
        });
    }

}

// function to use the customer user control on client side
function ShowMessage(message, type) {
    $('#msgRow #MessageBox').removeClass();
    $('#msgRow #MessageBox').addClass(type);
    $('#msgRow #MessageBox p').html(message);
}

function BindTreeRootEvent(deletedTreeNode) {
    $('.treeView a').live('click', (function () {
        if ($(this).html().toLowerCase().indexOf("<img") == -1) {
            if ($(this).attr('href').indexOf("javascript:return false") != -1) {
                var thisId = $(this).attr('id');
                var originalId = thisId.replace('ctl00_bodyContainer_TreeControl_treeViewt', '');

                var nId = 'ctl00_bodyContainer_TreeControl_treeViewn' + originalId;
                var nodeId = nId + 'Nodes';
                $(this).attr('href', 'javascript:TreeView_ToggleNode(ctl00_bodyContainer_TreeControl_treeView_Data,' + originalId + ',' + nId + '," ",' + nodeId + ')');
                StyleDeletedNode();
            }
            else {
                $('#hdnTreeNodeID').val(this.id);
                $('#' + deletedTreeNode).css({ 'text-decoration': 'line-through', 'cursor': 'auto' });
                $('#' + _hdnDeletedNodeID).val(deletedTreeNode);
            }
        }
    }));
}
function StyleDeletedNode() {
    var _hdnDeletedNodeID;
    if (_hdnDeletedNodeID != undefined || _hdnDeletedNodeID!=null) {
        var deletedNodeId = $('#' + _hdnDeletedNodeID).val();
        var originalId = deletedNodeId.replace('ctl00_bodyContainer_TreeControl_treeViewt', '');

        var nId = 'ctl00_bodyContainer_TreeControl_treeViewn' + originalId;
        var noId = 'ctl00_bodyContainer_TreeControl_treeViewt' + originalId
        var nodeId = nId + 'Nodes';
        $('#' + nId).css({ 'text-decoration': 'line-through', 'cursor': 'auto' });
        $('#' + noId).css({ 'text-decoration': 'line-through', 'cursor': 'auto' });
        $('#' + nodeId).css({ 'text-decoration': 'line-through', 'cursor': 'auto' }).live('click', function () {
            return false;
        });
    }
}