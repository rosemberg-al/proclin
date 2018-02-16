(function () {
    'use strict';

    angular
           .module('app') // Define a qual módulo seu .service pertence
           .factory('medicamentoservice', medicamentoservice); //Define o nome a função do seu .service

    medicamentoservice.$inject = ['$http', 'servicebase']; //Lista de dependências
    function medicamentoservice($http, servicebase) {

        var service = this;

        var service = {
            saveMedicamento: saveMedicamento,
            listarMedicamentos: listarMedicamentos,
            pesquisarMedicamentos: pesquisarMedicamentos,
            getMedicamentoById: getMedicamentoById,
            excluirMedicamentoById: excluirMedicamentoById
        }
        return service;

     
        //***********************************    Medicamentos   ***************************************************

        function getMedicamentoById(id) {
            return $http.get(servicebase.urlApi() + "/medicamentos/getFuncionarioById?id=" + id);
        }

        function pesquisarMedicamentos(nome) {
            return $http.get(servicebase.urlApi() + "/medicamentos/pesquisarMedicamentos?nome=" + nome);
        }

        function listarMedicamentos() {
            return $http.get(servicebase.urlApi() + "/medicamentos/listarMedicamentos");
        }

        function saveMedicamento(model) {
            return $http.post(servicebase.urlApi() + "/medicamentos/saveMedicamento", model);
        }

        function excluirMedicamentoById(id) {
            return $http.get(servicebase.urlApi() + "/medicamentos/excluirMedicamentoById?id=" + id);
        }

    }
})();