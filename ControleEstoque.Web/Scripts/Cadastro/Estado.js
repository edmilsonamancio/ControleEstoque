function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_nome').val(dados.Nome);
    $('#txt_uf').val(dados.Uf);
    $('#ddl_paises').val(dados.Id_Pais);
    $('#cbx_ativo').prop('checked', dados.Ativo);
}

function get_dados_form() {
    return {
        Id: $('#id_cadastro').val(),
        Nome: $('#txt_nome').val(),
        Uf: $('#txt_uf').val(),
        Id_Pais: $('#ddl_paises').val(),
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
        Uf: '',
        Id_Pais: '',
        Ativo: true
    };
}

function preencher_linha_grid(param, linha) {
    linha
        .eq(0).html(param.Nome).end()
        .eq(1).html(param.Uf).end()
        .eq(2).html(param.Ativo ? 'Sim' : 'Não').end();
}