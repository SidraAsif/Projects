
// controls id
var _hdnContentType = "hdnContentType";
var _hdnSectionId = "hdnSectionID";
var _hdnTreeNodeID = "hdnTreeNodeID";
var _hdnDeletedNodeID = "hdnDeletedNodeID";

// shared variables
var selectedNode;

$(document).ready(function () {
    var browser = navigator.appName;

    // Remove td to fix close[x] tag wrapping in IE
    if (browser == "Microsoft Internet Explorer")
        $('td.trow').remove();

    // Fetch store values from hidden fields
    var treeNodeId = $('#' + _hdnTreeNodeID).val();

    if (treeNodeId != null && treeNodeId != "") {
        $('#' + treeNodeId).css('color', 'blue');
    }

    StyleDeleteNode();

    $('div#ctl00_bodyContainer_TreeControl_treeView a').live('click', function () {
        //        $(this).css('color', 'blue');
        $('#' + _hdnTreeNodeID).val(this.id);
    });

    $("input#ctl00_innerMenuConatiner_btnUploadDocuments").click(function (e) {
        $("#lightbox, #lightbox-popup").fadeIn(300);
    });

    $("a#close-popup").click(function () {
        $("#lightbox, #lightbox-popup").fadeOut(300);

        var toolSectionId = $('#' + _hdnSectionId).val();
        var contentType = $('#' + _hdnContentType).val();

        onclicknode(contentType, toolSectionId);

    });

});

function onclicknode(ContentType, ToolSectionId) {
    $('#' + _hdnSectionId).val(ToolSectionId);
    $('#' + _hdnContentType).val(ContentType);

    var arg = ContentType + '/' + ToolSectionId;
    __doPostBack('UpdatePanelControl', arg);

    StyleDeleteNode();
}
function StyleDeleteNode() {
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