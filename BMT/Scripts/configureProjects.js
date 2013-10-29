function OnCheckCorporate(tempId) {
        $('#hiddenTemplateId').val(tempId);
        $('#btnGetSiteInfo')[0].click();
        $('#lightbox, .Corporate-template-popup').fadeIn(300);
}

$('a#close-Corporate-popup').live('click', function () {
    refreshProjectsGrids();
    $('#lightbox, .Corporate-template-popup').fadeOut(300);
});

function OnCorporateTypeChanged(rdo) {
    if (rdo.id == "ctl00_bodyContainer_ConfigProj_corporateTypeYes") {
        document.getElementById('ctl00_bodyContainer_ConfigProj_ddlPracSiteName').disabled = false;
        document.getElementById('ctl00_bodyContainer_ConfigProj_CorpMessage').style.display = "block";
    }
    else if (rdo.id == "ctl00_bodyContainer_ConfigProj_corporateTypeNo") {
        document.getElementById('ctl00_bodyContainer_ConfigProj_ddlPracSiteName').disabled = true;
        document.getElementById('ctl00_bodyContainer_ConfigProj_CorpMessage').style.display = "none";
    }
}

function CheckCorporateTemplate() {
    var chkboxYes = document.getElementById('ctl00_bodyContainer_ConfigProj_corporateTypeYes');
    if (chkboxYes.checked) {
        document.getElementById('ctl00_bodyContainer_ConfigProj_ddlPracSiteName').disabled = false;
        var validator = document.getElementById('ctl00_bodyContainer_ConfigProj_rfvDdlSiteName');
        ValidatorEnable(validator, true);
        checkSiteForCorporateSiteMORe();
    }
    else {
        document.getElementById('ctl00_bodyContainer_ConfigProj_ddlPracSiteName').disabled = true;
        var validator = document.getElementById('ctl00_bodyContainer_ConfigProj_rfvDdlSiteName');
        ValidatorEnable(validator, false);
        removeCorporateMORe();
    }
}
function checkSiteForCorporateSiteMORe() {
    var PracticeSiteId = $('#ctl00_bodyContainer_ConfigProj_ddlPracSiteName').val();
    var TemplateId = $('#hiddenTemplateId').val();

    checkSiteForChangeCorporateMORe(PracticeSiteId, TemplateId);
}

function checkSiteForChangeCorporateMORe(PracticeSiteId, TemplateId) {
    var jsonText = JSON.stringify({ practiceSiteId: PracticeSiteId, templateId: TemplateId });

    $.ajax({
        type: "POST",
        url: "../WebServices/NCQAService.asmx/CheckSiteForChangeCorporateMORe",
        data: jsonText,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
        },
        complete: function () {
        },
        success: function (response) {
            if (response.d == false) {
                $('#Corporate-template-popup').css('opacity', '0.1');
                $('#lightbox, .confirmationTemplate-popup').fadeIn(300);
                $('#btnChangeCorpSiteMORe').addClass("hideDisplay");
                $('#btnCopyCorpElementMORe').addClass("hideDisplay");
                $('#btnCancelNotificationPopup').addClass("hideDisplay");
                $('#alertNotificationMORe').html("Your Corporate site contains answers and/or documents. This action cannot be completed. To change your Corporate site, first delete all documents and answers for all Elements in your existing Corporate site.");
            }
            else {
                checkSiteForCorporateMORe(PracticeSiteId, TemplateId);
            }
        },
        failure: function (msg) {
        }
    });
}
function checkSiteForCorporateMORe(PracticeSiteId, TemplateId) {
    var jsonText = JSON.stringify({ practiceSiteId: PracticeSiteId, templateId: TemplateId });

    $.ajax({
        type: "POST",
        url: "../WebServices/NCQAService.asmx/CheckSiteForCorporateMORe",
        data: jsonText,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
        },
        complete: function () {
        },
        success: function (response) {
            if (response.d == false) {
                $('#Corporate-template-popup').css('opacity', '0.1');
                $('#lightbox, .confirmationTemplate-popup').fadeIn(300);
                $('#btnCopyCorpElementMORe').addClass("hideDisplay");
                $('#btnChangeCorpSiteMORe').removeClass();
                $('#alertNotificationMORe').html("This site contains data and documents. If you designate this site for Corporate submission, all materials disallowed for Corporate submission will be deleted.");
                $('#warningMORe').html("Warning: This action cannot be undone. Do you want to continue?");
            }
            else {
                var isCorporate = true;
                updatePracticeTemplate(TemplateId, isCorporate, PracticeSiteId);
            }
        },
        failure: function (msg) {
        }
    });
}
function updatePracticeTemplate(TemplateId, isCorporate, PracticeSiteId) {
    var jsonText = JSON.stringify({ templateId: TemplateId, isCorporate: isCorporate, practiceSiteId: PracticeSiteId });

    $.ajax({
        type: "POST",
        url: "../WebServices/NCQAService.asmx/UpdatePracticeTemplate",
        data: jsonText,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
        },
        complete: function () {
        },
        success: function (response) {
            var element = document.getElementById('upnlCorporate').getElementsByTagName('div');
            element.MessageBox.className = "";
            element.MessageBox.className = "success";
            element.MessageBox.getElementsByTagName('p')[0].innerHTML = ('Corporate Site Saved Successfully. ');
        },
        failure: function (msg) {
        }
    });
}

function removeCorporateMORe() {
    var PracticeSiteId = $('#ctl00_bodyContainer_ConfigProj_ddlPracSiteName').val();
    var TemplateId = $('#hiddenTemplateId').val();

    var jsonText = JSON.stringify({ practiceSiteId: PracticeSiteId, templateId: TemplateId });

    $.ajax({
        type: "POST",
        url: "../WebServices/NCQAService.asmx/CheckForRemoveCorporateSiteInTemplate",
        data: jsonText,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
        },
        complete: function () {
        },
        success: function (response) {
            if (response.d == false) {
                $('#Corporate-template-popup').css('opacity', '0.1');
                $('#lightbox, .confirmationTemplate-popup').fadeIn(300);
                $('#btnChangeCorpSiteMORe').addClass("hideDisplay");
                $('#btnCopyCorpElementMORe').removeClass();
                $('#alertNotificationMORe').html("If you choose to remove your Corporate designation, all documents and answers from your Corporate site will be copied to all your other sites. ");
                $('#warningMORe').html("Warning: This action cannot be undone. Do you want to continue?");
            }
            else {
                var isCorporate = false;
                updatePracticeTemplate(TemplateId, isCorporate, PracticeSiteId);
            }
        },
        failure: function (msg) {
        }
    });
}

$('a#close-confirmationTemplate').live('click', function () {
    $('.confirmationTemplate-popup').fadeOut(300);
});

function changeCorporateSiteMORe() {

    var PracticeSiteId = $('#ctl00_bodyContainer_ConfigProj_ddlPracSiteName').val();
    var TemplateId = $('#hiddenTemplateId').val();


    var jsonText = JSON.stringify({ practiceSiteId: PracticeSiteId, tempId: TemplateId });

    $.ajax({
        type: "POST",
        url: "../WebServices/NCQAService.asmx/ChangeCorporateSiteMORe",
        data: jsonText,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
        },
        complete: function () {
            var isCorporate = true;
            updatePracticeTemplate(TemplateId, isCorporate, PracticeSiteId);
        },
        success: function (response) {
        },
        failure: function (msg) {
        }
    });
}

function TemplateCopyToNonCorporateSite() {

    var PracticeSiteId = $('#ctl00_bodyContainer_ConfigProj_ddlPracSiteName').val();
    var TemplateId = $('#hiddenTemplateId').val();

    var jsonText = JSON.stringify({ practiceSiteId: PracticeSiteId, templateId: TemplateId });

    $.ajax({
        type: "POST",
        url: "../WebServices/NCQAService.asmx/TemplateCopyToNonCorporateSite",
        data: jsonText,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
        },
        complete: function () {
            var isCorporate = false;
            updatePracticeTemplate(TemplateId, isCorporate, PracticeSiteId);
        },
        success: function (response) {
        },
        failure: function (msg) {
        }
    });
}