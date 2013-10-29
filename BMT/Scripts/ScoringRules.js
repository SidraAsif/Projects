function PCMHtoggle(standardSequence) {
    if ($('#PCMHTable' + standardSequence).is(':visible')) {
        $('#PCMHTable' + standardSequence).removeClass();
        $('#imgElement' + standardSequence).attr('src', '../Themes/Images/Plus-1.png');
        $('#PCMHTable' + standardSequence).hide();
    }
    else {
        $('#PCMHTable' + standardSequence).addClass("subheaders-table");
        $('table#NCQASubmissionTable table').hide();
        $('#PCMHTable' + standardSequence).show();
        $('table#NCQASubmissionTable img.toggle-img').attr('src', '../Themes/Images/Plus-1.png');
        $('#imgElement' + standardSequence).attr('src', '../Themes/Images/Minus-1.png');
    }
}

function PCMHElementtoggle(pcmhSequence, standardSequence) {
    if ($('#elementTable' + pcmhSequence + standardSequence).is(':visible')) {
        $('#imgStandard' + pcmhSequence + standardSequence).attr('src', '../Themes/Images/Plus-1.png');
        $('#scoringRules' + pcmhSequence + standardSequence).hide();
        $('#simpleSum' + pcmhSequence + standardSequence).hide();
        $('#elementTable' + pcmhSequence + standardSequence).hide();
        $('#ctl00_bodyContainer_sr_subHeader' + pcmhSequence + standardSequence).hide();
        $('#ctl00_bodyContainer_sr_subHeaderRulesBase' + pcmhSequence + standardSequence).hide();
    } 
    
    else {
        $('table#PCMHTable' + pcmhSequence + ' table').hide();
        $('#scoringRules' + pcmhSequence + standardSequence).show();
        $('#simpleSum' + pcmhSequence + standardSequence).show();
        $('#elementTable' + pcmhSequence + standardSequence).show();

        if ($('#scoringRules' + pcmhSequence + standardSequence)[0].rows.length > 0) {
        $('#ctl00_bodyContainer_sr_subHeader' + pcmhSequence + standardSequence).show();
        $('#ctl00_bodyContainer_sr_subHeaderRulesBase' + pcmhSequence + standardSequence).show();
        }

        $('table#PCMHTable' + pcmhSequence + ' img.toggle-img').attr('src', '../Themes/Images/Plus-1.png');
        $('#imgStandard' + pcmhSequence + standardSequence).attr('src', '../Themes/Images/Minus-1.png');
    }
}


$(document).ready(function () {

});


function OnClickScoringRule(pcmhSequence, standardSequence) {
    $('#scoringRules' + pcmhSequence + standardSequence).show(); 
     }

function OnClickSimpleSum(pcmhSequence, standardSequence) {

    $('#simpleSum' + pcmhSequence + standardSequence).show();
   $('#scoringRules' + pcmhSequence + standardSequence).hide();

}

var elementMax;
var maxPoints;

function numberOnly(arg) {
    var textBox = $('#' + arg).val();
    if (textBox != '') {
        if (textBox != '0' && textBox != '1' && textBox != '2' && textBox != '3' && textBox != '4' && textBox != '5'
            && textBox != '6' && textBox != '7' && textBox != '8' && textBox != '9') {
            $('#' + arg).val('');
        }
    }
}


function isElementMaxNumberKey(evt, labelId) {
   var textBox = $('#' + evt).val();
    elementMax = parseFloat(textBox);
    //&& charCode != 110  &&  charCode != 190
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31
            && ((charCode < 48 || charCode > 57) && (charCode < 96 || charCode > 105))) {
        $('#' + labelId).show();
        $('#' + evt).val('');
        return false;
    }

    if (charCode == 16 || charCode == 220 || charCode == 109 || charCode == 187 || charCode == 219 || charCode == 221 || charCode == 222 || charCode == 186 || charCode == 190 || charCode == 188) {
        $('#' + labelId).show();
        $('#' + evt).val('');
        return false;
    }

    if ($('#' + evt)[0].value == "") {
        $('#' + labelId).show();
    }
    else {
        $('#'+labelId).hide();
    }
    return true;
}

function isNumberKey(evt, labelId) {
    var textBox = $('#' + evt).val();
    maxPoints = parseFloat(textBox);
    //&& charCode != 110 && charCode != 190
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31
            && ((charCode < 48 || charCode > 57) && (charCode < 96 || charCode > 105))) {
        $('#' + labelId).show();
        $('#' + evt).val('');
        return false;
    }

    if ($('#' + evt)[0].value == "") {
        $('#' + labelId).show();
    }
    else {
        $('#' + labelId).hide();
    }

    if (isNaN(parseFloat(textBox))) {
        $('#' + evt)[0].value = "";
    }


    return true;
}

function ConditionChange(evt, rowId, labelId) {
    if ($('#' + evt).val() != "pick") {
        //$("#scoringRulesTextBox" + rowId).prop("disabled", false);
        if ($("#scoringRulesTextBox" + rowId)[0].value == "")
        { $('#' + labelId).show(); }
               
    }
    else {
        //$("#scoringRulesTextBox" + rowId).prop("disabled", true);
        $('#' + labelId).hide();
    }

    //return true;
}


function AndOrChange(evt, rowId, labelId, listBoxSelected) {
    if ($('#' + evt).val() != "pick") {

        //$("#ddMustPresent" + rowId).prop("disabled", false);
        $('#' + labelId).show();

        if ($('#' + listBoxSelected).find(':selected').text() == "") {
            $('#' + labelId).show();
        }
        else {
            $('#' + labelId).hide(); 
        }
        
    }
    else {
        //$("#ddMustPresent" + rowId).prop("disabled", true);
        $('#' + labelId).hide();
    }
}

function ListBoxChange(labelId) {

        $('#' + labelId).hide();
    
}


