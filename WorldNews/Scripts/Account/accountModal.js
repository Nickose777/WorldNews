function showLoginView(isAlreadyShown) {
    var loginHandler = function () {
        var data = $("#modalWindow .modal-body").find("form").serialize();

        $.ajax({
            url: "/Account/LoginJson",
            type: "POST",
            dataType: "json",
            data: data,
            success: function (response) {
                if (response.success) {
                    window.location.reload(true);
                }
                else {
                    $("#modalWindow .modal-body").html(response.html);
                    $("#modalWindow #goToRegister").on("click", function (e) {
                        e.preventDefault();
                        showRegisterView(true);
                    });
                }
            },
            error: function (error) {
                alert("Couldn't connect to server. Try a bit later");
            }
        });
    }

    $.ajax({
        url: "/Account/LoginPartial",
        type: "GET",
        dataType: "html",
        success: function (html) {
            var loginViewReady = function () {
                $("#modalWindow #modalTitle").text("Login");
                $("#modalWindow #btnModalAction").on("click", loginHandler);
                $("#modalWindow #goToRegister").on("click", function (e) {
                    e.preventDefault();
                    showRegisterView(true);
                });
            }

            if (isAlreadyShown) {
                $("#modalWindow .modal-body").fadeOut("slow", function () {
                    $(this).empty();
                    $(this).hide().html(html).fadeIn("slow", loginViewReady);
                });
            }
            else {
                $("#modalWindow .modal-body").html(html);
                loginViewReady();
            }

            if (!isAlreadyShown) {
                $("#modalWindow").modal("show");
            }
        },
        error: function (error) {
            alert("Couldn't connect to server. Try a bit later");
        }
    });
}

function showRegisterView(isAlreadyShown) {
    var registerHandler = function () {
        var data = $("#modalWindow .modal-body").find("form").serialize();

        $.ajax({
            url: "/Account/RegisterUserJson",
            type: "POST",
            dataType: "json",
            data: data,
            success: function (response) {
                if (response.success) {
                    showLoginView(true);
                }
                else {
                    $("#modalWindow .modal-body").html(response.html);
                    $("#modalWindow #goToLogin").on("click", function (e) {
                        e.preventDefault();

                        showLoginView(true);
                    });
                }
            },
            error: function (error) {
                alert("Couldn't connect to server. Try a bit later");
            }
        });
    }

    $.ajax({
        url: "/Account/RegisterUserPartial",
        type: "GET",
        dataType: "html",
        success: function (html) {
            var registerViewReady = function () {
                $("#modalWindow #modalTitle").text("Register");
                $("#modalWindow #btnModalAction").on("click", registerHandler);
                $("#modalWindow #goToLogin").on("click", function (e) {
                    e.preventDefault();

                    showLoginView(true);
                });
            }

            if (isAlreadyShown) {
                $("#modalWindow .modal-body").fadeOut("slow", function () {
                    $(this).empty();
                    $(this).hide().html(html).fadeIn("slow", registerViewReady);
                });
            }
            else {
                $("#modalWindow .modal-body").html(html);
                registerViewReady();
            }

            if (!isAlreadyShown) {
                $("#modalWindow").modal("show");
            }
        },
        error: function (error) {
            alert("Couldn't connect to server. Try a bit later");
        }
    });
}