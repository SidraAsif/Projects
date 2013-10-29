// ***************************************************
// TODO: Keep hilght currrent active menu!
// ***************************************************
$(document).ready(function () {
    var browser = navigator.appName;
    if (browser == "Microsoft Internet Explorer")
        $('.footer-container').height('84px');
    else
        $('.footer-container').height('80px');

    // ******************************************
    // Add/Remove Top Menu "active" class dynamically */
    // ******************************************
    var page, index;
    page = window.location.pathname;
    page = page.replace(/^.*\//, "").replace(/\?.*$/, "");

    $(".inner-icon-menu ul li a").removeClass('active');

    if (page == "Projects.aspx")
    { $(".inner-icon-menu-projects ul li a").addClass('active'); }
    else if (page == "ToolBox.aspx")
    { $(".inner-icon-menu-toolbox ul li a").addClass('active'); }
    else if (page == "Library.aspx")
    { $(".inner-icon-menu-library ul li a").addClass('active'); }
    else if (page == "Settings.aspx")
    { $(".inner-icon-menu-settings ul li a").addClass('active'); $(".inner-menu-hover-container-left div").hide(); }
    else if (page == "Editor.aspx" || page == "ConsultingUser.aspx" || page == "NCQASubmission.aspx" || page == "Templates.aspx")
    { $(".inner-icon-menu-admin ul li a").addClass('active'); $(".inner-menu-hover-container-left div").hide(); }
    else if (page == "Dashboard.aspx" || page == "Reports.aspx")
    { $(".inner-icon-menu-home ul li a").addClass('active'); }
    else
    { $(".inner-icon-menu-home ul li a").addClass('active'); $(".inner-icon-menu-home ul li a").addClass('active'); }

    setInterval('changeFooterAd()', 50000);




});

// ************************************************************
// TODO: This script will be use to open/close the popup window
// ************************************************************
$(function () {
    $("a#Requesthelp").click(function (e) {
        $('#iFrameRequestHelp').attr('src', 'Requesthelp.aspx');
        $(".request, .request-popup").fadeIn(300);
    });
    $("a#close-popup-Request").click(function () {
        $(".request, .request-popup").fadeOut(300);
    });

    $("a#Submitfeedback").click(function (e) {
        $('#iFrameSubmitFeedBack').attr('src', 'SubmitFeedback.aspx');
        $(".Submit, .Submit-popup").fadeIn(300);
    });
    $("a#close-popup1").click(function () {
        $(".Submit, .Submit-popup").fadeOut(300);
    });

    $("a#Invitefriend").click(function (e) {
        $('#Iframe1').attr('src', 'InviteFriend.aspx');
        $(".Invite, .Invitefriend-popup").fadeIn(300);
    });
    $("a#close-popup2").click(function () {
        $(".Invite, .Invitefriend-popup").fadeOut(300);
    });

    $("a#About").click(function (e) {
        $(".About, .About-popup").fadeIn(300);
    });

    $("a#close-popup3").click(function () {
        $(".About, .About-popup").fadeOut(300);

    });

    $("a#MakeASuggestion").click(function (e) {
        $(".Submit, .Submit-popup").fadeIn(300);
    });
});



function changeFooterAd() {
    var footerAds = $('#hdnAdRotatorList').val().split(';');

    if (footerAds.length > 2) {
        var currentIndex = parseInt($('#hdnCurrentAdIndex').val());

        var nextAd = footerAds[++currentIndex];
        var nextImageUrl = nextAd.split('|')[0];
        var nextNavigateUrl = nextAd.split('|')[1];

        $('#footerAd').attr("href", nextNavigateUrl);
        $('#footerImg').attr("src", nextImageUrl);

        $('#hdnCurrentAdIndex').val(currentIndex);
        if ($('#hdnCurrentAdIndex').val() == footerAds.length - 2) {
            $('#hdnCurrentAdIndex').val(-1);
        }
    }
};


function CheckSession() {
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
            if (response.d) {
                alert('For security reasons and protection of your personal data, your session timed out due to inactivity . Please log in again.');
                window.location = "/Account/Login.aspx?LastRequest=" + window.location.href;
            }
        },
        failure: function (msg) {
        }
    });
}


$('#btnKeepAlive').live('click', function () {
    if ($("#btnResetSession")[0])
        $("#btnResetSession").click();

    $(".sessionWarning, .sessionWarning-popup").fadeOut(300);
});


$('#btnSignout').live('click', function () {
    var bmtLocation = window.location.href.split("//")[1].split('/')[1];

    if (bmtLocation == "Webforms") {
        window.location = "/Account/Login.aspx?LastRequest=" + window.location.href;
    }
    else {
        window.location = "/" + bmtLocation + "/Account/Login.aspx?LastRequest=" + window.location.href;
    }

});