function callUploader() {

    var Name = $('#txtDocName').val();

    if (Name != "") {
        var splt = Name.split(" ");
        var find = false;
        for (var i = 0; i < splt.length; i++) {
            if (splt[i] != "") {
                find = true;
                break;
            }
        }

        if (!find) {
            ShowMessage("Space(s) is not a valid document name.", "error");
            return;
        }

    }

    $('#uploadcontainer .files').empty();
    //var regExp = /^[a-zA-Z0-9\s\-\#\\\/\&\.\,\'\(\)]{1,50}$/;
    var regExp = /^[a-zA-Z0-9\_\-\.\s\(\)]{1,50}$/;
    var file = $('#fudoc').val();
    var docName = $('#txtDocName').val();
    var expressVarify = regExp.test(docName);
    if (docName == "")
        expressVarify = true;
    var fileName = file.split('\\').pop();
    if (fileName.length == 0)
        ShowMessage("Please select a file.", "error");

    else if (!expressVarify)
        ShowMessage("Only Letters, numbers and - or _ are allowed in file names.", "error");

    else if (docName.length > 100)
        ShowMessage("Doc Name can be up to 120 characters long.", "error");

    else {
        var selecteddocType = "";

        $('#imgUploadDoc').attr('src', 'Themes/Images/uploading.gif');
        $.ajaxFileUpload({
            url: 'MOReFileHandler.ashx',
            secureuri: false,
            fileElementId: 'fudoc',
            data: { DocName: docName,
                elementId: $('#hiddenElementId').val(),
                factorId: $('#hiddenFactorId').val(),
                PCMHId: $('#hiddenPCMHId').val(),
                ReferencePage: document.getElementById('txtReferencePage').value,
                RelevancyLevel: $('#ddlRelevancyLevel option:selected').text(),
                docType: $('#ddldocType option:selected').text(),
                PracticeName: $('#hiddenPracticeName').val(),
                SiteName: $('#hiddenSiteName').val(),
                Node: $('#hiddenNode').val(),
                ProjectUsageId: $('#hiddenProjectUsageId').val(),
                SiteId: $('#hiddenSiteId').val(),
                PracticeId: $('#hiddenPracticeId').val(),
                templateId: $('#hiddenTemplateId').val()
            },
            complete: function () {
                $('#imgUploadDoc').attr('src', 'Themes/Images/uploadimg.png');
            },
            success: function (response) {

                var receivedResponse = $(response).find("pre").html();
                if (receivedResponse == 'success') {
                    // success message          
                    ShowMessage('File "' + fileName + '" uploaded successfully.', "success");

                    // clear existing file
                    var file = $('#fudoc').val('');

                    // update control value of parent window
                    UpdateParentControl();
                }
                else {
                    // MIME Type message
                    ShowMessage("Only images & documents are allowed.", "error");
                }
            },
            error: function (response) {
                // error message 
                ShowMessage("An error occured while uploading the file.", "error");
            }
        })
    }
    return false;
}


// **********************************************************
// TODO: Disable tab key on last control focus
// **********************************************************

$(document).ready(function () {
    // close popUp automatically if value capture failed from General tab & PCMH tab    
    var siteId = $('#hiddenSiteId').val();
    if (siteId == "0" || siteId == 0 || siteId == "") {
        window.parent.$('#lightbox, .uploadbox-popup').fadeOut(300);
    }

    $('input:text:first').focus();

    $('#fudoc').live('focus', function (e) {
        $(this).keypress(function (e) {
            if (e.keyCode == '9') { e.preventDefault(); }
        });
    });


    $("#rbDocSelecttion").live('click', function () {
        var uploadType = $("#rbDocSelecttion input:checked").val();
        if (uploadType == 1) //New
        {
            $("#pnlExistingDoc").hide();
            $("#lbDocs").hide();
            $("#imgLinkDoc").hide();

            $("#fudoc").show();
            $("#imgUploadDoc").show();
            $('#txtDocName').removeAttr('disabled');
        }
        else if (uploadType == 2) //Existing
        {
            $("#pnlExistingDoc").show();
            $("#lbDocs").show();
            $("#imgLinkDoc").show();

            $("#fudoc").hide();
            $("#imgUploadDoc").hide();
            GetStandards();

            $("#ddlElements").append('<option value="0">--Select--</option>');
            $("#ddlElements").attr('disabled', 'disabled');

            $("#ddlFactors").append('<option value="0">--Select--</option>');
            $("#ddlFactors").attr('disabled', 'disabled');

            $('#lbDocs >option').remove();
            $('#lbDocs').attr('disabled', 'disabled');

            $('#txtDocName').attr('disabled', 'disabled');
            $('#txtDocName').val("");
        }
    });
});


// Create copy of document for current factor by using selected critiria
function CreateDocLink() {
    var selectedDoc = $("#lbDocs option:selected").val();
    var fileTitle = $("#lbDocs option:selected").text();
    if (selectedDoc == undefined) {
        var totalDocs = $("#lbDocs option").length;
        if (totalDocs == 0)
            ShowMessage("No documents found.", "error");
        else
            ShowMessage("Please select a document to link.", "error");
    }
    else
        DocLinkGenerator(selectedDoc, fileTitle);

}

// create link of selected doc agaist current information
function DocLinkGenerator(selectedDoc, fileTitle) {
    $.ajax({
        type: "POST",
        url: "WebServices/NCQAService.asmx/DocLinkGeneratorForMORe",
        data: "{'docName': '" + $('#txtDocName').val()
        + "','elementId': '" + $('#hiddenElementId').val()
        + "','factorId': '" + $('#hiddenFactorId').val()
        + "','pcmhId': '" + $('#hiddenPCMHId').val()
        + "','referencePage': '" + document.getElementById('txtReferencePage').value
        + "','relevancyLevel': '" + $('#ddlRelevancyLevel option:selected').text()
        + "','docType': '" + $('#ddldocType option:selected').text()
        + "','practiceName': '" + $('#hiddenPracticeName').val()
        + "','siteName': '" + $('#hiddenSiteName').val()
        + "','node': '" + $('#hiddenNode').val()
        + "','projectUsageId': '" + $('#hiddenProjectUsageId').val()
        + "','siteId': '" + $('#hiddenSiteId').val()
        + "','practiceId': '" + $('#hiddenPracticeId').val()
        + "','location': '" + selectedDoc
        + "','fileTitle':'" + fileTitle
        + "','selectedStandard': '" + $("#ddlStandards option:selected").val() + "'}",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $('#imgLinkDoc').attr('src', 'Themes/Images/uploading.gif');
        },
        complete: function () {
            $('#imgLinkDoc').attr('src', 'Themes/Images/doc-link.png');
        },
        success: function (response) {
            var receivedResponse = response.d;
            if (receivedResponse) {
                // success message          
                ShowMessage('File "' + fileTitle + '" copied successfully.', "success");

                // update control value of parent window
                UpdateParentControl();
            }
            else {
                // if file is already linked
                ShowMessage('File(' + fileTitle + ') is already linked!', "warning");
            }

        },
        failure: function (msg) {

        }

    });
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
        window.parent.$('td#CellUploadedDocs' + elementId + factorId + '_' + PCMHId + selectedvalue).removeClass();
        window.parent.$('td#CellUploadedDocs' + elementId + factorId + '_' + PCMHId + selectedvalue).addClass('factor-control-hightlight');
    }
    else {
    
        window.parent.$('td#CellUploadedDocs' + elementId + factorId + '_' + PCMHId + selectedvalue).removeClass();
        window.parent.$('td#CellUploadedDocs' + elementId + factorId + '_' + PCMHId + selectedvalue).addClass('factor-control-important');
    }

}

// function to use the customer user control on client side
function ShowMessage(message, type) {
    $('#MessageBox').removeClass();
    $('#MessageBox').addClass(type);
    $('#MessageBox p').html(message);
}



function upload_Click(_docName, _referencePage, _relevancyLevel, _existingFile, _docLinkedTo, _docType) {

    var file = $('#fudoc').val();
    var fileName = file.split('\\').pop();
    if (fileName.length == 0)
        ShowMessage("Please select a file.", "error");

    else {
        $('#imgUploadDoc').attr('src', 'Themes/Images/uploading.gif');
        $.ajaxFileUpload({
            url: 'MOReFileHandler.ashx',
            secureuri: false,
            fileElementId: 'fudoc',
            data: { DocName: _docName,
                elementId: $('#hiddenElementId').val(),
                factorId: $('#hiddenFactorId').val(),
                PCMHId: $('#hiddenPCMHId').val(),
                ReferencePage: _referencePage,
                RelevancyLevel: _relevancyLevel,
                docType: _docType,
                PracticeName: $('#hiddenPracticeName').val(),
                SiteName: $('#hiddenSiteName').val(),
                Node: $('#hiddenNode').val(),
                ProjectUsageId: $('#hiddenProjectUsageId').val(),
                SiteId: $('#hiddenSiteId').val(),
                PracticeId: $('#hiddenPracticeId').val(),
                ExistingFile: _existingFile,
                DocLinkedTo: _docLinkedTo,
                templateId: $('#hiddenTemplateId').val(),
                filename: fileName
            },
            complete: function () {
                $('#imgUploadDoc').hide();
                $('#lblDocumentName').hide();
                $('#fudoc').hide();
                $('#imgOkCancel').parent().show();
            },
            success: function (response) {
                var receivedResponse = $(response).find("pre").html();
                if (receivedResponse == 'success') {
                    // success message          
                    ShowMessage('File "' + fileName + '" replaced successfully.', "success");

                    // clear existing file
                    var file = $('#fudoc').val('');
                }
                else if (receivedResponse == 'errorFileExt') {
                    // MIME Type message
                    ShowMessage("Only images & documents are allowed.", "error");
                }
                else {
                    // error message 
                    ShowMessage("An error occured while uploading the file.", "error");
                }
            },
            error: function (response) {
                // error message 
                ShowMessage("An error occured while uploading the file.", "error");
            }
        })
    }

}


$('#imgOkCancel').live('click', function () {
    window.parent.location.reload();
});


$('#ddlStandards').live('change', function () {
    var standardSequence = $("#ddlStandards option:selected").val();
    if (parseInt(standardSequence) == 0) {

        $('#ddlElements option').each(function (i, option) { $(option).remove(); });
        $("#ddlElements").append('<option value="0">--Select--</option>');
        $("#ddlElements").attr('disabled', 'disabled');

        $('#ddlFactors option').each(function (i, option) { $(option).remove(); });
        $("#ddlFactors").append('<option value="0">--Select--</option>');
        $("#ddlFactors").attr('disabled', 'disabled');

        $('#lbDocs >option').remove();
        $('#lbDocs').attr('disabled', 'disabled');
    }
    else if (parseInt(standardSequence) == 100) // to configure please go the NCQA Service constants area
    {
        GetDocs();
        $('#ddlElements option').each(function (i, option) { $(option).remove(); });
        $("#ddlElements").append('<option value="0">--Select--</option>');
        $("#ddlElements").attr('disabled', 'disabled');

        $('#ddlFactors option').each(function (i, option) { $(option).remove(); });
        $("#ddlFactors").append('<option value="0">--Select--</option>');
        $("#ddlFactors").attr('disabled', 'disabled');
    }
    else {
        $('#ddlFactors option').each(function (i, option) { $(option).remove(); });
        $("#ddlFactors").append('<option value="0">--Select--</option>');
        $("#ddlFactors").attr('disabled', 'disabled');

        $('#lbDocs >option').remove();
        $('#lbDocs').attr('disabled', 'disabled');

        GetElements();
    }

});

$('#ddlElements').live('change', function () {
    var standardSequence = $("#ddlElements option:selected").val();
    if (parseInt(standardSequence) == 0) {
        $('#ddlFactors option').each(function (i, option) { $(option).remove(); });
        $("#ddlFactors").append('<option value="0">--Select--</option>');
        $("#ddlFactors").attr('disabled', 'disabled');

        $('#lbDocs >option').remove();
        $('#lbDocs').attr('disabled', 'disabled');
    }
    else {
        GetFactors();
    }
});


$('#ddlFactors').live('change', function () {
    GetDocs();
});

function GetElements() {
    $.ajax({
        type: "POST",
        url: "WebServices/NCQAService.asmx/GetSubHeaders",
        data: "{ 'sequence': '" + $("#ddlStandards option:selected").val() + "' }",
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            $('#ddlElements').removeAttr('disabled');
            var ddlElements = $('#ddlElements');

            //Remove existing Option
            $('#ddlElements option').each(function (i, option) { $(option).remove(); });

            //Add Default itme in standard list
            $(ddlElements).append('<option value="0">--Select--</option>');

            var NCQADetails = response.d;
            $.each(NCQADetails, function (index, NCQADetail) {
                $(ddlElements).append('<option value="' + NCQADetail.Sequence + '">' + NCQADetail.Name + '</option>');
            });

        },
        failure: function (msg) {
        }

    });
}


function GetStandards() {
    $.ajax({
        type: "POST",
        url: "WebServices/NCQAService.asmx/GetHeaders",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            var ddlStandards = $('#ddlStandards');

            //Remove existing Option
            $('#ddlStandards option').each(function (i, option) { $(option).remove(); });

            //Add Default itme in standard list
            $(ddlStandards).append('<option value="0">--Select--</option>');

            var NCQADetails = response.d;
            $.each(NCQADetails, function (index, NCQADetail) {
                $(ddlStandards).append('<option value="' + NCQADetail.Sequence + '">' + NCQADetail.Name + '</option>');
            });

        },
        failure: function (msg) {
        }

    });
}



function GetFactors() {
    $.ajax({
        type: "POST",
        url: "WebServices/NCQAService.asmx/GetQuestions",
        data: "{ 'standardSequence': '" + $("#ddlStandards option:selected").val()
        + "', 'elementSequence': '" + $("#ddlElements option:selected").val()
        + "', 'currentFactor': '" + $('#hiddenFactorId').val()
        + "', 'currentStandard': '" + $('#hiddenPCMHId').val()
        + "', 'currentElement': '" + $('#hiddenElementId').val()
        + "' }",
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            var ddlFactors = $('#ddlFactors');
            $('#ddlFactors').removeAttr('disabled');

            //Remove existing Option
            $('#ddlFactors option').each(function (i, option) { $(option).remove(); });

            //Add Default itme in standard list
            $(ddlFactors).append('<option value="0">--Select--</option>');

            var NCQADetails = response.d;
            $.each(NCQADetails, function (index, NCQADetail) {
                $(ddlFactors).append('<option value="' + NCQADetail.Sequence + '">' + NCQADetail.Name + " " + +NCQADetail.Sequence + '</option>');
            });

        },
        failure: function (msg) {
        }

    });
}



function GetDocs() {
    $.ajax({
        type: "POST",
        url: "WebServices/NCQAService.asmx/GetDocsForMORe",
        data: "{ 'standardSequence': '" + $("#ddlStandards option:selected").val() + "', 'elementSequence': '" + $("#ddlElements option:selected").val() + "', 'factorSequence': '" + $("#ddlFactors option:selected").val() + "' }",
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            var lbDocs = $('#lbDocs');
            $('#lbDocs').removeAttr('disabled');

            //Remove existing Option
            $('#lbDocs option').each(function (i, option) { $(option).remove(); });

            var NCQADetails = response.d;
            $.each(NCQADetails, function (index, NCQADetail) {
                $(lbDocs).append('<option value="' + NCQADetail.Location + '">' + NCQADetail.Name + '</option>');
            });

        },
        failure: function (msg) {
        }

    });
}