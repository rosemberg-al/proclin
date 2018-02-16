(function () {
    'use strict';

    angular
           .module('app') // Define a qual módulo seu .service pertence
           .factory('dashboardservice', dashboardservice); //Define o nome a função do seu .service

    dashboardservice.$inject = ['$http', 'servicebase']; //Lista de dependências
    function dashboardservice($http, servicebase) {

        var service = this;
        //  var header = { headers: { 'Authorization': "Bearer " + localStorage.getItem('apptoken') } };   header

        var service = {
            getTotaisfinanceiro: getTotaisfinanceiro,
            getdespesas: getdespesas,
            getResceitaxDespesas: getResceitaxDespesas
        }
        return service;


        //Implementação das funções

        function getTotaisfinanceiro() {
            return $http.get(servicebase.urlApi() + "/dashboard/getTotaisfinanceiro");
        }
        function getdespesas() {
            return $http.get(servicebase.urlApi() + "/dashboard/getdespesas");
        }
        function getResceitaxDespesas() {
            return $http.get(servicebase.urlApi() + "/dashboard/getResceitaxDespesas");
        }

    }
})();