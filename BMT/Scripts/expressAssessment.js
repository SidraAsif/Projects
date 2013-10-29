/*To maintain the right border hight update hight on each expand/collapse*/
$(document).ready(function () {
    //$('.body-container-left-tree').css("height", $(".body-container-right").height());
    $('#ctl00_bodyContainer_ExpressAssessment_btnSave').click(function () {
        $('html,body').animate({
            scrollTop: $(".body-container-right").offset().top
        }, 0);
    });
});