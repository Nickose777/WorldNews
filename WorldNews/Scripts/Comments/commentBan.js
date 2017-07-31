function displayBanModal(commentId, articleId) {
    $.ajax({
        url: "/Comment/Ban?commentId=" + encodeURIComponent(commentId) + "&articleId=" + encodeURIComponent(articleId),
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

function banComment(sender, event, articleId) {
    event.preventDefault();
    var data = $(sender).closest("form").serialize();

    $.ajax({
        url: "/Comment/Ban",
        dataType: "json",
        type: "POST",
        data: data,
        success: function (response) {
            if (response.success) {
                var modal = $("#commentModal");
                modal.modal("hide");
                $("#commentModal .modal-body").empty();
                $.get("/Article/Details/" + articleId, onGetRequestSuccess);
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