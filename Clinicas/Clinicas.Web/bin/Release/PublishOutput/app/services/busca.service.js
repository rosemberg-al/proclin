(function () {
    'use strict';

    angular
           .module('app') // Define a qual módulo seu .service pertence
           .factory('buscaservice', buscaservice); //Define o nome a função do seu .service

    buscaservice.$inject = ['$http', 'servicebase']; //Lista de dependências
    function buscaservice($http, servicebase) {

        var service = this;
        //  var header = { headers: { 'Authorization': "Bearer " + localStorage.getItem('apptoken') } };   header

        var service = {
            busca: busca

        }
        return service;


        //Implementação das funções
        function busca(search) {
            return $http.get(servicebase.urlApi() + "/busca/funcionalidade/?search=" + search);
        }

    }
})();