/*To maintain the right border hight update hight on each expand/collapse*/
$(document).ready(function () {
    //$('.body-container-left-tree').css("height", $(".body-container-right").height());
});

$('.uploadPopUp').click(function (e) {
    $('.uploadbox, .uploadbox-popup').fadeIn(300);
});
$('a#close-UploadPopUp').click(function () {
    $('.uploadbox, .uploadbox-popup').fadeOut(300);
});