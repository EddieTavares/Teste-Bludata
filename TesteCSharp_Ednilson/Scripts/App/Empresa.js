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


function RecarregaGrade(e) {
    if (!$('.field-validation-error').length && !$('.validation-summary-errors').length ) {
        $('.close').click();  // fecha o modal

        setTimeout(
            function () {
                window.location.replace(baseUrl + 'Empresa');
            }, 300);

    }

}


