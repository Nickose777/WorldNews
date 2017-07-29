$(document).ajaxSend(function () {
    $.LoadingOverlay("show");
});

$(document).ajaxComplete(function () {
    $.LoadingOverlay("hide");
});

function onSuccess(html) {
    var content = $("#main-content");
    content.fadeOut("fast", function () {
        window.scrollTo(0, 0);
        content.html(html).hide();
        content.fadeIn("slow", function () {
            $("html, body").animate({
                scrollTop: $("#main-content").offset().top
            }, 1000);
        });
    });
}