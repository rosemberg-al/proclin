(function () {
    'use strict';

    angular
           .module('app') // Define a qual módulo seu .service pertence
           .factory('pacienteservice', pacienteservice); //Define o nome a função do seu .service

    pacienteservice.$inject = ['$http', 'servicebase']; //Lista de dependências
    function pacienteservice($http, servicebase) {

        var service = this;

        var service = {
            savePaciente: savePaciente,
            listarPacientes: listarPacientes,
            pesquisarPacientes: pesquisarPacientes,
            getPacienteById: getPacienteById,
            excluirPacienteById: excluirPacienteById,
            getPacientesPorNome: getPacientesPorNome,
            getConveniosByPaciente: getConveniosByPaciente,
            alterarfoto: alterarfoto
        }
        return service;


        //Implementação das funções
        //***********************************   paciente     ***************************************************
        function getPacienteById(id) {
            return $http.get(servicebase.urlApi() + "/cadastros/getPacienteById?id=" + id);
        }

        function pesquisarPacientes(nome, idPaciente) {
            return $http.get(servicebase.urlApi() + "/cadastros/pesquisarPacientes?nome=" + nome + "&idPaciente=" + idPaciente);
        }

        function listarPacientes() {
            return $http.get(servicebase.urlApi() + "/cadastros/listarPacientes");
        }

        function alterarfoto(model) {
            return $http.post(servicebase.urlApi() + "/paciente/alterarfoto", model);
        }

        function savePaciente(model) {
            return $http.post(servicebase.urlApi() + "/cadastros/savePaciente", model);
        }

        function excluirPacienteById(id) {
            return $http.get(servicebase.urlApi() + "/cadastros/excluirPacienteById?id=" + id);
        }

        function getPacientesPorNome(nome) {
            return $http.get(servicebase.urlApi() + "/paciente/getPacientesPorNome?nome=" + nome);
        }

        function getConveniosByPaciente(id) {
            return $http.get(servicebase.urlApi() + "/paciente/getConveniosByPaciente?id=" + id);
        }

    }
})();