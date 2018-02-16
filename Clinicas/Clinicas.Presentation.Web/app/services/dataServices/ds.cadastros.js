(function () {
    'use strict';

    angular.module('app.dataServices')
		.factory('ds.cadastros', dsCadastros);

    dsCadastros.$inject = ['$http', 'common', 'appConfig'];
    function dsCadastros($http, common, appConfig) {

        var apiRoute = common.makeApiRoute('cadastros');

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
            getConvenioById: getConvenioById,
            getConveniosPorNome: getConveniosPorNome,
            getAllConvenios: getAllConvenios,
            saveConvenio: saveConvenio,
            getTabelaConvenio: getTabelaConvenio,
            saveTabelaConvenio: saveTabelaConvenio,
            excluirTabelaConvenio: excluirTabelaConvenio,
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
            listarFornecedoresPorNome: listarFornecedoresPorNome,
            getFornecedorById: getFornecedorById

        }
        return service;

        //Implementação das funções
        //***********************************   especialidades     ***************************************************
        function getEspecialidadeById(id) {
            return $http.get(common.makeUrl([apiRoute, 'getEspecialidadeById']), { params: { id: id } });
        }

        function getEspecialidadePorNome(nome) {
            return $http.get(common.makeUrl([apiRoute, 'getEspecialidadePorNome']), { params: { nome: nome } });
        }

        function getAllEspecialidades() {
            return $http.get(common.makeUrl([apiRoute, 'getAllEspecialidades']));
        }

        function saveEspecialidade(model) {
            return $http.post(common.makeUrl([apiRoute, 'saveEspecialidade']), model);
        }

        function excluirEspecialidade(id) {
            return $http.get(common.makeUrl([apiRoute, 'excluirEspecialidade']), { params: { id: id } });
        }

        //***********************************    procedimentos    ***************************************************
        function getProcedimentoById(id) {
            return $http.get(common.makeUrl([apiRoute, 'getProcedimentoById']), { params: { id: id } });
        }

        function getProcedimentosPorNome(nome) {
            return $http.get(common.makeUrl([apiRoute, 'getProcedimentosPorNome']), { params: { nome: nome } });
        }

        function getAllProcedimentos() {
            return $http.get(common.makeUrl([apiRoute, 'getAllProcedimentos']));
        }

        function saveProcedimento(model) {
            return $http.post(common.makeUrl([apiRoute, 'saveProcedimento']), model);
        }

        //***********************************   Convênios     ***************************************************
        function getConvenioById(id) {
            return $http.get(common.makeUrl([apiRoute, 'getConvenioById']), { params: { id: id } });
        }

        function getConveniosPorNome(nome) {
            return $http.get(common.makeUrl([apiRoute, 'getConveniosPorNome']), { params: { nome: nome } });
        }

        function getAllConvenios() {
            return $http.get(common.makeUrl([apiRoute, 'getAllConvenios']));
        }

        function saveConvenio(model) {
            return $http.post(common.makeUrl([apiRoute, 'saveConvenio']), model);
        }
        //***********************************    Tabela Convenio    ***************************************************

        function getTabelaConvenio(idConvenio) {
            return $http.get(common.makeUrl([apiRoute, 'getTabelaConvenio']), { params: { idConvenio: idConvenio } });
        }

        function excluirTabelaConvenio(idConvenio, idProcedimento) {
            return $http.get(common.makeUrl([apiRoute, 'excluirTabelaConvenio']), { params: { idConvenio: idConvenio, idProcedimento: idProcedimento } });
        }

        function saveTabelaConvenio(model) {
            return $http.post(common.makeUrl([apiRoute, 'saveTabelaConvenio']), model);
        }

        //***********************************   CID   ***************************************************
        function getCidByCodigo(codigo) {
            return $http.get(common.makeUrl([apiRoute, 'getCidByCodigo']), { params: { codigo: codigo } });
        }

        function getCidPorNome(nome) {
            return $http.get(common.makeUrl([apiRoute, 'getCidPorNome']), { params: { nome: nome } });
        }

        function getAllCid() {
            return $http.get(common.makeUrl([apiRoute, 'getAllCid']));
        }

        function saveCid(model) {
            return $http.post(common.makeUrl([apiRoute, 'saveCid']), model);
        }

        //***********************************  Ocupações    ***************************************************

        function getOcupacaoById(id) {
            return $http.get(common.makeUrl([apiRoute, 'getOcupacaoById']), { params: { id: id } });
        }

        function getOcupacoesPorNome(nome) {
            return $http.get(common.makeUrl([apiRoute, 'getOcupacoesPorNome']), { params: { nome: nome } });
        }

        function getAllOcupcaoes() {
            return $http.get(common.makeUrl([apiRoute, 'getAllOcupcaoes']));
        }

        function saveOcupacao(model) {
            return $http.post(common.makeUrl([apiRoute, 'saveOcupacao']), model);
        }

        //***********************************    Fornecedor   ***************************************************

        function getFornecedorById(id) {
            return $http.get(common.makeUrl([apiRoute, 'getFornecedorById']), { params: { id: id } });
        }

        function listarFornecedoresPorNome(nome) {
            return $http.get(common.makeUrl([apiRoute, 'listarFornecedoresPorNome']), { params: { nome: nome } });
        }

        function listarFornecedores() {
            return $http.get(common.makeUrl([apiRoute, 'listarFornecedores']));
        }

        function saveFornecedor(model) {
            return $http.post(common.makeUrl([apiRoute, 'saveFornecedor']), model);
        }
    }
})();