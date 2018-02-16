(function () {
    'use strict';

    angular
           .module('app') // Define a qual módulo seu .service pertence
           .factory('funcionarioservice', funcionarioservice); //Define o nome a função do seu .service

    funcionarioservice.$inject = ['$http', 'servicebase']; //Lista de dependências
    function funcionarioservice($http, servicebase) {

        var service = this;

        var service = {
            getFuncionarioById: getFuncionarioById,
            listarProfissionaisAtivos: listarProfissionaisAtivos,
            listarProfissionais: listarProfissionais,
            getMedicoById: getMedicoById,
            listarMedicos: listarMedicos,
            listarMedicosPorNome: listarMedicosPorNome,
            saveMedico: saveMedico,
            saveFuncionario: saveFuncionario,
            listarfuncionarios: listarfuncionarios,
            listarfuncionariosPorNome: listarfuncionariosPorNome
        }
        return service;


        //Implementação das funções
        //***********************************   anamnese pacinte     ***************************************************

        function getFuncionarioById(id) {
            return $http.get(servicebase.urlApi() + "/funcionario/getFuncionarioById?id=" + id);
        }
        function listarProfissionaisAtivos() {
            return $http.get(servicebase.urlApi() + "/funcionario/listarProfissionaisAtivos");
        }
        function listarProfissionais() {
            return $http.get(servicebase.urlApi() + "/funcionario/listarProfissionais");
        }
        function getMedicoById(id) {
            return $http.get(servicebase.urlApi() + "funcionario/getMedicoById?id=" + id);
        }

        function listarMedicos() {
            return $http.get(servicebase.urlApi() + "funcionario/listarMedicos");
        }

        function listarMedicosPorNome(nome) {
            return $http.get(servicebase.urlApi() + "funcionario/listarMedicosPorNome?nome=" + nome);
        }

        function getFuncionarioById(id) {
            return $http.get(servicebase.urlApi() + "funcionario/getFuncionarioById?id=" + id);
        }

        function saveMedico(model) {
            return $http.post(servicebase.urlApi() + "funcionario/saveMedico", model);
        }
        function saveFuncionario(model) {
            return $http.post(servicebase.urlApi() + "funcionario/saveFuncionario", model);
        }
        function listarfuncionariosPorNome(nome) {
            return $http.get(servicebase.urlApi() + "funcionario/listarfuncionariosPorNome?nome=" + nome);
        }

        function listarfuncionarios() {
            return $http.get(servicebase.urlApi() + "funcionario/listarfuncionarios");
        }


    }
})();