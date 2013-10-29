function PCMHtoggle(standardSequence) {
    if ($('#PCMHTable' + standardSequence).is(':visible')) {
        $('#imgElement' + standardSequence).attr('src', '../Themes/Images/Plus-1.png');
        $('#PCMHTable' + standardSequence).hide();
    }
    else {
        $('table#NCQASubmissionTable table').hide();
        $('#PCMHTable' + standardSequence).show();
        $('table#NCQASubmissionTable img.toggle-img').attr('src', '../Themes/Images/Plus-1.png');
        $('#imgElement' + standardSequence).attr('src', '../Themes/Images/Minus-1.png');
    }
}

function PCMHElementtoggle(pcmhSequence, standardSequence) {
    if ($('#elementTable' + pcmhSequence + standardSequence).is(':visible')) {
        $('#imgStandard' + pcmhSequence + standardSequence).attr('src', '../Themes/Images/Plus-1.png');
        $('#elementTable' + pcmhSequence + standardSequence).hide();
    }
    else {
        $('table#PCMHTable' + pcmhSequence + ' table').hide(); $('#elementTable' + pcmhSequence + standardSequence).show();
        $('table#PCMHTable' + pcmhSequence + ' img.toggle-img').attr('src', '../Themes/Images/Plus-1.png');
        $('#imgStandard' + pcmhSequence + standardSequence).attr('src', '../Themes/Images/Minus-1.png');
    }
}



function showPassword(_projectUsageId, _practiceSiteId, _requestedOn, _submissionType) {
    if (_projectUsageId) {
        $.ajax({
            type: "POST",
            url: "../WebServices/NCQAService.asmx/ShowPassword",
            data: "{'_projectUsageId':'" + _projectUsageId + "','_practiceSiteId':'" + _practiceSiteId + "','_requestedOn':'" + _requestedOn + "','_submissionType':" + _submissionType + "}",
            contentType: "application/json; charset=utf-8",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.d) {
                    alert('Password: ' + response.d);
                }
            },
            failure: function (msg) {
            }
        });
    }
}


//close edit popUp event bind
$('#close-UploadPopUp, #btnCancel, #close-lightbox-popup-delete, #btnDelCancel').live('click', function () {
    $('.lightbox, #lightbox-popup, #lightbox-popup-delete').fadeOut(300);
});


function ProcessDeleteFile(page, pcmhId, elementId, factorId, docType, factorTitle, linkedTo) {
    // **************************************************************
    // Page = Contain page with querystring (Note: can be configure from grid html) 
    // PCMHId, ElementId, FactorId, docType, factorTitle parament use to update the value of parent window and popup labels (Note: can be configure from grid html)
    // ************************************************************** 
    var popUpStatus = $('#lightbox, .edit-popup').css('display');

    if (popUpStatus == "none") {

        //open popup
        $('#lightbox, .delete-popup').fadeIn(300);

        //remove scroll
        //RemoveScroll();

        //reset PopUp Position
        //ResetPopUpPosition();

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
    page += '&docLinkedTo=' + docLinkedTo + '&operation=' + operation;
    page = page.replace("#", "hashsign");
    //DeleteFiles.aspx?pcmh=1&element=1&factor=1&file=StDocs/NCQA Documentation/2. NCQA PCMH 2011_Standards_11.21.2011.pdf#page=4|Sample Document|&project=268&practiceId=239&siteId=323&pageNo=2&docLinkedTo=2A1,
    if (operation == 'save') {
        if (confirm('Are you sure you want to disassociate this document from the unchecked factors?')) {
            $('#iFrameFileDelete').attr('src', page);

            // add effect on background
            $('#lightbox, .delete-popup').fadeOut(300);

        }
    }
    else {
        if (confirm('Are you sure you want to completely remove this document from your project?')) {
            $('#iFrameFileDelete').attr('src', page);

            // add effect on background
            $('#lightbox, .delete-popup').fadeOut(300);


        }

    }
}



function SaveChanges() {
    
    var regExp = /^[a-zA-Z0-9\_\-\.\s\(\)]{1,100}$/;
    var docName = $('#txtDocName').val();
    var expressVarify = regExp.test(docName);

    if (!expressVarify)
        alert("File name is invalid.", "error");
    else if (docName.length > 100) {
        alert("Doc Name can be up to 100 characters long.");
    }
    else {
        $.ajax({
            type: "POST",
            url: "../WebServices/NCQAService.asmx/EditDocument",
            data: "{ 'pcmhId': '" + $("#hdnPCMHId").val()
                    + "', 'elementId': '" + $("#hdnElementId").val()
                    + "', 'factorId': '" + $("#hdnFactorId").val()
                    + "', 'file': '" + $("#hdnFile").val()
                    + "', 'docName': '" + $('#txtDocName').val()
                    + "', 'referencePage': '" + $('#txtReferencePage').val()
                    + "', 'relevancyLevel': '" + $('#ddlRelevancyLevel option:selected').text()
                    + "', 'docType': '" + $('#ddldocType option:selected').text()
                    + "', 'projectUsageId': '" + $("#hdnProjectUsageId").val()
                    + "', 'siteId': '" + $("#hdnSiteId").val()
                    + "', 'templateId': '" + $("#hdnTemplateId").val()
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

function UpdateParentControl() {
    // Update parent window control value
    var selectedvalue = $('#ddldocType option:selected').val();

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

    var elementId = $('#hiddenElementId').val();
    var factorId = $('#hiddenFactorId').val();
    var PCMHId = $('#hiddenPCMHId').val();

    var oldValue = window.parent.$('#lblfactorDoc' + elementId + factorId + PCMHId + selectedvalue).html();
    var newValue = parseInt(oldValue) + 1;
    window.parent.$('#lblfactorDoc' + elementId + factorId + PCMHId + selectedvalue).html(newValue);

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

}


// FILE Edit/Move FUNCTION
function FileMove(pcmhId, elementId, factorId, file, docName, referencePage, relevancyLevel, docType, factorTitle, projectUsageId) {
    var popUpStatus = $('#lightbox-popup, .edit-popup').css('display');

    if (popUpStatus == "none") {
        $('#txtDocName').val(docName.replace(/Apostrophe/g, "'").replace(/circumflex/g, "^").replace(/plussign/g, "+").replace(/hashsign/g, "#").replace(/squarebraketopen/g, "[").replace(/squarebraketclose/g, "]").replace(/curlybraketopen/g, "{").replace(/curlybraketclose/g, "}").replace(/dotsign/g, "."));
        $('#txtReferencePage').val(referencePage);

        // save id into hidden fields
        $("#hdnPCMHId").val(pcmhId);
        $("#hdnElementId").val(elementId);
        $("#hdnFactorId").val(factorId);
        $("#hdnFile").val(file);
        $("#hdnProjectUsageId").val(projectUsageId);
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
        $('#lightbox, .edit-popup').fadeIn(300);
        //$('#lightbox-popup.edit-popup').fadeIn(300);


        //reset PopUp Position
        ResetPopUpPosition();

    }
}

function ResetPopUpPosition() {
    // re-adjust the postion of popup
    //    var browser = navigator.appName;

    //    // Remove td to fix close[x] tag wrapping in IE
    //    if (browser == "Microsoft Internet Explorer") {
    //        $('#lightbox, .delete-popup').css('top', '10%');
    //        $('#lightbox, .edit-popup').css('top', '10%');
    //        $('#lightbox-popup').css('top', '10%');
    //    }
    //    else {
    //        $('#lightbox, .delete-popup').css('top', '15%');
    //        $('#lightbox, .edit-popup').css('top', '15%');
    //        $('#lightbox-popup').css('top', '15%');
    //    }
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



////Replace File

// File Upload
function ProcessReplaceFile(title, elementId, factorId, PCMHId, ProjectUsageId, Node, PracticeId, SiteId, DocName, ReferencePage, RelevancyLevel, File, DocLinkedTo, DocType) {
    $('#lblFUDInfo').html(title.replace(/Apostrophe/g, "'").replace(/circumflex/g, "^").replace(/plussign/g, "+").replace(/hashsign/g, "#").replace(/squarebraketopen/g, "[").replace(/squarebraketclose/g, "]").replace(/curlybraketopen/g, "{").replace(/curlybraketclose/g, "}").replace(/dotsign/g, "."));
    $('#hiddenFUDElementId').val(elementId);
    $('#hiddenFUDFactorId').val(factorId);
    $('#hiddenFUDPCMH').val(PCMHId);
    var PracticeName = $('#hiddenPracticeName').val();
    var SiteName = $('#hiddenSiteName').val();
    $('#fuPage').attr('src', '../MOReFileUpload.aspx?elementId=' + elementId + '&factorId=' + factorId + '&PCMHId=' + PCMHId + '&ProjectUsageId=' + ProjectUsageId +
                '&PracticeName=' + PracticeName + '&SiteName=' + SiteName + '&Node=' + Node + '&PracticeId=' + PracticeId + '&SiteId=' + SiteId + '&DocName=' + DocName +
                '&ReferencePage=' + ReferencePage + '&RelevancyLevel=' + RelevancyLevel + '&File=' + File + '&DocLinkedTo=' + DocLinkedTo + '&DocType=' + DocType
                + '&templateId=' + $("#hdnTemplateId").val());
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

$('#btnCancel,#btnDelCancel').live('click', function () {
    $('#lightbox, .edit-popup,delete-popup').fadeOut(300);
});



$('a#close-lightbox-popup-delete').live('click', function () {
    $('#lightbox, .delete-popup').fadeOut(300);

});

$('a#close-UploadPopUp').live('click', function () {
    $('#lightbox, .edit-popup').fadeOut(300);

});


function ProcessDeleteUnAssociatedFile(page, list) {
    // go get the the position of pcmh querystring
    var indexOfPCMH = page.lastIndexOf('?');

    //extract pcmh postion from querystring to display the proper message
    var pcmhSequence = page.substring(indexOfPCMH + 6, indexOfPCMH + 7);

    if (parseInt(pcmhSequence) <= 6) {
        apprise('Are you sure you want to disassociate this document from the factor? <br /> Note: The document may be associated with other factors.', { 'verify': true }, function (response) {
            if (response) {

                $('#iFrameFileDelete').attr('src', page);
                $('.lightbox, .lightbox-popup').fadeIn(300);

                // Disable selected file when file deleted
                GetSelectedList(list);
            }
            else {

            }

        });
    }
    else {
        page += '&operation=deleteUnAssociated';
        page = page.replace("#", "hashsign");
        apprise('Are you sure you want to permanently delete this file?', { 'verify': true }, function (response) {
            if (response) {

                $('#iFrameFileDelete').attr('src', page);
                window.location.reload();
            }
            else { }
        });
    }

}


