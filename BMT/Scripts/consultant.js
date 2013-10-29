function OnChangePassword() {
    $('#btnChangePassword').focus();
    $("#lightbox, .password-popup").fadeIn(300);
}

$('.close-popup').live('click', function () {
    $("#lightbox, #lightbox-popup").fadeOut(300);
});


function uploadLogo() {
    if ($('#txtUserName')) {
        if ($('#txtUserName').val() != "") {
            $("#LogoUploaderPopup").attr('src', '../LogoUploader.aspx?userName=' + $('#txtUserName').val());
        }
    }
    $("#lightbox, .uploadbox-popup").fadeIn(300);
}

$('#txtUserName').live('keyup', function () {
    if ($('#txtUserName')) {
        if ($('#txtUserName').val() != "") {
            $('#btnUploadLogo').removeAttr('disabled');
        }
        else {
            $('#btnUploadLogo').attr('disabled', 'disabled');
        }
    }
});