(function () {
    'use strict';

    angular
        .module('app') // Define a qual módulo seu .service pertence
        .factory('cadastroservice', cadastroservice); //Define o nome a função do seu .service

    cadastroservice.$inject = ['$http', 'servicebase']; //Lista de dependências
    function cadastroservice($http, servicebase) {

        var service = this;

        var service = {
            getEspecialidadeById: getEspecialidadeById,
            getAllEspecialidades: getAllEspecialidades,
            getEspecialidadePorNome: getEspecialidadePorNome,
            saveEspecialidade: saveEspecialidade,
            excluirEspecialidade: excluirEspecialidade,
            getProcedimentoById: getProcedimentoById,
            getProcedimentosPorNome: getProcedimentosPorNome,
            getAllProcedimentos: getAllProcedimentos,
            saveProcedimento: saveProcedimento,
            getProcedimentosPorNomeOuCodigo: getProcedimentosPorNomeOuCodigo,

            getAllCid: getAllCid,
            getCidByCodigo: getCidByCodigo,
            saveCid: saveCid,
            getCidPorNome: getCidPorNome,
            getAllOcupcaoes: getAllOcupcaoes,
            getOcupacoesPorNome: getOcupacoesPorNome,
            getOcupacaoById: getOcupacaoById,
            saveOcupacao: saveOcupacao,

            saveFornecedor: saveFornecedor,
            listarFornecedores: listarFornecedores,
            pesquisarFornecedores: pesquisarFornecedores,
            getFornecedorById: getFornecedorById,
            excluirFornecedorById: excluirFornecedorById,

            saveConvenio: saveConvenio,
            listarConvenios: listarConvenios,
            pesquisarConvenios: pesquisarConvenios,
            getConvenioById: getConvenioById,
            excluirConvenioById: excluirConvenioById,

            saveFuncionario: saveFuncionario,
            listarFuncionarios: listarFuncionarios,
            listarProfissionaisSaude: listarProfissionaisSaude,
            pesquisarFuncionarios: pesquisarFuncionarios,
            getFuncionarioById: getFuncionarioById,
            excluirFuncionarioById: excluirFuncionarioById,


            listarEspecialidadesPorProfissionalSaude: listarEspecialidadesPorProfissionalSaude,
            listarProcedimentosPorEspecialidade: listarProcedimentosPorEspecialidade,

            saveClinica: saveClinica,
            getDadosClinica: getDadosClinica,
            addUnidade: addUnidade,
            getDadosUnidade: getDadosUnidade,
            listarUnidadesAtendimento: listarUnidadesAtendimento,
            excluirUnidade: excluirUnidade,


            listarTabelas: listarTabelas,
            saveTabela: saveTabela,
            getTabelaById: getTabelaById,
            getprocedimentosByTabela: getprocedimentosByTabela,
            saveprocedimentostabela: saveprocedimentostabela,
            getAllTabelasPorConvenio: getAllTabelasPorConvenio,
            getAllTabelasPorConvenioAtivas: getAllTabelasPorConvenioAtivas,
            getProcedimentoTabela: getProcedimentoTabela,
            excluirTabela: excluirTabela,

            listarConsultorios: listarConsultorios,
            salvarConsultorio: salvarConsultorio,
            excluirConsultorio: excluirConsultorio,
            obterConsultorioPorId: obterConsultorioPorId,

            getPessoaPorNome: getPessoaPorNome
        }
        return service;


        function getPessoaPorNome(nome) {
            return $http.get(servicebase.urlApi() + "/cadastros/getPessoaPorNome?nome=" + nome);
        }

        //Implementação das funções

        //***********************************   consultório     ***************************************************
        function listarConsultorios() {
            return $http.get(servicebase.urlApi() + "/cadastros/listarConsultorios");
        }

        function obterConsultorioPorId(id) {
            return $http.get(servicebase.urlApi() + "/cadastros/obterConsultorioPorId?id=" + id);
        }

        function salvarConsultorio(model) {
            return $http.post(servicebase.urlApi() + "/cadastros/salvarConsultorio", model);
        }

        function excluirConsultorio(id) {
            return $http.get(servicebase.urlApi() + "/cadastros/excluirConsultorio?id=" + id);
        }


        //***********************************   especialidades     ***************************************************
        function getEspecialidadeById(id) {
            return $http.get(servicebase.urlApi() + "/cadastros/getEspecialidadeById?id=" + id);
        }

        function getEspecialidadePorNome(nome) {
            return $http.get(servicebase.urlApi() + "/cadastros/getEspecialidadePorNome?nome=" + nome);
        }

        function getAllEspecialidades() {
            return $http.get(servicebase.urlApi() + "/cadastros/getAllEspecialidades");
        }

        function saveEspecialidade(model) {
            return $http.post(servicebase.urlApi() + "/cadastros/saveEspecialidade", model);
        }

        function excluirEspecialidade(id) {
            return $http.get(servicebase.urlApi() + "/cadastros/excluirEspecialidade?id=" + id);
        }

        //***********************************    procedimentos    ***************************************************
        function getProcedimentoById(id) {
            return $http.get(servicebase.urlApi() + "/cadastros/getProcedimentoById?id=" + id);
        }

        function getProcedimentosPorNome(nome) {
            return $http.get(servicebase.urlApi() + "/cadastros/getProcedimentosPorNome?nome=" + nome);
        }

        function getProcedimentosPorNomeOuCodigo(search) {
            return $http.get(servicebase.urlApi() + "/cadastros/getProcedimentosPorNomeOuCodigo?search=" + search);
        }

        function getAllProcedimentos() {
            return $http.get(servicebase.urlApi() + "/cadastros/getAllProcedimentos");
        }

        function saveProcedimento(model) {
            return $http.post(servicebase.urlApi() + "/cadastros/saveProcedimento", model);
        }

        //***********************************   Convênios     ***************************************************

        function getConvenioById(id) {
            return $http.get(servicebase.urlApi() + "/cadastros/getConvenioById?id=" + id);
        }

        function pesquisarConvenios(nome, idConvenio) {
            return $http.get(servicebase.urlApi() + "/cadastros/pesquisarConvenios?nome=" + nome + "&idConvenio=" + idConvenio);
        }

        function listarConvenios() {
            return $http.get(servicebase.urlApi() + "/cadastros/listarConvenios");
        }

        function saveConvenio(model) {
            return $http.post(servicebase.urlApi() + "/cadastros/saveConvenio", model);
        }

        function excluirConvenioById(id) {
            return $http.get(servicebase.urlApi() + "/cadastros/excluirConvenioById?id=" + id);
        }
        //***********************************    Tabela Convenio    ***************************************************

        /*function getTabelaConvenio(idConvenio) {
            return $http.get(servicebase.urlApi() + "/cadastros/getTabelaConvenio?idConvenio=" + idConvenio);
        }

        function excluirTabelaConvenio(idConvenio, idProcedimento) {
            return $http.get(servicebase.urlApi() + "/cadastros/excluirTabelaConvenio?idConvenio=" + idConvenio);
        }

        function saveTabelaConvenio(model) {
            return $http.post(servicebase.urlApi() + "/cadastros/saveTabelaConvenio", model);
        }  */

        //***********************************   CID   ***************************************************
        function getCidByCodigo(codigo) {
            return $http.get(servicebase.urlApi() + "/cadastros/getCidByCodigo?codigo=" + codigo);
        }

        function getCidPorNome(nome) {
            return $http.get(servicebase.urlApi() + "/cadastros/getCidPorNome?nome=" + nome);
        }

        function getAllCid() {
            return $http.get(servicebase.urlApi() + "/cadastros/getAllCid");
        }

        function saveCid(model) {
            return $http.post(servicebase.urlApi() + "/cadastros/saveCid", model);
        }

        //***********************************  Ocupações    ***************************************************

        function getOcupacaoById(id) {
            return $http.get(servicebase.urlApi() + "/cadastros/getOcupacaoById?id=" + id);
        }

        function getOcupacoesPorNome(nome) {
            return $http.get(servicebase.urlApi() + "/cadastros/getOcupacoesPorNome?nome=" + nome);
        }

        function getAllOcupcaoes() {
            return $http.get(servicebase.urlApi() + "/cadastros/getAllOcupcaoes");
        }

        function saveOcupacao(model) {
            return $http.post(servicebase.urlApi() + "/cadastros/saveOcupacao", model);
        }

        //***********************************    Fornecedor   ***************************************************

        function getFornecedorById(id) {
            return $http.get(servicebase.urlApi() + "/cadastros/getFornecedorById?id=" + id);
        }

        function pesquisarFornecedores(nome, idFornecedor) {
            return $http.get(servicebase.urlApi() + "/cadastros/pesquisarFornecedores?nome=" + nome + "&idFornecedor=" + idFornecedor);
        }

        function listarFornecedores() {
            return $http.get(servicebase.urlApi() + "/cadastros/listarFornecedores");
        }

        function saveFornecedor(model) {
            return $http.post(servicebase.urlApi() + "/cadastros/saveFornecedor", model);
        }

        function excluirFornecedorById(id) {
            return $http.get(servicebase.urlApi() + "/cadastros/excluirFornecedorById?id=" + id);
        }

        //***********************************    Funcionario   ***************************************************

        function getFuncionarioById(id) {
            return $http.get(servicebase.urlApi() + "/cadastros/getFuncionarioById?id=" + id);
        }

        function pesquisarFuncionarios(nome, idFuncionario) {
            return $http.get(servicebase.urlApi() + "/cadastros/pesquisarFuncionarios?nome=" + nome + "&idFuncionario=" + idFuncionario);
        }

        function listarFuncionarios() {
            return $http.get(servicebase.urlApi() + "/cadastros/listarFuncionarios");
        }

        function listarProfissionaisSaude() {
            return $http.get(servicebase.urlApi() + "/cadastros/listarProfissionaisSaude");
        }

        function saveFuncionario(model) {
            return $http.post(servicebase.urlApi() + "/cadastros/saveFuncionario", model);
        }

        function excluirFuncionarioById(id) {
            return $http.get(servicebase.urlApi() + "/cadastros/excluirFuncionarioById?id=" + id);
        }

        //*****************clinica******************
        function getDadosClinica() {
            return $http.get(servicebase.urlApi() + "/cadastros/getDadosClinica");
        }

        function getDadosUnidade(id) {
            return $http.get(servicebase.urlApi() + "/cadastros/getDadosUnidade?id=" + id);
        }

        function excluirUnidade(id) {
            return $http.get(servicebase.urlApi() + "/cadastros/excluirUnidade?id=" + id);
        }

        function listarUnidadesAtendimento() {
            return $http.get(servicebase.urlApi() + "/cadastros/listarUnidadesAtendimento");
        }

        function saveClinica(model) {
            return $http.post(servicebase.urlApi() + "/cadastros/saveClinica", model);
        }
        function addUnidade(model) {
            return $http.post(servicebase.urlApi() + "/cadastros/addUnidade", model);
        }

        /******* Alterar *********/
        function listarEspecialidadesPorProfissionalSaude(idprofissional) {
            return $http.get(servicebase.urlApi() + "/cadastros/listarEspecialidadesPorProfissionalSaude/?idprofissional=" + idprofissional);
        }

        function listarProcedimentosPorEspecialidade(idespecialidade) {
            return $http.get(servicebase.urlApi() + "/cadastros/listarProcedimentosPorEspecialidade/?idespecialidade=" + idespecialidade);
        }

        /******* tabelas de preço *********/
        function listarTabelas() {
            return $http.get(servicebase.urlApi() + "/cadastros/listarTabelas");
        }
        function getTabelaById(id) {
            return $http.get(servicebase.urlApi() + "/cadastros/getTabelaById?id=" + id);
        }
        function getprocedimentosByTabela(id) {
            return $http.get(servicebase.urlApi() + "/cadastros/getprocedimentosByTabela?id=" + id);
        }
        function saveTabela(model) {
            return $http.post(servicebase.urlApi() + "/cadastros/saveTabela", model);
        }
        function saveprocedimentostabela(model) {
            return $http.post(servicebase.urlApi() + "/cadastros/saveprocedimentostabela", model);
        }
        function getAllTabelasPorConvenio(tipo, id) {
            return $http.get(servicebase.urlApi() + "/cadastros/getAllTabelasPorConvenio?tipo=" + tipo + "&id=" + id);
        }
        function getAllTabelasPorConvenioAtivas(tipo, id) {
            return $http.get(servicebase.urlApi() + "/cadastros/getAllTabelasPorConvenioAtivas?tipo=" + tipo + "&id=" + id);
        }
        function getProcedimentoTabela(idproc, id) {
            return $http.get(servicebase.urlApi() + "/cadastros/getProcedimentoTabela?idproc=" + idproc + "&id=" + id);
        }
        function excluirTabela(id) {
            return $http.get(servicebase.urlApi() + "/cadastros/excluirTabela?id=" + id);
        }
    }
})(); 