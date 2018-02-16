(function () {
    'use strict';


    angular.module('app')
           .factory('ds.session', dsSession);

    dsSession.$inject = ['$rootScope', 'localStorageService'];

    function dsSession($rootScope, localStorageService) {

        var service = {
            getOperador: getOperador,
            setOperador: setOperador,
            getUsuario: getUsuario,
            setUsuario: setUsuario,
            limparSessao: limparSessao,
            removerPropriedade: removerPropriedade
        };

        return service;

        function setUsuario(value) {
            localStorageService.set('usuario', value);
        }

        function getUsuario() {
            return localStorageService.get('usuario');
        }

        function setOperador(value) {
           // localStorageService.set('operador', value);
        }

        //Retorno: ID Usuario Solus
        function getOperador() {
            //return localStorageService.get('operador');
        }

        //Limpar o localStorage
        function limparSessao() {
           // return localStorageService.clearAll();
        }

        //Remover propriedade
        function removerPropriedade(prop) {
          //  return localStorageService.remove(prop);
        }

    }
})();
