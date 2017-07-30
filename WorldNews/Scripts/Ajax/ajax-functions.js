$(document).ajaxSend(function (event, request, settings) {
    $.LoadingOverlay("show");
});

$(document).ajaxComplete(function () {
    $.LoadingOverlay("hide");
});

function onGetRequestSuccess(html) {
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

function onFormPostRequestSuccess(data, url) {
    if (data.success) {
        $.get(url, function (html) {
            $("#main-content").html(html);
        });
    }
    else {
        $("#main-content").html(data.html);
    }
}

function onActionPostRequestSuccess(data, url) {
    if (data.success) {
        $.get(url, function (html) {
            $("#main-content").html(html);
        });
    }
    else {
        //TODO - error display
        alert(JSON.stringify(data.errors));
    }
}

function closeSidebar() {
    $("#sidebar").removeClass("active");
    $(".overlay").fadeOut();
}