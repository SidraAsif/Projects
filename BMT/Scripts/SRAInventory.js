function SRAtoggle(elementId) {
    if ($('#tableElement' + elementId).is(':visible')) {
        $('#imgElement' + elementId).attr('src', '../Themes/Images/Plus-1.png');
        $('#tableElement' + elementId).hide();
    }
    else {
        $('.element-table').hide();
        $('#tableElement' + elementId).show();
        $('img.toggle-img').attr('src', '../Themes/Images/Plus-1.png');
        $('#imgElement' + elementId).attr('src', '../Themes/Images/Minus-1.png');
    }
}


$(document).ready(function () {
    $('.element-table').hide();
});


function editLabel(sourceId, hiddenSourceId) {
    if (!$('#' + hiddenSourceId).is(':disabled')) {
        $('#' + hiddenSourceId).show();
        $('#' + hiddenSourceId).val($('#' + sourceId).text());
        $('#' + hiddenSourceId).css("height", "14px");
        $('#' + hiddenSourceId).css("min-height", "14px");
        $('#' + hiddenSourceId).focus();

        $('#' + sourceId).css("display", "none");
        $('#' + sourceId).css("width", "0px");
        $('#' + sourceId).removeClass();
        $('#' + sourceId).removeProp("width");
    }
}

function setLabel(sourceId, hiddenSourceId, className) {
    var tempWidth = parseInt($('#' + hiddenSourceId).css("width").replace("px", "")) + 5;

    $('#' + hiddenSourceId).hide();
    $('#' + sourceId).text($('#' + hiddenSourceId).val());

    $('#' + sourceId).show();
    $('#' + sourceId).addClass(className);
    $('#' + sourceId).css("display", "inline-block");
    $('#' + sourceId).css("width", tempWidth);
}


function GenerateInventoryRows(index) {
    var rowCount = $('#hdnRowCount' + index).val().split(',');
    $('#hdnRowCount' + index).val(rowCount[0] + "," + (parseInt(rowCount[1]) + parseInt(10)));

    for (var counter = 0; counter < 10; counter++) {
        var genericRow = "<tr>" + $('#tableElement' + index + ' tr:last').prev().html() + "</tr>";
        $('#tableElement' + index + ' tr:last').before(genericRow);

        $('#tableElement' + index + ' tr:last').prev().find('td select').removeAttr('Name');
        var ddlAssetTypeId = $('#tableElement' + index + ' tr:last').prev().find('td select').attr('id');

        var newId = parseInt(ddlAssetTypeId.replace('ddlAssetType' + index, ''));

        $('#tableElement' + index + ' tr:last').prev().find('td select').attr('id', 'ddlAssetType' + index + (newId + 1).toString());
        $('#tableElement' + index + ' tr:last').prev().find('td span.assetdesc').attr('id', 'lblAssetDesc' + index + (newId + 1).toString());
        $('#tableElement' + index + ' tr:last').prev().find('td span.practicenotes').attr('id', 'lblPracticeNotes' + index + (newId + 1).toString());
        $('#tableElement' + index + ' tr:last').prev().find('td span.reviewnotes').attr('id', 'lblReviewNotes' + index + (newId + 1).toString());

        $('#tableElement' + index + ' tr:last').prev().find('td input.assetdesc').attr('id', 'txtboxAssetDesc' + index + (newId + 1).toString());
        $('#tableElement' + index + ' tr:last').prev().find('td input.practicenotes').attr('id', 'txtboxPracticeNotes' + index + (newId + 1).toString());
        $('#tableElement' + index + ' tr:last').prev().find('td input.reviewnotes').attr('id', 'txtboxReviewNotes' + index + (newId + 1).toString());

        $('#tableElement' + index + ' tr:last').prev().find('td input.assetdesc').attr('name', '');
        $('#tableElement' + index + ' tr:last').prev().find('td input.practicenotes').attr('name', '');
        $('#tableElement' + index + ' tr:last').prev().find('td input.reviewnotes').attr('name', '');

        $('#tableElement' + index + ' tr:last').prev().find('td select').attr('onchange', "javascript:onAssetTypeChange('" + index + (newId + 1).toString() + "')");
        $('#tableElement' + index + ' tr:last').prev().find('td span.assetdesc').attr('onclick', "javascript:editLabel('lblAssetDesc" + index + (newId + 1).toString() + "','txtboxAssetDesc" + index + (newId + 1).toString() + "')");
        $('#tableElement' + index + ' tr:last').prev().find('td span.practicenotes').attr('onclick', "javascript:editLabel('lblPracticeNotes" + index + (newId + 1).toString() + "','txtboxPracticeNotes" + index + (newId + 1).toString() + "')");
        $('#tableElement' + index + ' tr:last').prev().find('td span.reviewnotes').attr('onclick', "javascript:editLabel('lblReviewNotes" + index + (newId + 1).toString() + "','txtboxReviewNotes" + index + (newId + 1).toString() + "')");

        $('#tableElement' + index + ' tr:last').prev().find('td input.assetdesc').attr('onblur', "javascript:setLabel('lblAssetDesc" + index + (newId + 1).toString() + "','txtboxAssetDesc" + index + (newId + 1).toString() + "','" + $('#tableElement' + index + ' tr:last').prev().find('td span.assetdesc').attr('class') + "')");
        $('#tableElement' + index + ' tr:last').prev().find('td input.practicenotes').attr('onblur', "javascript:setLabel('lblPracticeNotes" + index + (newId + 1).toString() + "','txtboxPracticeNotes" + index + (newId + 1).toString() + "','" + $('#tableElement' + index + ' tr:last').prev().find('td span.practicenotes').attr('class') + "')");
        $('#tableElement' + index + ' tr:last').prev().find('td input.reviewnotes').attr('onblur', "javascript:setLabel('lblReviewNotes" + index + (newId + 1).toString() + "','txtboxReviewNotes" + index + (newId + 1).toString() + "','" + $('#tableElement' + index + ' tr:last').prev().find('td span.reviewnotes').attr('class') + "')");

        //Clear Existing Values
        $('#lblAssetDesc' + index + (newId + 1).toString()).text('');
        $('#lblPracticeNotes' + index + (newId + 1).toString()).text('');
        $('#lblReviewNotes' + index + (newId + 1).toString()).text('');

        $('#txtboxAssetDesc' + index + (newId + 1).toString()).val('');
        $('#txtboxPracticeNotes' + index + (newId + 1).toString()).val('');
        $('#txtboxReviewNotes' + index + (newId + 1).toString()).val('');
    }
}


function SaveGenericInventoryRows() {
    for (var index = 1; index <= 3; index++) {
        var rowCount = $('#hdnRowCount' + index).val().split(',');
        var firstRowIndex = parseInt(rowCount[0]) + parseInt(1);
        var lastRowIndex = parseInt(rowCount[1]);

        if (lastRowIndex >= firstRowIndex) {
            for (var rowCounter = firstRowIndex; rowCounter <= lastRowIndex; rowCounter++) {
                var inventoryRow = $('#ddlAssetType' + index + rowCounter + ' :selected').text() + ',' + $('#txtboxAssetDesc' + index + rowCounter).val() + ',' +
                $('#txtboxPracticeNotes' + index + rowCounter).val() + ',' + $('#txtboxReviewNotes' + index + rowCounter).val() + '|';
                $('#hdnFieldElement' + index).val($('#hdnFieldElement' + index).val() + inventoryRow);
            }
        }

    }


}


function onAssetTypeChange(elementId) {
    $('#ddlAssetType' + elementId + ' option').each(function () {
        if ($(this).val() == 0 || $(this).val() == -1) { 
        }
        else if ($(this).html().indexOf('&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;') == -1) {            
            var tempOption = String.fromCharCode(0x00a0) + String.fromCharCode(0x00a0) + String.fromCharCode(0x00a0) + String.fromCharCode(0x00a0) + String.fromCharCode(0x00a0) + String.fromCharCode(0x00a0) + $.trim($(this).text());
            $(this).text(tempOption);
        }
    });

    //Remove White Spaces
    $('#ddlAssetType' + elementId + ' :selected').text($.trim($('#ddlAssetType' + elementId + ' :selected').text()));
}