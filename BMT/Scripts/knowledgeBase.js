
$(document).ready(function () {
    var enddf = $("#btnSaveEditTemplate").position();
    var mytop = enddf.top;
});

function DisplayHeaderPopUp() {
    $('#CurentTemplateForm').css('opacity', '0.1');
    $('#kbPopup').css('opacity', '0.1');
    $('#lightbox, .header-popup').fadeIn(300);
    var browser = navigator.appName;
    element = document.getElementById('kbExistingPopup').getElementsByTagName('div');
    element.MessageBox.className = "";
    element.MessageBox.className = "";
    element.MessageBox.getElementsByTagName('p')[0].innerHTML = "";

    // Remove td to fix close[x] tag wrapping in IE
    if (browser != "Microsoft Internet Explorer") {
        $('.header-popup').css('top', '0%');
        $('.header-popup').css('top', '0%');
    }
}


$('a#close-Header').live('click', function () {
    $('#CurentTemplateForm').css('opacity', '100');
    $('#kbPopup').css('opacity', '100');
    $('#lightbox, .header-popup').fadeOut(300);
});

function DisplayInstructions(Instruction) {
    $('#lightbox, .Instrction-popup').fadeIn(300);
    var browser = navigator.appName;
    document.getElementById("Instruction").innerHTML = Instruction;
    return false;
    // Remove td to fix close[x] tag wrapping in IE
    if (browser != "Microsoft Internet Explorer") {
        $('.Instrction-popup').css('top', '0%');
        $('.Instrction-popup').css('top', '0%');
    }
}

$('a#close-Instrction-popup').live('click', function () {
    $('#lightbox, .Instrction-popup').fadeOut(300);
});

function savingTemplate(userId, KbType, templateId, kbId, parentId, grandParentId, isEditOrAdd) {
    var element = document.getElementById('kbPopup').getElementsByTagName('div');
    element.MessageBox.className = "";
    element.MessageBox.getElementsByTagName('p')[0].innerHTML = "";
    var frame = document.getElementById('formKBDocs');
    var displayName = frame.txtDisplayName.value;
    var instruction = frame.txtInstruction.value;
    var answerTypeId = "";
    var criticalToolText = "";
    var dataBoxHeader = "";
    var pageReference = 0;
    var copyTemplate = $('input:checkbox[id$=Copy]:checked', frame.headerInfo);
    var myarray = new Array();
    var isInfoDocsEnable = false;
    for (var i = 0; i < copyTemplate.length; i++) {
        myarray[i] = copyTemplate[i].defaultValue;
    }
    if (kbId == 0 && KbType != "Header") {
        if (!(document.getElementById('rdoAddExistingKB').checked) && !(document.getElementById('rdoAddNewKB').checked)) {
            var element = document.getElementById('kbPopup').getElementsByTagName('div');
            element.MessageBox.className = "";
            element.MessageBox.className = "error";
            element.MessageBox.getElementsByTagName('p')[0].innerHTML = "Please Select Knowledge Base Either from Existing or Create New";
            return false;
        }
    }
    else if (kbId == 0 && KbType == "Header") {
        if (document.getElementById('txtAddHeader').value == "") {
            var element = document.getElementById('kbPopup').getElementsByTagName('div');
            element.MessageBox.className = "";
            element.MessageBox.className = "error";
            element.MessageBox.getElementsByTagName('p')[0].innerHTML = "Enter Header Text";
            return false;
        }
    }

    var regExp = /^[a-zA-Z0-9\_\-\.\,\s\(\)\:\+\*\>\'\<\[\]\\\/\’]{1,250}$/;
    var regExpOnlyNum = /^[0-9]{1,50}$/;

    $('#rfvTxtDisplayName').css({ 'display': 'none' });
    $('#rfvTextTabLine1').css({ 'display': 'none' });
    $('#rfvMustPass').css({ 'display': 'none' });
    $('#rfvRdoCritical').css({ 'display': 'none' });
    $('#ErrorMsg1').css({ 'display': 'none' });
    $('#ErrorMsg2').css({ 'display': 'none' });
    $('#ErrorMsg3').css({ 'display': 'none' });
    $('#rfvTxtDataBoxHeader').css({ 'display': 'none' });
    $('#rfvRdoCriticalTool').css({ 'display': 'none' });

    if (KbType == "Header") {
        var tabName = frame.txtTabLine1.value;

        var kbTypeId = 1;
        var mustPass = false;
        var isCritical = false;
    }
    if (KbType == "SubHeader") {
        if (frame.rdoMustPassNo.checked) {
            var mustPass = false;
            var tabName = null;
            var kbTypeId = 2;
            var isCritical = false;
        }
        else if (frame.rdoMustPassYes.checked) {
            var mustPass = true;
            var tabName = null;
            var kbTypeId = 2;
            var isCritical = false;
        }
        if (frame.rbInfoDocYes.checked) {
            isInfoDocsEnable = true;
            var pageReference = document.getElementById("txtPageReference").value;
            if (pageReference == '') {
                $('#rfvPageReference').css({ 'display': 'inline', 'color': 'red', 'width': '4px' });
                return false;
            }
            else if (!(pageReference.match(regExpOnlyNum))) {
                $('#ErrorMsg3').css({ 'display': 'inline' });
                document.getElementById('ErrorMsgOfDataBox').innerHTML = "Page Reference must be in numbers!";
                return false;
            }
        }
    }
    if (KbType == "Question") {
        if (frame.rdoCriticalNo.checked) {
            criticalToolText = "";
            var mustPass = false;
            var tabName = null;
            var kbTypeId = 3;
            var isCritical = false;
            answerTypeId = $('input[name=Answer]:checked').val();
        }
        else if (frame.rdoCriticalYes.checked) {
            criticalToolText = document.getElementById('txtToolTip').value;
            if (criticalToolText == '') {
                $('#rfvRdoCriticalTool').css({ 'display': 'inline', 'color': 'red', 'width': '4px' });
                return false;
            }
            else if (!(criticalToolText.match(regExp))) {
                $('#ErrorMsg3').css({ 'display': 'inline' });
                document.getElementById('ErrorMsgOfDataBox').innerHTML = "Critical Tool Tip Text is Invalid!";
                return false;
            }
            else if (criticalToolText != "") {
                var splt = criticalToolText.split(" ");
                var find = false;
                for (var i = 0; i < splt.length; i++) {
                    if (splt[i] != "") {
                        find = true;
                        break;
                    }
                }
                if (!find) {
                    $('#rfvRdoCriticalTool').css({ 'display': 'inline', 'color': 'red', 'width': '4px' });
                    return false;
                }
            }
            var criticalToolTextfirstChar = criticalToolText.charAt(0);
            if (criticalToolTextfirstChar == " ") {
                $('#ErrorMsg3').css({ 'display': 'inline' });
                document.getElementById('ErrorMsgOfDataBox').innerHTML = "Critical Tool Tip Text is Invalid!";
                return false;
            }
            var tabName = null;
            var mustPass = false;
            var kbTypeId = 3;
            var isCritical = true;
            answerTypeId = $('input[name=Answer]:checked').val();
        }
    }
    if (parentId == "") {
        parentId = 0;
    }
    if (document.getElementById('chkDataBox').checked) {
        var dataBoxHeader = document.getElementById('txtDataBoxHeader').value;
        if (dataBoxHeader == '') {
            $('#rfvTxtDataBoxHeader').css({ 'display': 'inline', 'color': 'red', 'width': '4px' });
            return false;
        }
        else if (!(dataBoxHeader.match(regExp))) {
            $('#ErrorMsg3').css({ 'display': 'inline' });
            document.getElementById('ErrorMsgOfDataBox').innerHTML = "Data Box Header Name is Invalid!";
            return false;
        }
        else if (dataBoxHeader != "") {
            var splt = dataBoxHeader.split(" ");
            var find = false;
            for (var i = 0; i < splt.length; i++) {
                if (splt[i] != "") {
                    find = true;
                    break;
                }
            }
            if (!find) {
                $('#rfvTxtDataBoxHeader').css({ 'display': 'inline', 'color': 'red', 'width': '4px' });
                return false;
            }
        }
        var dataBoxHeaderfirstChar = dataBoxHeader.charAt(0);
        if (dataBoxHeaderfirstChar == " ") {
            $('#ErrorMsg3').css({ 'display': 'inline' });
            document.getElementById('ErrorMsgOfDataBox').innerHTML = "Data Box Header Name is Invalid!";
            return false;
        }
    }
    if (displayName == '') {
        $('#rfvTxtDisplayName').css({ 'display': 'inline', 'color': 'red', 'float': 'left', 'width': '4px' });
        if (KbType == "Header") {
            if (tabName == '' || tabName == undefined) {
                $('#rfvTextTabLine1').css({ 'display': 'inline', 'color': 'red', 'float': 'left', 'width': '4px' });
            }
        }
        else if (KbType == "SubHeader") {
            if (!(frame.rdoMustPassNo.checked) && !(frame.rdoMustPassYes.checked)) {
                $('#rfvMustPass').css({ 'display': 'inline', 'color': 'red', 'float': 'left', 'width': '4px' });
            }
        }
        else if (KbType == "Question") {
            if (!(frame.rdoCriticalYes.checked) && !(frame.rdoCriticalNo.checked)) {
                $('#rfvRdoCritical').css({ 'display': 'inline', 'color': 'red', 'float': 'left', 'width': '4px' });
            }
        }
        return false;
    }


    if (displayName != "") {
        var splt = displayName.split(" ");
        var find = false;
        for (var i = 0; i < splt.length; i++) {
            if (splt[i] != "") {
                find = true;
                break;
            }
        }
        if (!find) {
            $('#rfvTxtDisplayName').css({ 'display': 'inline', 'color': 'red', 'float': 'left', 'width': '4px' });
            return false;
        }

        var displayNamefirstChar = displayName.charAt(0);
        if (displayNamefirstChar == " ") {
            $('#ErrorMsg1').css({ 'display': 'inline' });
            document.getElementById('ErrorMessageOfKB').innerHTML = "Display Name is Invalid! Only Alphabet,Numbers and Spaces are allowed.";
            if (KbType == "Header") {
                var tabNamefirstChar = tabName.charAt(0);
                if (tabNamefirstChar == " ") {
                    $('#ErrorMsg2').css({ 'display': 'inline' });
                    document.getElementById('ErrorMsgOfKb').innerHTML = "Tab Name is Invalid! Only Alphabet,Numbers and Spaces are allowed.";
                } 
            }
            return false;
        }
    }
    if (!(displayName.match(regExp))) {
        $('#ErrorMsg1').css({ 'display': 'inline' });
        document.getElementById('ErrorMessageOfKB').innerHTML = "Display Name is Invalid! Only Alphabet,Numbers and Spaces are allowed.";
        if (KbType == "Header") {
            if (!(tabName.match(regExp))) {
                $('#ErrorMsg2').css({ 'display': 'inline' });
                document.getElementById('ErrorMsgOfKb').innerHTML = "Tab Name is Invalid! Only Alphabet,Numbers and Spaces are allowed.";
            }
        }
        return false;
    }

    if (KbType == "Header") {
        if (tabName == '') {
            $('#rfvTextTabLine1').css({ 'display': 'inline', 'color': 'red', 'float': 'left', 'width': '4px' });
            return false;
        }
        if (!(tabName.match(regExp))) {
            $('#ErrorMsg2').css({ 'display': 'inline' });
            document.getElementById('ErrorMsgOfKb').innerHTML = "Tab Name is Invalid! Only Alphabet,Numbers and Spaces are allowed.";
            return false;
        }
    }
    else if (KbType == "SubHeader") {
        if (!(frame.rdoMustPassNo.checked) && !(frame.rdoMustPassYes.checked)) {
            $('#rfvMustPass').css({ 'display': 'inline', 'color': 'red', 'float': 'left', 'width': '4px' });
            return false;
        }
    }
    else if (KbType == "Question") {
        if (!(frame.rdoCriticalYes.checked) && !(frame.rdoCriticalNo.checked)) {
            $('#rfvRdoCritical').css({ 'display': 'inline', 'color': 'red', 'float': 'left', 'width': '4px' });
            return false;
        }
    }
    if (grandParentId == "") {
        grandParentId = 0;
    }
    if (answerTypeId == "") {
        answerTypeId = "None";
    }
    SaveKb(templateId, kbId, kbTypeId, displayName, tabName, instruction, mustPass, answerTypeId, userId, isCritical, parentId, grandParentId, myarray, isEditOrAdd, dataBoxHeader, criticalToolText, isInfoDocsEnable, pageReference);
}

function SaveKb(templateId, kbId, kbTypeId, displayName, tabName, instruction, mustPass, answerTypeId, userId, isCritical, parentId, grandParentId, myarray, isEditOrAdd, dataBoxHeader, criticalToolText, isInfoDocsEnable, pageReference) {

    var kbType = "";
    if (kbTypeId == 1)
        kbType = "Header";
    else if (kbTypeId == 2)
        kbType = "Sub-Header";
    else if (kbTypeId == 3)
        kbType = "Question";

    var jsonText = JSON.stringify({ templateId: templateId, kbId: kbId, kbTypeId: kbTypeId, displayName: displayName, tabName: tabName, instruction: instruction, mustPass: mustPass, answerTypeId: answerTypeId, userId: userId, isCritical: isCritical, parentId: parentId, grandParentId: grandParentId, list: myarray, isEditOrAdd: isEditOrAdd, dataBoxHeader: dataBoxHeader, criticalToolText: criticalToolText, isInfoDocsEnable: isInfoDocsEnable, pageReference: pageReference });
    $.ajax({
        type: "POST",
        url: "../WebServices/KBServices.asmx/SaveKnowledgeBase",
        data: jsonText,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
        },
        complete: function () {
        },
        success: function (response) {
            if (response.d == "saved successfully") {
                var element = document.getElementById('kbPopup').getElementsByTagName('div');
                element.MessageBox.className = "";
                element.MessageBox.className = "success";
                if (kbId == 0)
                    element.MessageBox.getElementsByTagName('p')[0].innerHTML = kbType + " saved successfully";
                else
                    element.MessageBox.getElementsByTagName('p')[0].innerHTML = kbType + " updated successfully";


                document.getElementById('CurentTemplateForm').disabled = true;
                document.getElementById('kbPopup').disabled = true;
                document.getElementById('btnSaveEditTemplate').disabled = true;
                document.getElementById('txtDisplayName').disabled = true;
                document.getElementById('txtTabLine1').disabled = true;
                document.getElementById('txtInstruction').disabled = true;
                document.getElementById('txtTabLine2').disabled = true;
                document.getElementById('txtDataBoxHeader').disabled = true;
                document.getElementById('txtToolTip').disabled = true;
                if (isEditOrAdd == "Add" && kbTypeId != 1) {
                    document.getElementById('Select').disabled = true;
                    document.getElementById("KBSearch").disabled = true;
                    document.getElementById('txtAddNewKB').disabled = true;
                    document.getElementById('txtExistingKBName').disabled = true;
                    document.getElementById('rdoAddExistingKB').disabled = true;
                    document.getElementById('rdoAddNewKB').disabled = true;
                }
                else if (isEditOrAdd == "Add" && kbTypeId == 1) {
                    document.getElementById('txtAddHeader').disabled = true;
                }
                DisableKBPopUp();
            }
            else if (response.d == "This item is share in other template and you can not Edit it.") {
                var element = document.getElementById('kbPopup').getElementsByTagName('div');
                element.MessageBox.className = "";
                element.MessageBox.className = "warning";
                element.MessageBox.getElementsByTagName('p')[0].innerHTML = response.d;


                document.getElementById('CurentTemplateForm').disabled = true;
                document.getElementById('kbPopup').disabled = true;
                document.getElementById('btnSaveEditTemplate').disabled = true;
                document.getElementById('txtDisplayName').disabled = true;
                document.getElementById('txtTabLine1').disabled = true;
                document.getElementById('txtInstruction').disabled = true;
                document.getElementById('txtTabLine2').disabled = true;
                document.getElementById('txtDataBoxHeader').disabled = true;
                document.getElementById('txtToolTip').disabled = true;
                if (isEditOrAdd == "Add" && kbTypeId != 1) {
                    document.getElementById('Select').disabled = true;
                    document.getElementById("KBSearch").disabled = true;
                    document.getElementById('txtAddNewKB').disabled = true;
                    document.getElementById('txtExistingKBName').disabled = true;
                    document.getElementById('rdoAddExistingKB').disabled = true;
                    document.getElementById('AnsYesNo').disabled = true;
                    document.getElementById('AnsYesNoNA').disabled = true;
                    //                    document.getElementById('AnsNone').disabled = true;
                }
                else if (isEditOrAdd == "Add" && kbTypeId == 1) {
                    document.getElementById('txtAddHeader').disabled = true;
                }
                DisableKBPopUp();
            }
            else if (response.d == "Question Name Already Exists.") {
                var element = document.getElementById('kbPopup').getElementsByTagName('div');
                element.MessageBox.className = "";
                element.MessageBox.className = "error";
                element.MessageBox.getElementsByTagName('p')[0].innerHTML = response.d;
            }
            else if (response.d == "Sub-Header Name Already Exists.") {
                var element = document.getElementById('kbPopup').getElementsByTagName('div');
                element.MessageBox.className = "";
                element.MessageBox.className = "error";
                element.MessageBox.getElementsByTagName('p')[0].innerHTML = response.d;
            }
            else if (response.d == "Header Name Already Exists.") {
                var element = document.getElementById('kbPopup').getElementsByTagName('div');
                element.MessageBox.className = "";
                element.MessageBox.className = "error";
                element.MessageBox.getElementsByTagName('p')[0].innerHTML = response.d;
            }
            else if (response.d == "Scoring Rules Exists.") {
                if (confirm('By adding a content here will remove its saved data from this template. Are you sure you want to continue?')) {
                    var jsonText2 = JSON.stringify({ subHeaderId: parentId, tempId: templateId });
                    $.ajax({
                        type: "POST",
                        url: "../WebServices/KBServices.asmx/DeleteScoringRules",
                        data: jsonText2,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        beforeSend: function () {
                        },
                        complete: function () {
                        },
                        success: function (response) {
                            $("#btnSaveEditTemplate")[0].click();
                        },
                        failure: function (msg) {
                        }
                    });
                }
            }
            else {
                var element = document.getElementById('kbPopup').getElementsByTagName('div');
                element.MessageBox.className = "";
                element.MessageBox.className = "error";
                element.MessageBox.getElementsByTagName('p')[0].innerHTML = response.d;
                DisableKBPopUp();
            }

        },
        failure: function (msg) {
            var element = document.getElementById('kbPopup').getElementsByTagName('div');
            element.MessageBox.className = "";
            element.MessageBox.className = "error";
            element.MessageBox.getElementsByTagName('p')[0].innerHTML = kbType + "Not Saved";
        }
    });
}

function CopyKnowledgebase() {
    var ncqa = "";
    document.getElementById('formKBDocs');
    var len = $('#pnlHeaderList :input:radio').length;
    $('#pnlHeaderList :input[type=radio]').each(function () {
        if ($(this).is(':checked'))
            if (ncqa == "") {
                ncqa = $(this).next('label')[0].innerHTML;
            }
            else {
                ncqa = ncqa + ", " + $(this).next('label')[0].innerHTML;
            }
    });
    if (ncqa == "") {
        element = document.getElementById('kbExistingPopup').getElementsByTagName('div');
        element.MessageBox.className = "";
        element.MessageBox.className = "error";
        element.MessageBox.getElementsByTagName('p')[0].innerHTML = "Select Any Knowledge Base element first.";
    }
    else {
        element = document.getElementById('kbExistingPopup').getElementsByTagName('div');
        element.MessageBox.className = "";
        element.MessageBox.className = "success";
        element.MessageBox.getElementsByTagName('p')[0].innerHTML = "Knowledge Base selected Successfully.";
        document.getElementById('txtExistingKBName').value = ncqa;
    }
}

function OnSelectExistingKB(rdo) {
    if (rdo.id == "rdoAddExistingKB") {
        document.getElementById("txtExistingKBName").setAttribute('readonly');
        document.getElementById("txtAddNewKB").disabled = true;
        document.getElementById("KBSearch").disabled = false;
        document.getElementById("KBSearch").setAttribute("onclick", "DisplayHeaderPopUp(); return false;");
        document.getElementById('txtDisplayName').setAttribute('readonly');
        document.getElementById('txtTabLine1').setAttribute('readonly');
        document.getElementById('txtInstruction').setAttribute('readonly');
    }
    else if (rdo.id == "rdoAddNewKB") {
        document.getElementById("txtExistingKBName").setAttribute('readonly');
        document.getElementById("txtAddNewKB").disabled = false;
        document.getElementById("KBSearch").disabled = true;
        document.getElementById('txtDisplayName').removeAttribute('readonly');
        document.getElementById('txtTabLine1').removeAttribute('readonly');
        document.getElementById('txtInstruction').removeAttribute('readonly');
    }
}

$("a.ttt").hover(function () {
    $("body,html").css("overflow", "hidden");
    window.parent.$("#IframeDocViewer").css("overflow", "hidden");
});

$("a.ttt").mouseleave(function () {
    var iframeHeight = window.parent.$('#IframeDocViewer').height();
    var gridHeight = $('#datagridDocViewer').height();

    if (parseInt(gridHeight) < parseInt(iframeHeight) - 15) {
        $('html,body').animate({
            scrollTop: $("body").offset().top
        }, 0);
    }
});

function textCounter(field, maxlimit) {
    if (field.value.length > (maxlimit)) {
        field.value = field.value.substring(0, maxlimit);
        return false;
    }
    else {
        document.getElementById('descriptionLength').innerHTML = maxlimit - field.value.length + " characters left";
    }
}

function btnSave() {
    var end = $("#btnSaveEditTemplate").position();
    alert(end.top + "," + end.left);

}

function Copy() {
    var frame = document.getElementById('formKBDocs');
    frame.txtDisplayName.value = document.getElementById('txtAddHeader').value;
}

function CopyBack() {
    if (document.getElementById('txtAddHeader')) {
        var frame = document.getElementById('formKBDocs');
        document.getElementById('txtAddHeader').value = frame.txtDisplayName.value;
    }
}

function DisableKBPopUp() {
    if ($("#kbPopup").is(':disabled') == false) {
        $("#kbPopup").removeAttr('disabled');
        $("#kbPopup").addClass("is-disabled");
        $("#kbPopup").css({ 'color': 'Gray' });
    }
    if ($("#CurentTemplateForm").is(':disabled') == false) {
        $("#CurentTemplateForm").removeAttr('disabled');
        $("#CurentTemplateForm").addClass("is-disabled");
        $("#CurentTemplateForm").css({ 'color': 'Gray' });
        $("#right").css({ 'color': 'Gray' });
        $("#Critical").css({ 'color': 'Gray' });
        $("#CriticalToolTip").css({ 'color': 'Gray' });
        $("#infoDocument").css({ 'color': 'Gray' });
        $("#pageReference").css({ 'color': 'Gray' });
    }
    document.getElementById('chkDataBox').disabled = true;
    document.getElementById('rdoMustPassNo').disabled = true;
    document.getElementById('rdoMustPassYes').disabled = true;
    document.getElementById('rdoCriticalNo').disabled = true;
    document.getElementById('rdoCriticalYes').disabled = true;
    document.getElementById('AnsYesNo').disabled = true;
    document.getElementById('AnsYesNoNA').disabled = true;
    document.getElementById('rbInfoDocNo').disabled = true;
    document.getElementById('rbInfoDocYes').disabled = true;
    document.getElementById('txtPageReference').disabled = true;
}

function OnDataBoxCheckChanged() {
    if (document.getElementById('chkDataBox').checked) {
        document.getElementById('databoxHeader').disabled = false;
        document.getElementById('txtDataBoxHeader').removeAttribute('disabled');
    }
    else {
        document.getElementById('databoxHeader').disabled = true;
        document.getElementById('txtDataBoxHeader').disabled = true;
        $('#rfvTxtDataBoxHeader').css({ 'display': 'none' });
    }
}

function OnCriricalChanged(rdo) {
    if (rdo.id == "rdoCriticalNo") {
        document.getElementById('CriticalToolTip').disabled = true;
        document.getElementById('txtToolTip').disabled = true;
        $('#rfvRdoCriticalTool').css({ 'display': 'none' });
    }
    else if (rdo.id == "rdoCriticalYes") {
        document.getElementById('CriticalToolTip').disabled = false;
        document.getElementById('txtToolTip').removeAttribute('disabled');
    }
}

function OnInfoDocChanged(rdo) {
    if (rdo.id == "rbInfoDocNo") {
        document.getElementById('pageReference').disabled = true;
        document.getElementById('txtPageReference').disabled = true;
        $('#rfvPageReference').css({ 'display': 'none' });
    }
    else if (rdo.id == "rbInfoDocYes") {
        document.getElementById('pageReference').disabled = false;
        document.getElementById('txtPageReference').removeAttribute('disabled');
    }
}