// document confirmation call
function ProcessDeleteFile(documentId, link, docSection, ajaxControlPrefix, delControlPrefix, rowId) {
    apprise('Are you sure you want to permanently delete this file?', { 'verify': true }, function (response) {
        if (response) {
            DeleteDoc(documentId, link, docSection, ajaxControlPrefix, delControlPrefix, rowId);
        }
        else {

        }
    });
}


// document delete process
function DeleteDoc(documentId, link, docSection, ajaxControlPrefix, delControlPrefix, rowId) {
    $.ajax({
        type: "POST",
        url: "../WebServices/DocumentService.asmx/DeleteDocument",
        data: "{'documentId':'" + documentId + "','link':'" + link + "','docSection':'" + docSection + "'}",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $('#' + delControlPrefix + documentId).hide();
            $('#' + ajaxControlPrefix + documentId).show();
        },
        complete: function () {
            $('#' + delControlPrefix + documentId).show();
            $('#' + ajaxControlPrefix + documentId).hide();
        },
        success: function (response) {
            $('tr#' + rowId + ' img').remove();
            $('tr#' + rowId + " a").css('text-decoration', 'line-through');
            $('tr#' + rowId + " a").removeClass();
            $('tr#' + rowId + ' a').removeAttr('href');
            $('tr#' + rowId + ' a').focus();
        },
        failure: function (msg) {

        }

    });
}




