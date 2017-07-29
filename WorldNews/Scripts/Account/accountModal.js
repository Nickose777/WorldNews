function showLoginView() {
    function onclickFunction(e) {
        e.preventDefault();

        showRegisterView();
    }

    var clickHandler = function () {
        var data = $("#modalWindow .modal-body").find("form").serialize();

        $.ajax({
            url: "/Account/Login",
            type: "POST",
            dataType: "json",
            data: data,
            success: function (response) {
                if (response.success) {
                    window.location.reload(true);
                }
                else {
                    $("#modalWindow .modal-body").html(response.html);
                    $("#modalWindow #goToRegister").on("click", onclickFunction);
                }
            },
            error: onError
        });
    }

    function prepareModal() {
        $("#modalWindow #modalTitle").text("Login");
        $("#modalWindow #btnModalAction").on("click", clickHandler);
        $("#modalWindow #goToRegister").on("click", onclickFunction);
    }

    function onModalReady(html) {
        if (isModalDisplayed()) {
            $("#modalWindow .modal-body").fadeOut("slow", function () {
                $(this).empty();
                $(this).hide().html(html).fadeIn("slow", prepareModal);
            });
        }
        else {
            $("#modalWindow .modal-body").html(html);
            prepareModal();
            $("#modalWindow").modal("show");
        }
    }

    $.get("/Account/Login", onModalReady).fail(onError);
}

function showRegisterView() {
    function onclickFunction(e) {
        e.preventDefault();

        showLoginView();
    }

    var clickHandler = function () {
        var data = $("#modalWindow .modal-body").find("form").serialize();

        $.ajax({
            url: "/Account/RegisterUser",
            type: "POST",
            dataType: "json",
            data: data,
            success: function (response) {
                if (response.success) {
                    showLoginView(true);
                }
                else {
                    $("#modalWindow .modal-body").html(response.html);
                    $("#modalWindow #goToLogin").on("click", onclickFunction);
                }
            },
            error: onError
        });
    }

    function prepareModal() {
        $("#modalWindow #modalTitle").text("Register");
        $("#modalWindow #btnModalAction").on("click", clickHandler);
        $("#modalWindow #goToLogin").on("click", onclickFunction);
    }

    function onModalReady(html) {
        if (isModalDisplayed()) {
            $("#modalWindow .modal-body").fadeOut("slow", function () {
                $(this).empty();
                $(this).hide().html(html).fadeIn("slow", prepareModal);
            });
        }
        else {
            $("#modalWindow .modal-body").html(html);
            prepareModal();
            $("#modalWindow").modal("show");
        }
    }

    $.get("/Account/RegisterUser", onModalReady).fail(onError);
}

function isModalDisplayed() {
    return $("#modalWindow").hasClass("show");
}

function onError(error) {
    alert("Couldn't connect to server...");
}