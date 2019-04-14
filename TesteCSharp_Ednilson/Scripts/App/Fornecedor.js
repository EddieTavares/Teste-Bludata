$(document).ready(function () {
    $.LimpaPesquisa();

    //carrega modal de edicao
    $(".pesquisar").click(function () {
        $.CarregaPartialIndex();
    });

    // Limpa os campos do form pesquisa
    $(".limpar").click(function () {
        $.LimpaPesquisa();
    });
});

$.LimpaPesquisa = function () {
    $("#FiltroNome").val("");
    $("#FiltroCpfCnpj").val("");

    var d = new Date();
    var twoDigitMonth = ((d.getMonth().length + 1) === 1) ? (d.getMonth() + 1) : '0' + (d.getMonth() + 1);

    var strDate = d.getFullYear() + "-" + twoDigitMonth + "-" + d.getDate();
    $("#FiltroDataFinal").val(strDate);

    strDate = (d.getFullYear() - 5) + "-" + twoDigitMonth + "-" + d.getDate();
    $("#FiltroDataInicial").val(strDate);

    $.CarregaPartialIndex();
};


$.CarregaPartialIndex = function () {

    var FiltroNome = document.getElementById("FiltroNome").value;
    var FiltroCpfCnpj = document.getElementById("FiltroCpfCnpj").value;
    var FiltroDataInicial = document.getElementById("FiltroDataInicial").value;
    var FiltroDataFinal = document.getElementById("FiltroDataFinal").value;

    var url = baseUrl + 'Fornecedor/GetLista';

    $.get(
        url,
        {
            FiltroNome: FiltroNome,
            FiltroCpfCnpj: FiltroCpfCnpj,
            FiltroDataInicial: FiltroDataInicial,
            FiltroDataFinal: FiltroDataFinal
        },
        function (data) {
            $("#divListaFornecedor").html(data);

            $('.ntooltip').tooltipster({
                theme: 'tooltipster-shadow',
                plugins: ['follower']
            });

            $.CarrregaModal();
        });
};

function RecarregaGradeFornecedor(e) {
    if (!$('.field-validation-error').length && !$('.validation-summary-errors').length) {
        $('.close').click();  // fecha o modal
        $.CarregaPartialIndex();
    }
}

$.CarrregaModal = function () {
    //carrega modal de edicao
    $(".edit").click(function () {
        var id = JSON.parse($(this).attr("data-id"));

        $("#modal").load("/Fornecedor/Edit", { EmpresaCnpj: id[0], FornecedorCpfCnpj: id[1] }, function () {
            $.validator.unobtrusive.parse("#modal");
            $("#modal").modal();
            $.AtualizaFone();
        });
    });

    //carrega modal create
    $(".create").click(function () {
        $("#modal").load("/Fornecedor/Create", function () {
            $.validator.unobtrusive.parse("#modal");
            $("#modal").modal();
        });
    });

    //carrega modal details
    $(".details").click(function () {
        var id = JSON.parse($(this).attr("data-id"));
        $("#modal").load("/Fornecedor/Details", { EmpresaCnpj: id[0], FornecedorCpfCnpj: id[1] }, function () {
            $("#modal").modal();
        });
    });

    //carrega modal delete
    $(".delete").click(function () {
        var id = JSON.parse($(this).attr("data-id"));
        $("#modal").load("/Fornecedor/Delete", { EmpresaCnpj: id[0], FornecedorCpfCnpj: id[1] }, function () {
            $("#modal").modal();
        });
    });
};

$.AtualizaFone = function () {
    // adiciona telefone aa lista
    $('#btnAddFone').click(function () {
        if ($("input[name=Telefone]").val() != '') {
            var ind = $('#Telefones_Count').val();
            var telefone =
                "<li>" +
                "   <div name='ulFone' style='float: left;'>" + $("input[name=Telefone]").val() + "   </div>" +
                "   <input id='Telefones_" + ind + "_' name='Telefones[" + ind + "]' type='hidden' value='" + $("input[name=Telefone]").val() + "' class='telefone'>" +
                "   <a href='#' class='close ntooltip' title = 'Excluir telefone da lista.' aria-hidden='true'>&nbsp;&times;&nbsp;</a>" +
                "</li>";

            $('#todo').append(telefone);
            $('#Telefone').val('');
            ind++;
            $('#Telefones_Count').val(ind.toString());
        }
    });

    // remove telefone ao clicar no x
    $("body").on('click', '#todo a', function () {
        $(this).closest("li").remove();
        var telefones = $('.telefone');
        var telefoneshtml = "";

        // refatora a lista de telefones para serem enviadas à controller
        for (var i = 0; i < telefones.length; i++) {
            telefoneshtml = telefoneshtml +
                "<li>" +
                "   <div name='ulFone' style='float: left;'>" + telefones[i].value + "   </div>" +
                "   <input id='Telefones_" + i + "_' name='Telefones[" + i + "]' type='hidden' value='" + telefones[i].value + "' class='telefone'>" +
                "   <a href='#' class='close ntooltip' title = 'Excluir telefone da lista.' aria-hidden='true'>&nbsp;&times;&nbsp;</a>" +
                "</li>";
        }

        $('#todo').empty();
        $('#todo').append(telefoneshtml);
    });
};

$.AplicaMascaras = function () {
    $('.rg').mask('000.000.000-0');
    $('.money').mask('000.000.000.000.000,00', { reverse: true });
    $('.date').mask('00/00/0000');
    $('.time').mask('00:00:00');
    $('.cep').mask('00000-000');
    $('.phone').mask('(00) 00009-0000');
    $('.cpf').mask('000.000.000-00', { reverse: true });
    $('.cnpj').mask('00.000.000/0000-00', { reverse: true });
};
