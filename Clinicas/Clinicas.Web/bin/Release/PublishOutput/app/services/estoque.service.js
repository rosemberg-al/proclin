(function () {
    'use strict';

    angular
           .module('app') // Define a qual módulo seu .service pertence
           .factory('estoqueservice', estoqueservice); //Define o nome a função do seu .service

    estoqueservice.$inject = ['$http', 'servicebase']; //Lista de dependências
    function estoqueservice($http, servicebase) 
    {

        var service = this;
        //  var header = { headers: { 'Authorization': "Bearer " + localStorage.getItem('apptoken') } };   header

          var service = {
            listaMateriais: listaMateriais,
            obterMaterialPorId: obterMaterialPorId,
            excluirMaterial:excluirMaterial,
            salvarMaterial:salvarMaterial,

            listaTipoMateriais:listaTipoMateriais,
            excluirTipoMaterial:excluirTipoMaterial,
            salvarTipoMaterial:salvarTipoMaterial,
            obterTipoMaterialPorId:obterTipoMaterialPorId,

            listarMovimentoEstoque:listarMovimentoEstoque,
            excluirMovimentoEstoque:excluirMovimentoEstoque,
            salvarMovimentoEstoque:salvarMovimentoEstoque
        }
        return service;


        //Implementação das funções

        function salvarMovimentoEstoque(model) {
            return $http.post(servicebase.urlApi() + "/estoque/salvarMovimentoEstoque", model);
        }

        function listarMovimentoEstoque() {
            return $http.get(servicebase.urlApi() + "/estoque/listarMovimentoEstoque");
        }

        function excluirMovimentoEstoque(id) {
            return $http.get(servicebase.urlApi() + "/estoque/excluirMovimentoEstoque/?id="+id);
        }



        function listaMateriais() {
            return $http.get(servicebase.urlApi() + "/estoque/listaMateriais");
        }

        function obterMaterialPorId(id) {
            return $http.get(servicebase.urlApi() + "/estoque/obterMaterialPorId/?id="+id);
        }

        function excluirMaterial(id) {
            return $http.get(servicebase.urlApi() + "/estoque/excluirMaterial/?id="+id);
        }

        function salvarMaterial(model) {
            return $http.post(servicebase.urlApi() + "/estoque/salvarMaterial", model);
        }

        function listaTipoMateriais() {
            return $http.get(servicebase.urlApi() + "/estoque/listaTipoMateriais");
        }

        function excluirTipoMaterial(id) {
            return $http.get(servicebase.urlApi() + "/estoque/excluirTipoMaterial/?id="+id);
        }

        function obterTipoMaterialPorId(id) {
            return $http.get(servicebase.urlApi() + "/estoque/obterTipoMaterialPorId/?id="+id);
        }

        function salvarTipoMaterial(model) {
            return $http.post(servicebase.urlApi() + "/estoque/salvarTipoMaterial",model);
        }

    }
})();