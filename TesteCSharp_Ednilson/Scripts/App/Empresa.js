$(function () {
    $(".create").click(function () {
        $("#modal").load("/Empresa/Create", function () {
            $.validator.unobtrusive.parse("#modal");
            $("#modal").modal();
        });
    });

    $(".edit").click(function () {
        var id = $(this).attr("data-id");
        $("#modal").load("/Empresa/Edit?id=" + id, function () {
            $.validator.unobtrusive.parse("#modal");
            $("#modal").modal();
        });
    });

    $(".details").click(function () {
        var id = $(this).attr("data-id");
        $("#modal").load("/Empresa/Details?id=" + id, function () {
            $("#modal").modal();
        });
    });

    $(".delete").click(function () {
        var id = $(this).attr("data-id");
        $("#modal").load("/Empresa/Delete?id=" + id, function () {
            $("#modal").modal();
        });
    });
});

$.RecarregaGrade = function (e) {
    if (e.responseText == "Success") {
        $('.close').click();  // fecha o modal
        $('.modal-backdrop').hide(); // fecha o blackdrop

        window.location.replace(baseUrl + 'Empresa');
    }
};
