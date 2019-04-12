$(document).ready(function () {
    //$("#btnShowModal").click(function () {
    //    CarregaPartialCreate();
    //    $("#modal").modal();
    //});
    //
    //$("input[name$='rdtipo']").click(function () {
    //    var test = $(this).val();
    //
    //    $("#DataNascimento").hide();
    //    $("#Rg").show();
    //});

    CarregaPartialIndex();
});

function RecarregaGradeFornecedor(e) {
    if (e.responseText == "Success") {
        $('.close').click();  // fecha o modal
        $('.modal-backdrop').hide(); // fecha o blackdrop
        // $(".indexcontainer").load("/Empresa/Index");
        window.location.replace(baseUrl + 'Fornecedor');
    }
}

function CarregaPartialIndex() {

    var EmpresaCnpj = document.getElementById("EmpresaCnpj").value;
    var Nome = document.getElementById("txtFiltroNome").value;
    var CpfCnpj = document.getElementById("txtFiltroCpfCnpj").value;
    var DataInicial = document.getElementById("dtpFiltroDataInicial").value;
    var DataFinal = document.getElementById("dtpFiltroDataFinal").value;

    var url = baseUrl + 'Fornecedor/GetPartialIndex';

    $.get(
        url,
        {
            EmpresaCnpj: EmpresaCnpj,
            Nome: Nome,
            CpfCnpj: CpfCnpj,
            DataInicial: DataInicial,
            DataFinal: DataFinal
        },
        function (data) {
            $("#divIndexFornecedor").html(data);

            $('.ntooltip').tooltipster({
                theme: 'tooltipster-shadow',
                plugins: ['follower']
            });

            $.CarrregaModal();
        });
}


$.CarrregaModal = function () {

    $(".edit").click(function () {
        var id = JSON.parse($(this).attr("data-id"));

        $("#modal").load("/Fornecedor/Edit", { EmpresaCnpj: id[0], FornecedorCpfCnpj: id[1] }, function () {
            $.validator.unobtrusive.parse("#modal");
            $("#modal").modal();
            $.AtualizaFone();
        });
    });

    $(".create").click(function () {
        $("#modal").load("/Fornecedor/Create", function () {
            $.validator.unobtrusive.parse("#modal");
            $("#modal").modal();

            // // oculta os campos Rg e Data de nascimento
            // 
            // 
            // // exibe os campos Rg e Data de nascimento quando pessoa física
            // $(".showrg").change(function () { 
            //     $(".divrg").show();
            //     $("#Cpf_Cnpj").val("");
            //     $("#Cpf_Cnpj").addClass("cpf");
            //     $("#Cpf_Cnpj").removeClass("cnpj");
            //     $('.cpf').mask('000.000.000-00', { reverse: true });
            // });
            // 
            // // exibe os campos Rg e Data de nascimento quando pessoa Jurídica
            // $(".hiderg").change(function () { 
            //     $(".divrg").hide();
            //     $("#Cpf_Cnpj").val("");
            //     $("#Cpf_Cnpj").addClass("cnpj");
            //     $("#Cpf_Cnpj").removeClass("cpf");
            //     $('.cnpj').mask('00.000.000/0000-00', { reverse: true });
            // });

            // $.AtualizaFone();
        });
    });

    $(".details").click(function () {
        var id = JSON.parse($(this).attr("data-id"));
        $("#modal").load("/Fornecedor/Details", { EmpresaCnpj: id[0], FornecedorCpfCnpj: id[1] }, function () {
            $("#modal").modal();
        });
    });

    $(".delete").click(function () {
        var id = JSON.parse($(this).attr("data-id"));

        debugger;


        $("#modal").load("/Fornecedor/Delete", { EmpresaCnpj: id[0], FornecedorCpfCnpj: id[1] }, function () {
            $("#modal").modal();
        });
    });
};

$.AtualizaFone = function () {
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

function show() {
    document.getElementById('divfis').style.display = 'block';
    document.getElementById("Cpf_Cnpj").value = '';
}

function hide() {
    document.getElementById('divfis').style.display = 'none';
    document.getElementById("Cpf_Cnpj").value = '';
}