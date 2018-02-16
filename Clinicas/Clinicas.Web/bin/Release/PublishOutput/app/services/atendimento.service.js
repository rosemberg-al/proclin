(function () {
    'use strict';

    angular
           .module('app') // Define a qual módulo seu .service pertence
           .factory('atendimentoservice', atendimentoservice); //Define o nome a função do seu .service

    atendimentoservice.$inject = ['$http', 'servicebase']; //Lista de dependências
    function atendimentoservice($http, servicebase) {

        var service = this;

        var service = {
            getAnamneseById: getAnamneseById,
            getAnamneseByPaciente: getAnamneseByPaciente,
            saveAnamnese: saveAnamnese,
            saveRegistroVacina: saveRegistroVacina,
            getRegistroVacinaById: getRegistroVacinaById,
            getVacinasAtivas: getVacinasAtivas,
            getVacinasByPaciente: getVacinasByPaciente
        }
        return service;


        //Implementação das funções
        //***********************************   anamnese pacinte     ***************************************************

        function saveAnamnese(model) {
            return $http.post(servicebase.urlApi() + "/atendimento/saveAnamnese", model);
        }
        function getAnamneseById(id) {
            return $http.get(servicebase.urlApi() + "/atendimento/getAnamneseById?id=" + id);
        }
        function getAnamneseByPaciente(id) {
            return $http.get(servicebase.urlApi() + "/atendimento/getAnamneseByPaciente?id=" + id);
        }

        //***********************************   vacinas pacinte     ***************************************************
        function saveRegistroVacina(model) {
            return $http.post(servicebase.urlApi() + "/atendimento/saveRegistroVacina", model);
        }
        function getRegistroVacinaById(id) {
            return $http.get(servicebase.urlApi() + "/atendimento/getRegistroVacinaById?id=" + id);
        }
        function getVacinasByPaciente(id) {
            return $http.get(servicebase.urlApi() + "/atendimento/getVacinasByPaciente?id=" + id);
        }
        function getVacinasAtivas() {
            return $http.get(servicebase.urlApi() + "/atendimento/getVacinasAtivas");
        }
    }
})();