(function () {
    'use strict';

    angular
           .module('app') // Define a qual módulo seu .service pertence
           .factory('segurancaservice', segurancaservice); //Define o nome a função do seu .service

    segurancaservice.$inject = ['$http', 'servicebase']; //Lista de dependências
    function segurancaservice($http, servicebase) {

        var service = this;
        //  var header = { headers: { 'Authorization': "Bearer " + localStorage.getItem('apptoken') } };   header

        var service = {
            getMenu: getMenu,
            obterUsuarioPorLogin: obterUsuarioPorLogin,

            // usuário 
            listarUsuarios: listarUsuarios,
            salvarUsuario: salvarUsuario,
            alterarSenha: alterarSenha,
            desativarUsuario: desativarUsuario,
            ativarUsuario:ativarUsuario,
            excluirUsuario: excluirUsuario,
            obterUsuarioPorId: obterUsuarioPorId,
            listarGrupoUsuario:listarGrupoUsuario,
            obterGrupoUsuarioPorId:obterGrupoUsuarioPorId,
            resetarSenha:resetarSenha,
            validalogin: validalogin,
            solicitaracesso: solicitaracesso,
            getAllEspecialidades:getAllEspecialidades
        }
        return service;

        function solicitaracesso(model) {
            return $http.post(servicebase.urlApi() + "/servico/solicitaracesso", model);
        }
        //Implementação das funções

        function getMenu(id) {
            return $http.get(servicebase.urlApi() + "/seguranca/obterFuncionalidadesPorGrupoUsuario/?idgrupoUsuario=" + id);
        }

        function getAllEspecialidades() {
            return $http.get(servicebase.urlApi() + "/servico/getAllEspecialidades");
        }

        function obterUsuarioPorLogin(login) {
            return $http.get(servicebase.urlApi() + "/seguranca/obterUsuarioPorLogin/?login=" + login);
        }

        function validalogin(login) {
            return $http.get(servicebase.urlApi() + "/seguranca/validalogin/?login=" + login);
        }


        function listarUsuarios() {
            return $http.get(servicebase.urlApi() + "/seguranca/listarUsuarios");
        }

        function salvarUsuario(model) {
            return $http.post(servicebase.urlApi() + "/seguranca/salvarUsuario",model);
        }

        function alterarSenha(novasenha,confirmar) {
            return $http.get(servicebase.urlApi() + "/seguranca/alterarSenha/?novasenha=" + novasenha+"&confirmar="+confirmar);
        }

        function desativarUsuario(idusuario) {
            return $http.get(servicebase.urlApi() + "/seguranca/desativarUsuario/?idusuario=" + idusuario);
        }

        function ativarUsuario(idusuario) {
            return $http.get(servicebase.urlApi() + "/seguranca/ativarUsuario/?idusuario=" + idusuario);
        }

        function excluirUsuario(idusuario) {
            return $http.get(servicebase.urlApi() + "/seguranca/excluirUsuario/?idusuario=" + idusuario);
        }

        function obterUsuarioPorId(idusuario) {
            return $http.get(servicebase.urlApi() + "/seguranca/obterUsuarioPorId/?idusuario=" + idusuario);
        }

        function resetarSenha(login) {
            return $http.get(servicebase.urlApi() + "/seguranca/resetarSenha/?login=" + login);
        }

        function listarGrupoUsuario() {
            return $http.get(servicebase.urlApi() + "/seguranca/listarGrupoUsuario");
        }

        function obterGrupoUsuarioPorId(idgrupousuario) {
            return $http.get(servicebase.urlApi() + "/seguranca/obterGrupoUsuarioPorId/?idgrupousuario=" + idgrupousuario);
        }

    }
})();