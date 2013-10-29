// Register Handlder On Page Load
var bfirstTime = true;
if (bfirstTime) {
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(InitRequestHandler);
    bFirstTime = false;
}

function InitRequestHandler(sender, args) {
    if (args.get_postBackElement().nodeName == 'A') {
        if ($("#btnsave")[0]) {
            args.set_cancel(true);
            $("#btnsave")[0].click();
        }
    }
}


function EndRequestHandler(sender, args) {

    $('.factor-table').hide();
    $('div.body-container-left').css("height", $(".body-container-right").height());


    BindContextMenu();

    var width = 0;
    $('.tabs li').each(function () {

        width += $(this).width();
    });


    $('.divTabs').width(width + 'px');

    var list = $(".tabs");
    var count = 0;

    list.children().each(function (index) {
        count = index;
    });

    if (count <= 7) {
        $("#nextButton").removeClass("next").addClass("nextDim");
    }

    if ($("#nextButton").attr("class") == "nextDim" && $("#prevButton").attr("class") == "prevDim") { return; }
    else {
        if ($("#hdnTabDiv").val() != null && $("#hdnTabDiv").val() != "undefined" && $("#hdnTabDiv").val() != '') { // inner if

            $(".divTabs").css("left", $("#hdnTabDiv").val());

            if ($("#hdnTabDiv").val() == "0px") {
                $("#nextButton").removeClass("nextDim").addClass("next");
                $("#prevButton").removeClass("prev").addClass("prevDim");
            }
            else {
                $("#nextButton").removeClass("next").addClass("nextDim");
                $("#prevButton").removeClass("prevDim").addClass("prev");
            }


        } // inner if ends
    } // else ends
}


// On Document Ready
$(document).ready(function () {
    var defaultValue = $('#hiddenClickTab').val();

    var list = $(".tabs");
    var count = 0;

    list.children().each(function (index) {
        count = index;
    });

    if (count <= 7) {
        $("#nextButton").removeClass("next").addClass("nextDim");
    }
   

});


function updateClickTab(value) {
    $('#hiddenOldClickTab').val($('#hiddenClickTab').val());
    $('#hiddenClickTab').val(value);
}

// This function representing general Method Call
function updateSrc(e) {
    $('#fuPage').attr('src', '../MOReFileUpload.aspx?projectUsageId=' + $('#hiddenProjectUsageId').val() + '&practiceName=' + $('#hiddenPracticeName').val() + '&siteName=' + $('#hiddenSiteName').val() + '&siteId=' + $('#hiddenSiteId').val() + '&PracticeId=' + $('#hiddenPracticeId').val() + '&templateId=' + $('#hiddenTemplateId').val());

}



function OpenStandard(standardSequence, ElementSequence) {
    $('#hiddenClickTab').val(standardSequence);
    $('#hdnSummaryElementId').val(ElementSequence);
}

/**************************************      Slider panel       *************************************************/

$('#navigator a').live("click", function () {

    var divWidth = $('.divTabs').width();
    var panelWidth = $('.panelTabs').width();

    //alert('panel = ' + panelWidth + ' , div = ' + divWidth);
    if ($("#nextButton").attr("class") == "nextDim" && $("#prevButton").attr("class") == "prevDim")
        return;
    if (this.id == 'next') {

        $(".divTabs").animate({ "left": -panelWidth + "px" }, "slow");
        $("#hdnTabDiv").val(-panelWidth + "px");

        $("#nextButton").removeClass("next").addClass("nextDim");
        $("#prevButton").removeClass("prevDim").addClass("prev");
    }
    else {


        $(".divTabs").animate({ "left": "0px" }, "slow");
        $("#hdnTabDiv").val("0px");

        $("#nextButton").removeClass("nextDim").addClass("next");
        $("#prevButton").removeClass("prev").addClass("prevDim");

    }

});

