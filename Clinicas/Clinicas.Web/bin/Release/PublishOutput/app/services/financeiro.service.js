(function () {
    'use strict';

    angular
           .module('app') // Define a qual módulo seu .service pertence
           .factory('financeiroservice', financeiroservice); //Define o nome a função do seu .service

    financeiroservice.$inject = ['$http', 'servicebase']; //Lista de dependências
    function financeiroservice($http, servicebase) {

        var service = this;

        var service = {
            getContasApagarReceber: getContasApagarReceber,
            getParcelaById: getParcelaById,
            salvarFinanceiro: salvarFinanceiro,
            listarmeiospagamento: listarmeiospagamento,

            getTransferencias: getTransferencias,
            getTransferenciaById: getTransferenciaById,
            saveTransferencia: saveTransferencia,
            excluirTransferencia: excluirTransferencia,
            alterarParcela: alterarParcela,
            excluirParcela: excluirParcela,
            pesquisar: pesquisar,
            gerarparcelas: gerarparcelas,
            
            listarcheques: listarcheques,
            pesquisarCheques: pesquisarCheques,
            excluircheque: excluircheque,
            getChequeById: getChequeById,
            salvarCheque: salvarCheque,
            listarbancos: listarbancos,
            excluirbanco: excluirbanco,
            getBancoById: getBancoById,
            salvarBanco: salvarBanco,

            // contas
            listarContas: listarContas,
            excluirConta: excluirConta,
            getContaById: getContaById,
            saveConta: saveConta,
            pesquisarContas: pesquisarContas,

            // plano de contas 
            listarPlanodeContas: listarPlanodeContas,
            excluirPlanoConta: excluirPlanoConta,
            savePlanoConta: savePlanoConta,
            pesquisarPlanoContas: pesquisarPlanoContas,
            getPlanoContaById: getPlanoContaById,

            getFinanceiroPorId:getFinanceiroPorId

        }
        return service;
        

        function getFinanceiroPorId(id) {
            return $http.get(servicebase.urlApi() + "/financeiro/getFinanceiroPorId?id=" + id);
        }

        //Implementação das funções
        //***********************************   plano de conta     ***************************************************

        function listarPlanodeContas() {
            return $http.get(servicebase.urlApi() + "/financeiro/listarPlanodeContas");
        }

        function excluirPlanoConta(id) {
            return $http.get(servicebase.urlApi() + "/financeiro/excluirPlanoConta?id=" + id);
        }

        function savePlanoConta(model) {
            return $http.post(servicebase.urlApi() + "/financeiro/savePlanoConta", model);
        }

        function pesquisarPlanoContas(nome,codigo,tipo) {
            return $http.get(servicebase.urlApi() + "/financeiro/pesquisarPlanoContas?nome=" + nome+"&codigo="+codigo+"&tipo="+tipo);
        }

        function getPlanoContaById(id) {
            return $http.get(servicebase.urlApi() + "/financeiro/getPlanoContaById?id=" + id);
        }

        //Implementação das funções
        //***********************************   meio de pagamento     ***************************************************
        function listarmeiospagamento() {
            return $http.get(servicebase.urlApi() + "/financeiro/listarmeiospagamento");
        }

        //Implementação das funções
        //***********************************   conta     ***************************************************
        function listarcontas() {
            return $http.get(servicebase.urlApi() + "/financeiro/listarContas");
        }

        function excluirConta(id) {
            return $http.get(servicebase.urlApi() + "/financeiro/excluirConta?id=" + id);
        }

        function getContaById(id) {
            return $http.get(servicebase.urlApi() + "/financeiro/getContaById?id=" + id);
        }

        function saveConta(model) {
            return $http.post(servicebase.urlApi() + "/financeiro/saveConta", model);
        }

        function pesquisarContas(nome, codigo) {
            return $http.get(servicebase.urlApi() + "/financeiro/pesquisarContas?nome=" + nome + "&codigo=" + codigo);
        }


        //***********************************   financeiro     ***************************************************

        function getContasApagarReceber(tipo) {
            return $http.get(servicebase.urlApi() + "/financeiro/getContasApagarReceber?tipo=" + tipo);
        }

        

        function excluirParcela(idFinanceiro, idParcela) {
            return $http.get(servicebase.urlApi() + "/financeiro/excluirParcela?idFinanceiro=" + idFinanceiro + "&idParcela=" + idParcela);
        }

        function pesquisar(tipo, descricao, tipoConta) {
            return $http.get(servicebase.urlApi() + "/financeiro/pesquisar?tipo=" + tipo + "&descricao=" + descricao + "&tipoConta=" + tipoConta);
        }
        

        function listarContas() {
            return $http.get(servicebase.urlApi() + "/financeiro/listarContas");
        }

        function getTransferencias() {
            return $http.get(servicebase.urlApi() + "/financeiro/getTransferencias");
        }

        function getTransferenciaById(id) {
            return $http.get(servicebase.urlApi() + "/financeiro/getTransferenciaById?id=" + id);
        }

        function excluirTransferencia(id) {
            return $http.get(servicebase.urlApi() + "/financeiro/excluirTransferencia?id=" + id);
        }

        function getParcelaById(id) {
            return $http.get(servicebase.urlApi() + "/financeiro/getById?id=" + id);
        }

        function gerarparcelas(model) {
            return $http.post(servicebase.urlApi() + "/financeiro/gerarparcelas", model);
        }

        function saveTransferencia(model) {
            return $http.post(servicebase.urlApi() + "/financeiro/saveTransferencia", model);
        }

        function alterarParcela(model) {
            return $http.post(servicebase.urlApi() + "/financeiro/alterarParcela", model);
        }

        function salvarFinanceiro(model) {
            return $http.post(servicebase.urlApi() + "/financeiro/salvarFinanceiro", model);
        }

        //***********************************   cheques     ***************************************************
        function listarcheques() {
            return $http.get(servicebase.urlApi() + "/financeiro/listarcheques");
        }

        function excluircheque(id) {
            return $http.get(servicebase.urlApi() + "/financeiro/excluircheque?id=" + id);
        }

        function getChequeById(id) {
            return $http.get(servicebase.urlApi() + "/financeiro/getChequeById?id=" + id);
        }

        function pesquisarCheques(emitente, banco) {
            return $http.get(servicebase.urlApi() + "/financeiro/pesquisarCheques?emitente=" + emitente + "&banco=" + banco);
        }

        function salvarCheque(model) {
            return $http.post(servicebase.urlApi() + "/financeiro/salvarCheque", model);
        }

        //***********************************   bancos     ***************************************************
        function listarbancos() {
            return $http.get(servicebase.urlApi() + "/financeiro/listarbancos");
        }

        function excluirbanco(id) {
            return $http.get(servicebase.urlApi() + "/financeiro/excluirbanco?id=" + id);
        }

        function getBancoById(id) {
            return $http.get(servicebase.urlApi() + "/financeiro/getBancoById?id=" + id);
        }

        function salvarBanco(model) {
            return $http.post(servicebase.urlApi() + "/financeiro/salvarBanco", model);
        }


        
    }
})();