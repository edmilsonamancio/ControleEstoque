function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_nome').val(dados.Nome);
    $('#dd_paises').val(dados.idPais);
    $('#cbx_ativo').prop('checked', dados.Ativo);

    $('#ddl_estado').val(dados.idEstado);
    $('#ddl_estado').prop('disabled', dados.idEstado <= 0 || dados.idEstado == undefined);
}

function get_dados_form() {
    return {
        Id: $('#id_cadastro').val(),
        Nome: $('#txt_nome').val(),
        Uf: $('#txt_uf').val(),
        Ativo: $('#cbx_ativo').prop('checked')
    };
}

function set_focus_form() {
    $('#txt_nome').focus();
}

function set_dados_grid(dados) {
    return '<td>' + dados.Nome + '</td>' +
        '<td>' + dados.Uf + '</td>' +
        '<td>' + (dados.Ativo ? 'Sim' : 'Não') + '</td>';
}

function get_dados_inclusao() {
    return {
        Id: 0,
        Nome: '',
        IdPais: 0,
        IdEstado: 0,
        Ativo: true
    };
}

function preencher_linha_grid(param, linha) {
    linha
        .eq(0).html(param.Nome).end()
        .eq(1).html(param.Uf).end()
        .eq(2).html(param.Ativo ? 'Sim' : 'Não').end();
}

$(document).on('change', '#ddl_paises', function () {
    var ddl_paises = $(this),
        id_pais = parseInt(ddl_paises.val()),
        ddl_estados = $('#ddl_estados');

    if (id_pais > 0) {
        var url = url_lista_estados,
            param = { idPais: id_pais };

        ddl_estados.empty();
        $('#ddl_estados').prop('disabled', true);

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response && response.length > 0) {
                for (var i = 0; i < response.length; i++) {
                    ddl_estados.append('<option value=' + response[i].Id + '>' + response[i].Nome + '</option>');
                }
                $('#ddl_estados').prop('disabled', false);
            }
        });
    }
});








