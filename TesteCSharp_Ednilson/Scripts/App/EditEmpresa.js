$(document).ready(function () {
    $('.money').mask('000.000.000.000.000,00', { reverse: true });
    $('.date').mask('00/00/0000');
    $('.time').mask('00:00:00');
    $('.cep').mask('00000-000');
    $('.phone').mask('(00) 00009-0000');
    $('.cpf').mask('000.000.000-00', { reverse: true });
    $('.cnpj').mask('00.000.000/0000-00', { reverse: true });
});

function SalvaEditEmpresa() {

    var txtcnpj = document.getElementById("Cnpj");
    var Cnpj = document.getElementById("Cnpj").value;
    var UF = document.getElementById("UF").value;
    var NomeFantasia = document.getElementById("NomeFantasia").value;

    spncnpjerr.style.display = "none";
    spnnomeerr.style.display = "none";

    txtcnpj.style.borderColor = "rgb(204, 204, 204)";
    txtcnpj.style.outline = "rgb(204, 204, 204)";

    if (ValidaNome() & ValidaCNPJ()) {
        var token = $('input[name="__RequestVerificationToken"]').val();
        var tokenadr = $('form[action="' + baseUrl + 'Empresa/Edit"] input[name="__RequestVerificationToken"]').val();
        var headersadr = {};
        headersadr['__RequestVerificationToken'] = tokenadr;

        var url = baseUrl + "Empresa/Edit";

        $.ajax({
            url: url
            , type: "POST"
            , datatype: "json"
            , headers: headersadr
            , data:
            {
                Cnpj: Cnpj,
                UF: UF,
                NomeFantasia: NomeFantasia,
                __RequestVerificationToken: token
            }
            , success: function (data) {
                document.getElementById("msgSucesso").style.display = "block";
                document.getElementById("Cnpj").disabled = true;
            }
        });
        return;
    }
    return;
}

function ValidaCNPJ() {

    var txtcnpj = document.getElementById("Cnpj");
    var cnpj = document.getElementById("Cnpj").value;
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

function ValidaNome() {
    var filter_nome = /^([a-zA-Zà-úÀ-Ú]|\s+)+$/;
    var txtnome = document.getElementById("NomeFantasia");
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

