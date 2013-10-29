Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (evt, args) {
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

    function EndRequestHandler(sender, args) {
        BindContextMenu();
    }
});

function BeginRequestHandler() {
    $('#reportLightbox, .reportLightbox').fadeIn(300);
}

$(document).ready(function () {
    $('input').keypress(function (evt) {

        //Deterime where our character code is coming from within the event
        var charCode = evt.charCode || evt.keyCode;
        if (charCode == 13) { //Enter key's keycode
            return false;
        }
    });
});


function print() {
    $('#reportLightbox, .reportLightbox').fadeOut(300);
    window.open('PrintReport.aspx');
}

//################# STANDARD_ELEMENTS_TOGGLE  #####################################
function standardTracking(standardSequence) {
    if ($('#elementTable' + standardSequence).is(':visible')) {
        $('#imgStandard' + standardSequence).attr('src', '../Themes/Images/Plus.png');
        $('#elementTable' + standardSequence).hide();
    }
    else {
        $('table#masterTable table').hide(); $('#elementTable' + standardSequence).show();
        $('.img-toggle').attr('src', '../Themes/Images/Plus.png');
        $('#imgStandard' + standardSequence).attr('src', '../Themes/Images/Minus.png');
    }
}


$('#chbReviewed').live('click', function () {
    var status;
    if ($('#chbReviewed').attr('checked'))
        status = 'true';
    else {
        status = 'false';
        $('#btnRequestSubmission').attr('disabled', true);
    }

    $.ajax({
        type: "POST",
        url: "../WebServices/NCQAService.asmx/SaveNCQAStatus",
        data: "{'_element':'Reviewed','_status':'" + status + "','_projectId':'" + $('#hdnSummaryProjectId').val() + "','_level':'" + $('#hdnLevel').val() + "'}",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
        },
        complete: function () {
        },
        success: function (response) {
            if (status == 'true') {
                if ($('#hdnRequestExists').val() != 'True')
                    $('#btnRequestSubmission').attr('disabled', false);
            }
        },
        failure: function (msg) {
        }
    });
});

$('#chbSubmitted').live('click', function () {
    var status;
    if ($('#chbSubmitted').attr('checked')) {
        status = 'true';
    }
    else {
        status = 'false';
        $('#lblSubmitted').text('');
    }

    $.ajax({
        type: "POST",
        url: "../WebServices/NCQAService.asmx/SaveNCQAStatus",
        data: "{'_element':'Submitted','_status':'" + status + "','_projectId':'" + $('#hdnSummaryProjectId').val() + "','_level':'" + $('#hdnLevel').val() + "'}",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
        },
        complete: function () {
        },
        success: function (response) {
            if (status == 'true') {
                var curDate = new Date();
                var curdateTime = curDate.format("MM-dd-yyyy");
                $('#lblSubmitted').text('On ' + curdateTime);
            }
        },
        failure: function (msg) {
        }
    });
});


$('#chbRecognized').live('click', function () {
    var status;
    if ($('#chbRecognized').attr('checked')) {
        status = 'true';
    }
    else {
        $('#lblRecognized').text('');
        status = 'false';
    }

    $.ajax({
        type: "POST",
        url: "../WebServices/NCQAService.asmx/SaveNCQAStatus",
        data: "{'_element':'Recognized','_status':'" + status + "','_projectId':'" + $('#hdnSummaryProjectId').val() + "','_level':'" + $('#hdnLevel').val() + "'}",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
        },
        complete: function () {
        },
        success: function (response) {
            if (status == 'true') {
                var curDate = new Date();
                var curdateTime = curDate.format("MM-dd-yyyy");
                $('#lblRecognized').text('On ' + curdateTime + $('#hdnLevel').val());
            }
        },
        failure: function (msg) {
        }
    });

});

//Facilitators Popup

function DisplayFacilitators() {
    $('#lightbox, .Facilitators-popup').fadeIn(300);
    var browser = navigator.appName;

    // Remove td to fix close[x] tag wrapping in IE
    if (browser != "Microsoft Internet Explorer") {
        $('.Facilitators-popup').css('top', '0%');
        $('.Facilitators-popup').css('top', '0%');
    }
}

$('a#close-Facilitators').live('click', function () {
    $('#lightbox, .Facilitators-popup').fadeOut(300);
});


//NCQA Submission Popup

$('#btnRequestSubmission').live('click', function () {
    $('#txtLicenseNumber').val('');
    $('#rfvtxtLicenseNumber').parent().hide();
    $('#txtUserName').val('');
    $('#rfvtxtUserName').parent().hide();
    $('#txtPassword').val('');
    $('#rfvtxtPassword').parent().hide();


    $('#lightbox, .NCQACredentials-popup').fadeIn(300);
    var browser = navigator.appName;

    // Remove td to fix close[x] tag wrapping in IE
    if (browser != "Microsoft Internet Explorer") {
        $('.NCQACredentials-popup').css('top', '0%');
        $('.NCQACredentials-popup').css('top', '0%');
    }
});

$('a#close-NCQACredentials').live('click', function () {
    $('#lightbox, .NCQACredentials-popup').fadeOut(300);
});


$('#btnCancelNCQA').live('click', function () {
    $('#lightbox, .NCQACredentials-popup').fadeOut(300);
});


$('#btnSubmitNCQA').live('click', function () {
    var isRequired = 'false';
    $('#rfvtxtLicenseNumber').parent().hide();
    $('#rfvtxtUserName').parent().hide();
    $('#rfvtxtPassword').parent().hide();

    if ($('#txtLicenseNumber').val() == '') {
        $('#rfvtxtLicenseNumber').parent().show();
        isRequired = 'true';
    }

    if ($('#txtUserName').val() == '') {
        $('#rfvtxtUserName').parent().show();
        isRequired = 'true';
    }

    if ($('#txtPassword').val() == '') {
        $('#rfvtxtPassword').parent().show();
        isRequired = 'true';
    }

    if (isRequired == 'false') {
        if ($("#btnSaveNCQACredentials")[0]) {
            $("#btnSaveNCQACredentials").click();
            $('#lightbox, .NCQACredentials-popup').fadeOut(300);
            $('#btnRequestSubmission').attr('disabled', true);
        }
    }
});

//Corporate Submission Popup

function DisplayCorporateElement() {
    $('#lightbox, .CorporateElement-popup').fadeIn(300);
    var browser = navigator.appName;
    var practiceId = $('#hiddenPracticeId').val();
    var siteId = $('#hiddenSiteId').val();
    $('#IframeDocViewer').attr('src', '../Webforms/CorporateSubmission.aspx?practiceId=' + practiceId + '&SiteId=' + siteId);
    // Remove td to fix close[x] tag wrapping in IE
    if (browser != "Microsoft Internet Explorer") {
        $('.CorporateElement-popup').css('top', '0%');
        $('.CorporateElement-popup').css('top', '0%');
    }
}

$('a#close-CorporateElement').live('click', function () {
    $('#lightbox, .CorporateElement-popup').fadeOut(300);
});
