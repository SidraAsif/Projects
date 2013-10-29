
$(function () {

    $(".bodytxt-field").each(function () {
        $tb = $(this);
        if ($tb.val() != this.title) {
            $tb.removeClass("bodytxt-field");
        }
    });

    $(".bodytxt-field").focus(function () {
        $tb = $(this);
        if ($tb.val() == this.title) {
            $tb.val("");
            $tb.removeClass("bodytxt-field");
        }
    });

    $(".bodytxt-field").blur(function () {
        $tb = $(this);
        if ($.trim($tb.val()) == "") {
            $tb.val(this.title);
            $tb.addClass("bodytxt-field");
        }
    });
}); 