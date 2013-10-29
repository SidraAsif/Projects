var selectedNode;
$(document).ready(function () {

    $("input#ctl00_innerMenuConatiner_btnUploadDocuments").click(function (e) {
        $("#lightbox, #lightbox-popup").fadeIn(300);
    });

    $("a#close-popup").click(function () {
        $("#lightbox, #lightbox-popup").fadeOut(300);
        window.location = window.location.href;

    });

    $('div.body-container-left').css("height", $(".body-container-right").height());
    $('div.body-container-left-tree').css("height", "auto");

    $('div.body-container-left').css("min-height", "300px");
    //$('div.body-container-left-tree').css("min-height", "300px");

});

