
$(document).ready(function () {
    // remove edit and delete option for commment
    $('table tr').each(function () {
        var docLinkedTo = $(this).find("td").eq(3).html(); // document Linked To cell which will be empty in comments case
        if (docLinkedTo == '&nbsp;' || docLinkedTo == '' || docLinkedTo == null) {
            $(this).find("img").remove();
        }
    });

    //close edit popUp event bind
    $('#close-UploadPopUp, #btnCancel, #close-lightbox-popup-delete, #btnDelCancel').live('click', function () {
        // Reset Scroll state
        BodyScrollHandling();

        $('#datagridDocViewer').css('opacity', '100');
        $('.lightbox, #lightbox-popup, #lightbox-popup-delete').fadeOut(300);
    });

    // on tooltip hover
    $("a.tt").hover(function () {
        $("body,html").css("overflow", "auto");
        window.parent.$("#IframeDocViewer").css("overflow", "auto");
    });

    // on tooltip mouse leave
    $("a.tt").mouseleave(function () {
        var iframeHeight = window.parent.$('#IframeDocViewer').height();
        var gridHeight = $('#datagridDocViewer').height();

        if (parseInt(gridHeight) < parseInt(iframeHeight) - 15) {
            $('html,body').animate({
                scrollTop: $("body").offset().top
            }, 0);
        }

        BodyScrollHandling();
    });

    // Reset scroll
    BodyScrollHandling();
});

// FILE DELETE FUNCTION
function ProcessDeleteFile(page, pcmhId, elementId, factorId, docType, factorTitle, linkedTo) {
    // **************************************************************
    // Page = Contain page with querystring (Note: can be configure from grid html) 
    // PCMHId, ElementId, FactorId, docType, factorTitle parament use to update the value of parent window and popup labels (Note: can be configure from grid html)
    // ************************************************************** 
    var popUpStatus = $('#lightbox-popup').css('display');

    if (popUpStatus == "none") {

        //open popup
        $('#lightbox-popup-delete').fadeIn(300);

        //remove scroll
        RemoveScroll();

        //reset PopUp Position
        ResetPopUpPosition();

        $('#hdnPCMHId').val(pcmhId);
        $('#hdnElementId').val(elementId);
        $('#hdnFactorId').val(factorId);
        $("#lblDelFactorInfo").html(factorTitle);
        $("#hdnPageUrl").val(page);
        $("#hdnDocLinkedTo").val(linkedTo);
        TrackDocType(docType);

        $('#dynamicFactorsList').html('');
        // generate dynamic checkboxes
        var docLinked = linkedTo.split(",");
        for (var index = 0; index <= docLinked.length - 1; index++) {
            var input = '<input type="checkbox" name="' + docLinked[index] + '" value="' + docLinked[index] + '" checked="checked" />' + docLinked[index] + '&nbsp;';
            $('#dynamicFactorsList').append(input);
        }

    }
}

function DeleteFile(operation) {
    var page = $("#hdnPageUrl").val();
    var docLinkedTo = '';
    if (operation == 'save') {
        var len = $('#dynamicFactorsList :input:checkbox').length;
        if (len > 1) {
            $('#dynamicFactorsList :input[type=checkbox]').each(function () {
                if (!$(this).is(':checked'))
                    docLinkedTo += $(this).val() + ',';
            });
        }
        else {
            $('#dynamicFactorsList :input[type=checkbox]').each(function () {
                    docLinkedTo += $(this).val() + ',';
            });
        }

        docLinkedTo = docLinkedTo.slice(0, docLinkedTo.length - 1);
    }
    else
        docLinkedTo = $("#hdnDocLinkedTo").val();

    // add new querystring in existing page url
    page += '&docLinkedTo=' + docLinkedTo;

    if (operation == 'save') {
        if (confirm('Are you sure you want to disassociate this document from the unchecked factors?')) {
            $('#iFrameFileDelete').attr('src', page);

            // add effect on background
            $('#lightbox-popup-delete').css('opacity', '0.1');

        }
    }
    else {
        if (confirm('Are you sure you want to completely remove this document from your project?')) {
            $('#iFrameFileDelete').attr('src', page);

            // add effect on background
            $('#lightbox-popup-delete').css('opacity', '0.1');


        }

    }
}

// FILE Edit/Move FUNCTION
function FileMove(pcmhId, elementId, factorId, file, docName, referencePage, relevancyLevel, docType, factorTitle, projectId) {
    var popUpStatus = $('#lightbox-popup-delete').css('display');

    if (popUpStatus == "none") {
        $('#txtDocName').val(docName.replace(/Apostrophe/g, "'").replace(/circumflex/g, "^").replace(/plussign/g, "+").replace(/hashsign/g, "#").replace(/squarebraketopen/g, "[").replace(/squarebraketclose/g, "]").replace(/curlybraketopen/g, "{").replace(/curlybraketclose/g, "}").replace(/dotsign/g,"."));
        $('#txtReferencePage').val(referencePage);

        // save id into hidden fields
        $("#hdnPCMHId").val(pcmhId);
        $("#hdnElementId").val(elementId);
        $("#hdnFactorId").val(factorId);
        $("#hdnFile").val(file);
        $("#hdnProjectId").val(projectId);
        $("#lblFactorInfo").html(factorTitle);

        // update relevancy level with received value
        if (relevancyLevel == "Primary")
            $('#ddlRelevancyLevel').val(1);
        else if (relevancyLevel == "Secondary")
            $('#ddlRelevancyLevel').val(2);
        else if (relevancyLevel == "Supporting")
            $('#ddlRelevancyLevel').val(3);

        // update doc type from received doc type
        TrackDocType(docType);

        //open popup
        $('#lightbox-popup').fadeIn(300);

        //remove scroll
        RemoveScroll();

        //reset PopUp Position
        ResetPopUpPosition();

    }
}

////Replace File

// File Upload
function ProcessReplaceFile(title, elementId, factorId, PCMHId, ProjectId, Node, PracticeId, SiteId, DocName, ReferencePage, RelevancyLevel, File, DocLinkedTo, DocType) {
    $('#lightbox, .replace-popup').fadeIn(300);

    //remove scroll
    RemoveScroll();
    //reset PopUp Position
    ResetPopUpPosition();

    $('#lblFUDInfo').html('Replacing file ' + title.replace(/Apostrophe/g, "'").replace(/circumflex/g, "^").replace(/plussign/g, "+").replace(/hashsign/g, "#").replace(/squarebraketopen/g, "[").replace(/squarebraketclose/g, "]").replace(/curlybraketopen/g, "{").replace(/curlybraketclose/g, "}").replace(/dotsign/g,"."));
    $('#hiddenFUDElementId').val(elementId);
    $('#hiddenFUDFactorId').val(factorId);
    $('#hiddenFUDPCMH').val(PCMHId);
    var PracticeName = $('#hdnPracticeName').val();
    var SiteName = $('#hdnSiteName').val();
    $('#fuPage').attr('src', '../FileUpload.aspx?elementId=' + elementId + '&factorId=' + factorId + '&PCMHId=' + PCMHId + '&ProjectId=' + ProjectId +
                '&PracticeName=' + PracticeName + '&SiteName=' + SiteName + '&Node=' + Node + '&PracticeId=' + PracticeId + '&SiteId=' + SiteId + '&DocName=' + DocName +
                '&ReferencePage=' + ReferencePage + '&RelevancyLevel=' + RelevancyLevel + '&File=' + File + '&DocLinkedTo=' + DocLinkedTo + '&DocType=' + DocType);
}


// Upload PopUp
$('.Replace-popup').live('click', function (e) {
    $('#lightbox, .replace-popup').fadeIn(300);
    var browser = navigator.appName;

    // Remove td to fix close[x] tag wrapping in IE
    if (browser != "Microsoft Internet Explorer") {
        $('.replace-popup').css('top', '0%');
        $('.replace-popup').css('top', '0%');
    }
});

$('a#close-Replace-popup').live('click', function () {
    $('#lightbox, .replace-popup').fadeOut(300);
    window.location.reload();
});

function BodyScrollHandling() {
    var iframeHeight = window.parent.$('#IframeDocViewer').height();
    var gridHeight = $('#datagridDocViewer').height();

    if (parseInt(gridHeight) < parseInt(iframeHeight) - 15) {
        $("body,html").css("overflow", "hidden");
        window.parent.$("#IframeDocViewer").css("overflow", "hidden");
    }
    else {
        $("body,html").css("overflow", "auto");
        window.parent.$("#IframeDocViewer").css("overflow", "auto");
    }
}

function RemoveScroll() {
    // disbale scroll    
    $("body,html").css("overflow", "hidden");
    window.parent.$("#IframeDocViewer").css("overflow", "hidden");

    // add effect on background
    $('#datagridDocViewer').css('opacity', '0.1');
}

function ResetPopUpPosition() {
    // re-adjust the postion of popup
    var browser = navigator.appName;

    // Remove td to fix close[x] tag wrapping in IE
    if (browser == "Microsoft Internet Explorer") {
        $('#lightbox-popup-delete').css('top', '10%');
        $('#lightbox-popup').css('top', '10%');
    }
    else {
        $('#lightbox-popup-delete').css('top', '15%');
        $('#lightbox-popup').css('top', '15%');
    }
}

function SaveChanges() {

    var regExp = /^[a-zA-Z0-9\_\-\.\s\(\)]{1,100}$/;
    var docName = $('#txtDocName').val();
    var expressVarify = regExp.test(docName);

    if (!expressVarify)
        alert("Only Letters, numbers and - or _ are allowed in file names.", "error");
    else if (docName.length > 100) {
        alert("Doc Name can be up to 100 characters long.");
    }
    else {
        $.ajax({
            type: "POST",
            url: "../WebServices/NCQAService.asmx/EditDoc",
            data: "{ 'pcmhId': '" + $("#hdnPCMHId").val()
                    + "', 'elementId': '" + $("#hdnElementId").val()
                    + "', 'factorId': '" + $("#hdnFactorId").val()
                    + "', 'file': '" + $("#hdnFile").val()
                    + "', 'docName': '" + $('#txtDocName').val()
                    + "', 'referencePage': '" + $('#txtReferencePage').val()
                    + "', 'relevancyLevel': '" + $('#ddlRelevancyLevel option:selected').text()
                    + "', 'docType': '" + $('#ddldocType option:selected').text()
                    + "', 'projectId': '" + $("#hdnProjectId").val()
                    + "' }",
            contentType: "application/json; charset=utf-8",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                var receivedResponse = response.d;
                if (receivedResponse) {

                    // Update parent window control value
                    var selectedvalue = $('#ddldocType option:selected').val();
                    var OldDocTypeValue = $("#hdnCurrentDocType").val();

                    if (selectedvalue != OldDocTypeValue) {

                        // 5,7,9,11,13 use to find the control by using their column index
                        if (selectedvalue == 1)
                            selectedvalue = 5;
                        else if (selectedvalue == 2)
                            selectedvalue = 7;
                        else if (selectedvalue == 3)
                            selectedvalue = 9;
                        else if (selectedvalue == 4)
                            selectedvalue = 11;
                        else if (selectedvalue == 5)
                            selectedvalue = 13;

                        var PCMHId = $('#hdnPCMHId').val();
                        var elementId = $('#hdnElementId').val();
                        var factorId = $('#hdnFactorId').val();

                        // update values of new doc type column
                        var oldValue = window.parent.$('#lblfactorDoc' + elementId + factorId + PCMHId + selectedvalue).html();
                        var newValue = parseInt(oldValue) + 1;
                        window.parent.$('#lblfactorDoc' + elementId + factorId + PCMHId + selectedvalue).html(newValue);

                        // current Doc Type Value update
                        var columnIndexIdOfText = parseInt(selectedvalue) - 1;
                        var textControld = "txtfactorDoc" + elementId + factorId + PCMHId + columnIndexIdOfText;

                        var requiredDocs = window.parent.$('#' + textControld).val();
                        requiredDocs = parseInt(requiredDocs);

                        // new value = uploaded docs
                        if (requiredDocs > newValue) {
                            window.parent.$('td#CellUploadedDocs' + elementId + factorId + PCMHId + selectedvalue).removeClass();
                            window.parent.$('td#CellUploadedDocs' + elementId + factorId + PCMHId + selectedvalue).addClass('factor-control-hightlight');
                        }
                        else {
                            window.parent.$('td#CellUploadedDocs' + elementId + factorId + PCMHId + selectedvalue).removeClass();
                            window.parent.$('td#CellUploadedDocs' + elementId + factorId + PCMHId + selectedvalue).addClass('factor-control-important');
                        }

                        // update Old Doc Type Value and status
                        UpdateDocStatusOnParentWindow();
                    }

                    // referesh page to reloaded the updated progress
                    window.location.href = window.location.href;
                }

            },
            failure: function (msg) {
            }

        });
    }
}

function TrackDocType(docType) {
    // update doc type from received doc type
    docType = docType.replace('/', 'Or');
    if (docType == "PoliciesOrProcess") {
        $('#ddldocType').val(1);
        $("#hdnCurrentDocType").val(1);
    }
    else if (docType == "ReportsOrLogs") {
        $('#ddldocType').val(2);
        $("#hdnCurrentDocType").val(2);
    }
    else if (docType == "ScreenshotsOrExamples") {
        $('#ddldocType').val(3);
        $("#hdnCurrentDocType").val(3);
    }
    else if (docType == "RRWB") {
        $('#ddldocType').val(4);
        $("#hdnCurrentDocType").val(4);
    }
    else if (docType == "Extra") {
        $('#ddldocType').val(5);
        $("#hdnCurrentDocType").val(5);
    }
}

// update parent window doc status
function UpdateDocStatusOnParentWindow() {

    // Update parent window control value    
    var OldDocTypeValue = $("#hdnCurrentDocType").val();

    // 5,7,9,11,13 use to find the control by using their column index of old doc type
    if (OldDocTypeValue == 1)
        OldDocTypeValue = 5;
    else if (OldDocTypeValue == 2)
        OldDocTypeValue = 7;
    else if (OldDocTypeValue == 3)
        OldDocTypeValue = 9;
    else if (OldDocTypeValue == 4)
        OldDocTypeValue = 11;
    else if (OldDocTypeValue == 5)
        OldDocTypeValue = 13;

    var PCMHId = $('#hdnPCMHId').val();
    var elementId = $('#hdnElementId').val();
    var factorId = $('#hdnFactorId').val();

    // update values of Old doc type column
    var oldValueOfOldDocType = window.parent.$('#lblfactorDoc' + elementId + factorId + PCMHId + OldDocTypeValue).html();
    var newValueOldDocType = parseInt(oldValueOfOldDocType) - 1;
    window.parent.$('#lblfactorDoc' + elementId + factorId + PCMHId + OldDocTypeValue).html(newValueOldDocType);

    // Old Doc Type Value update
    var columnIndexIdOfText = parseInt(OldDocTypeValue) - 1;
    var textControld = "txtfactorDoc" + elementId + factorId + PCMHId + columnIndexIdOfText;

    var requiredDocs = window.parent.$('#' + textControld).val();
    requiredDocs = parseInt(requiredDocs);

    // new value = uploaded docs
    if (requiredDocs > newValueOldDocType) {
        window.parent.$('td#CellUploadedDocs' + elementId + factorId + PCMHId + OldDocTypeValue).removeClass();
        window.parent.$('td#CellUploadedDocs' + elementId + factorId + PCMHId + OldDocTypeValue).addClass('factor-control-hightlight');
    }
    else {
        window.parent.$('td#CellUploadedDocs' + elementId + factorId + PCMHId + OldDocTypeValue).removeClass();
        window.parent.$('td#CellUploadedDocs' + elementId + factorId + PCMHId + OldDocTypeValue).addClass('factor-control-important');
    }
}