// controls id
var _hdnLibrarySectionID = "hdnLibrarySectionID";
var _hdnTreeNodeID = "hdnTreeNodeID";
var _hdnContentType = "hdnContentType";

// shared variables
var contentype;

$(document).ready(function () {
    var browser = navigator.appName;

    // Remove td to fix close[x] tag wrapping in IE
    if (browser == "Microsoft Internet Explorer")
        $('td.trow').remove();

    // keep active node selected
    var treeNodeId = $('#' + _hdnTreeNodeID).val();
    if (treeNodeId != null && treeNodeId != "") {
        $('#' + treeNodeId).css('color', 'blue');
    }

    // track active node id on each click
    $('div#ctl00_bodyContainer_TreeControl_treeView a').live('click', function () {
//        $(this).css('color', 'blue');
        $('#' + _hdnTreeNodeID).val(this.id);       

    });

    $("input#ctl00_innerMenuConatiner_btnUploadDocuments").click(function (e) {
        $("#lightbox, #lightbox-popup").fadeIn(300);
    });

    $("a#close-popup").click(function () {
        $("#lightbox, #lightbox-popup").fadeOut(300);

        contentype = $('#' + _hdnContentType).val();
        var librarySectionId = $('#' + _hdnLibrarySectionID).val();

        onclicknode(contentype, librarySectionId);
    });

    //$('.body-container-left-tree').css("min-height", 430);
});

function onclicknode(ContentType, LibrarySectionId, path) {
    $('#' + _hdnLibrarySectionID).val(LibrarySectionId);
    $('#' + _hdnContentType).val(ContentType);
    
    $('#hdnTreeNodePath').val(path);

    var arg = ContentType + '/' + LibrarySectionId;
    __doPostBack('UpdatePanelControl', arg);
    var name = $('#hdnElementQuantity').val();    
    $('#' + name).show();
    //window.location = "Library.aspx?NodeContentType=" + ContentType + "&NodeProjectID=" + ProjectID + '&Active=' + ActiveNodeId + '&Path=' + path;
    
}