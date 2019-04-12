$(document).ready(function () {
    $("#btnShowModal").click(function () {
        CarregaPartialCreate();
        $("#mdlFornecedor").modal();
    });

    $("input[name$='rdtipo']").click(function () {
        var test = $(this).val();

        $("#DataNascimento").hide();
        $("#Rg").show();
    });

    CarregaPartialIndex();
});

function OnkeyCpfCnpj() {

    var TipoPessoa = 'J';

    if (document.getElementById('rdtipoF').checked) {
        TipoPessoa = document.getElementById('rdtipoF').value;
    }

    if (TipoPessoa == 'F') {
        $('#Cpf_Cnpj').mask('000.000.000-00', { reverse: true });
    } else if (TipoPessoa == 'J') {
        $('#Cpf_Cnpj').mask('00.000.000/0000-00', { reverse: true });
    }
    return;
}




function OnkeyRg() {
    $('#Rg').mask('000.000.000-0', { reverse: true });
}

function show() {
    document.getElementById('divfis').style.display = 'block';
    document.getElementById("Cpf_Cnpj").value = '';
}

function hide() {
    document.getElementById('divfis').style.display = 'none';
    document.getElementById("Cpf_Cnpj").value = '';
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
        });
}



function CarregaPartialCreate() {

    var url = baseUrl + 'Fornecedor/GetPartialCreate';

    $.get(
        url,
        function (data) {
            $("#divPartialCreate").html(data);

            $('.phone').mask('(00) 00009-0000');

            $('#btnAddFone').click(function () {
                if ($("input[name=Telefone]").val() != '') {
                    $('#todo').append("<li><div name='ulFone' style='float: left;'>" + $("input[name=Telefone]").val() + "</div> <a href='#' class='close ntooltip' title = 'Excluir telefone da lista.' aria-hidden='true'>&nbsp;&times;&nbsp;</a></li>");
                    $('#Telefone').val('');
                }
            });
            $("body").on('click', '#todo a', function () {
                $(this).closest("ul").remove();
            });

            $('.ntooltip').tooltipster({
                theme: 'tooltipster-shadow',
                plugins: ['follower']
            });
        });
}

function SalvaCreateFornecedor() {

    var txtcnpj = document.getElementById("Cpf_Cnpj");
    var txtNome = document.getElementById("Nome");
    var txtRg = document.getElementById("Rg");
    var txtDataNascimento = document.getElementById("DataNascimento");

    txtcnpj.style.borderColor = "rgb(204, 204, 204)";
    txtcnpj.style.outline = "rgb(204, 204, 204)";
    txtNome.style.borderColor = "rgb(204, 204, 204)";
    txtNome.style.outline = "rgb(204, 204, 204)";
    txtRg.style.borderColor = "rgb(204, 204, 204)";
    txtRg.style.outline = "rgb(204, 204, 204)";
    txtDataNascimento.style.borderColor = "rgb(204, 204, 204)";
    txtDataNascimento.style.outline = "rgb(204, 204, 204)";

    spncnpjerr.style.display = "none";
    spnnomeerr.style.display = "none";
    spnrgerr.style.display = "none";
    spndataerr.style.display = "none";

    var CpfCnpj = document.getElementById("Cpf_Cnpj").value;
    var EmpresaCnpj = document.getElementById("EmpresaCnpj").value;
    var Nome = document.getElementById("Nome").value;
    var Rg = document.getElementById("Rg").value;
    var DataNascimento = document.getElementById("DataNascimento").value;

    var ulFones = document.getElementsByName("ulFone");
    var Fones = { 0: [] };

    for (var i = 0; i < ulFones.length; i++) {
        Fones[i] = ulFones[i].innerText.replace('×', '');
    }

    var TipoPessoa = 'J';
    var erro = false;

    if (document.getElementById('rdtipoF').checked) {
        TipoPessoa = document.getElementById('rdtipoF').value;
    }    

    if (TipoPessoa == 'F') {

        if (DataNascimento == '') {
            spndataerr.innerText = "Informe a data de nascimento";
            spndataerr.style.display = "block";
            txtDataNascimento.style.borderColor = "#ff0000";
            txtDataNascimento.style.outline = "#ff0000";
            txtDataNascimento.focus();
            erro = true;
        }

        if (Rg == '') {
            spnrgerr.innerText = "Informe o RG";
            spnrgerr.style.display = "block";
            txtRg.style.borderColor = "#ff0000";
            txtRg.style.outline = "#ff0000";
            txtRg.focus();
            erro = true;
        }
    }

    if (!ValidaNome()) {
        erro = true;
    }

    if (TipoPessoa == 'F' && !TestaCPF()) {
        erro = true;
    } else if (TipoPessoa == 'J' && !ValidaCNPJ()) {
        erro = true;
    }

    if (erro == true) {
        return;
    }

    CpfCnpj = CpfCnpj.replace(/[^\d]+/g, '');
    CpfCnpj = parseInt(CpfCnpj);

    var token = $('input[name="__RequestVerificationToken"]').val();
    var tokenadr = $('form[action="' + baseUrl + 'Fornecedor/PostConfirmaCreate"] input[name="__RequestVerificationToken"]').val();
    var headersadr = {};
    headersadr['__RequestVerificationToken'] = tokenadr;

    var url = baseUrl + "Fornecedor/PostConfirmaCreate";

    $.ajax({
        url: url
        , type: "POST"
        , datatype: "json"
        , headers: headersadr
        , data:
        {
            CpfCnpj: CpfCnpj,
            EmpresaCnpj: EmpresaCnpj,
            TipoPessoa: TipoPessoa,
            Nome: Nome,
            Rg: Rg,
            DataNascimento: DataNascimento,
            Fones: Fones,
            __RequestVerificationToken: token
        }
        , success: function (data) {
            CarregaPartialIndex();

            if (data.Rc == 0) {
                document.getElementById("msgSucesso").style.display = "block";
                setTimeout(function () {
                    $('#mdlFornecedor').modal('hide');
                }, 1300);
            } else {
                document.getElementById("msgErro").style.display = "block";

                if (data.Message != "") 
                    document.getElementById("msgErro").innerHTML = "<strong>Erro!</strong> " + data.Message;
            }
        }
    });
    return;
}



function CarregaPartialDelete(EmpresaCnpj, Cpf_Cnpj) {

    Cpf_Cnpj = Cpf_Cnpj.replace(/[^\d]+/g, '');

    var url = baseUrl + 'Fornecedor/GetPartialDelete';

    $.get(
        url,
        {
            EmpresaCnpj: EmpresaCnpj,
            FornecedorCpfCnpj: Cpf_Cnpj
        },
        function (data) {
            $("#divPartialCreate").html(data);

            $('.ntooltip').tooltipster({
                theme: 'tooltipster-shadow',
                plugins: ['follower']
            });

            $("#mdlFornecedor").modal();
        });
}

function CarregaPartialEdit(EmpresaCnpj, Cpf_Cnpj) {

    Cpf_Cnpj = Cpf_Cnpj.replace(/[^\d]+/g, '');

    var url = baseUrl + 'Fornecedor/GetPartialEdit';

    $.get(
        url,
        {
            EmpresaCnpj: EmpresaCnpj,
            FornecedorCpfCnpj: Cpf_Cnpj
        },
        function (data) {
            $("#divPartialCreate").html(data);

            $("input[type=radio]").attr('disabled', true)

            $('.phone').mask('(00) 00009-0000');

            $('#btnAddFone').click(function () {
                if ($("input[name=Telefone]").val() != '') {
                    $('#todo').append("<ul><div name='ulFone' style='float: left;'>" + $("input[name=Telefone]").val() + "</div> <a href='#' class='close ntooltip' title = 'Excluir telefone da lista.' aria-hidden='true'>&nbsp;&times;&nbsp;</a></ul>");
                    $('#Telefone').val('');
                }
            });

            $("body").on('click', '#todo a', function () {
                $(this).closest("ul").remove();
            });

            $('.ntooltip').tooltipster({
                theme: 'tooltipster-shadow',
                plugins: ['follower']
            });

            $("#mdlFornecedor").modal();
        });
}


function SalvaEditFornecedor() {

    var txtcnpj = document.getElementById("Cpf_Cnpj");
    var txtNome = document.getElementById("Nome");


    txtcnpj.style.borderColor = "rgb(204, 204, 204)";
    txtcnpj.style.outline = "rgb(204, 204, 204)";
    txtNome.style.borderColor = "rgb(204, 204, 204)";
    txtNome.style.outline = "rgb(204, 204, 204)";

    spnnomeerr.style.display = "none";

    var CpfCnpj = document.getElementById("Cpf_Cnpj").value;
    var EmpresaCnpj = document.getElementById("EmpresaCnpj").value;
    var Nome = document.getElementById("Nome").value;
    var Rg;
    var DataNascimento;

    var ulFones = document.getElementsByName("ulFone");
    var Fones = { 0: [] };

    for (var i = 0; i < ulFones.length; i++) {
        Fones[i] = ulFones[i].innerText.replace("x", "");
    }

    var erro = false;

    var TipoPessoa = document.getElementById('Tipo_Pessoa').value;

    if (TipoPessoa == 'F') {
        Rg = document.getElementById("Rg").value;
        DataNascimento = document.getElementById("DataNascimento").value;

        spnrgerr.style.display = "none";
        spndataerr.style.display = "none";

        var txtRg = document.getElementById("Rg");
        var txtDataNascimento = document.getElementById("DataNascimento");

        txtRg.style.borderColor = "rgb(204, 204, 204)";
        txtRg.style.outline = "rgb(204, 204, 204)";
        txtDataNascimento.style.borderColor = "rgb(204, 204, 204)";
        txtDataNascimento.style.outline = "rgb(204, 204, 204)";


        if (DataNascimento == '') {
            spndataerr.innerText = "Informe a data de nascimento";
            spndataerr.style.display = "block";
            txtDataNascimento.style.borderColor = "#ff0000";
            txtDataNascimento.style.outline = "#ff0000";
            txtDataNascimento.focus();
            erro = true;
        }

        if (Rg == '') {
            spnrgerr.innerText = "Informe o RG";
            spnrgerr.style.display = "block";
            txtRg.style.borderColor = "#ff0000";
            txtRg.style.outline = "#ff0000";
            txtRg.focus();
            erro = true;
        }
    }

    if (!ValidaNome()) {
        erro = true;
    }

    if (TipoPessoa == 'F' && !TestaCPF()) {
        erro = true;
    } else if (TipoPessoa == 'J' && !ValidaCNPJ()) {
        erro = true;
    }

    if (erro == true) {
        return;
    }

    CpfCnpj = CpfCnpj.replace(/[^\d]+/g, '');
    CpfCnpj = parseInt(CpfCnpj);

    var token = $('input[name="__RequestVerificationToken"]').val();
    var tokenadr = $('form[action="' + baseUrl + 'Fornecedor/PostConfirmaEdit"] input[name="__RequestVerificationToken"]').val();
    var headersadr = {};
    headersadr['__RequestVerificationToken'] = tokenadr;

    var url = baseUrl + "Fornecedor/PostConfirmaEdit";

    $.ajax({
        url: url
        , type: "POST"
        , datatype: "json"
        , headers: headersadr
        , data:
        {
            CpfCnpj: CpfCnpj,
            EmpresaCnpj: EmpresaCnpj,
            TipoPessoa: TipoPessoa,
            Nome: Nome,
            Rg: Rg,
            DataNascimento: DataNascimento,
            Fones: Fones,
            __RequestVerificationToken: token
        }
        , success: function (data) {
            CarregaPartialIndex();

            if (data.Rc == 0) {
                document.getElementById("msgSucesso").style.display = "block";
                setTimeout(function () {
                    $('#mdlFornecedor').modal('hide');
                }, 1300);
            } else {
                document.getElementById("msgErro").style.display = "block";

                if (data.Message != "")
                    document.getElementById("msgErro").innerHTML = "<strong>Erro!</strong> " + data.Message;
            }
        }
    });
    return;
}

function CarregaPartialDetalhes(EmpresaCnpj, Cpf_Cnpj) {

    Cpf_Cnpj = Cpf_Cnpj.replace(/[^\d]+/g, '');

    var url = baseUrl + 'Fornecedor/GetPartialDetalhes';

    $.get(
        url,
        {
            EmpresaCnpj: EmpresaCnpj,
            FornecedorCpfCnpj: Cpf_Cnpj
        },
        function (data) {
            $("#divPartialCreate").html(data);

            $('.ntooltip').tooltipster({
                theme: 'tooltipster-shadow',
                plugins: ['follower']
            });

            $("#mdlFornecedor").modal();
        });
}

function ConfirmaDelete(EmpresaCnpj, Cpf_Cnpj) {

    Cpf_Cnpj = Cpf_Cnpj.replace(/[^\d]+/g, '');

    var token = $('input[name="__RequestVerificationToken"]').val();
    var tokenadr = $('form[action="' + baseUrl + 'Fornecedor/DeleteConfirmed"] input[name="__RequestVerificationToken"]').val();
    var headersadr = {};
    headersadr['__RequestVerificationToken'] = tokenadr;

    var url = baseUrl + "Fornecedor/DeleteConfirmed";

    $.ajax({
        url: url
        , type: "POST"
        , datatype: "json"
        , headers: headersadr
        , data:
        {
            EmpresaCnpj: EmpresaCnpj,
            FornecedorCpfCnpj: Cpf_Cnpj,
            __RequestVerificationToken: token
        }
        , success: function (data) {
            CarregaPartialIndex();

            if (data.Rc == 0) {
                document.getElementById("msgSucesso").style.display = "block";
                document.getElementById("btnConfirmaDelete").style.display = "none";
                document.getElementById("btnCancelExclui").value = "Fechar";

                setTimeout(function () {
                    $('#mdlFornecedor').modal('hide');
                }, 1300);
            } else {
                document.getElementById("msgErro").style.display = "block";
            }
        }
    });
    return;
}

// Consistencias
function ValidaCNPJ() {

    var txtcnpj = document.getElementById("Cpf_Cnpj");
    var cnpj = document.getElementById("Cpf_Cnpj").value;

    cnpj = cnpj.replace(/[^\d]+/g, '');
    var spncnpjerr = document.getElementById("spncnpjerr");

    if (cnpj === '') {
        spncnpjerr.innerText = "Informe o CNPJ";
        spncnpjerr.style.display = "block";
        txtcnpj.style.borderColor = "#ff0000";
        txtcnpj.style.outline = "#ff0000";
        txtcnpj.focus();
        return false;
    }
    if (cnpj.length !== 14) {
        spncnpjerr.innerText = "CNPJ informado inválido";
        spncnpjerr.style.display = "block";
        txtcnpj.style.borderColor = "#ff0000";
        txtcnpj.style.outline = "#ff0000";
        txtcnpj.focus();
        return false;
    }

    // Elimina CNPJs invalidos conhecidos
    if (cnpj == "00000000000000" ||
        cnpj == "11111111111111" ||
        cnpj == "22222222222222" ||
        cnpj == "33333333333333" ||
        cnpj == "44444444444444" ||
        cnpj == "55555555555555" ||
        cnpj == "66666666666666" ||
        cnpj == "77777777777777" ||
        cnpj == "88888888888888" ||
        cnpj == "99999999999999") {
        spncnpjerr.innerText = "CNPJ informado inválido";
        spncnpjerr.style.display = "block";
        txtcnpj.style.borderColor = "#ff0000";
        txtcnpj.style.outline = "#ff0000";
        txtcnpj.focus();
        return false;
    }

    // Valida DVs
    tamanho = cnpj.length - 2;
    numeros = cnpj.substring(0, tamanho);
    digitos = cnpj.substring(tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2)
            pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(0)) {
        spncnpjerr.innerText = "CNPJ informado inválido";
        spncnpjerr.style.display = "block";
        txtcnpj.style.borderColor = "#ff0000";
        txtcnpj.style.outline = "#ff0000";
        txtcnpj.focus();
        return false;
    }

    tamanho = tamanho + 1;
    numeros = cnpj.substring(0, tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2)
            pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(1)) {
        spncnpjerr.innerText = "CNPJ informado inválido";
        spncnpjerr.style.display = "block";
        txtcnpj.style.borderColor = "#ff0000";
        txtcnpj.style.outline = "#ff0000";
        txtcnpj.focus();
        return false;
    }

    return true;
}

function TestaCPF() {

    var txtcnpj = document.getElementById("Cpf_Cnpj");
    var strCPF = document.getElementById("Cpf_Cnpj").value;
    strCPF = strCPF.replace(/[^\d]+/g, '');

    var Soma;
    var Resto;
    Soma = 0;
    if (strCPF == "00000000000") {

        spncnpjerr.innerText = "Cpf informado inválido";
        spncnpjerr.style.display = "block";
        txtcnpj.style.borderColor = "#ff0000";
        txtcnpj.style.outline = "#ff0000";
        txtcnpj.focus();
        return false;
    }

    for (i = 1; i <= 9; i++) Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (11 - i);
    Resto = (Soma * 10) % 11;

    if ((Resto == 10) || (Resto == 11)) Resto = 0;
    if (Resto != parseInt(strCPF.substring(9, 10))) {
        spncnpjerr.innerText = "Cpf informado inválido";
        spncnpjerr.style.display = "block";
        txtcnpj.style.borderColor = "#ff0000";
        txtcnpj.style.outline = "#ff0000";
        txtcnpj.focus();
        return false;
    }

    Soma = 0;
    for (i = 1; i <= 10; i++) Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (12 - i);
    Resto = (Soma * 10) % 11;

    if ((Resto == 10) || (Resto == 11)) Resto = 0;
    if (Resto != parseInt(strCPF.substring(10, 11))) {
        spncnpjerr.innerText = "Cpf informado inválido";
        spncnpjerr.style.display = "block";
        txtcnpj.style.borderColor = "#ff0000";
        txtcnpj.style.outline = "#ff0000";
        txtcnpj.focus();
        return false;
    }


    return true;
}

function ValidaNome() {
    var filter_nome = /^([a-zA-Zà-úÀ-Ú]|\s+)+$/;
    var txtnome = document.getElementById("Nome");
    var spnnomeerr = document.getElementById("spnnomeerr");

    spnnomeerr.innerText = "";
    spnnomeerr.style.display = "none";
    txtnome.style.borderColor = "#999999";
    txtnome.style.outline = null;

    if (!filter_nome.test(txtnome.value)) {
        spnnomeerr.innerText = "Nome inválido";
        if (txtnome.value === "")
            spnnomeerr.innerText = "Informe o nome";

        spnnomeerr.style.display = "block";
        txtnome.style.borderColor = "#ff0000";
        txtnome.style.outline = "#ff0000";
        txtnome.focus();
        return false;
    }
    return true;
}

function add() {

    //Create an input type dynamically.
    var element = document.createElement("input");

    //Create Labels
    var label = document.createElement("Label");
    label.innerHTML = "New Label";

    //Assign different attributes to the element.
    element.setAttribute("type", "text");
    element.setAttribute("value", "");
    element.setAttribute("name", "Test Name");
    element.setAttribute("style", "width:200px");

    label.setAttribute("style", "font-weight:normal");

    // 'foobar' is the div id, where new fields are to be added
    var foo = document.getElementById("fooBar");

    //Append the element in page (in span).
    foo.appendChild(label);
    foo.appendChild(element);
}

