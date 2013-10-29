function CorpElement() {
    var count = 0;
    var ncqa = "";
    var practiceId = $('#hdnPracticeId').val();
    var RecievedQuestionnaire = $('#hdnRecievedQuestionnaire').val();
    var frame = document.getElementById('formCorporateSubmission');
    var len = $('#elementListDiv :input:checkbox', frame);
    $('#elementListDiv :input[type=checkbox]').each(function () {
        if ($(this).is(':checked')) {
            count = count + 1;
            corpElement = $(this).next('label')[0].innerHTML;
            if (ncqa == "") {
                ncqa = corpElement.substring(8, 16);
            }

            else {
                ncqa = ncqa + "," + corpElement.substring(8, 16);
            }
        }
    });
    if (count < 11) {
        element = document.getElementById('corporatePopUp').getElementsByTagName('div');
        element.MessageBox.className = "";
        element.MessageBox.className = "error";
        $('#MessageBox p').html('Select atleast Eleven elements.');
    }
    else {
        var jsonText = JSON.stringify({ CorpElementList: ncqa, practiceId: practiceId });
        $.ajax({
            type: "POST",
            url: "../WebServices/NCQAService.asmx/CheckForEnableDisable",
            data: jsonText,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.d[0] == "CorpSite") {
                    $('#corporatePopUp').css('opacity', '0.1');
                    $('#lightbox, .confirmation-popup').fadeIn(300);
                    $('#alertNotification').html("The following Elements contain data and are not selected for inclusion in your Corporate submission:");
                    $('#selectedElementName').html(response.d[1]);
                    $('#alertNotificationRem').html("If you do not select these Elements all materials associated with them will be deleted.");
                    $('#warning').html("Warning: This action cannot be undone. Do you want to continue?");
                }
                else if (response.d[0] == "NonCorpSite") {
                    $('#corporatePopUp').css('opacity', '0.1');
                    $('#lightbox, .confirmation-popup').fadeIn(300);
                    $('#alertNotification').html("The following Elements contain data in other sites and are selected for inclusion in your Corporate submission:");
                    $('#selectedElementName').html(response.d[1]);
                    $('#alertNotificationRem').html("If you select these Elements all materials associated with them will be deleted.");
                    $('#warning').html("Warning: This action cannot be undone. Do you want to continue?");
                }
                else
                    deletePreviousElement();

            },
            failure: function (msg) {
            }
        });
    }
}

function saveCorpElement() {
    var corpElement = "";
    var corporateElementName = "";
    $('#elementListDiv :input[type=checkbox]').each(function () {
        if ($(this).is(':checked')) {
            corpElement = $(this).next('label')[0].innerHTML;
            corporateElementName = corpElement.substring(8, 16);
            $.ajax({
                type: "POST",
                url: "../WebServices/NCQAService.asmx/Save",
                data: "{'practiceId':'" + $('#hdnPracticeId').val() + "','corporateElementName':'" + corporateElementName + "'}",
                contentType: "application/json; charset=utf-8",
                beforeSend: function () {
                },
                complete: function () {
                },
                success: function (response) {
                    if (response.d) {
                        element = document.getElementById('corporatePopUp').getElementsByTagName('div');
                        element.MessageBox.className = "";
                        element.MessageBox.className = "success";
                        $('#MessageBox p').html('Corporate Element Submitted Successfully.');
                    }
                },
                failure: function (msg) {
                }
            });
        }
    });
}

$('a#close-confirmation').live('click', function () {
    $('#corporatePopUp').css('opacity', '100');

    $('#lightbox, .confirmation-popup').fadeOut(300);

});

function deleteFromXMLNode() {
    $.ajax({
        type: "POST",
        url: "../WebServices/NCQAService.asmx/DeleteSelectedElementFromXML",
        data: "{'practiceId':'" + $('#hdnPracticeId').val() + "'}",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
        },
        complete: function () {
            $('#saveElement')[0].click();
        },
        success: function (response) {
        },
        failure: function (msg) {
        }
    });
}

function deletePreviousElement() {
    $.ajax({
        type: "POST",
        url: "../WebServices/NCQAService.asmx/DeletePreviousCorporateSubmissionList",
        data: "{'practiceId':'" + $('#hdnPracticeId').val() + "'}",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
        },
        complete: function () {
        },
        success: function (response) {
            saveCorpElement();
        },
        failure: function (msg) {
        }
    });
}

