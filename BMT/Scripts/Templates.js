
$(document).ready(function () {


});

var validFilesTypes = ["xls" , "xlsx"];

function ValidateFile() {

    var regExp = /^[a-zA-Z0-9\_\-\.\s\(\)]{1,50}$/;

    var file = $('#uploadFile').val();
    file = file.split('\\').pop();

    var expressVarify = regExp.test(file);
    if (file == "") {
        ShowMessage("Please Select a file to import data.", "error");
        return false;
    }
    if (file.length == 0) {
        ShowMessage("Please Select a file to import data.", "error");
        return false;
    }
    else if (!expressVarify) {
        ShowMessage("File name is invalid.", "error");
        return false;
    }

    else if (file.length > 100) {
        ShowMessage("Doc Name can be up to 120 characters long.", "error");
        return false;
    }

    var path = file;
    var ext = path.substring(path.lastIndexOf(".") + 1, path.length).toLowerCase();
    var isValidFile = false;
    for (var i = 0; i < validFilesTypes.length; i++) {
        if (ext == validFilesTypes[i]) {
            isValidFile = true;
            break;
        }
    }
    if (!isValidFile) {
        ShowMessage("Only .xls and .xlsx Files are Allowed.", "error");
        return false;
    }
}


function ValidateName() {

    var regExp = /^[a-zA-Z0-9\_\-\.\s\(\)]{1,50}$/;
    var header = $('#txtAddHeader').val();
    alert('header');
    return false;
    
}

function ShowMessage(message, type) {

    $('#ctl00_bodyContainer_DisplayMessage1_CloseButton').parent().show();
    $('#ctl00_bodyContainer_DisplayMessage1_CloseButton').parent().removeClass();
    $('#ctl00_bodyContainer_DisplayMessage1_CloseButton').parent().addClass(type);
    $('#ctl00_bodyContainer_DisplayMessage1_CloseButton').parent().children().html(message);
    $('#ctl00_bodyContainer_DisplayMessage1_CloseButton').html("");
}

function ValidateTemplate() {

    //var regExp = /^(?!\s*$)[a-zA-Z0-9\_\-\.\s\(\)]{1,50}$/;
    var regExp = /^[a-zA-Z0-9\_\-\.\,\s\(\)\:\+\*\>\'\<\[\]\\\/\’]{1,250}$/;
    var templateName = $('#txtTemplateName').val();
    var shortName = $('#txtShortName').val();

    if (templateName == "" && shortName == "") {
        ShowTemplateMessage("Template Name And Short Name Are Required.", "error");
        return false;
    } 

    var expressVarify = regExp.test(templateName);
    if (templateName == "") {
        ShowTemplateMessage("Template Name Is Required.", "error");
        return false;
    }    
    else if (!expressVarify) {
        ShowTemplateMessage("Template Name Is Invalid.", "error");
        return false;
    }

    else if (templateName.length > 120) {
        ShowTemplateMessage("Template Name can be up to 120 characters long.", "error");
        return false;
    }

    
    var expressionVarify = regExp.test(shortName);
    if (shortName == "") {
        ShowTemplateMessage("Template Short Name Is Required.", "error");
        return false;
    }
    else if (!expressionVarify) {
        ShowTemplateMessage("Template Short name is invalid.", "error");
        return false;
    }

    else if (shortName.length > 50) {
        ShowTemplateMessage("Template Short Name can be up to 50 characters long.", "error");
        return false;
    } 
    
     
}

function ShowTemplateMessage(message, type) {

    $('#ctl00_bodyContainer_DisplayMessage2_CloseButton').parent().show();
    $('#ctl00_bodyContainer_DisplayMessage2_CloseButton').parent().removeClass();
    $('#ctl00_bodyContainer_DisplayMessage2_CloseButton').parent().addClass(type);
    //$('#MessageBox').addClass(type);
    $('#ctl00_bodyContainer_DisplayMessage2_CloseButton').parent().children().html(message);
    $('#ctl00_bodyContainer_DisplayMessage2_CloseButton').html("");
}

function selectAll(invoker) {
    // Since ASP.NET checkboxes are really HTML input elements
    //  let's get all the inputs 
    var inputElements = document.getElementsByTagName('input');

    for (var i = 0; i < inputElements.length; i++) {
        var myElement = inputElements[i];

        // Filter through the input types looking for checkboxes
        if (myElement.type === "checkbox") {

            // Use the invoker (our calling element) as the reference 
            //  for our checkbox status
            myElement.checked = invoker.checked;
        }
    }
}

$("#btnUploadSheet").live('click', function (e) {
    $("#lightbox, .edit-popup").fadeIn(300);

});

$("a#close-editpopup").live('click', function (e) {

    $('#MessageBox p').html('');
    $('#uploadFile').replaceWith('<input name="ctl00$bodyContainer$uploadFile" id="uploadFile" style="width: 240px;" type="file"/>');
    $("#lightbox, .edit-popup").fadeOut(300);
});

$("#btnTemplate").live('click', function (e) {
    $("#lightbox, .uploadbox-popup").fadeIn(300);    
});

$("a#close-popup").live('click', function (e) {
    $("#lightbox, .uploadbox-popup").fadeOut(300);
    $("#txtTemplateName").val('');
    $("#txtShortName").val('');
    $("#txtDescription").val('');
});

function cancelpopup() {
    $("#lightbox, .uploadbox-popup").fadeOut(300);
    $("#txtTemplateName").val('');
    $("#txtShortName").val('');
    $("#txtDescription").val('');   
}


function onclicknode(ContentType, ToolSectionId) {
    $('#' + _hdnSectionId).val(ToolSectionId);
    $('#' + _hdnContentType).val(ContentType);
}

function onDeleteCofirmation() {

    if (confirm('By deleting this content will delete this perminantly and remove all its saved data from its all template. Are you sure you want to continue?')) {
        return true;
    }
    else
        return false;

}

function onEditCofirmation() {

    if (confirm('By editing this content will update this on its all linked tepmlates. Are you sure you want to continue?')) {
        return true;
    }
    else
        return false;

}



 
    