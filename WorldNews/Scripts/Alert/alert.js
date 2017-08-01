$(".alert").hide();

$(".close").on("click", function () {
    $(".alert").children().not(":first").remove();
    $(".alert").hide();
});

function displayErrors(errors) {
    for (var i = 0; i < errors.length; i++) {
        var error = errors[i];
        $(".alert").append("<span>" + error + "</span><br />");
    }

    $("html, body").animate({
        scrollTop: $(".alert").offset().top
    }, 1000);
    $(".alert").show();
}