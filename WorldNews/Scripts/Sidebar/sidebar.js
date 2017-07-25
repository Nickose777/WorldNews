$(document).ready(function () {
    $("#sidebar").niceScroll({
        cursorcolor: "#53619d",
        cursorwidth: 4,
        cursorborder: "none"
    });

    $("#dismiss-sidebar, .overlay").on("click", function () {
        $("#sidebar").removeClass("active");
        $(".overlay").fadeOut();
    });

    $("#sidebarCollapse").on("click", function () {
        $("#sidebar").addClass("active");
        $(".overlay").fadeIn();
        $(".collapse.in").toggleClass("in");
        $("#sidebar").children(".components").addClass("w-100");
    });
});