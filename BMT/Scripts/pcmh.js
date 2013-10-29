// ############################      POSTBACK_HANDLER      ##########################################
Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (evt, args) {
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    function EndRequestHandler(sender, args) {
        var elementId = $('#hiddenActiveElementId').val();
        $('#tableElement' + elementId).show();
        $('#imgElement' + elementId + ' img').attr('src', '../Themes/Images/Minus.png');
        //$('.body-container-left-tree').css('height', $('.body-container-right').height());
        DisableElement();
        ChangeElementIconImage();
    }
    // ############################      VALIDATION      ##########################################
    $('.factor-textbox').numeric({
        decimal: false, negative: false
    }, function () {
        alert('Positive integers only'); this.value = ''; this.focus();
    });
});

Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
function EndRequestHandler(sender, args) {
//    if (args.get_error() != undefined) {
//        args.set_errorHandled(true);
//    }
}
// ############################      ELEMENTS_FACTOR_TOGGLE      ##########################################
// Default all nested tables on page_load
$(document).ready(function () {
    $('.factor-table').hide();

    if ($('#hdnSummaryElementId')) {
        var elementId = $('#hdnSummaryElementId').val();
        if (elementId != "") {
            $('#tableElement' + elementId).show();
            $('#hiddenActiveElementId').val(elementId);
            $('img.toggle-img').attr('src', '../Themes/Images/Plus.png');
            $('#imgElement' + elementId + ' img').attr('src', '../Themes/Images/Minus.png');
            $('#hdnSummaryElementId').val("");
        }
    }

    $('input:text').live('keyup', function () {
        var receivedId = this.id;
        var requiredDocs = $(this).val();
        requiredDocs = parseInt(requiredDocs);

        var removeControlPrefix = 'txtfactorDoc';
        var labelIdSuffix = receivedId.replace(removeControlPrefix, '');
        labelIdSuffix = parseInt(labelIdSuffix) + 1;

        var labelIdPrefix = 'lblfactorDoc';
        var uploadedDocs = $('#' + labelIdPrefix + labelIdSuffix).html();

        // compare required docs and uploaded docs
        if (requiredDocs > uploadedDocs) {
            $('td#CellUploadedDocs' + labelIdSuffix).removeClass();
            $('td#CellUploadedDocs' + labelIdSuffix).addClass('factor-control-hightlight');
        }
        else {
            $('td#CellUploadedDocs' + labelIdSuffix).removeClass();
            $('td#CellUploadedDocs' + labelIdSuffix).addClass('factor-control-important');
        }

    });

});

function toggleElement(elementId) {
    if ($('#tableElement' + elementId).is(':visible')) {
        $('#imgElement' + elementId + ' img').attr('src', '../Themes/Images/Plus.png');
        $('#tableElement' + elementId).hide();
    }
    else {
        $('.factor-table').hide(); $('#tableElement' + elementId).show();
        $('#hiddenActiveElementId').val(elementId);
        $('img.toggle-img').attr('src', '../Themes/Images/Plus.png');
        $('#imgElement' + elementId + ' img').attr('src', '../Themes/Images/Minus.png');

        DisableTextBoxes();
    }
   // $('.body-container-left-tree').css('height', $('.body-container-right').height());
}

// ############################      JSCRIPT_FUNCTIONS      ##########################################

$('#btnCancelElementNote, #btnFCNCancel, #btnFNCancel').live('focus', function (e) {
    $(this).keypress(function (e) {
        if (e.keyCode == '9') { e.preventDefault(); }
    });
});

// Upload PopUp
$('.uploadPopUp').live('click', function (e) {

    $.ajax({
        type: "POST",
        url: "../WebServices/DocumentService.asmx/IsSessionExpired",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
        },
        complete: function () {
        },
        success: function (response) {
            if (!response.d) {
                $('#lightbox, .uploadbox-popup').fadeIn(300);
                var browser = navigator.appName;

                // Remove td to fix close[x] tag wrapping in IE
                if (browser != "Microsoft Internet Explorer") {
                    $('.uploadbox-popup').css('top', '0%');
                    $('.uploadbox-popup').css('top', '0%');
                }
            }
            else {
                window.location = "/Account/Login.aspx";
            }
        },
        failure: function (msg) {
        }
    });



});

$('a#close-UploadPopUp').live('click', function () {
    $('#lightbox, .uploadbox-popup').fadeOut(300);
});

// Factor Note
$('.factorNotePopUp').live('click', function (e) {
    $('#lightbox, .factornotebox-popup').fadeIn(300);
});

$('a#close-FactorNotePopUp, .cancel-popUp').live('click', function () {
    $('#lightbox, .factornotebox-popup').fadeOut(300);
});

// Factor Critical Notes
$('.factorCriticalNotePopUp').live('click', function (e) {
    $('#lightbox, .factorCriticalnotebox-popup').fadeIn(300);
});

$('a#close-FactorCriticalNotePopUp, .cancel-popUp').live('click', function () {
    $('#lightbox, .factorCriticalnotebox-popup').fadeOut(300);
});

// Element Notes
$('.elementNotePopUp').live('click', function (e) {
    $('#lightbox, .elementnotebox-popup').fadeIn(300);
});

$('a#close-elementnotebox-popup, .cancel-popUp').live('click', function () {
    $('#lightbox, .elementnotebox-popup').fadeOut(300);
});

// ############################      JSCRIPT_FUNCTIONS      ##########################################

// capture current element values
function elementNoteTracking(title, elementId, PCMHId, hiddenFieldId) {
    $('#lblElementInfo').html(title);
    $('#hiddenElementId').val(elementId);
    $('#hiddenElementPCMH').val(PCMHId);
    var elementComment = $('#' + hiddenFieldId).val().replace(/\~\~\~\~\~/g, /</g).replace(/\|\|\|\|\|/g, />/g);
    $('#txtElementComments').val(elementComment);
}

// capture current Factor critical notes values
function fcnTracking(title, elementId, factorId, PCMHId, hiddenFieldId, note) {
    $('#lblFCNInfo').html(title);
    $('#lblFCNNote').html(note);
    $('#hiddenFCNElementId').val(elementId);
    $('#hiddenFCNFactorId').val(factorId);
    $('#hiddenFCNPCMH').val(PCMHId);
    var fcnComment = $('#' + hiddenFieldId).val();
    $('#txtFCNComments').val(fcnComment);
}

// capture current Factor notes values
function fnTracking(title, elementId, factorId, PCMHId, hiddenFieldId) {
    $('#lblFNInfo').html(title);
    $('#hiddenFNElementId').val(elementId);
    $('#hiddenFNFactorId').val(factorId);
    $('#hiddenFNPCMH').val(PCMHId);

    var fnComment = $('#' + hiddenFieldId).val();
    var noteHtml = '<div class=\"NCQA-Note\">';
    $('#txtFNComments').val('');

    var noteList = fnComment.split('@');

    // Parse Notes history
    for (var row = 0; row < noteList.length; row++) {

        //Parse Note Details
        var noteDetails = noteList[row].split(',');

        // Start reading details
        if (noteDetails[0] != '') {
            if (noteDetails[0] != undefined && noteDetails[1] != undefined && noteDetails[2] != undefined && noteDetails[3] != undefined) {
                // Sequence
                noteHtml = noteHtml + noteDetails[0] + ': ';
                // Note
                noteHtml = noteHtml + noteDetails[3];
                // User
                noteHtml = noteHtml + '<br /> <div class=\"user-info\"> Posted by ' + noteDetails[2] + ' ';
                // Date
                noteHtml = noteHtml + 'on ' + noteDetails[1] + '</div>';
                noteHtml = noteHtml.replace(/\~\~\~\~\~/g, ",").replace(/\|\|\|\|\|/g, "@") + '<br />';
            } // close reading details
        }
    }
    noteHtml = noteHtml + '</div>'; //Close Notes history

    $('#note-history').html(noteHtml);
}

// File Upload
function fudTracking(title, elementId, factorId, PCMHId, ProjectId, PracticeName, SiteName, Node, PracticeId, SiteId) {
    $('#lblFUDInfo').html(title);
    $('#hiddenFUDElementId').val(elementId);
    $('#hiddenFUDFactorId').val(factorId);
    $('#hiddenFUDPCMH').val(PCMHId);
    $('#fuPage').attr('src', '../FileUpload.aspx?elementId=' + elementId + '&factorId=' + factorId + '&PCMHId=' + PCMHId + '&ProjectId=' + ProjectId +
                '&PracticeName=' + PracticeName + '&SiteName=' + SiteName + '&Node=' + Node + '&PracticeId=' + PracticeId +
                 '&SiteId=' + SiteId);
}
// ############################      CALCULATE_SUUMARY_ON_CLIENT_SIDE      ##########################################

function calculateSummary(elementId, PCMHId, summaryCell2Id, dropDownId, factorControlPrefix, mustPass, hiddencontrolId) {
    // To Get the ControlId of Summary Table use following pattern ::summaryCell2Id +RowNumber
    // Roll Number 1 =Total Possible Points for PCMH :
    // Roll Number 2 =Total # of Factors with Yes for PCMH :
    // Roll Number 3 =% Points Received for PCMH :
    // Roll Number 4 =Total # of Points Received for PCMH :
    var answer = $('#' + dropDownId + ' option:selected').text();
    var oldAnswer = $('#' + hiddencontrolId).val();
    var maxPoint = parseInt($('#' + summaryCell2Id + '1').html());
    var NoOfFactorsArePresent = parseInt($('#' + summaryCell2Id + '2').html());

    if ((answer == 'Yes' || answer == 'NA') && (oldAnswer != 'Yes' && oldAnswer != 'NA')) {
        NoOfFactorsArePresent = NoOfFactorsArePresent + 1;
    }
    else if (answer == 'No') {
        if (NoOfFactorsArePresent > 0) {
            NoOfFactorsArePresent = NoOfFactorsArePresent - 1;
        }
    }

    $('#' + summaryCell2Id + '2').html(NoOfFactorsArePresent);
    var collectionString = $('#hiddenRules' + PCMHId).val();

    // To get the desired checkBox Id (to check either factor is present or not) checkBoxIdPrefix + elementId + factorId
    var result = calculateResult(NoOfFactorsArePresent, factorControlPrefix, elementId, collectionString);

    // Update % Points Received for PCMH :
    $('#' + summaryCell2Id + '3').html(result);
    $('#hiddenSummarycell2' + PCMHId + elementId + '3').val(result);

    // Update Total # of Points Received for PCMH :
    $('#' + summaryCell2Id + '4').html((parseInt(result) * parseInt(maxPoint)) / 100);

    // Update MUST PASS Element - Passed at 50% Level?
    if (mustPass == 'Yes' || mustPass == 'NA') {
        if (parseInt(result) >= 50) {
            $('#' + summaryCell2Id + '5').html('Yes');
        }
        else {
            $('#' + summaryCell2Id + '5').html('No');
        }
    }
    $('#' + hiddencontrolId).val(answer);
    DisableTextBoxes();
} // Store current Answer and close the function

// ############################      APPLY_RULES      ##########################################

function calculateResult(NoOfFactorsArePresent, factorControlPrefix, elementId, collectionString) {
    var score = 0;
    var elementCollection = collectionString.split('&');
    var rulesCollection = elementCollection[parseInt(elementId) - 1].split(':'); //ELEMENT
    for (var ruleSequence = 1; ruleSequence < rulesCollection.length; ruleSequence++) { //RULES
        var rules = rulesCollection[ruleSequence].split('#');
        for (var row = 0; row < rules.length; row++) {
            if (score == 0 && rules[row] != '' && rules[row] != 'undefined') { //Rule Details
                var ruleCell = rules[row].split(',');
                var presentFactorList = ruleCell[3].split('|'); // if Multiple Must pass factor sequence exists
                var availablePresentFactorList = presentFactorList.length;
                var absentFactorList = ruleCell[4].split('|');  // if Multiple absent factor sequence exists
                var availableAbsentFactorList = absentFactorList.length;

                // ruleCell[0] = score;ruleCell[1] = minYesFactors;ruleCell[2] = maxYesFactors;ruleCell[3] = mustPresentFactorSequence;
                // ruleCell[4] = absentFactorSequence

                // mustPresentFactorSequence & absentFactorSequence not found
                if (availablePresentFactorList == 1 && availableAbsentFactorList == 1) {
                    if ((ruleCell[3] == 0) && (ruleCell[4] == 0)) {
                        if (NoOfFactorsArePresent >= ruleCell[1] && NoOfFactorsArePresent <= ruleCell[2]) {
                            score = ruleCell[0];
                            //mustPresentFactorSequence & absentFactorSequence Close
                        }
                    }

                    else if (ruleCell[3] > 0) {//mustPresentFactorSequence found
                        var isPresent = $('#' + factorControlPrefix + elementId + ruleCell[3] + ' option:selected').text();
                        if ((isPresent == 'Yes' || isPresent == 'NA') && (NoOfFactorsArePresent >= ruleCell[1]) && (NoOfFactorsArePresent <= ruleCell[2])) {
                            score = ruleCell[0];
                        }
                    } //mustPresentFactorSequence Close

                    else if (ruleCell[4] > 0) {//absentFactorSequence found
                        var isPresent = $('#' + factorControlPrefix + elementId + ruleCell[4] + ' option:selected').text();
                        if ((isPresent == 'No') && (NoOfFactorsArePresent >= ruleCell[1]) && (NoOfFactorsArePresent <= ruleCell[2])) {
                            score = ruleCell[0];
                        }
                    } //absentFactorSequence Close

                }
                // Check multiple Factors Start
                else {
                    score = 0;

                    if (availablePresentFactorList > 1) {//Multiple mustPresentFactorSequence found
                        for (var presentFactorSequence = 0; presentFactorSequence < presentFactorList.length; presentFactorSequence++) {
                            var isPresent = $('#' + factorControlPrefix + elementId + presentFactorList[presentFactorSequence] + ' option:selected').text();
                            if ((isPresent == 'Yes' || isPresent == 'NA') && (NoOfFactorsArePresent >= ruleCell[1]) && (NoOfFactorsArePresent <= ruleCell[2])) {
                                score = ruleCell[0];
                            } else {
                                score = 0; break;
                            }
                        }

                    } // Multiple mustPresentFactorSequence Close
                    else if (availableAbsentFactorList > 1) { //Multiple absentFactorSequence found
                        for (var absentFactorSequence = 0; absentFactorSequence < absentFactorList.length; absentFactorSequence++) {
                            var isPresent = $('#' + factorControlPrefix + elementId + absentFactorList[absentFactorList] + ' option:selected').text();
                            if ((isPresent == 'No') && (NoOfFactorsArePresent >= ruleCell[1]) && (NoOfFactorsArePresent <= ruleCell[2])) {
                                score = ruleCell[0];
                            } else {
                                score = 0; break;
                            }
                        }
                    } // Multiple absentFactorSequence Close
                    else { score = 0; } // Multiple absentFactorSequence Close

                } // Check multiple Factors Close
            }
        } // Rule Details Close                        
        return score; // return Score
    } // Main function Close
} // Main function Close

// ************************************************ Change icon images *****************************************
function ChangeElementIconImage() {

    // get value from NCQA Requirement control
    var selectedTab = $('#hiddenClickTab').val();

    if (selectedTab != "7" && selectedTab != "8" && selectedTab != "0") {
        var elementId = $('#hiddenElementId').val();
        var pcmhId = $('#hiddenElementPCMH').val();
        var elementComment = $('#txtElementComments').val();

        if (elementComment.length == 0) {
            $("#imgElementNote" + elementId + pcmhId).attr('src', '../Themes/Images/element-note-empty.png');
        }
        else {
            $("#imgElementNote" + elementId + pcmhId).attr('src', '../Themes/Images/element-note.png');
        }

        
    }

    DisableTextBoxes();
}

// *************************************************** Uploaded Document Viewer ***********************************
$('.link').live('click', function (e) {
    $('.lightbox, #uploadedDocViewer').fadeIn(300);
});

$('a#closeUploadedDocViewer').live('click', function () {
    $('.lightbox, #uploadedDocViewer').fadeOut(300);
});

function UploadedDocViewer(pcmhId, elementId, title, practiceId, siteId, projectId) {
    $('#lblDocViewerTitle').html(title);
    $('#IframeDocViewer').attr('src', 'NCQAUploadedDocs.aspx?pcmhId=' + pcmhId + '&elementId=' + elementId + '&practiceId=' + practiceId +
    '&siteId=' + siteId + '&projectId=' + projectId);
}

function DisableTextBoxes() {
    if ($('#hdnRequiredDocsEnabled').val() == "YES") {
        if ($('#hdnIsConsultant').val() == "false") {
            $('.factor-control input:text').each(function () {
                $(this).attr("disabled", "disabled");
            });
        }
    };


    if ($('#hdnMarkAsCompleteEnabled').val() == "YES") {
        if ($('#hdnIsConsultant').val() == "false") {
            $('.note-Area input:checkbox').each(function () {
                $(this).attr("disabled", "disabled");
            });
        }
    }
}


$(".inner-menu-header-container").click(function (e) {
    SaveQuestionnaire();
});


$('.treeView a').live('click', (function () {
    if ($(this).html().toLowerCase().indexOf("<img") == -1) {
        SaveQuestionnaire();        
    }
}));

function DisableElement() {
    for (var elementTableIndex = 0; elementTableIndex < 8; elementTableIndex++) {
        if ($("#tableElement" + elementTableIndex).is(':disabled') == false) {
            $("#tableElement" + elementTableIndex).removeAttr('disabled');
            $("#tableElement" + elementTableIndex).addClass("is-disabled");
        }
    }
}