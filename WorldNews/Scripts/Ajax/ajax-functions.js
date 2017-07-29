$(document).ajaxSend(function (event, request, settings) {
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

function onSuccessStay(data, url) {
    if (data.success) {
        $.get(url, function (html) {
            var content = $("#main-content");
            content.fadeOut("fast", function () {
                content.html(html).hide();
                content.fadeIn("slow");
            });
        });
    }
    else {
        var html = "";
        for (var i = 0; i < data.errors.length; i++) {
            html += data.errors[i] + "<br />";
        }

        alert("(TODO - display normal errors) Error!");
    }
}

function closeSidebar() {
    $("#sidebar").removeClass("active");
    $(".overlay").fadeOut();
}