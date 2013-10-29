
Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
function BeginRequestHandler(sender, args) {

    if (document.getElementById('ctl00_bodyContainer_txtLine1').value != "") {
        //$(".reportLightbox").fadeIn();
        $('#reportLightbox, .reportLightbox').fadeIn(300);
        $('#LoadingDiv').removeAttr("style");
    }
}
function EndRequestHandler(sender, args) {
    //$(".reportLightbox").fadeOut();
    $('#reportLightbox, .reportLightbox').fadeOut(300);
    $('.UpdateProgressContent').hide();
}


// On Document Ready
$(document).ready(function () {

    //$('#chkFactor').attr('checked', true);

    //Get the PCMH Standards...

    if (($('#hiddenContentType')[0].defaultValue) == "General Status Report") {

        $('#hiddenIsNewRport').val('0');
        //            $.ajax({
        //                type: "POST",
        //                url: "../WebServices/ReportService.asmx/GetNCQAStandards",
        //                data: "{}",
        //                contentType: "application/json; charset=utf-8",
        //                beforeSend: function () {
        //                },
        //                complete: function () {
        //                },
        //                success: function (response) {
        //                    var lstStandard = $('#lstPCMHStandard');
        //                    $('#lstPCMHStandard option').each(function (i, option) { $(option).remove(); });

        //                    var NCQADetails = response.d;
        //                    $.each(NCQADetails, function (index, NCQADetail) {
        //                        $(lstStandard).append('<option value="' + NCQADetail.Sequence + '">' + NCQADetail.Name + '</option>');
        //                    });
        //                    if ($('#lstPCMHStandard option')[0] != undefined) {
        //                        $('#lstPCMHStandard option')[0].selected = true;
        //                        $('#hiddenPCMHId').val('0');
        //                        $('#hiddenPCMHTitle').val('All Standards');
        //                    }
        //                },
        //                failure: function (msg) {
        //                }
        //            });

        //Get PCMH Elements ...

        $.ajax({
            type: "POST",
            url: "../WebServices/ReportService.asmx/GetNCQAElements",
            data: "{'standardSequence': '0'}",
            contentType: "application/json; charset=utf-8",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                var lstElements = $('#lstPCMHElements');
                $('#lstPCMHElements option').each(function (i, option) { $(option).remove(); });

                var NCQADetails = response.d;

                $.each(NCQADetails, function (index, NCQADetail) {
                    $(lstElements).append('<option value="' + NCQADetail.Sequence + '">' + NCQADetail.Name + '</option>');
                });
                if ($('#lstPCMHElements option')[0] != undefined) {
                    $('#lstPCMHElements option')[0].selected = true;
                    $('#hiddenElementId').val('0');
                    $('#hiddenElementTitle').val('All Elements');
                }
            },
            failure: function (msg) {
            }
        });

        //Get PCMH Factors...

        var pcmhFactorTitle = new Array();

        $.ajax({
            type: "POST",
            url: "../WebServices/ReportService.asmx/GetNCQAFactors",
            data: "{ 'standardSequence': '0', 'elementSequence': '0','selectedElements' :'0' }",
            contentType: "application/json; charset=utf-8",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                var lstFacotrs = $('#lstPCMHFacotrs');
                $('#lstPCMHFacotrs option').each(function (i, option) { $(option).remove(); });

                var NCQADetails = response.d;

                $.each(NCQADetails, function (index, NCQADetail) {
                    $(lstFacotrs).append('<option value="' + NCQADetail.Sequence + '">' + NCQADetail.Name + '</option>');
                });
                if ($('#lstPCMHFacotrs option')[0] != undefined) {
                    $('#lstPCMHFacotrs option')[0].selected = true;
                }
                if ($('#lstPCMHFacotrs').val() == "0") {
                    pcmhFactorsTitleArray.push("All Factors");
                    for (var allfactors = 1; allfactors < $('#lstPCMHFacotrs')[0].length; allfactors++) {
                        pcmhFactorTitle.push($('#lstPCMHFacotrs')[0][allfactors].text);
                        $('#HiddenFactorTitle').val(pcmhFactorTitle);
                        pcmhFactorsArray.push(pcmhFactorTitle[allfactors - 1].replace('-', '').replace('A', '1').replace('B', '2').replace('C', '3').replace('D', '4').replace('E', '5').replace('F', '6').replace('G', '7'));
                    }
                    $('#hiddenFactorTitleArray').val(pcmhFactorsTitleArray);
                    $('#hiddenFactorArray').val(pcmhFactorsArray);
                }
            },
            failure: function (msg) {
            }
        });

        //            // Get Consultant List..

        //            $.ajax({
        //                type: "POST",
        //                url: "../WebServices/ReportService.asmx/GetConsultant",
        //                data: "{}",
        //                contentType: "application/json; charset=utf-8",
        //                beforeSend: function () {
        //                },
        //                complete: function () {
        //                },
        //                success: function (response) {
        //                    var lstConsultant = $('#lstConsultant');
        //                    $('#lstConsultant option').each(function (i, option) { $(option).remove(); });

        //                    var NCQADetails = response.d;
        //                    $.each(NCQADetails, function (index, NCQADetail) {
        //                        $(lstConsultant).append('<option value="' + NCQADetail.UserID + '">' + NCQADetail.UserName + '</option>');
        //                    });
        //                    if ($('#lstConsultant option')[0] != undefined) {
        //                        $('#lstConsultant option')[0].selected = true;
        //                        $('#hiddenConsultantId').val('0');
        //                        $('#hiddenConsultantName').val('All Consultants');
        //                    }
        //                },
        //                failure: function (msg) {
        //                }
        //            });

        //            //Get Practices Sizes ... 

        //            $.ajax({
        //                type: "POST",
        //                url: "../WebServices/ReportService.asmx/PracticeSize",
        //                data: "{}",
        //                contentType: "application/json; charset=utf-8",
        //                beforeSend: function () {
        //                },
        //                complete: function () {
        //                },
        //                success: function (response) {
        //                    var lstPracticeSize = $('#lstPracticeSize');
        //                    $('#lstPracticeSize option').each(function (i, option) { $(option).remove(); });

        //                    var NCQADetails = response.d;
        //                    $.each(NCQADetails, function (index, NCQADetail) {
        //                        $(lstPracticeSize).append('<option value="' + NCQADetail.ID + '">' + NCQADetail.Name + '</option>');
        //                    });
        //                    if ($('#lstPracticeSize option')[0] != undefined) {
        //                        $('#lstPracticeSize option')[0].selected = true;
        //                        $('#hiddenPracticeSizeId').val('0');
        //                        $('#hiddenPracticeSizeTitle').val('All Practices');
        //                    }
        //                },
        //                failure: function (msg) {
        //                }
        //            });


        if ($('#lstComplete option')[0] != undefined) {
            $('#lstComplete option')[0].selected = true;
            $('#hiddenCompleteId').val('0');
            $('#hiddenCompleteText').val('All percentages');
        }
    }

    ///////////////////////////////////////////# XML  RELOAD #/////////////////////////////////////////////////////////

    else if (($('#hiddenContentType')[0].defaultValue) != "") {
        $('#hiddenIsNewRport').val('1');

        //Get the PCMH Standards...

        $.ajax({
            type: "POST",
            url: "../WebServices/ReportService.asmx/GetNCQAStandards",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                var lstStandard = $('#lstPCMHStandard');
                $('#lstPCMHStandard option').each(function (i, option) { $(option).remove(); });

                var NCQADetails = response.d;
                $.each(NCQADetails, function (index, NCQADetail) {
                    $(lstStandard).append('<option value="' + NCQADetail.Sequence + '">' + NCQADetail.Name + '</option>');
                });
                if ($('#lstPCMHStandard option') != undefined)
                    $('#lstPCMHStandard option')[$('#hiddenPCMHId').val()].selected = true;
            },
            failure: function (msg) {
            }
        });

        //Get PCMH Elements ...

        var sequense = $('#hiddenPCMHId').val();
        $.ajax({
            type: "POST",
            url: "../WebServices/ReportService.asmx/GetNCQAElements",
            data: "{'standardSequence': '" + sequense + "'}",
            contentType: "application/json; charset=utf-8",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                var lstElements = $('#lstPCMHElements');
                $('#lstPCMHElements option').each(function (i, option) { $(option).remove(); });

                var NCQADetails = response.d;

                $.each(NCQADetails, function (index, NCQADetail) {
                    $(lstElements).append('<option value="' + NCQADetail.Sequence + '">' + NCQADetail.Name + '</option>');
                });

                if ($('#lstPCMHElements option') != undefined) {
                    if ($('#hiddenElementId').val().length == 1) {
                        $('#lstPCMHElements option')[$('#hiddenElementId').val()].selected = true;
                    }
                    else
                        $('#lstPCMHElements option')[$('#hiddenElementId').val().split("")[1, 1]].selected = true;
                }
            },
            failure: function (msg) {
            }
        });

        // Get PCMH Factors ...

        var pcmhFactorTitle = new Array();
        var standardSequence = $('#hiddenPCMHId').val();
        var elementSequence = ($('#hiddenPCMHId').val() + $('#hiddenElementId').val());
        var selectedElements = $('#hiddenElementTitle').val();
        var arrayFactor = new Array();

        for (var factorTitleIndex = 0; factorTitleIndex < $('#hiddenFactorTitleArray').val().split(',').length; factorTitleIndex++) {
            arrayFactor.push($('#hiddenFactorTitleArray').val().split(',')[factorTitleIndex]);
            pcmhFactorsTitleArray.push($('#hiddenFactorTitleArray').val().split(',')[factorTitleIndex]);
        }

        $.ajax({
            type: "POST",
            url: "../WebServices/ReportService.asmx/GetNCQAFactors",
            data: "{ 'standardSequence': '" + standardSequence
        + "', 'elementSequence': '" + elementSequence
        + "', 'selectedElements' : ' " + selectedElements
        + "' }",
            contentType: "application/json; charset=utf-8",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                var lstFacotrs = $('#lstPCMHFacotrs');
                $('#lstPCMHFacotrs option').each(function (i, option) { $(option).remove(); });

                var NCQADetails = response.d;

                $.each(NCQADetails, function (index, NCQADetail) {
                    $(lstFacotrs).append('<option value="' + index + '">' + NCQADetail.Name + '</option>');
                });
                for (var factorIndex = 0; factorIndex < $('#lstPCMHFacotrs option').length; factorIndex++)
                    for (var arrayFactorIndex = 0; arrayFactorIndex < arrayFactor.length; arrayFactorIndex++) {
                        if ($('#lstPCMHFacotrs option')[factorIndex].text == arrayFactor[arrayFactorIndex]) {
                            $('#lstPCMHFacotrs option')[factorIndex].selected = true;
                            break;
                        }
                    }
                if ($('#lstPCMHFacotrs').val() == "0") {
                    pcmhFactorsTitleArray.push("All Factors");
                    for (var allfactors = 1; allfactors < $('#lstPCMHFacotrs')[0].length; allfactors++) {
                        pcmhFactorTitle.push($('#lstPCMHFacotrs')[0][allfactors].text);
                        $('#HiddenFactorTitle').val(pcmhFactorTitle);
                        pcmhFactorsArray.push(pcmhFactorTitle[allfactors - 1].replace('-', '').replace('A', '1').replace('B', '2').replace('C', '3').replace('D', '4').replace('E', '5').replace('F', '6').replace('G', '7'));
                    }
                    $('#hiddenFactorTitleArray').val(pcmhFactorsTitleArray);
                    $('#hiddenFactorArray').val(pcmhFactorsArray);
                }

            },
            failure: function (msg) {
            }
        });

        //Get Consultants Name ...

        var selectedConsultants = $('#hiddenConsultantName').val();
        var arrayConsultants = new Array();
        for (var k = 0; k < $('#hiddenConsultantName').val().split(',').length; k++) {
            arrayConsultants.push($('#hiddenConsultantName').val().split(',')[k]);
        }
        $.ajax({
            type: "POST",
            url: "../WebServices/ReportService.asmx/GetConsultant",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                var lstConsultant = $('#lstConsultant');
                $('#lstConsultant option').each(function (i, option) { $(option).remove(); });

                var NCQADetails = response.d;
                $.each(NCQADetails, function (index, NCQADetail) {
                    $(lstConsultant).append('<option value="' + NCQADetail.UserID + '">' + NCQADetail.UserName + '</option>');
                });
                if ($('#lstConsultant option') != undefined)
                    for (var consultantIndex = 0; consultantIndex < $('#lstConsultant option').length; consultantIndex++)
                        for (var j = 0; j < arrayConsultants.length; j++)
                            if ($('#lstConsultant option')[consultantIndex].text == arrayConsultants[j]) {
                                $('#lstConsultant option')[consultantIndex].selected = true;
                                break;
                            }
            },
            failure: function (msg) {
            }
        });

        //Get Practice Sizes ...

        var selectedPracticeSize = $('#hiddenPracticeSizeTitle').val();
        var arrayPracticeSize = new Array();
        for (var k = 0; k < $('#hiddenPracticeSizeTitle').val().split(',').length; k++) {
            arrayPracticeSize.push($('#hiddenPracticeSizeTitle').val().split(',')[k]);
        }
        $.ajax({
            type: "POST",
            url: "../WebServices/ReportService.asmx/PracticeSize",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                var lstPracticeSize = $('#lstPracticeSize');
                $('#lstPracticeSize option').each(function (i, option) { $(option).remove(); });

                var NCQADetails = response.d;
                $.each(NCQADetails, function (index, NCQADetail) {
                    $(lstPracticeSize).append('<option value="' + NCQADetail.ID + '">' + NCQADetail.Name + '</option>');
                });
                if ($('#lstPracticeSize option') != undefined)
                    for (var consultantIndex = 0; consultantIndex < $('#lstPracticeSize option').length; consultantIndex++)
                        for (var j = 0; j < arrayPracticeSize.length; j++)
                            if ($('#lstPracticeSize option')[consultantIndex].text == arrayPracticeSize[j]) {
                                $('#lstPracticeSize option')[consultantIndex].selected = true;
                                break;
                            }
            },
            failure: function (msg) {
            }
        });
        if ($('#lstComplete option') != undefined)
            $('#lstComplete option')[$('#hiddenCompleteId').val()].selected = true;
    }

});

///////////////////////////////////// END  OF  DOCUMENT  READY  FUNCTION  //////////////////////////////////////////////////////

function GetElements() {
    var sequense = $('#lstPCMHStandard').val();
    $('#hiddenPCMHId').val($('#lstPCMHStandard').val());
    $('#hiddenElementId').val($('#lstPCMHElements').val());

    document.getElementById("lstPCMHFacotrs").options.length = 0;

    $.ajax({
        type: "POST",
        url: "../WebServices/ReportService.asmx/GetNCQAElements",
        data: "{'standardSequence': '" + sequense + "'}",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
        },
        complete: function () {
        },
        success: function (response) {
            var lstElements = $('#lstPCMHElements');
            $('#lstPCMHElements option').each(function (i, option) { $(option).remove(); });

            var NCQADetails = response.d;

            $.each(NCQADetails, function (index, NCQADetail) {
                $(lstElements).append('<option value="' + NCQADetail.Sequence + '">' + NCQADetail.Name + '</option>');
            });

        },
        failure: function (msg) {
        }
    });

}

function GetFactors() {
    var arrayFactor = new Array();
    for (var k = 0; k < pcmhFactorsTitleArray.length; k++) {
        arrayFactor.push(pcmhFactorsTitleArray[k]);
    }

    $('#hiddenPCMHId').val($('#lstPCMHStandard').val());
    $('#hiddenElementId').val($('#lstPCMHElements').val());
    $('#hiddenFactorId').val($('#hiddenFactorArray').val());
    $('#HiddenFactorTitle').val(arrayFactor);
    var standardSequence = $('#lstPCMHStandard').val();
    var elementSequence = $('#lstPCMHElements').val();
    var selectedElements = $('#lstPCMHElements :selected').text();
    $.ajax({
        type: "POST",
        url: "../WebServices/ReportService.asmx/GetNCQAFactors",
        data: "{ 'standardSequence': '" + standardSequence
        + "', 'elementSequence': '" + elementSequence
        + "', 'selectedElements' : ' " + selectedElements
        + "' }",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
        },
        complete: function () {
        },
        success: function (response) {
            var lstFacotrs = $('#lstPCMHFacotrs');
            $('#lstPCMHFacotrs option').each(function (i, option) { $(option).remove(); });

            var NCQADetails = response.d;

            $.each(NCQADetails, function (index, NCQADetail) {
                $(lstFacotrs).append('<option value="' + index + '">' + NCQADetail.Name + '</option>');
            });
            for (var i = 0; i < $('#lstPCMHFacotrs option').length; i++)
                for (var j = 0; j < arrayFactor.length; j++)
                    if ($('#lstPCMHFacotrs option')[i].text == arrayFactor[j]) {
                        $('#lstPCMHFacotrs option')[i].selected = true;
                        break;
                    }
        },
        failure: function (msg) {
        }
    });
}


var pcmhFactorsArray = new Array();
var pcmhFactorsTitleArray = new Array();

function GetSelectedFactors() {
    var pcmhStandardTitle = new Array();
    var pcmhStandardId = new Array();

    $('#hiddenPCMHId').val($('#lstPCMHStandard').val());
    for (var i = 0; i < $('#lstPCMHStandard').val().length; i++) {
        pcmhStandardTitle.push($('#lstPCMHStandard')[0][$('#lstPCMHStandard').val()[i]].text);
        $('#hiddenPCMHTitle').val(pcmhStandardTitle);
    }

    var pcmhElementTitle = new Array();
    var pcmhElementId = new Array();
    if ($('#lstPCMHElements').val() == "0") {

        pcmhElementTitle.push($('#lstPCMHElements')[0][0].text);
        $('#hiddenElementId').val("0");
        $('#hiddenElementTitle').val(pcmhElementTitle);
    }
    else {
        $('#hiddenElementId').val($('#lstPCMHElements').val());
        pcmhElementTitle.push($('#lstPCMHElements')[0][$('#lstPCMHElements').val().split("")[1, 1]].text);
        $('#hiddenElementTitle').val(pcmhElementTitle);

    }

    var pcmhFactorTitle = new Array();
    var pmcnFactorId = new Array();
    $('#hiddenFactorId').val($('#lstPCMHFacotrs').val());

    if (pcmhFactorsTitleArray[0] == "All Factors") {
        while (pcmhFactorsTitleArray[0] != null) {
            pcmhFactorsTitleArray.pop();
        }
        pcmhFactorsArray = [];
    }

    for (var h = 0; h < $('#lstPCMHFacotrs').val().length; h++) {
        if (pcmhFactorsTitleArray[0] != undefined) {
            for (var s = pcmhFactorsTitleArray.length; s >= 0; s--) {
                if (pcmhFactorsTitleArray[s] != undefined) {
                    if (pcmhFactorsTitleArray[s].split("-")[0] == $('#lstPCMHFacotrs')[0][$('#lstPCMHFacotrs').val()[h]].text.split("-")[0]) {
                        pcmhFactorsTitleArray.pop(s);
                        pcmhFactorsArray.pop(s);
                    }
                }
            }
        }
    }

    if ($('#lstPCMHFacotrs').val() == "0") {
        for (var allfactors = 1; allfactors < $('#lstPCMHFacotrs')[0].length; allfactors++) {
            pcmhFactorTitle.push($('#lstPCMHFacotrs')[0][allfactors].text);
            for (var selectedFactor = 0; selectedFactor < pcmhFactorsTitleArray.length; selectedFactor++) {
                if (pcmhFactorsTitleArray[selectedFactor] == pcmhFactorTitle[allfactors - 1]) {
                    pcmhFactorsTitleArray.splice(selectedFactor, 1);
                }
            }
            $('#HiddenFactorTitle').val(pcmhFactorTitle);
            if (pcmhFactorTitle[allfactors - 1] != undefined) {
                pcmhFactorsTitleArray.push(pcmhFactorTitle[allfactors - 1]);
                pcmhFactorsArray.push(pcmhFactorTitle[allfactors - 1].replace('-', '').replace('A', '1').replace('B', '2').replace('C', '3').replace('D', '4').replace('E', '5').replace('F', '6').replace('G', '7'));
            }
        }
    }
    else {
        for (var k = 0; k < $('#lstPCMHFacotrs').val().length; k++) {
            pcmhFactorTitle.push($('#lstPCMHFacotrs')[0][$('#lstPCMHFacotrs').val()[k]].text);
            $('#HiddenFactorTitle').val(pcmhFactorTitle);
            pcmhFactorsTitleArray.push(pcmhFactorTitle[k]);
            pcmhFactorsArray.push(pcmhFactorTitle[k].replace('-', '').replace('A', '1').replace('B', '2').replace('C', '3').replace('D', '4').replace('E', '5').replace('F', '6').replace('G', '7'));
        }
    }

    $('#hiddenFactorTitleArray').val(pcmhFactorsTitleArray);
    $('#hiddenFactorArray').val(pcmhFactorsArray);

}

function GetSelectedConsultant() {
    var consultantId = new Array();
    var consultantName = new Array();
    $('#hiddenConsultantId').val($('#lstConsultant').val());

    for (var selectedIndex = 0; selectedIndex < $('#lstConsultant').val().length; selectedIndex++) {
        for (var n = 0; n < $('#lstConsultant')[0].length; n++) {
            if ($('#lstConsultant').val()[selectedIndex] == $('#lstConsultant')[0][n].value) {
                consultantName.push($('#lstConsultant')[0][n].text);
                $('#hiddenConsultantName').val(consultantName);
            }
        }
    }
}

function GetSelectedComplete() {
    var completeId = new Array();
    var completeText = new Array();
    $('#hiddenCompleteId').val($('#lstComplete').val());

    for (var q = 0; q < $('#lstComplete').val().length; q++) {
        completeText.push($('#lstComplete')[0][$('#lstComplete').val()[q]].text);
        $('#hiddenCompleteText').val(completeText);
    }

}

function GetSelectedPractice() {
    var practiceId = new Array();
    var practiceTitle = new Array();
    $('#hiddenPracticeSizeId').val($('#lstPracticeSize').val());

    for (var q = 0; q < $('#lstPracticeSize').val().length; q++) {
        practiceTitle.push($('#lstPracticeSize')[0][$('#lstPracticeSize').val()[q]].text);
        $('#hiddenPracticeSizeTitle').val(practiceTitle);
    }
}

// controls id
var _hdnContentType = "hdnContentType";
var _hdnSectionId = "hdnSectionID";
var _hdnTreeNodeID = "hdnTreeNodeID";
function onsavereport() {
    if ($('#hiddenSectionId').val() == "2")
        onclicknode("General Status Report", $('#hiddenSectionId').val());
    else
        onclicknode($('#hiddenContentType').val(), $('#hiddenSectionId').val());
}
function onclicknode(ContentType, ReportSectionId) {
    var sectionId = $('#' + _hdnSectionId).val(ReportSectionId);
    $('#hdnContentType').val(ContentType);
    var arg = ContentType + '/' + ReportSectionId;
    var ActiveNodeId = $('#hiddenActiveNode').val();

    if (ActiveNodeId.length == 0) {
        ActiveNodeId = $.QueryString("Active");
        ActiveNodeId = (!ActiveNodeId) ? "null" : ActiveNodeId
    }
    var functionCall = "Reports.aspx?NodeContentType=" + ContentType + "&sectionId=" + ReportSectionId + '&Active=' + ActiveNodeId;

    setTimeout("window.location = \"" + functionCall + "\"", 2000);

}
function print() {

    //window.open("", '_blank');
    //  location.replace("PrintReport.aspx");
    //location.replace("PrintReport.aspx");
    window.open('PrintReport.aspx');
}


$('#chkOverallbarGraph').live('click', function () {
    if ($('#chkOverallbarGraph').attr('checked'))
        $('#chkGroupGraphs').attr('checked', false);

});

$('#chkGroupGraphs').live('click', function () {
    if ($('#chkGroupGraphs').attr('checked'))
        $('#chkOverallbarGraph').attr('checked', false);

});