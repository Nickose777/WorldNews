function displayBanModal(commentId) {
    $.ajax({
        url: "/Comment/BanPartial/" + encodeURIComponent(commentId),
        dataType: "json",
        type: "GET",
        success: function (response) {
            if (response.success) {
                var modal = $("#commentModal");
                $("#commentModal .modal-body").html(response.html);
                modal.modal();
            }
            else {
                alert(response.html);
            }
        },
        error: function (error) {
            alert(JSON.stringify(error));
        }
    });
}

function banComment(sender, event) {
    event.preventDefault();
    var data = $(sender).closest("form").serialize();

    $.ajax({
        url: "/Comment/Ban",
        dataType: "json",
        type: "POST",
        data: data,
        success: function (response) {
            if (response.success) {
                window.location.reload(true);
            }
            else {
                $("#commentModal .modal-body").html(response.html);
            }
        },
        error: function (error) {
            alert(JSON.stringify(error));
        }
    });
}