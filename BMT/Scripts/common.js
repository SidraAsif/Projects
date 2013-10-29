// ************************************************
// TODO: To clear existing message on each popup display
// ************************************************
$(document).ready(function () {
    $('a').live('click', function (e) {
        if ($('#MessageBox')) {
            $('div#MessageBox').removeClass();
            $('div#MessageBox').addClass('clear');
            $('div#MessageBox').addClass('clear');
        }
    });

});

// **********************************************************
// TODO: Disable tab key on last control focus
// **********************************************************
$(document).ready(function () {
    $('input:text:first').focus();

    if ($("#btnSubmit")) {
        $("#btnSubmit").live('focus', function (e) {
            $(this).keypress(function (e) {
                if (e.keyCode == "9") { e.preventDefault(); }
            });
        });
    }

    if ($("#btnFNCancel")) {
        $("#btnFNCancel").live('focus', function (e) {
            $(this).keypress(function (e) {
                if (e.keyCode == "9") { e.preventDefault(); }
            });
        });
    }

    if ($("#btnFCNCancel")) {
        $("#btnFCNCancel").live('focus', function (e) {
            $(this).keypress(function (e) {
                if (e.keyCode == "9") { e.preventDefault(); }
            });
        });
    }

    if ($("#btnCancelElementNote")) {
        $("#btnCancelElementNote").live('focus', function (e) {
            $(this).keypress(function (e) {
                if (e.keyCode == "9") { e.preventDefault(); }
            });
        });
    }
});


