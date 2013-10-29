// Register Handlder On Page Load
Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (evt, args) {

    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

    function BeginRequestHandler(sender, args) {
        SaveQuestionnaire();
    }

    function EndRequestHandler(sender, args) {
        $('.factor-table').hide();
        $('div.body-container-left').css("height", $(".body-container-right").height());
        //$('.body-container-left-tree').css('height', $('.body-container-right').height());
        var value = $('#hiddenClickTab').val();

        if (parseInt(value) > 0) {
            $(".tabs li").removeClass('activeTab');
            $(".tabs li#tabList" + value).addClass('activeTab');
        }
        BindContextMenu();
    }

});

// On Document Ready
$(document).ready(function () {
    var defaultValue = $('#hiddenClickTab').val();
    if (defaultValue > 0) {
        $(".tabs li#tabList8").removeClass('activeTab');
        $('div.body-container-left').css("height", $(".body-container-right").height());
        $(".tabs li#tabList" + defaultValue).addClass('activeTab');        
    }
});


function updateClickTab(value) {
    $('#hiddenClickTab').val(value);    
}

function SaveQuestionnaire() {
    if ($("#btnsave")[0]) {
        $("#btnsave")[0].click();
    }
}

// This function representing general Method Call
function updateSrc(e) {
    $('#fuPage').attr('src', '../FileUpload.aspx?projectId=' + $('#hiddenProjectId').val() + '&practiceName=' + $('#hiddenPracticeName').val() + '&siteName=' + $('#hiddenSiteName').val() + '&siteId=' + $('#hiddenSiteId').val() + '&PracticeId=' + $('#hiddenPracticeId').val());

}



function OpenStandard(standardSequence, ElementSequence) {
    $('#hiddenClickTab').val(standardSequence);
    $('#hdnSummaryElementId').val(ElementSequence);       
}