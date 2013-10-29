// Register Handlder On Page Load
Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (evt, args) {
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

    function BeginRequestHandler(sender, args) {
        $('.UpdateProgressContent').show();
    }

    function EndRequestHandler(sender, args) {
        $('.element-table').hide();

        var value = $('#hdnNextTab').val();
        if (parseInt(value) > 0) {
            $(".tabs li").removeClass('activeTab');
            $(".tabs li#tabList" + value).addClass('activeTab');
        }
        BindContextMenu();
        $('.UpdateProgressContent').hide();

        disableAllControls();
    }
});


function BeginRequestHandler() {
    $('#reportLightbox, .reportLightbox').fadeIn(300);   
}


////////////////////////////////////////////////////////////////////////////////////
// On Document Ready
///////////////////////////////////////////////////////////////////////////////////

$(document).ready(function () {
    $(".datepicker").datepicker();
    $('input').keypress(function (evt) {

        //Deterime where our character code is coming from within the event
        var charCode = evt.charCode || evt.keyCode;
        if (charCode == 13) { //Enter key's keycode
            return false;
        }
    });
});


function updateClickTab(value) {
    if ($('#hdnCurrentTab').val() == 2)
        SaveGenericInventoryRows();

    $('#hdnNextTab').val(value);
}


///////////////////////////////////////////////////////////////////////////////////
//Save SRA Questionaire
///////////////////////////////////////////////////////////////////////////////////

function SavingSRAQuestionaire() {
    if ($("#btnSRASave")[0])
        $("#btnSRASave").click();
}


///////////////////////////////////////////////////////////////////////////////////
//Review Notes
///////////////////////////////////////////////////////////////////////////////////

function onClickCloseReview(elementId) {
    $('#divCloseReviewNotes' + elementId).hide();
    $('#divOpenReviewNotes' + elementId).show();
}

function onClickOpenReview(elementId) {    
    $('#divOpenReviewNotes' + elementId).hide();
    $('#divCloseReviewNotes' + elementId).show();
}

function onAddClick(elementId) {
    if ($('#txtComments' + elementId).val() != "") {
        var currDate = new Date();
        var currdateTime = currDate.format("MM/dd/yyyy hh:mm tt") + " CST";
        var commentText = $('#hdnUsrName').val() + ' ' + currdateTime + '<br />' + $('#txtComments' + elementId).val() + '<br /><br />';
        $('#divcomments' + elementId).append(commentText);

        var hdnCommentText = $('#hdnUsrName').val() + ',' + currdateTime + ',' + $('#txtComments' + elementId).val() + '|';
        $('#hdnComments' + elementId).val($('#hdnComments' + elementId).val() + hdnCommentText);

        $('#imgOpenReview' + elementId).attr('src', '../Themes/Images/element-note.png');
        $('#imgCloseReview' + elementId).attr('src', '../Themes/Images/element-note.png');
    }
}



function onTechClickCloseReview(elementId) {
    $('#divTechCloseReviewNotes' + elementId).hide();
    $('#divTechOpenReviewNotes' + elementId).show();
}

function onTechClickOpenReview(elementId) {
    $('#divTechOpenReviewNotes' + elementId).hide();
    $('#divTechCloseReviewNotes' + elementId).show();
}

function onTechAddClick(elementId) {
    if ($('#txtTechComments' + elementId).val() != "") {
        var currDate = new Date();
        var currdateTime = currDate.format("MM/dd/yyyy hh:mm tt") + " CST";
        var commentText = $('#hdnUsrName').val() + ' ' + currdateTime + '<br />' + $('#txtTechComments' + elementId).val() + '<br /><br />';
        $('#divTechcomments' + elementId).append(commentText);

        var hdnCommentText = $('#hdnUsrName').val() + ',' + currdateTime + ',' + $('#txtTechComments' + elementId).val() + '|';
        $('#hdnTechComments' + elementId).val($('#hdnTechComments' + elementId).val() + hdnCommentText);

        $('#imgTechOpenReview' + elementId).attr('src', '../Themes/Images/element-note.png');
        $('#imgTechCloseReview' + elementId).attr('src', '../Themes/Images/element-note.png');
    }
}


///////////////////////////////////////////////////////////////////////////////////
//Save SRA Questionaire on Top Menu Click
///////////////////////////////////////////////////////////////////////////////////

$(".inner-menu-header-container").click(function (e) {
    SavingSRAQuestionaire();
});

///////////////////////////////////////////////////////////////////////////////////
//Save SRA Questionaire on Treeview Click
///////////////////////////////////////////////////////////////////////////////////

$('.treeView a').live('click', (function () {
    if ($(this).html().toLowerCase().indexOf("<img") == -1) {
        SavingSRAQuestionaire();
    }
}));



///////////////////////////////////////////////////////////////////////////////////
//On Change Score
///////////////////////////////////////////////////////////////////////////////////

function OnChangeScore(elementId) {
    var likelihoodScore = parseInt($('#ddlLikelihood' + elementId).val());
    var impactScore = parseInt($('#ddlImpact' + elementId).val());
    var riskRatingScore = likelihoodScore * impactScore;

    if (riskRatingScore >= 4 && riskRatingScore < 7)
        riskRating = "mediumRisk";
    else if (riskRatingScore < 4 && riskRatingScore >= 1)
        riskRating = "lowRisk";
    else if (riskRatingScore > 7)
        riskRating = "highRisk";
    else if (riskRatingScore == 0)
        riskRating = "null";

    var imageSrc = '../Themes/Images/' + riskRating + '.png';
    $('#imgRiskRating' + elementId).attr('src', imageSrc);
}


//On Change Tech Score
function OnChangeTechScore(elementId) {
    var likelihoodScore = parseInt($('#ddltechLikelihood' + elementId).val());
    var impactScore = parseInt($('#ddltechImpact' + elementId).val());
    var riskRatingScore = likelihoodScore * impactScore;

    if (riskRatingScore >= 4 && riskRatingScore < 7)
        riskRating = "mediumRisk";
    else if (riskRatingScore < 4 && riskRatingScore >= 1)
        riskRating = "lowRisk";
    else if (riskRatingScore > 7)
        riskRating = "highRisk";
    else if (riskRatingScore == 0)
        riskRating = "null";

    var imageSrc = '../Themes/Images/' + riskRating + '.png';
    $('#imgtechRiskRating' + elementId).attr('src', imageSrc);
}


///////////////////////////////////////////////////////////////////////////////////
// Date Picker
///////////////////////////////////////////////////////////////////////////////////

function DatePicker(elementId) {
    $('#' + elementId).datepicker().datepicker("show");
}


///////////////////////////////////////////////////////////////////////////////////
// On FindingChecked Changed()
///////////////////////////////////////////////////////////////////////////////////

$('.finding-checkbox').live('click', function () {
    if ($('input[type=checkbox]').attr('checked')) {
        $.ajax({
            type: "POST",
            url: "../WebServices/SRAServices.asmx/GetUserDetails",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                $('#lblFindUserInfo').text(response.d);
            },
            failure: function (msg) {
            }
        });
    }
    else {
        $('#lblFindUserInfo').text('');
    }
});



///////////////////////////////////////////////////////////////////////////////////
// On Followup Checked Changed()
///////////////////////////////////////////////////////////////////////////////////

$('.followup-checkbox').live('click', function () {
    if ($('input[type=checkbox]').attr('checked')) {
        $.ajax({
            type: "POST",
            url: "../WebServices/SRAServices.asmx/GetUserDetails",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                $('#lblFollowUserInfo').text(response.d);
            },
            failure: function (msg) {
            }
        });
    }
    else {
        $('#lblFollowUserInfo').text('');
    }
});



///////////////////////////////////////////////////////////////////////////////////
//Start New Assessment popup
///////////////////////////////////////////////////////////////////////////////////

$('.newAssessment-btn').live('click', function () {

    $('#lightbox, .newAssessment-popup').fadeIn(300);
    var browser = navigator.appName;

    // Remove td to fix close[x] tag wrapping in IE
    if (browser != "Microsoft Internet Explorer") {
        $('.newAssessment-popup').css('top', '0%');
        $('.newAssessment-popup').css('top', '0%');
    }
});

$('a#close-newAssessment').live('click', function () {
    $('#lightbox, .newAssessment-popup').fadeOut(300);
});


$('#btnCancelAssessment').live('click', function () {
    $('#lightbox, .newAssessment-popup').fadeOut(300);
});



$('#btnDeleteAssessment').live('click', function () {
    if ($("#btnRefreshSRA")[0]) {
        $("#btnRefreshSRA").click();
        $('#lightbox, .newAssessment-popup').fadeOut(300);
    }

});



//////////////////////////////////////////////////////////////////////////////////
//Contributor value
///////////////////////////////////////////////////////////////////////////////////

$('.contributors-text').live('change', function () {
    $('#hdnIsEdited').val('true');
    $('#hdnContributor').val($('#txtPCName').val() + '|' + $('#txtPCPhone').val() + '|' + $('#txtPCEmail').val() + '|' +
    $('#txtITName').val() + '|' + $('#txtITPhone').val() + '|' + $('#txtITEmail').val());
});



///////////////////////////////////////////////////////////////////////////////////
//Print Report
///////////////////////////////////////////////////////////////////////////////////
//-- start of snippet --

function print() {
    $('#reportLightbox, .reportLightbox').fadeOut(300);
    //setTimeout(function () { window.open('PrintReport.aspx') }, 3000);
   window.open('PrintReport.aspx');                  
}

function disableAllControls() {

    if ($("#pnlInventory").is(':disabled') == false) {
        $("#pnlInventory").removeAttr('disabled');
        $("#pnlInventory").addClass("is-disabled");
    }

    if ($("#pnlProcess").is(':disabled') == false) {
        $("#pnlProcess").removeAttr('disabled');
        $("#pnlProcess").addClass("is-disabled");
    }
    if ($("#pnlFindings").is(':disabled') == false) {
        $("#pnlFindings").removeAttr('disabled');
        $("#pnlFindings").addClass("is-disabled");
    }
    if ($("#pnlFollowup").is(':disabled') == false) {
        $("#pnlFollowup").removeAttr('disabled');
        $("#pnlFollowup").addClass("is-disabled");
    }
    if ($("#pnlScreening").is(':disabled') == false) {
        $("#pnlScreening").removeAttr('disabled');
        $("#pnlScreening").addClass("is-disabled");
    }
    if ($("#pnlTable").is(':disabled') == false) {
        $("#pnlTable").removeAttr('disabled');
        $("#pnlTable").addClass("is-disabled");
    }
    if ($("#pnlTechnology").is(':disabled') == false) {
        $("#pnlTechnology").removeAttr('disabled');
        $("#pnlTechnology").addClass("is-disabled");
    }
}