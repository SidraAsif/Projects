

/*====================== HTML Input fields Id ==========================*/

var _txtSiteAddress1 = "ctl00_bodyContainer_txtSiteAddress1";
var _txtSiteAddress2 = "ctl00_bodyContainer_txtSiteAddress2";
var _txtSiteCity = "ctl00_bodyContainer_txtSiteCity";
var _txtSiteContactName = "ctl00_bodyContainer_txtSiteContactName";
var _txtSiteContactEmail = "ctl00_bodyContainer_txtSiteContactEmail";
var _txtConfrmSiteContactEmail = "ctl00_bodyContainer_txtConfrmSiteContactEmail";
var _txtSiteNumberOfProvider = "ctl00_bodyContainer_txtSiteNumberOfProvider";
var _txtSiteFax = "ctl00_bodyContainer_txtSiteFax";
var _txtSiteName = "ctl00_bodyContainer_txtSiteName";
var _txtSiteNPI = "ctl00_bodyContainer_txtSiteNPI";
var _txtSitePhone = "ctl00_bodyContainer_txtSitePhone";
var _txtSiteState = "ctl00_bodyContainer_txtSiteState";
var _txtSiteZipCode = "ctl00_bodyContainer_txtSiteZipCode";
var _chkboxMainSite = "ctl00_bodyContainer_chkboxMainSite";

var _txtUserName = "ctl00_bodyContainer_txtUserName";
var _txtUserPassword = "ctl00_bodyContainer_txtUserPassword";
var _txtUserBPRP = "ctl00_bodyContainer_txtUserBPRP";
var _txtUserDRP = "ctl00_bodyContainer_txtUserDRP";
var _txtUserEmail = "ctl00_bodyContainer_txtUserEmail";
var _txtConfrmUserEmail = "ctl00_bodyContainer_txtConfrmUserEmail";
var _txtUserFirstName = "ctl00_bodyContainer_txtUserFirstName";
var _txtUserHSRP = "ctl00_bodyContainer_txtUserHSRP";
var _txtUserLastName = "ctl00_bodyContainer_txtUserLastName";
var _txtUserRole = "ctl00_bodyContainer_txtUserRole";
var _chkBoxUserResetPwd = "ctl00_bodyContainer_chkBoxUserResetPwd";

var _pnlPracticeMessage = "ctl00_bodyContainer_pnlPracticeMessage";
var _pnlSiteMessage = "ctl00_bodyContainer_pnlSiteMessage";
var _pnlUserMessage = "ctl00_bodyContainer_pmlUserMessage"
var _pnlMOReMessage = "ctl00_bodyContainer_CreateEditTemplate_pnlMOReMassage"
var _btnSiteDiscardChanges = "ctl00_bodyContainer_btnSiteDiscardChanges";

var _ddlPracticePrimarySpeciality = "ctl00_bodyContainer_ddlPracticePrimarySpeciality";
var _ddlPracticeSize = "ctl00_bodyContainer_ddlPracticeSize";
var _ddlSitePrimarySpeciality = "ctl00_bodyContainer_ddlSitePrimarySpeciality";
var _ddlUserCredential = "ctl00_bodyContainer_ddlUserCredential";
var _ddlUserPrimarySite = "ctl00_bodyContainer_ddlUserPrimarySite";
var _ddlUserSpeciality = "ctl00_bodyContainer_ddlUserSpeciality";
var _ddlUserRole = "ctl00_bodyContainer_ddlUserRole";
var _chkBoxUserBPRP = "ctl00_bodyContainer_chkBoxUserBPRP";
var _chkBoxUserDRP = "ctl00_bodyContainer_chkBoxUserDRP";
var _chkBoxUserHSRP = "ctl00_bodyContainer_chkBoxUserHSRP";
var _txtTemplateName = "ctl00_bodyContainer_CreateEditTemplate_txtTemplateName";
var _txtTempName = "ctl00_bodyContainer_CreateEditTemplate_txtTempName";
var _txtTemplateShortName = "ctl00_bodyContainer_CreateEditTemplate_txtTemplateShortName";
var _txtTemplateDescription = "ctl00_bodyContainer_CreateEditTemplate_txtTemplateDescription";
var _ddlTemplateType = "ctl00_bodyContainer_CreateEditTemplate_ddlTemplateType";
var _ddlTemplateCategory = "ctl00_bodyContainer_CreateEditTemplate_ddlTemplateCategory";
var _ddlAllowAccess = "ctl00_bodyContainer_CreateEditTemplate_ddlAllowAccess";
var _ddlTemplateSubmittedTo = "ctl00_bodyContainer_CreateEditTemplate_ddlTemplateSubmittedTo";
var _txtAnotherTemp = "ctl00_bodyContainer_CreateEditTemplate_txtAnotherTemp";
var _txtCopyFromExisting = "ctl00_bodyContainer_CreateEditTemplate_txtCopyFromExisting";
var _txtTempDocs = "ctl00_bodyContainer_CreateEditTemplate_txtTemplateDocument";
var _txtStore = "ctl00_bodyContainer_CreateEditTemplate_txtStoreName";
var _txtTemplateDocs = "ctl00_bodyContainer_CreateEditTemplate_txtTempDocs";
var _ddlToolLevel = "ctl00_bodyContainer_CreateEditTemplate_ddlToolLevel";
var _ddlTools = "ctl00_bodyContainer_CreateEditTemplate_ddlTools";
var _txtTemp = "ctl00_bodyContainer_CreateEditProject_txtSelectTemplate";
var _txtTempp = "ctl00_bodyContainer_CreateEditProject_txtSelectTemp";
var _ddlProjAccess = "ctl00_bodyContainer_CreateEditProject_ddlAllowAccessTo";
var _txtProjDesc = "ctl00_bodyContainer_CreateEditProject_txtProjectDescription";
var _txtProjName = "ctl00_bodyContainer_CreateEditProject_txtProjectName";
var _txtSelectedTemp = "ctl00_bodyContainer_CreateEditProject_txtSelectTemplate";
var _txtForm = "ctl00_bodyContainer_CreateEditProject_txtForm";
var _txtSelectedForm = "ctl00_bodyContainer_CreateEditProject_txtSelectedForm";
var _rdoCreateTemplateYes = "ctl00_bodyContainer_CreateEditTemplate_rbYes";
var _rdoCreateTemplateNo = "ctl00_bodyContainer_CreateEditTemplate_rbNo";
var _rdoEditTemplateYes = "ctl00_bodyContainer_CreateEditTemplate_rbDocStoreYes";
var _rdoEditTemplateNo = "ctl00_bodyContainer_CreateEditTemplate_rbDocStoreNo";
var _ddlAllowProjAccess = "ctl00_bodyContainer_CreateEditProject_ddlAllowProjAccess";
var _tableDiv = "ctl00_bodyContainer_CreateEditProject_tableDiv";
var _divFolderList = "ctl00_bodyContainer_CreateEditProject_divFolderList";
var _divEditFolderList = "ctl00_bodyContainer_CreateEditProject_divEditFolderList";
var _btnTemp = "ctl00_bodyContainer_CreateEditProject_imgTemplate";
var _btnTempp = "ctl00_bodyContainer_CreateEditProject_imgTemp";
var _btnForm = "ctl00_bodyContainer_CreateEditProject_imgForm"
var _btnSelectedForm = "ctl00_bodyContainer_CreateEditProject_imgSelForm"
// ***************************** Submit Form on press enter **************************
$(window).keypress(function (e) {
    var userSection = $("#ctl00_bodyContainer_pnlUserForm").css('display');
    var siteSection = $("#ctl00_bodyContainer_pnlSiteForm").css('display');
    var MOReCreateSection = $("#ctl00_bodyContainer_CreateEditTemplate_pnlCreateTempForm").css('display');
    var MOReEditSection = $("#ct100_bodyContainer_CreateEditTemplate_pnlEditTemplate").css('display');

    var key = e.which;

    if (userSection != "none" && key == 13) {
        $("#ctl00_bodyContainer_btnUserSave")[0].click();
        e.preventDefault();
    }
    else if (siteSection != "none" && key == 13) {
        $("#ctl00_bodyContainer_btnSiteSave")[0].click();
        e.preventDefault();
    }
    else if (userSection == "none" && siteSection == "none" && key == 13) {
        $("#ctl00_bodyContainer_btnPracticeSave")[0].click();
        e.preventDefault();
    }
    else if (MOReCreateSection == "none" && key == 13) {
        $("#ctl00_bodyContainer_btnCreateTemplate")[0].click();
        e.preventDefault();
    }
    else if (MOReEditSection == "none" && key == 13) {
        $("#ctl00_bodyContainer_btnCreateTemplate")[0].click();
        e.preventDefault();
    }
});

// ************************ Check Expiry date checks ***************************
$(function () {

    /*========================= New Site form ===============*/

    $('#ctl00_innerMenuConatiner_btnAddNewSite').live('click', function (e) {
        clearFields();
        SettingsFormSection('#sites');
        $('#ctl00_bodyContainer_pnlSiteForm').show();
    });

    /*========================= New User form ===============*/
    $('#ctl00_innerMenuConatiner_btnAddUser').live('click', function (e) {
        var key = e.which;
        if (key != 113) {
            clearFields();
            SettingsFormSection('#users');
            $('#ctl00_bodyContainer_pnlUserForm').show();
        }

    });
    /*========================= New CreateMORe form ===============*/
    $('#ctl00_innerMenuConatiner_btnCreateMORe').live('click', function (e) {
        var key = e.which;
        if (key != 113) {
            clearFields();
            SettingsFormSection('#createMORe');
            $('#ctl00_bodyContainer_CreateEditTemplate_pnlCreateTempForm').show();
            $('#ctl00_bodyContainer_CreateEditTemplate_pnlEditTemplate').hide();
        }

    });


    /*========================= New CreateProjectMORe form ===============*/
    $('#ctl00_innerMenuConatiner_btncreateProjectMORe').live('click', function (e) {
        var key = e.which;
        if (key != 113) {
            clearFields();
            SettingsFormSection('#createProjectMORe');
            $('#ctl00_bodyContainer_CreateEditProject_pnlCreateProjectForm').show();
            $('#ctl00_bodyContainer_CreateEditProject_pnlEditProject').hide();
        }

    });



    // User Administrator Data picker handling
    $('#ctl00_bodyContainer_chkBoxUserBPRP').live('change', function () {
        var isChecked = this.checked ? true : false;
        if (isChecked)
        { $('#' + _txtUserBPRP).removeAttr('disabled'); }
        else { $('#' + _txtUserBPRP).attr('disabled', 'disabled'); $('#' + _txtUserBPRP).val(''); }

    });

    $('#ctl00_bodyContainer_chkBoxUserDRP').live('change', function () {
        var isChecked = this.checked ? true : false;
        if (isChecked)
        { $('#' + _txtUserDRP).removeAttr('disabled'); }
        else { $('#' + _txtUserDRP).attr('disabled', 'disabled'); $('#' + _txtUserDRP).val(''); }

    });

    $('#ctl00_bodyContainer_chkBoxUserHSRP').live('change', function () {
        var isChecked = this.checked ? true : false;
        if (isChecked)
        { $('#' + _txtUserHSRP).removeAttr('disabled'); }
        else { $('#' + _txtUserHSRP).attr('disabled', 'disabled'); $('#' + _txtUserHSRP).val(''); }

    });

    // ***********************************************************************
    // TODO: Disable Tab key after focus on cancel button in popup
    // ======================== Reset Fields on New Record ====================
    $('#btnCancel').live('focus', function (e) {
        $(this).keypress(function (e) {
            if (e.keyCode == '9') { e.preventDefault(); }
        });
    });
});

function clearFields() {

    document.getElementById(_txtUserName).value = document.getElementById(_txtUserPassword).value =
            document.getElementById(_txtUserBPRP).value = document.getElementById(_txtConfrmUserEmail).value =
            document.getElementById(_txtUserName).value = document.getElementById(_txtUserDRP).value =
            document.getElementById(_txtUserEmail).value = document.getElementById(_txtUserFirstName).value =
            document.getElementById(_txtUserHSRP).value = document.getElementById(_txtUserLastName).value =
            document.getElementById(_txtTempDocs).value = document.getElementById(_txtTemplateDocs).value =
            document.getElementById(_txtStore).value = "";


    document.getElementById(_txtSiteAddress1).value = document.getElementById(_txtSiteAddress2).value =
            document.getElementById(_txtSiteCity).value = document.getElementById(_txtSiteContactEmail).value = document.getElementById(_txtConfrmSiteContactEmail).value =
            document.getElementById(_txtSiteFax).value = document.getElementById(_txtSiteName).value =
            document.getElementById(_txtSiteNPI).value = document.getElementById(_txtSitePhone).value =
            document.getElementById(_txtSiteState).value = document.getElementById(_txtSiteZipCode).value =
            document.getElementById(_txtSiteNumberOfProvider).value = document.getElementById(_txtSiteContactName).value = document.getElementById(_txtTempName).value = "";
    document.getElementById(_chkBoxUserBPRP).checked = document.getElementById(_chkBoxUserDRP).checked =
            document.getElementById(_chkBoxUserHSRP).checked = false;

    document.getElementById(_txtAnotherTemp).value = document.getElementById(_txtCopyFromExisting).value =
    document.getElementById(_txtTemplateName).value = document.getElementById(_txtTemplateShortName).value = "";
    document.getElementById(_txtTemplateDescription).innerText = "";

    document.getElementById(_txtProjDesc).innerText = document.getElementById(_txtProjName).value = document.getElementById(_txtSelectedTemp).value = document.getElementById(_txtForm).value = "";

    // Keep site list up-todate on each Site insert, update and delete
    var siteList = $('#hiddenPracticeSitesList').val();
    var option = "";
    var items = siteList.split('|');
    for (var index = 0; index < items.length - 1; index++) {

        var values = items[index].split(',');
        option += "<option value='" + values[0] + "' >" + values[1] + "</option>";
    }
    $('#' + _ddlUserPrimarySite).html(option);
    $("#ctl00_bodyContainer_pnlChangepwdlink").hide();

    $('#' + _ddlSitePrimarySpeciality).val(0);
    $('#' + _ddlUserCredential).val(0);
    $('#' + _ddlUserPrimarySite).val(0);
    $('#' + _ddlUserSpeciality).val(0);
    $('#' + _ddlUserRole).val(0);
    $('#' + _ddlTemplateType).val(0);
    $('#' + _ddlTemplateCategory).val(0);
    $('#' + _ddlTemplateSubmittedTo).val(0);
    $('#' + _ddlToolLevel).val(0);
    $('#' + _ddlProjAccess).val(0);
    var userType = $('#hdnUserType').val();
    $('#hdnFolderList').val("");

    if (userType == "Consultant" || userType == "User")
    { $('#' + _ddlAllowAccess).val(3); }
    else
    { $('#' + _ddlAllowAccess).val(0); }

    $('#divLstMGR').css("display", "none");
    $('#divLstENT').css("display", "none");
    $('#divLstPractice').css("display", "none");

    $('div.folderSection').css("display", "block");
    $('div.folderDocument').css("display", "block");

    $('#' + _rdoCreateTemplateYes).prop('checked', true);
    $('#' + _rdoCreateTemplateNo).prop('checked', false);
    $('#' + _rdoEditTemplateYes).prop('checked', true);
    $('#' + _rdoEditTemplateNo).prop('checked', false);

}


$(document).ready(function () {
    $('#' + _txtUserBPRP).attr('disabled', 'disabled');
    $('#' + _txtUserDRP).attr('disabled', 'disabled');
    $('#' + _txtUserHSRP).attr('disabled', 'disabled');

    if ($('#IsQueryString').val() != null || $('#IsQueryString').val() != '')
        var showControl = $('#IsQueryString').val(); //queryString["ShowControl"];

    if (showControl == '' || showControl == null || showControl == 'undefined') {
        $('#practice').css({ 'display': 'block' });
        SettingsFormSection('#practice');
    }
    else {
        $('#hdnActiveLinkId').val('ctl00_bodyContainer_lbCreateMORe');
        SettingsFormSection('#' + showControl);
        $('#MOReTree').show();
        $('.img-toggle').attr('src', '../Themes/Images/Plus.png');
        $('#imgMoreManage').attr('src', '../Themes/Images/Minus.png');
    }

    $("#ctl00_bodyContainer_btnPracticeSave").click(function () {
        $("#ctl00_bodyContainer_pnlPracticeMessage").show();
    });

    $("#ctl00_bodyContainer_btnSiteSave").click(function () {
        $("#ctl00_bodyContainer_pnlSiteMessage").show();
    });

    // track link selection
    $('div.left-settings-menu a').live('click', function () {
        $('#hdnActiveLinkId').val(this.id);
    });

    $('#hdnIsPracticeSave').val("false");

    KeepActiveNodeSelected();
    $('#manage').addClass("manageTreeHover");
});

/============================ Keep Active node Selected ==========*/
function KeepActiveNodeSelected() {
    var activeLinkId = $('#hdnActiveLinkId').val();
    $('div.left-settings-menu a').removeClass();
    $('#' + activeLinkId).addClass('active');

}

/*============================ Confirmation Box ==================*/

function SiteChangesDiscard() {

    apprise('Are you sure you want to discard the changes?', { 'verify': true }, function (r) {
        if (r) { return true; }
        else { return false; }

    });
}

/*========================= Active One form at a time ===============*/
function SettingsFormSection(sectinName) {


    $('#ctl00_bodyContainer_pnlSiteForm').hide();
    $('#ctl00_bodyContainer_UserMessage_vSummary').hide();
    $('#ctl00_bodyContainer_pnlUserForm').hide();
    $('#ctl00_bodyContainer_CreateEditTemplate_pnlCreateTempForm').hide();
    $('#ctl00_bodyContainer_CreateEditTemplate_pnlEditTemplate').hide();
    $('#ctl00_bodyContainer_CreateEditTemplate_upnleditTemplate').hide();

    $('#ctl00_bodyContainer_CreateEditProject_pnlCreateProjectForm').hide();
    $('#ctl00_bodyContainer_CreateEditProject_pnlEditProject').hide();

    $('#ctl00_bodyContainer_ConfigProj_upnlConfigureProjects').hide();
    $('#ctl00_bodyContainer_sr_upnlScoringRules').hide();



    $('#hdnIsEdit').val("false");


    if (sectinName == "#sites") {
        $('#hdnSubHeaderId').val('');
        $('#hdnHeaderId').val('');
        $("#ctl00_innerMenuConatiner_btnAddNewSite").show();
        $("#ctl00_innerMenuConatiner_btnAddUser").hide();
        $("#ctl00_innerMenuConatiner_btnCreateMORe").hide();
        $("#ctl00_innerMenuConatiner_btncreateProjectMORe").hide();
    }
    else if (sectinName == "#scoringRules") {
        $('#hdnSubHeaderId').val('');
        $('#hdnHeaderId').val('');
        $('#ctl00_bodyContainer_sr_upnlScoringRules').show();
    }
    else if (sectinName == "#users") {
        $('#hdnSubHeaderId').val('');
        $('#hdnHeaderId').val('');
        $("#ctl00_innerMenuConatiner_btnAddUser").show();
        $("#ctl00_innerMenuConatiner_btnAddNewSite").hide();
        $("#ctl00_innerMenuConatiner_btnCreateMORe").hide();
        $("#ctl00_innerMenuConatiner_btncreateProjectMORe").hide();
    }
    else if (sectinName == "#createMORe") {
        $('#hdnSubHeaderId').val('');
        $('#hdnHeaderId').val('');
        $("#ctl00_innerMenuConatiner_btnCreateMORe").show();
        $("#ctl00_innerMenuConatiner_btnAddUser").hide();
        $("#ctl00_innerMenuConatiner_btnAddNewSite").hide();
        $("#ctl00_innerMenuConatiner_btncreateProjectMORe").hide();
    }

    else if (sectinName == "#createProjectMORe") {
        refreshProjectsLists();
        $("#ctl00_innerMenuConatiner_btncreateProjectMORe").show();
        $("#ctl00_innerMenuConatiner_btnAddUser").hide();
        $("#ctl00_innerMenuConatiner_btnAddNewSite").hide();
        $("#ctl00_innerMenuConatiner_btnCreateMORe").hide();
    }
    else if (sectinName == "#ProjectMORe") {
        sectinName = "#createProjectMORe";
        $("#ctl00_innerMenuConatiner_btncreateProjectMORe").show();
        $("#ctl00_innerMenuConatiner_btnAddUser").hide();
        $("#ctl00_innerMenuConatiner_btnAddNewSite").hide();
        $("#ctl00_innerMenuConatiner_btnCreateMORe").hide();
    }

    else if (sectinName == "#editTemplate") {
        $("#ctl00_innerMenuConatiner_btnCreateMORe").hide();
        $("#ctl00_innerMenuConatiner_btnAddUser").hide();
        $("#ctl00_innerMenuConatiner_btnAddNewSite").hide();
        $("#ctl00_innerMenuConatiner_btncreateProjectMORe").hide();
    }
    else if (sectinName == "#configureProject") {
        $('#hdnSubHeaderId').val('');
        $('#hdnHeaderId').val('');
        refreshProjectsGrids();
        $('#ctl00_bodyContainer_ConfigProj_upnlConfigureProjects').show();
        $("#ctl00_innerMenuConatiner_btnCreateMORe").hide();
        $("#ctl00_innerMenuConatiner_btnAddUser").hide();
        $("#ctl00_innerMenuConatiner_btnAddNewSite").hide();
        $("#ctl00_innerMenuConatiner_btncreateProjectMORe").hide();

    }
    else {
        $('#hdnSubHeaderId').val('');
        $('#hdnHeaderId').val('');
        $("#ctl00_innerMenuConatiner_btnAddUser").hide();
        $("#ctl00_innerMenuConatiner_btnAddNewSite").hide();
        $("#ctl00_innerMenuConatiner_btnCreateMORe").hide();
        $("#ctl00_innerMenuConatiner_btncreateProjectMORe").hide();
    }

    $('#' + _pnlPracticeMessage).hide();
    $('#' + _pnlSiteMessage).hide();
    $('#' + _pnlUserMessage).hide();
    $('#' + _pnlMOReMessage).hide();

    if (sectinName == "#createMORe") {
        $('#hdnActiveLinkId').val('ctl00_bodyContainer_lbCreateMORe');
        KeepActiveNodeSelected();
    }
    else if (sectinName == "#configureProject") {
        $('#hdnActiveLinkId').val('ctl00_bodyContainer_lbConfigureMORe');
        KeepActiveNodeSelected();
    }

    else if (sectinName == "#createProjectMORe") {
        $('#hdnActiveLinkId').val('ctl00_bodyContainer_lbProjectMORe');
        KeepActiveNodeSelected();
    }

    else {
        $(".left-settings-menu a").removeClass('active');
    }

    $("#practice").hide();
    $("#sites").hide();
    $("#users").hide();
    $("#createMORe").hide();
    $("#editTemplate").hide();
    $("#configureProject").hide;
    $("#createProjectMORe").hide();
    $("#scoringRules").hide();
    $('#manage').addClass("manageTreeHover");
    $(sectinName).show();
}

function providerListDisable() {


    jQuery('#ctl00_bodyContainer_rblProviderType').find(':radio').each(function () {
        if (jQuery(this).is(':checked')) {

            var credential = $('#ctl00_bodyContainer_ddlUserCredential');
            var speciality = $('#ctl00_bodyContainer_ddlUserSpeciality');

            if ($(this).val() == 3) {
                credential.attr('disabled', true);
                speciality.attr('disabled', true);
            }
            else {
                if ($(this).val() == 1 || $(this).val() == 2) {
                    credential.attr('disabled', false);
                    speciality.attr('disabled', false);
                }
            }
        }
    });
}

function validation(source, args) {
    if ($('#ctl00_bodyContainer_rblProviderType_2').is(':checked')) {
        if ($('#ctl00_bodyContainer_ddlUserCredential option:selected').val() == 0) {
            args.IsValid = true;
        }
    }
    else {
        if ($('#ctl00_bodyContainer_ddlUserCredential option:selected').val() == 0) {
            args.IsValid = false;
        }
    }

}

function onNewSite() {
    $("#hdnSiteId").val("0");
    $("#hdnsiteAddressId").val("0");
}

function onNewUser() {
    $("#hdnUserId").val("0");
}

//template Popup

function DisplayTemplatePopup() {
    $('#lightbox, .template-popup').fadeIn(300);
    var browser = navigator.appName;
    var tempName = "";
    var templateName = document.getElementById(_txtTempName).value;
    $('#ProjectTemplatePopUp :input[type=checkbox]').each(function () {
        tempName = $(this).next('label')[0].innerHTML;
        if (tempName == templateName) {
            $(this).next('label').hide();
            $(this).hide();
        }
        else {
            $(this).next('label').show();
            $(this).show();
        }
        if ($(this).is(':checked')) {
            $(this).removeAttr('checked');
        }
    });
    var img = document.getElementById('TemplateTable').getElementsByTagName('img').length;
    for (var index = 0; index < img; index++) {
        $('#imgStandard' + index).attr('src', '../Themes/Images/Plus.png');
        $('#TemplateElement' + index).hide();
    }
    // Remove td to fix close[x] tag wrapping in IE
    if (browser != "Microsoft Internet Explorer") {
        $('.template-popup').css('top', '0%');
        $('.template-popup').css('top', '0%');
    }
}
$('a#close-template').live('click', function () {
    $('#lightbox, .template-popup').fadeOut(300);
});

//knowledgebase Popup
function DisplayCustomizeKnowledgebasePopup(kbId, KnowledgebaseType) {
    $('#lightbox, .knowledgebase-popup').fadeIn(300);
    var browser = navigator.appName;
    document.getElementById('popupHeader').innerHTML = "Customize Knowledge Base Element";

    $('#IframeDocViewer').attr('src', '../Webforms/KnowledgeBaseViewer.aspx?kbId=' + kbId + '&KnowledgebaseType=' + KnowledgebaseType);
    $('#cancelPopup').css({ 'margin-top': '539px', 'right': '375px', 'position': 'absolute', 'z-index': '5', 'visible': 'true' });
    // Remove td to fix close[x] tag wrapping in IE
    if (browser != "Microsoft Internet Explorer") {
        $('.knowledgebase-popup').css('top', '0%');
        $('.knowledgebase-popup').css('top', '0%');
    }
    return false;
}

function TemplateList(TemplateSequence) {
    if ($('#TemplateElement' + TemplateSequence).is(':visible')) {
        $('#imgStandard' + TemplateSequence).attr('src', '../Themes/Images/Plus.png');
        $('#TemplateElement' + TemplateSequence).hide();
    }
    else {
        $('#TemplateElement' + TemplateSequence).show();
        $('.img-toggle').attr('src', '../Themes/Images/Plus.png');
        $('#imgStandard' + TemplateSequence).attr('src', '../Themes/Images/Minus.png');
    }
}

function ManageProject() {

    if ($('#MOReTree').is(':visible')) {
        $('#manage').addClass("manageTreeHover");
        $('#imgMoreManage').attr('src', '../Themes/Images/Plus.png');
        $('#MOReTree').hide();
    }
    else {
        $('#MOReTree').show();
        $('#manage').addClass("manageTreeHover");
        $('.img-toggle').attr('src', '../Themes/Images/Plus.png');
        $('#imgMoreManage').attr('src', '../Themes/Images/Minus.png');
    }
}
function CopyTemplate() {
    var ncqa = "";
    var len = $('#ProjectTemplatePopUp :input:checkbox').length;
    $('#ProjectTemplatePopUp :input[type=checkbox]').each(function () {
        if ($(this).is(':checked'))
            if (ncqa == "") {
                ncqa = $(this).next('label')[0].innerHTML;
            }
            else {
                ncqa = ncqa + "," + $(this).next('label')[0].innerHTML;
            }
    });
    if (ncqa == "") {
        element = document.getElementById('templatePopUp').getElementsByTagName('div');
        element.MessageBox.className = "";
        element.MessageBox.className = "error";
        $('#MessageBox p').html('You have not selected any template.');
    }
    else {
        document.getElementById(_txtAnotherTemp).value = ncqa;
        document.getElementById(_txtCopyFromExisting).value = ncqa;
        element = document.getElementById('templatePopUp').getElementsByTagName('div');
        element.MessageBox.className = "";
        element.MessageBox.className = "success";
        $('#MessageBox p').html('Selected Template/Templates merge successfully.');
    }
}

function DisplayEnterKnowledgebasePopup(KnowledgebaseType) {
    $('#lightbox, .knowledgebase-popup').fadeIn(300);
    var browser = navigator.appName;
    var kbId = 0;
    var KnowledgebaseTypeId = 0;
    document.getElementById('popupHeader').innerHTML = "Add Knowledge Base Element";
    $('#cancelPopup').css({ 'margin-top': '614px', 'right': '375px', 'position': 'absolute', 'z-index': '5', 'visible': 'true' });

    if (KnowledgebaseType == "Header") {
        KnowledgebaseTypeId = 1;
    }
    else if (KnowledgebaseType == "SubHeader") {
        KnowledgebaseTypeId = 2;
    }
    else if (KnowledgebaseType == "Question") {
        KnowledgebaseTypeId = 3;
    }
    $('#IframeDocViewer').attr('src', '../Webforms/KnowledgeBaseViewer.aspx?kbId=' + kbId + '&KnowledgebaseType=' + KnowledgebaseTypeId);

    // Remove td to fix close[x] tag wrapping in IE
    if (browser != "Microsoft Internet Explorer") {
        $('.knowledgebase-popup').css('top', '0%');
        $('.Knowledgebase-popup').css('top', '0%');
    }
    return false;
}

$('a#close-knowledgebase-popup').live('click', function () {
    $('#lightbox, .knowledgebase-popup').fadeOut(300);
    //    document.getElementById('ctl00_bodyContainer_EditTemp_ldpnlEditTemp_UpdateProgress1').style.display = "block";
    __doPostBack('ShowControl', 'editTemplate');
});

function closekb() {
    var frame = document.getElementById('formKBDocs');
}

function getQueryStrings() {
    var assoc = {};
    var decode = function (s) { return decodeURIComponent(s.replace(/\+/g, " ")); };
    var queryString = location.search.substring(1);
    var keyValues = queryString.split('&');

    for (var i in keyValues) {
        var key = keyValues[i].split('=');
        if (key.length > 1) {
            assoc[decode(key[0])] = decode(key[1]);
        }
    }

    return assoc;
}

function textCounter(field, maxlimit) {
    if (field.value.length > (maxlimit - 1)) {
        field.value = field.value.substring(0, maxlimit);
        return false;
    }
}

function returnBackTemplate() {
    $('#ctl00_bodyContainer_CreateEditTemplate_pnlEditTemplate').show();
}

function checkSiteForCorporateSite() {
    var ddlSiteName = "ctl00_bodyContainer_ddlSiteName";
    var PracticeSiteId = $('#' + ddlSiteName).val();

    checkSiteForChangeCorporate(PracticeSiteId);
}

function removeCorporate() {
    var ddlSiteName = "ctl00_bodyContainer_ddlSiteName";
    var PracticeSiteId = $('#' + ddlSiteName).val();

    var jsonText = JSON.stringify({ practiceSiteId: PracticeSiteId });

    $.ajax({
        type: "POST",
        url: "../WebServices/NCQAService.asmx/CheckForRemoveCorporateSite",
        data: jsonText,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
        },
        complete: function () {
        },
        success: function (response) {
            if (response.d == false) {
                $('#lightbox, .confirmation-popup').fadeIn(300);
                $('#btnChangeCorpSite').addClass("hideDisplay");
                $('#btnCopyCorpElement').removeClass();
                $('#alertNotification').html("If you choose to remove your Corporate designation, all documents and answers from your Corporate site will be copied to all your other sites. ");
                $('#warning').html("Warning: This action cannot be undone. Do you want to continue?");
            }
            else {
                $("#ctl00_bodyContainer_btnPracticeSave")[0].click();
            }
        },
        failure: function (msg) {
        }
    });
}

$('a#close-confirmation').live('click', function () {
    $('#lightbox, .confirmation-popup').fadeOut(300);
});

function changeCorporateSite() {

    var ddlSiteName = "ctl00_bodyContainer_ddlSiteName";
    var PracticeSiteId = $('#' + ddlSiteName).val();


    var jsonText = JSON.stringify({ practiceSiteId: PracticeSiteId });

    $.ajax({
        type: "POST",
        url: "../WebServices/NCQAService.asmx/ChangeCorporateSite",
        data: jsonText,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
        },
        complete: function () {
            $("#ctl00_bodyContainer_btnPracticeSave")[0].click();
        },
        success: function (response) {
        },
        failure: function (msg) {
        }
    });
}

function copyToXML() {

    var ddlSiteName = "ctl00_bodyContainer_ddlSiteName";
    var PracticeSiteId = $('#' + ddlSiteName).val();

    var jsonText = JSON.stringify({ practiceSiteId: PracticeSiteId });

    $.ajax({
        type: "POST",
        url: "../WebServices/NCQAService.asmx/CopyToNonCorporateXML",
        data: jsonText,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
        },
        complete: function () {
            $("#ctl00_bodyContainer_btnPracticeSave")[0].click();
        },
        success: function (response) {
        },
        failure: function (msg) {
        }
    });
}

function checkSiteForChangeCorporate(PracticeSiteId) {
    var jsonText = JSON.stringify({ practiceSiteId: PracticeSiteId });

    $.ajax({
        type: "POST",
        url: "../WebServices/NCQAService.asmx/CheckSiteForChangeCorporate",
        data: jsonText,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
        },
        complete: function () {
        },
        success: function (response) {
            if (response.d == false) {
                $('#lightbox, .confirmation-popup').fadeIn(300);
                $('#btnChangeCorpSite').addClass("hideDisplay");
                $('#btnCopyCorpElement').addClass("hideDisplay");
                $('#btnCancelNotificationPopup').addClass("hideDisplay");
                $('#alertNotification').html("Your Corporate site contains answers and/or documents. This action cannot be completed. To change your Corporate site, first delete all documents and answers for all Elements in your existing Corporate site.");
            }
            else {
                checkSiteForCorporate(PracticeSiteId);
            }
        },
        failure: function (msg) {
        }
    });
}

function checkSiteForCorporate(PracticeSiteId) {
    var jsonText = JSON.stringify({ practiceSiteId: PracticeSiteId });

    $.ajax({
        type: "POST",
        url: "../WebServices/NCQAService.asmx/CheckSiteForCorporate",
        data: jsonText,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
        },
        complete: function () {
        },
        success: function (response) {
            if (response.d == false) {
                $('#lightbox, .confirmation-popup').fadeIn(300);
                $('#btnCopyCorpElement').addClass("hideDisplay");
                $('#btnChangeCorpSite').removeClass();
                $('#alertNotification').html("This site contains data and documents. If you designate this site for Corporate submission, all materials disallowed for Corporate submission will be deleted.");
                $('#warning').html("Warning: This action cannot be undone. Do you want to continue?");
            }
            else {
                $("#ctl00_bodyContainer_btnPracticeSave")[0].click();
            }
        },
        failure: function (msg) {
        }
    });
}

//function mynext(top) {
//    $('#cancelPopup').css({ 'margin-top': '539px', 'right': '340px', 'position': 'absolute', 'z-index': '5','visible':'true' });
//}
function OnCheckChange(chk) {
    if (chk.checked) {
        $('#hdnNotEditTempLoads').val('true');
        if (chk.id == "chkHeaderName") {
            $('#hdnKBType').val("Header");
            $("#ctl00_bodyContainer_EditTemp_btnSave")[0].click();
            $('#hdnNotEditTempLoads').val('true');
        }
        else if (chk.id == "chkSubHeaderName") {
            $('#hdnKBType').val("SubHeader");
            $("#ctl00_bodyContainer_EditTemp_btnSave")[0].click();
            $('#hdnNotEditTempLoads').val('true');
        }
        else if (chk.id == "chkQuestionName") {
            var subHeaderId = $("#hiddenQuestionParent").val();
            var tempId = $("#hdnTempId").val();
            var jsonText = JSON.stringify({ subHeaderId: subHeaderId, tempId: tempId });
            $.ajax({
                type: "POST",
                url: "../WebServices/KBServices.asmx/CheckTemplateScoringRules",
                data: jsonText,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                },
                complete: function () {
                },
                success: function (response) {
                    if (response.d == true) {
                        if (confirm('By selecting a content here will remove its saved data from this template. Are you sure you want to continue?')) {
                            $('#hdnKBType').val("Question");
                            $("#ctl00_bodyContainer_EditTemp_btnSave")[0].click();
                            $('#hdnNotEditTempLoads').val('true');
                        }
                        else {
                            chk.checked = false;
                        }
                    }
                    else if (response.d == false) {
                        $('#hdnKBType').val("Question");
                        $("#ctl00_bodyContainer_EditTemp_btnSave")[0].click();
                        $('#hdnNotEditTempLoads').val('true');
                    }
                },
                failure: function (msg) {
                }
            });
        }
    }
    else {
        if (confirm('By unselecting a content here will remove all its saved data from this template. Are you sure you want to continue?')) {
            $('#hdnNotEditTempLoads').val('true');
            if (chk.id == "chkHeaderName") {
                $('#hdnKBType').val("Header");
                $("#ctl00_bodyContainer_EditTemp_btnSave")[0].click();
                $('#hdnNotEditTempLoads').val('true');
            }
            else if (chk.id == "chkSubHeaderName") {
                $('#hdnKBType').val("SubHeader");
                $("#ctl00_bodyContainer_EditTemp_btnSave")[0].click();
                $('#hdnNotEditTempLoads').val('true');
            }
            else if (chk.id == "chkQuestionName") {
                $('#hdnKBType').val("Question");
                $("#ctl00_bodyContainer_EditTemp_btnSave")[0].click();
                $('#hdnNotEditTempLoads').val('true');
            }
        }
        else {
            chk.checked = true;
        }
    }
}
function OnCheckAll(chk) {
    if (chk.checked) {
        $('#hdnNotEditTempLoads').val('true');
        if (chk.id == "chkHeader") {
            $('#hdnKBType').val("Header");
            $("#ctl00_bodyContainer_EditTemp_btnSaveAll")[0].click();
            $('#hdnNotEditTempLoads').val('true');
        }
        else if (chk.id == "chkSubHeader") {
            $('#hdnKBType').val("SubHeader");
            $("#ctl00_bodyContainer_EditTemp_btnSaveAll")[0].click();
        }
        else if (chk.id == "chkQuestion") {
            var subHeaderId = $("#hiddenQuestionParent").val();
            var tempId = $("#hdnTempId").val();
            var jsonText = JSON.stringify({ subHeaderId: subHeaderId, tempId: tempId });
            $.ajax({
                type: "POST",
                url: "../WebServices/KBServices.asmx/CheckTemplateScoringRules",
                data: jsonText,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                },
                complete: function () {
                },
                success: function (response) {
                    if (response.d == true) {
                        if (confirm('By selecting a content here will remove its saved data from this template. Are you sure you want to continue?')) {
                            $('#hdnKBType').val("Question");
                            $("#ctl00_bodyContainer_EditTemp_btnSaveAll")[0].click();
                        }
                        else {
                            chk.checked = false;
                        }
                    }
                    else if (response.d == false) {
                        $('#hdnKBType').val("Question");
                        $("#ctl00_bodyContainer_EditTemp_btnSaveAll")[0].click();
                        $('#hdnNotEditTempLoads').val('true');
                    }
                },
                failure: function (msg) {
                }
            });
        }
    }
    else {
        if (confirm('By unselecting a content here will remove all its saved data from this template. Are you sure you want to continue?')) {
            $('#hdnNotEditTempLoads').val('true');
            if (chk.id == "chkHeader") {
                $('#hdnKBType').val("Header");
                $("#ctl00_bodyContainer_EditTemp_btnSaveAll")[0].click();
                $('#hdnNotEditTempLoads').val('true');
            }
            else if (chk.id == "chkSubHeader") {
                $('#hdnKBType').val("SubHeader");
                $("#ctl00_bodyContainer_EditTemp_btnSaveAll")[0].click();
            }
            else if (chk.id == "chkQuestion") {
                $('#hdnKBType').val("Question");
                $("#ctl00_bodyContainer_EditTemp_btnSaveAll")[0].click();
            }
        }
        else {
            chk.checked = true;
        }
    }
}

function refreshProjectsGrids() {
    $("#btnRefreshPage")[0].click();
}

/* Template document popup*/
function DisplayTemplateDocumentPopup() {
    $('#lightbox, .templateDocument-popup').fadeIn(300);
    var browser = navigator.appName;
    var fileName = "";
    var templateId = $("#hdnTemplateId").val();
    $('#pnlTempDocs :input[type=radio]').each(function () {
        fileName = $(this).next('label')[0].innerHTML;
        var rdo = $(this)[0];
        fileName = fileName.replace(/&amp;/g, "&");
        var jsonText = JSON.stringify({ templateId: templateId, fileName: fileName });
        $.ajax({
            type: "POST",
            url: "../WebServices/KBServices.asmx/DocExist",
            data: jsonText,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                if (response.d == true) {
                    rdo.checked = true;
                }
                else {
                    rdo.checked = false;
                }
            },
            failure: function (msg) {
            }
        });
    });

    // Remove td to fix close[x] tag wrapping in IE
    if (browser != "Microsoft Internet Explorer") {
        $('.template-popup').css('top', '0%');
        $('.template-popup').css('top', '0%');
    }
}
$('a#close-templateDocument').live('click', function () {
    $('#lightbox, .templateDocument-popup').fadeOut(300);
});

function SelectTemplateDocument() {
    var ncqa = "";
    var len = $('#pnlTempDocs :input:radio').length;
    $('#pnlTempDocs :input[type=radio]').each(function () {
        if ($(this).is(':checked'))
            if (ncqa == "") {
                ncqa = $(this).next('label')[0].innerHTML;
            }
            else {
                ncqa = ncqa + "," + $(this).next('label')[0].innerHTML;
            }
    });
    if (ncqa == "") {
        element = document.getElementById('tempDocsPopUp').getElementsByTagName('div');
        element.MessageBox.className = "";
        element.MessageBox.className = "error";
        $('#MessageBox p').html('You have not selected any document.');
    }
    else {
        ncqa = ncqa.replace(/&amp;/g, "&");
        document.getElementById(_txtTempDocs).value = ncqa;
        document.getElementById(_txtTemplateDocs).value = ncqa;
        element = document.getElementById('tempDocsPopUp').getElementsByTagName('div');
        element.MessageBox.className = "";
        element.MessageBox.className = "success";
        $('#MessageBox p').html('Selected Document added successfully.');
    }
}
function DisplayProjectTemplatePopup(event) {
    ResetProjectTemplatePopup();

    var templates;

    if ($(event.target).attr("id") == _btnTemp) {
        templates = $("#" + _txtTemp);
    }
    else if ($(event.target).attr("id") == _btnTempp) {
        templates = $("#" + _txtTempp);
    }

    if (templates != "") {
        var template = templates.val().split(",");

        $('#pnlTemp :input[type=checkbox]').each(function () {
            var rdo = $(this);
            $.each(template, function (i) {
                if (rdo.next().text() == template[i]) {
                    //checking template popup list againts the selected templates
                    rdo.attr("checked", true);
                }
            });
        });
    }

    $('#lightbox, .templateDocument-popup').fadeIn(300);
    var browser = navigator.appName;


    // Remove td to fix close[x] tag wrapping in IE
    if (browser != "Microsoft Internet Explorer") {
        $('.template-popup').css('top', '0%');
        $('.template-popup').css('top', '0%');
    }
}
function ResetProjectTemplatePopup() {
    $('#pnlTemp :input[type=checkbox]').each(function () {
        var rdo = $(this);
        rdo.attr("checked", false);
    });
}

$('a#close-templateDocument').live('click', function () {
    $('#lightbox, .templateDocument-popup').fadeOut(300);
});

function SelectTemplateDocument() {
    var ncqa = "";
    var len = $('#pnlTempDocs :input:radio').length;
    $('#pnlTempDocs :input[type=radio]').each(function () {
        if ($(this).is(':checked'))
            if (ncqa == "") {
                ncqa = $(this).next('label')[0].innerHTML;
            }
            else {
                ncqa = ncqa + "," + $(this).next('label')[0].innerHTML;
            }
    });
    if (ncqa == "") {
        element = document.getElementById('tempDocsPopUp').getElementsByTagName('div');
        element.MessageBox.className = "";
        element.MessageBox.className = "error";
        $('#MessageBox p').html('You have not selected any document.');
    }
    else {
        ncqa = ncqa.replace(/&amp;/g, "&");
        document.getElementById(_txtTempDocs).value = ncqa;
        document.getElementById(_txtTemplateDocs).value = ncqa;
        element = document.getElementById('tempDocsPopUp').getElementsByTagName('div');
        element.MessageBox.className = "";
        element.MessageBox.className = "success";
        $('#MessageBox p').html('Selected Document added successfully.');
    }
}

function DisplayProjectTemplatePopup(event) {

    $('#lightbox, .templateDocument-popup').fadeIn(300);
    var browser = navigator.appName;
    var fileName = "";
    var projectId = $("#hdnProjectId").val();
    var templates;

    if ($(event.target).attr("id") == _btnTemp) {
        templates = $("#" + _txtTemp);
    }
    else if ($(event.target).attr("id") == _btnTempp) {
        templates = $("#" + _txttempp);
    }

    if (templates != "") {
        var template = templates.val().split(",");
        $('#pnlTemp :input[type=checkbox]').each(function () {
            var rdo = $(this);
            for (var i = 0; i < template.length; i++) {
                if (rdo.next().text() == template[i]) {
                    //checking template popup list againts the selected templates
                    rdo.checked = true;
                }
                else {
                    rdo.checked = false;
                }
            }
        });
    }
}
$('a#close-templateDocument').live('click', function () {
    $('#lightbox, .templateDocument-popup').fadeOut(300);
    $('.templateDocument-popup').hide();
});




function SelectTemplateProject() {

    var ncqa = "";
    var len = $('#pnlTemp :input:checkbox').length;

    $('#pnlTemp :input[type=checkbox]').each(function () {
        if ($(this).is(':checked'))
            if (ncqa == "") {
                ncqa = $(this).next('label')[0].innerHTML;
            }
            else {
                ncqa = ncqa + "," + $(this).next('label')[0].innerHTML;
            }
    });
    if (ncqa == "") {
        element = document.getElementById('tempPopUp').getElementsByTagName('div');
        element.MessageBox.className = "";
        element.MessageBox.className = "error";
        $('#MessageBox p').html('You have not selected any Template.');
    }
    else {

        ncqa = ncqa.replace(/&amp;/g, "&");
        document.getElementById(_txtTemp).value = ncqa;
        document.getElementById(_txtTempp).value = ncqa;
        element = document.getElementById('tempPopUp').getElementsByTagName('div');
        element.MessageBox.className = "";
        element.MessageBox.className = "success";
        $('#MessageBox p').html('Selected Template added successfully.');
    }
}

function DisplayProjectFormPopup(event) {
    ResetProjectFormPopup();

    var forms;

    if ($(event.target).attr("id") == _btnForm) {
        forms = $("#" + _txtForm);
    }
    else if ($(event.target).attr("id") == _btnSelectedForm) {
        forms = $("#" + _txtSelectedForm);
    }

    if (forms != "") {
        var form = forms.val().split(",");

        $('#pnlForm :input[type=checkbox]').each(function () {
            var rdo = $(this);
            $.each(form, function (i) {
                if (rdo.next().text() == form[i]) {
                    //checking template popup list againts the selected templates
                    rdo.attr("checked", true);
                }
            });
        });
    }
    $('#lightbox, .Form-popup').fadeIn(300);
    var browser = navigator.appName;

    // Remove td to fix close[x] tag wrapping in IE
    if (browser != "Microsoft Internet Explorer") {
        $('.template-popup').css('top', '0%');
        $('.template-popup').css('top', '0%');
    }
}
$('a#close-Form').live('click', function () {
    $('#lightbox, .Form-popup').fadeOut(300);
    $('.Form-popup').hide();
});
function ResetProjectFormPopup() {
    $('#pnlForm :input[type=checkbox]').each(function () {
        var rdo = $(this);
        rdo.attr("checked", false);
    });
}
function SelectProjectForm() {

    var ncqa = "";
    var len = $('#pnlForm :input:checkbox').length;

    $('#pnlForm :input[type=checkbox]').each(function () {
        if ($(this).is(':checked'))
            if (ncqa == "") {
                ncqa = $(this).next('label')[0].innerHTML;
            }
            else {
                ncqa = ncqa + "," + $(this).next('label')[0].innerHTML;
            }
    });
    if (ncqa == "") {
        element = document.getElementById('tblForm').getElementsByTagName('div');
        element.MessageBox.className = "";
        element.MessageBox.className = "error";
        $('#MessageBox p').html('You have not selected any Form.');
    }
    else {

        ncqa = ncqa.replace(/&amp;/g, "&");
        document.getElementById(_txtForm).value = ncqa;
        document.getElementById(_txtSelectedForm).value = ncqa;
        element = document.getElementById('tblForm').getElementsByTagName('div');
        element.MessageBox.className = "";
        element.MessageBox.className = "success";
        $('#MessageBox p').html('Selected Form added successfully.');
    }
}

function GenerateFolderRows(index) {
    var newindex = index + 1;
    var genericRow = "<tr><td style=\"width: 125px;\"><span>Add Folder:</span></td><td><input id=\"Folder1\" class=\"bodytxt-field\" name=\"ctl00$bodyContainer$CreateEditProject$Folder1\" type=\"text\"></td><td><a id=\"hypSubAddMore1\" href=\"javascript:GenerateSubFolderRows(1);\">+ Add Child Folder</a></td></tr><tr><td colSpan=\"3\"><table id=\"SubFolderTable1\" border=\"0\"><tbody><tr></tr></tbody></table></td></tr>";
    $('#tableElement tr:last').before(genericRow);
    var ddlAssetTypeId = $('#tableElement tr:last').prev().prev().find('td input').attr('id');
    var newId = parseInt(ddlAssetTypeId.replace('Folder', ''));
    $('#tableElement tr:last').prev().prev().find('td input.bodytxt-field').attr('id', 'Folder' + (newindex).toString());
    $('#tableElement tr:last').prev().find('td table').attr('id', 'SubFolderTable' + (newindex).toString());
    $('#tableElement tr:last').prev().prev().find('td a').attr('href', "javascript:GenerateSubFolderRows(" + (newindex).toString() + ")");
    $('#tableElement tr:last').find('td a').attr('href', "javascript:GenerateFolderRows(" + (newindex).toString() + ")");
}

function GenerateSubFolderRows(index) {
    var rowId = 1;
    var newId = 0;
    var genericRow = "<tr><td style=\"width: 125px;\"><td><td style=\"width: 125px;\"><span>Sub Folder Name:</span></td><td><input id=\"SubFolder" + index + "0\" class=\"bodytxt-field\" name=\"ctl00$bodyContainer$CreateEditProject$SubFolder1\" type=\"text\"></td></tr>";
    var ddlAssetTypeId = $('#SubFolderTable' + index + ' tr:last').prev().find('td input').attr('id');
    $('#SubFolderTable' + index + ' tr:last').before(genericRow);
    if (ddlAssetTypeId == undefined) {
        var newId = 1;
    }
    else {
        var prevId = parseInt(ddlAssetTypeId.replace('SubFolder' + index, ''));
        newId = prevId + rowId;
    }
    $('#SubFolderTable' + index + ' tr:last').prev().find('td input.bodytxt-field').attr('id', 'SubFolder' + index.toString() + newId.toString());
}

function DisplayFolderPopup(event) {
    var FolderList;
    if ($(event.target).attr("id") == "ctl00_bodyContainer_CreateEditProject_ImgFolder") {
        FolderList = _divFolderList;
    }

    else if ($(event.target).attr("id") == "ctl00_bodyContainer_CreateEditProject_ImgEditFolder") {
        FolderList = _divEditFolderList;
    }
    if ($("#" + FolderList).find("ul.main").length > 0 || $('#hdnFolderList').val() != "") {
        if (confirm("Editing will delete the current folder list. Are you sure?")) {
            ClearhdnFolderList();
            ClearFolderList();
            ResetFolderPopup();
            $('#lightbox, .OtherFolders-popup').fadeIn(300);
            var browser = navigator.appName;
        }
    }
    else {
        $('#lightbox, .OtherFolders-popup').fadeIn(300);
        var browser = navigator.appName;
    }
    // Remove td to fix close[x] tag wrapping in IE
    if (browser != "Microsoft Internet Explorer") {
        $('.OtherFolders-popup').css('top', '0%');
        $('.OtherFolders-popup').css('top', '0%');
    }
}

$('a#close-OtherFolders').live('click', function () {
    $('#lightbox, .OtherFolders-popup').fadeOut(300);
    $('.OtherFolders-popup').hide();
    ResetFolderPopup();
});

function SaveFolderList() {
    $('#FolderPopUp #MessageBox p').html("");
    var validation = ValidateFolders();
    if (!validation) {
        element = document.getElementById('FolderPopUp').getElementsByTagName('div');
        element.MessageBox.className = "";
        element.MessageBox.className = "error";
        $('#FolderPopUp #MessageBox p').append('Invalid folder name.');
        return false;
    }
    ClearhdnFolderList();
    var folders = "";
    var mainFolder;
    var subFolder;
    $('#tableElement').find('td input.bodytxt-field[id^=Folder]').each(function () {
        mainFolder = $(this).val()
        folders = folders + mainFolder + "," + "ProjectFolder/";
        $(this).parent('td').parent('tr').next('tr').find("table[id^=SubFolder]").find("td input.bodytxt-field[id^=SubFolder]").each(function () {
            subFolder = $(this).val();
            folders = folders + subFolder + "," + mainFolder + "/";
        });
    });
    $("#hdnFolderList").val(folders);
    GenerateMainFolderList();
    element = document.getElementById("FolderPopUp").getElementsByTagName("div");
    element.MessageBox.className = "";
    element.MessageBox.className = "success";
    $("#MessageBox p").html("Folders added successfully.");

}

//Validate Folder Names
function ValidateFolders() {
    var test = true;
    var characterReg = /^([a-zA-Z0-9\s\-\.\,\&]{1,50})$/;
    var folderList = $('#tableElement').find('td input.bodytxt-field[id^=Folder], td input.bodytxt-field[id^=SubFolder]');
    folderList.each(function () {
        var name = $(this);
        if (name.val() == "" || !characterReg.test(name.val())) {
            name.css("border", "1px solid red");
            test = false;
        }
        else if (name.val() != "" || characterReg.test(name.val())) {
            var splt = name.val().split(" ");
            for (var i = 0; i < splt.length; i++) {
                if (splt[i] == "") {
                    name.css("border", "1px solid red");
                    test = false;
                    break;
                }
                else {
                    name.css("border", "");
                }
            }
        }
    });

    var dupTest = DuplicateFolders(folderList);

    if (test == true && dupTest == true) {
        return true;
    }
    else {
        return false;
    }
}

function DuplicateFolders(folderList) {
    var test = true;
    folderList.each(function () {
        var name = $(this);

        folderList.each(function () {
            if (name.attr("id") != $(this).attr("id")) {
                if (name.val().trim() == $(this).val().trim()) {
                    $(this).css("border", "1px solid red");
                    name.css("border", "1px solid red");
                    test = false;
                }
            }
        });
    });
    if (test == false) {
        element = document.getElementById('FolderPopUp').getElementsByTagName('div');
        element.MessageBox.className = "";
        element.MessageBox.className = "error";
        $('#FolderPopUp #MessageBox p').html('Folders cannot have same name. ');
    }
    return test;
}

function GenerateMainFolderList() {
    ClearFolderList();
    var mainFolderList = $("<ul class='main'></ul>");
    var folderList = $('#hdnFolderList').val();
    var folders = folderList.split("/");
    //$("#" + _divFolderList).append("<p>" + folders + "</p>");
    //$("#" + _divFolderList).append(mainFolderList);
    $.each(folders, function (i) {
        var folder = folders[i].split(",");
        var Name = folder[0];
        var Parent = folder[1];

        if (Name == undefined || Parent == undefined) {

        }
        else if (Parent == "ProjectFolder") {
            var MainFolder = $("<li>" + Name + "</li>");
            mainFolderList.append(MainFolder);
        }
    });

    //CreateProjectPanel & EditProjectPanel
    $("#" + _divFolderList + ",#" + _divEditFolderList).append(mainFolderList);
    GenerateSubFolderList();
    GenerateEditSubFolderList();
}

//CreateProjectPanel
function GenerateSubFolderList() {
    var folderList = $('#hdnFolderList').val();
    var subFolders = folderList.split("/");
    $("#" + _divFolderList + " ul.main li").each(function () {
        var subFolderList = $("<ul class='sub'></ul>");
        var mainFolder = $(this).text();
        $.each(subFolders, function (i) {
            var subFolder = subFolders[i].split(",");
            var sName = subFolder[0];
            var sParent = subFolder[1];
            if (sName == undefined || sParent == undefined) {

            }
            else if (sParent == mainFolder) {
                var SubFolder = $("<li>" + sName + "</li>");
                subFolderList.append(SubFolder);
            }
        });
        $(this).append(subFolderList);
    });
}

function GenerateEditSubFolderList() {
    var folderList = $('#hdnFolderList').val();
    var subFolders = folderList.split("/");
    $("#" + _divEditFolderList + " ul.main li").each(function () {
        var subFolderList = $("<ul class='sub'></ul>");
        var mainFolder = $(this).text();
        $.each(subFolders, function (i) {
            var subFolder = subFolders[i].split(",");
            var sName = subFolder[0];
            var sParent = subFolder[1];
            if (sName == undefined || sParent == undefined) {

            }
            else if (sParent == mainFolder) {
                var SubFolder = $("<li>" + sName + "</li>");
                subFolderList.append(SubFolder);
            }
        });
        $(this).append(subFolderList);
    });
}

function ClearFolderList() {
    $("#" + _divFolderList).find("ul.main").remove();
    $("#" + _divEditFolderList).find("ul.main").remove();
}

function ClearhdnFolderList() {
    $("#hdnFolderList").val("");
}

function ResetFolderPopup() {
    var tableElement = $("#tableElement");
    tableElement.find("tbody").remove();
    var body = $("<tbody></tbody>")
    var row1 = $("<tr><td style='width:125px;'><span>Add Folder:</span></td><td><input type='text' class='bodytxt-field' id='Folder1' /></td><td><a href='javascript:GenerateSubFolderRows(1);' id='hypSubAddMore1'>+ Add Child Folder</a></td></tr>");
    var row2 = $("<tr><td colspan='3'><table border='0' id='SubFolderTable1'><tbody><tr></tr></tbody></table></td></tr>");
    var row3 = $("<tr><td colspan='2'><a href='javascript:GenerateFolderRows(1);' id='hypLinkAddMore1'>+ Add Another Folder</a></td></tr>");
    body.append(row1);
    body.append(row2);
    body.append(row3);
    tableElement.append(body);
}

//For Dynamically Loading Create/Edit Panel Lists
//function CheckActivePanel() {
//    var FolderList;
//    if ($("#hdnActivePanel").val() == "Create") {
//        FolderList = _divFolderList;
//    }
//    else if ($("#hdnActivePanel").val() == "Edit") {
//        FolderList = _divEditFolderList;
//    }
//    return FolderList;
//}

function refreshProjectsLists() {
    $("#btnRefreshLists")[0].click();
}

function validation() {
    var validation = true;
    var selectedIndex = $('#' + _ddlProjAccess).val();
    if (selectedIndex == 2) {
        if ($("#ctl00_bodyContainer_CreateEditProject_lstSelectedEnterprises option").length == 0) {
            element = document.getElementById('ctl00_bodyContainer_CreateEditProject_pnlProjectMassage').getElementsByTagName('div');
            element.MessageBox.className = "";
            element.MessageBox.className = "error";
            element.MessageBox.getElementsByTagName('p')[0].innerHTML = "You have not selected any Enterprise.";
            validation = false;
        }
    }
    if (selectedIndex == 3) {
        if ($("#ctl00_bodyContainer_CreateEditProject_assignedPrac option").length == 0) {
            element = document.getElementById('ctl00_bodyContainer_CreateEditProject_pnlProjectMassage').getElementsByTagName('div');
            element.MessageBox.className = "";
            element.MessageBox.className = "error";
            element.MessageBox.getElementsByTagName('p')[0].innerHTML = "You have not selected any Practice.";
            validation = false;
        }
    }
    if (selectedIndex == 4) {
        if ($("#ctl00_bodyContainer_CreateEditProject_lstSelectedMGR option").length == 0) {
            element = document.getElementById('ctl00_bodyContainer_CreateEditProject_pnlProjectMassage').getElementsByTagName('div');
            element.MessageBox.className = "";
            element.MessageBox.className = "error";
            element.MessageBox.getElementsByTagName('p')[0].innerHTML = "You have not selected any Medical Group.";
            validation = false
        }
    }
    return validation;
}

function editValidation() {
    var validation = true;
    var selectedIndex = $('#' + _ddlAllowProjAccess).val();
    if (selectedIndex == 2) {
        if ($("#ctl00_bodyContainer_CreateEditProject_lstAssignedEnt option").length == 0) {
            element = document.getElementById('ctl00_bodyContainer_CreateEditProject_pnlEditProjMessage').getElementsByTagName('div');
            element.MessageBox.className = "";
            element.MessageBox.className = "error";
            element.MessageBox.getElementsByTagName('p')[0].innerHTML = "You have not selected any Enterprise.";
            validation = false
        }
    }
    if (selectedIndex == 3) {
        if ($("#ctl00_bodyContainer_CreateEditProject_assignedEditPrac option").length == 0) {
            element = document.getElementById('ctl00_bodyContainer_CreateEditProject_pnlEditProjMessage').getElementsByTagName('div');
            element.MessageBox.className = "";
            element.MessageBox.className = "error";
            element.MessageBox.getElementsByTagName('p')[0].innerHTML = "You have not selected any Practice.";
            validation = false
        }
    }
    if (selectedIndex == 4) {
        if ($("#ctl00_bodyContainer_CreateEditProject_lstBoxAssignedMGR option").length == 0) {
            element = document.getElementById('ctl00_bodyContainer_CreateEditProject_pnlEditProjMessage').getElementsByTagName('div');
            element.MessageBox.className = "";
            element.MessageBox.className = "error";
            element.MessageBox.getElementsByTagName('p')[0].innerHTML = "You have not selected any Medical Group.";
            validation = false
        }
    }
    return validation;
}