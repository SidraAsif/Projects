Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (evt, args) {
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

    function EndRequestHandler(sender, args) {
        BindContextMenu();
    }
});

/*To maintain the right border height*/
$(document).ready(function () {
    $('div.body-container-left').css("height", $(".body-container-right").height());
    //$('.body-container-left-tree').css("height", $(".body-container-right").height());
});

/*print Report in new window*/
function print() {
    window.open('PrintReport.aspx');
}