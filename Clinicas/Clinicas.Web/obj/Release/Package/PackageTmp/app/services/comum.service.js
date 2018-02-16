(function () {
    'use strict';

    angular
           .module('app') // Define a qual módulo seu .service pertence
           .factory('comumservice', comumservice); //Define o nome a função do seu .service

    comumservice.$inject = ['$http', 'servicebase']; //Lista de dependências
    function comumservice($http, servicebase) {

        var service = this;

        var service = {
            getEstados: getEstados,
            getCidadesByEstado: getCidadesByEstado,
        }
        return service;


        //Implementação das funções
        //***********************************   anamnese pacinte     ***************************************************

        function getCidadesByEstado(id) {
            return $http.get(servicebase.urlApi() + "/paciente/getCidadesByEstado?id=" + id);
        }
        function getEstados() {
            return $http.get(servicebase.urlApi() + "/paciente/getEstados");
        }

    }
})();